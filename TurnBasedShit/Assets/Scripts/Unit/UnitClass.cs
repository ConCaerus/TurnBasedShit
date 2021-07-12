using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;

    [SerializeField] UnitSpriteHolder u_sprite;
    public Color hitColor;

    public bool isPlayerUnit = true;
    public bool defending = false;
    public bool stunned = false;
    public bool isMouseOverUnit = false;

    public float tempPower = 0.0f;
    public float tempDefence = 0.0f;
    public float tempSpeed = 0.0f;

    [SerializeField] WeaponPreset weapon = null;
    [SerializeField] ArmorPreset armor = null;

    Vector2 normalSize;
    Coroutine attackAnim, defendAnim;


    public UnitStats stats;



    private void OnMouseEnter() {
        if(FindObjectOfType<InventoryCanvas>().isOpen() || FindObjectOfType<BattleResultsCanvas>() != null) {
            isMouseOverUnit = false;
            FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
            return;
        }
        isMouseOverUnit = true;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    private void OnMouseExit() {
        isMouseOverUnit = false;
        FindObjectOfType<UnitCombatHighlighter>().updateHighlights();
    }

    public void setup() {
        if(isPlayerUnit)
            u_sprite = FindObjectOfType<PresetLibrary>().getPlayerUnitSprite();
        else
            u_sprite = FindObjectOfType<PresetLibrary>().getEnemyUnitSprite(gameObject.GetComponent<EnemyUnitInstance>());

        if(string.IsNullOrEmpty(stats.u_name)) {
            setNewRandomName();
        }
        else
            name = stats.u_name;


        normalSize = transform.localScale;

        if(isPlayerUnit)
            Party.overrideUnit(stats);
    }



    public void takeBleedDamage() {
        if(stats.u_bleedCount > 0) {
            float temp = (stats.getModifiedMaxHealth() / 100.0f) * stats.u_bleedCount;
            stats.u_health -= temp;

            checkIfDead();

            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, temp, DamageTextCanvas.damageType.poison);
        }
    }
    public void cureBleed() {
        stats.u_bleedCount = 0;
    }

    public bool checkIfDead() {
        if(stats.u_health <= 0.0f) {
            die();
            return true;
        }
        return false;
    }

    public void addHealth(float h) {
        if(checkIfDead())
            return;

        stats.u_health = Mathf.Clamp(stats.u_health + h, -1.0f, stats.getModifiedMaxHealth());

        checkIfDead();
    }

    public void prepareUnitForNextRound() {
        attackingTarget = null;
        defending = false;

        takeBleedDamage();
    }


    public void attack(GameObject defender) {
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeAttacking, this, true);

        //  Attack unit
        defender.GetComponent<UnitClass>().defend(gameObject, stats.getDamageGiven(tempPower));

        //  triggers
        stats.equippedWeapon.applyAttributesAfterAttack(gameObject, defender);

        //  Flair
        attackAnim = StartCoroutine(attackingAnim(defender.transform.position));
        FindObjectOfType<AudioManager>().playHitSound();
    }
    public void defend(GameObject attacker, float dmg) {
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeDefending, this, true);


        //  if defender is an enemy, check if it's weak or strong to the attack
        if(!isPlayerUnit) {
            //  check if it's weak to the attack
            if(GetComponent<EnemyUnitInstance>().weakTo == attacker.GetComponent<UnitClass>().stats.equippedWeapon.w_element)
                dmg *= 1.25f;
            else if(GetComponent<EnemyUnitInstance>().strongTo == attacker.GetComponent<UnitClass>().stats.equippedWeapon.w_element)
                dmg *= 0.75f;
        }

        dmg = stats.getModifiedDamageTaken(dmg, defending, tempDefence);

        //  take damage
        stats.u_health = Mathf.Clamp(stats.u_health - dmg, -1.0f, stats.getModifiedMaxHealth());

        //  Flair
        if(defending)
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.defended);
        else
            FindObjectOfType<DamageTextCanvas>().showTextForUnit(gameObject, dmg, DamageTextCanvas.damageType.weapon);
        var blood = Instantiate(bloodParticles);
        Destroy(blood.gameObject, blood.main.startLifetimeMultiplier);
        blood.gameObject.transform.position = transform.position;
        defendAnim = StartCoroutine(defendingAnim());

        //  triggers
        int armorReaction = stats.equippedArmor.applyAttributesAfterAttack(gameObject, attacker);
        //  reflex
        if(armorReaction == 1) {
            return;
        }

        //  check if any unit died in the attack
        attacker.GetComponent<UnitClass>().checkIfDead();
        checkIfDead();

        //  end battle turn
        FindObjectOfType<TurnOrderSorter>().setNextInTurnOrder();
    }

    public void die() {
        //  things to do with removing the unit from the party and what to do with equippment
        if(isPlayerUnit)
            stats.die();
        else {
            //  triggers items
            FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.afterKill, this, false);

            //  increases acc quest counter
            for(int i = 0; i < ActiveQuests.getQuestTypeCount(typeof(KillQuest)); i++) {
                if(ActiveQuests.getKillQuest(i).enemyType == GetComponent<EnemyUnitInstance>().enemyType)
                    ActiveQuests.getKillQuest(i).howManyToKill++;
            }
        }

        //  flair
        FindObjectOfType<AudioManager>().playDieSound();

        //  removes unit from game world
        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        FindObjectOfType<DamageTextCanvas>().removeTextsWithUnit(gameObject);
        FindObjectOfType<UnitCombatInformationCanvas>().removeInfoForUnit(gameObject);
        Destroy(gameObject);
    }



    public void setRandomAttackingTarget() {
        int unitCount = 0;
        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.isPlayerUnit != isPlayerUnit) {
                unitCount++;
            }
        }

        int rand = Random.Range(0, unitCount);
        unitCount = 0;

        foreach(var i in FindObjectsOfType<UnitClass>()) {
            if(i.isPlayerUnit != isPlayerUnit) {
                if(unitCount == rand)
                    attackingTarget = i.gameObject;

                unitCount++;
            }
        }
    }

    public void setEquippedWeapon(Weapon w = null) {
        if(w == null && weapon != null) {
            w = weapon.preset;
            weapon = null;
        }
        if(w == null) return;
        stats.equippedWeapon = w;

        Party.overrideUnit(stats);
    }
    public void removeEquippedWeapon() {
        stats.equippedWeapon = null;
    }

    public void setEquippedArmor(Armor a = null) {
        if(a == null && armor != null) {
            a = armor.preset;
            armor = null;
        }
        if(a == null) return;
        stats.equippedArmor = a;

        Party.overrideUnit(stats);
    }
    public void removeEquippedArmor() {
        stats.equippedArmor = null;
    }

    public void setEquipment(Weapon w = null, Armor a = null) {
        setEquippedWeapon(w);
        setEquippedArmor(a);
    }
    public void removeEquipment() {
        stats.equippedWeapon = null;
        stats.equippedArmor = null;
    }

    public void resetSavedEquippment() {
        if(weapon == null)
            stats.equippedWeapon.resetWeaponStats();
        else
            stats.equippedWeapon.setToPreset(weapon);

        if(armor == null)
            stats.equippedArmor.resetArmorStats();
        else
            stats.equippedArmor.setToPreset(armor);

        Party.overrideUnit(stats);
    }

    public void populateEmptyEquippment() {
        if(stats.equippedWeapon.isEmpty() && weapon != null)
            stats.equippedWeapon.setToPreset(weapon);
        else if(!stats.equippedWeapon.isEmpty())
            weapon = stats.equippedWeapon.weaponToPreset();


        if(stats.equippedArmor.isEmpty() && armor != null)
            stats.equippedArmor.setToPreset(armor);
        else if(!stats.equippedArmor.isEmpty())
            armor = stats.equippedArmor.armorToPreset();

        Party.overrideUnit(stats);
    }

    public void setNewRandomName() {
        if(isPlayerUnit)
            stats.u_name = NameLibrary.getRandomPlayerName();
        name = stats.u_name;
    }

    IEnumerator defendingAnim() {
        if(attackAnim != null)
            StopCoroutine(attackAnim);

        GetComponent<SpriteRenderer>().DOColor(hitColor, 0.05f);
        transform.DOScale(normalSize * 1.5f, 0.15f);

        if(u_sprite.defendingSprite != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = u_sprite.defendingSprite;
            GetComponentInChildren<ShadowObject>().updateSprite(u_sprite.defendingSprite);
        }

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_color, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOScale(normalSize, 0.15f);
        gameObject.GetComponent<SpriteRenderer>().sprite = u_sprite.sprite;
        GetComponentInChildren<ShadowObject>().updateSprite(u_sprite.sprite);
    }
    IEnumerator attackingAnim(Vector3 targetPos) {
        if(defendAnim != null) {
            StopCoroutine(defendAnim);
            GetComponent<SpriteRenderer>().DOColor(stats.u_color, 0.25f);
        }

        gameObject.transform.DOPunchPosition(targetPos - transform.position, 0.25f);
        transform.DOScale(normalSize * 1.5f, 0.15f);

        if(GetComponent<Animator>() != null)
            GetComponent<Animator>().SetInteger("state", 1);
        if(u_sprite.attackingSprite != null) {
            gameObject.GetComponent<SpriteRenderer>().sprite = u_sprite.attackingSprite;
            GetComponentInChildren<ShadowObject>().updateSprite(u_sprite.attackingSprite);
        }

        if(u_sprite.attackAnim != null && !u_sprite.attackAnim.empty)
            yield return new WaitForSeconds(u_sprite.attackAnim.length);
        else
            yield return new WaitForSeconds(0.3f);
    

        if(GetComponent<Animator>() != null)
            GetComponent<Animator>().SetInteger("state", 0);
        transform.DOScale(normalSize, 0.15f);
        if(u_sprite.sprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = u_sprite.sprite;

        foreach(var i in GetComponentsInChildren<ShadowObject>()) {
            i.updateSprite(i.transform.parent.GetComponent<SpriteRenderer>().sprite);
        }
    }


    public UnitSpriteHolder getSpriteHolder() {
        return u_sprite;
    }
}