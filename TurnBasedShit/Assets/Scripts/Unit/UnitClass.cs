using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UnitClass : MonoBehaviour {
    [SerializeField] AudioClip hitSound, dieSound;
    public GameObject attackingTarget = null;
    public ParticleSystem bloodParticles;
    public Color hitColor;

    public bool isPlayerUnit = true;
    public bool defending = false;
    public bool stunned = false;
    public bool isMouseOverUnit = false;

    public float tempPower = 0.0f;
    public float tempDefence = 0.0f;
    public float tempSpeed = 0.0f;

    public float spotOffset = 0.0f;

    [SerializeField] WeaponPreset weapon = null;
    [SerializeField] ArmorPreset armor = null;

    Vector2 normalSize, normalPos;
    public Coroutine attackAnim, defendAnim;


    public UnitStats stats;



    private void OnMouseEnter() {
        if(FindObjectOfType<MenuCanvas>().isOpen() || FindObjectOfType<BattleResultsCanvas>() != null) {
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
        if(isPlayerUnit) {
            GetComponentInChildren<UnitSpriteHandler>().setEverything(stats.u_sprite, stats.equippedWeapon, stats.equippedArmor);
        }

        if(string.IsNullOrEmpty(stats.u_name)) {
            name = NameLibrary.getRandomUsablePlayerName();
        }
        else
            name = stats.u_name;


        normalSize = transform.localScale;

        if(isPlayerUnit)
            FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
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
        transform.DOComplete();
        normalPos = transform.position;
        //  triggers
        FindObjectOfType<ItemUser>().triggerTime(Item.useTimes.beforeAttacking, this, true);

        //  triggers
        stats.equippedWeapon.applyAttributesAfterAttack(gameObject, defender);

        //  Flair
        attackAnim = StartCoroutine(attackingAnim(defender));
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
        int armorReaction = stats.equippedArmor.applyAttributesAfterAttack(gameObject, attacker, FindObjectOfType<TurnOrderSorter>().playingUnit);
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
            for(int i = 0; i < ActiveQuests.getQuestTypeCount(GameInfo.questType.kill); i++) {
                if(ActiveQuests.getKillQuest(i).enemyType == GetComponent<EnemyUnitInstance>().enemyType)
                    ActiveQuests.getKillQuest(i).howManyToKill++;
            }
        }

        //  flair
        FindObjectOfType<AudioManager>().playSound(dieSound);

        //  removes unit from game world
        FindObjectOfType<TurnOrderSorter>().removeUnitFromList(gameObject);
        //FindObjectOfType<DamageTextCanvas>().removeTextsWithUnit(gameObject);
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

        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
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

        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(stats);
    }
    public void removeEquippedArmor() {
        stats.equippedArmor = null;
    }

    public void setEquipment(Weapon w = null, Armor a = null) {
        setEquippedWeapon(w);
        setEquippedArmor(a);
    }

    public abstract void setDefendingAnim();
    public abstract void setAttackingAnim();

    IEnumerator defendingAnim() {
        if(attackAnim != null)
            StopCoroutine(attackAnim);

        GetComponent<SpriteRenderer>().DOColor(hitColor, 0.05f);
        transform.DOScale(normalSize * 1.5f, 0.15f);

        setDefendingAnim();

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().DOColor(stats.u_sprite.color, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOScale(normalSize, 0.15f);
    }
    IEnumerator attackingAnim(GameObject defender) {
        //  move to target
        if(isPlayerUnit)
            transform.DOMove(defender.transform.position - new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        else
            transform.DOMove(defender.transform.position + new Vector3(1.25f, 0.0f, 0.0f), 0.25f);
        transform.DOScale(normalSize * 1.15f, 0.15f);

        //  wait for attacker to reach defender
        yield return new WaitForSeconds(0.35f);

        //  play animation and hold position
        setAttackingAnim();
        FindObjectOfType<AudioManager>().playSound(hitSound);

        //  actually deal damage to defender
        defender.GetComponent<UnitClass>().defend(gameObject, stats.getDamageGiven(tempPower));
        yield return new WaitForSeconds(0.4f);

        //  move back to original position
        transform.DOScale(normalSize, 0.15f);
        transform.DOMove(normalPos, 0.15f);
        attackAnim = null;
    }
}