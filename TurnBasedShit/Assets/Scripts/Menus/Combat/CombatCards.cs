using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CombatCards : MonoBehaviour {
    [SerializeField] GameObject cardPreset;

    [SerializeField] Color goodColor, badColor;


    List<cardInfo> cards = new List<cardInfo>();

    //  weapon atts
    List<Weapon.attribute> attackWeAtts = new List<Weapon.attribute>() {
        Weapon.attribute.Power, Weapon.attribute.Bleed, Weapon.attribute.Stun, Weapon.attribute.LifeSteal
    };
    List<Weapon.attribute> defendWeAtts = new List<Weapon.attribute>() {
    };
    List<Weapon.attribute> chargeWeAtts = new List<Weapon.attribute>() {
    };
    List<Weapon.attribute> specialWeAtts = new List<Weapon.attribute>() {
    };

    //  armor atts
    List<Armor.attribute> attackArAtts = new List<Armor.attribute>() {
        Armor.attribute.Power
    };
    List<Armor.attribute> defendArAtts = new List<Armor.attribute>() {
        Armor.attribute.Reflex, Armor.attribute.Turtle
    };
    List<Armor.attribute> chargeArAtts = new List<Armor.attribute>() {
    };
    List<Armor.attribute> specialArAtts = new List<Armor.attribute>() {
    };

    //  passive mods
    List<StatModifier.passiveModifierType> attackPMods = new List<StatModifier.passiveModifierType>() {
        StatModifier.passiveModifierType.allWeaponExpMod, StatModifier.passiveModifierType.bluntExpMod, StatModifier.passiveModifierType.edgedExpMod,
        StatModifier.passiveModifierType.healInsteadOfDamage, StatModifier.passiveModifierType.missChance, StatModifier.passiveModifierType.modBluntPower, StatModifier.passiveModifierType.modEdgedPower,
        StatModifier.passiveModifierType.modEnemyDropChance, StatModifier.passiveModifierType.modPower, StatModifier.passiveModifierType.stunSelfChance, StatModifier.passiveModifierType.stunTargetChance
    };
    List<StatModifier.passiveModifierType> defendPMods = new List<StatModifier.passiveModifierType>() {
        StatModifier.passiveModifierType.modBluntDefence, StatModifier.passiveModifierType.modDefence, StatModifier.passiveModifierType.modEdgedDefence, StatModifier.passiveModifierType.modMaxHealth
    };
    List<StatModifier.passiveModifierType> chargePMods = new List<StatModifier.passiveModifierType>() {   //  add this array to attackMods
        StatModifier.passiveModifierType.addChargedPower
    };
    List<StatModifier.passiveModifierType> specialPMods = new List<StatModifier.passiveModifierType>() {
        StatModifier.passiveModifierType.modHealthGiven, StatModifier.passiveModifierType.modSummonDefence, StatModifier.passiveModifierType.modSummonMaxHealth, StatModifier.passiveModifierType.modSummonPower,
        StatModifier.passiveModifierType.modSummonSpeed, StatModifier.passiveModifierType.modSummonDefence, StatModifier.passiveModifierType.summonExpMod
    };

    //  dont check for timed mods because fuck doing that

    public enum cardState {
        attacking, defending, special, charging
    }

    public cardState state { get; private set; } = (cardState)(-1);


    //  implement a show / hide that tweens that card object by these values
    public class cardInfo {
        public GameObject card;
        public Sprite sprite;
        public Color color;
        public string profText;
        public string infoText;
        public Vector2 pos;
        public float rot;
        public float size;
    }

    public void showCards(cardState s, List<StatModifier.useTimeType> relevantTimes) {
        StartCoroutine(waitToShowCards(s, relevantTimes));
    }
    public void hideCards() {
        StartCoroutine(animateHidingCards());
        state = (cardState)(-1);
    }

    //  big boi
    List<cardInfo> createCardInfo(cardState state, List<StatModifier.useTimeType> relevantTimes) {
        var temp = new List<cardInfo>();
        var unit = GetComponentInParent<UnitClass>();

        //  weapon shit
        if(unit.stats.weapon != null && !unit.stats.isEmpty()) {
            var relevantAtts = state == cardState.attacking ? attackWeAtts : state == cardState.defending ? defendWeAtts : state == cardState.charging ? chargeWeAtts : specialWeAtts;

            if(state == cardState.attacking || state == cardState.charging) {
                var t = new cardInfo();
                t.color = goodColor;
                t.profText = "W";
                if(unit.stats.u_type == GameInfo.combatUnitType.player || unit.stats.u_type == GameInfo.combatUnitType.deadUnit) {
                    t.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(unit.stats.weapon);
                    t.infoText = InfoTextCreator.createForCombatCardWeapon(unit.stats.weapon);
                }
                temp.Add(t);
            }

            foreach(var i in unit.stats.weapon.attributes) {
                if(!relevantAtts.Contains(i))
                    continue;

                var c = new cardInfo();
                c.profText = "A";
                c.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(unit.stats.weapon);
                c.color = goodColor;
                c.infoText = InfoTextCreator.createForWeaponAtt(unit.stats.weapon, i);
            }
        }

        //  armor shit
        if(unit.stats.armor != null && !unit.stats.armor.isEmpty()) {
            var relevantAtts = state == cardState.attacking ? attackArAtts : state == cardState.defending ? defendArAtts : state == cardState.charging ? chargeArAtts : specialArAtts;

            if(state == cardState.defending) {
                var t = new cardInfo();
                t.color = goodColor;
                t.profText = "A";
                if(unit.stats.u_type == GameInfo.combatUnitType.player || unit.stats.u_type == GameInfo.combatUnitType.deadUnit) {
                    t.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(unit.stats.armor);
                    t.infoText = InfoTextCreator.createForCombatCardArmor(unit.stats.armor);
                }
                temp.Add(t);
            }

            foreach(var i in unit.stats.armor.attributes) {
                if(!relevantAtts.Contains(i))
                    continue;

                var c = new cardInfo();
                c.profText = "A";
                c.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(unit.stats.armor);
                c.color = goodColor;
                c.infoText = InfoTextCreator.createForArmorAtt(unit.stats.armor, i);
            }
        }


        var relevantPassives = state == cardState.attacking ? attackPMods : state == cardState.defending ? defendPMods : state == cardState.charging ? chargePMods : specialPMods;

        //  check for item shit
        if(unit.stats.item != null && !unit.stats.item.isEmpty()) {
            foreach(var i in unit.stats.item.pMods) {
                if(!relevantPassives.Contains(i.type))
                    continue;

                var info = InfoTextCreator.createForCombatCardItemPassive(unit.stats.item, unit.stats, i.type);
                if(string.IsNullOrEmpty(info))
                    continue;

                var t = new cardInfo();
                t.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(unit.stats.item);
                t.color = i.good ? goodColor : badColor;
                t.infoText = info;
            }
        }

        //  traits shit
        foreach(var tr in unit.stats.u_traits) {
            foreach(var i in tr.pMods) {
                if(!relevantPassives.Contains(i.type))
                    continue;

                var info = InfoTextCreator.createForTraitPassive(tr, unit.stats, i.type);
                if(string.IsNullOrEmpty(info))
                    continue;

                var t = new cardInfo();
                t.profText = "t";
                t.color = i.good ? goodColor : badColor;
                t.infoText = info;
            }
        }

        //  talent shit
        foreach(var i in unit.stats.u_talent.pMods) {
            if(!relevantPassives.Contains(i.type))
                continue;

            var info = InfoTextCreator.createForTraitPassive(unit.stats.u_talent, unit.stats, i.type);
            if(string.IsNullOrEmpty(info))
                continue;

            var t = new cardInfo();
            t.profText = "T";
            t.color = i.good ? goodColor : badColor;
            t.infoText = info;
        }
        return temp;
    }


    public bool isShowingCards() {
        return cards.Count > 0;
    }


    void spawnCards(List<cardInfo> info, bool highlighted = false) {
        int c = info.Count;
        //  delete cards that aren't going to be used
        for(int i = transform.childCount - 1; i >= c; i--) {
            Destroy(transform.GetChild(i).gameObject);
            if(i < cards.Count)
                cards.RemoveAt(i);
        }


        float maxRad = highlighted ? Mathf.PI / 2.0f : Mathf.PI / 4.0f;
        float r = 1.0f; //  radius length
        float h = transform.position.x; //  x origin
        float k = transform.position.y; //  y origin


        var fullLength = r * ((Mathf.PI / 2f) + maxRad);
        var midPoint = r * ((Mathf.PI / 2f) - maxRad);

        var maxLength = fullLength - midPoint;   //  arc length of the usable area
        var d = maxLength / c;    //  dist btw points
        float t = (Mathf.PI / 2f) + maxRad - (d / 2.0f);

        float scaleMod = Mathf.Clamp(c <= 2 ? 0.0f : (c - 1) * (0.05f), 0.0f, .75f) * (5f / 4f);

        for(int i = 0; i < c; i++, t -= d) {
            float x = r * Mathf.Cos(t) + h;
            float y = r * Mathf.Sin(t) + k;

            var obj = Instantiate(cardPreset.gameObject, transform);
            obj.transform.position = new Vector2(x, y);
            obj.transform.localScale = new Vector3(.75f - scaleMod, .75f - scaleMod);

            var objInfo = new cardInfo();
            objInfo.card = obj;
            objInfo.size = obj.transform.localScale.x;
            objInfo.rot = Mathf.Rad2Deg * t - (Mathf.PI / 2f) * Mathf.Rad2Deg;
            objInfo.pos = obj.transform.localPosition;

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.zero;

            if(info[i].sprite != null) {
                obj.GetComponent<SlotObject>().setImage(1, info[i].sprite);
                obj.GetComponent<SlotObject>().setText(0, "");
            }
            else {
                obj.GetComponent<SlotObject>().setText(0, info[i].profText);
                obj.GetComponent<SlotObject>().setImage(1, null);
            }
            obj.GetComponent<SlotObject>().setImageColor(0, info[i].color);
            obj.GetComponent<SlotObject>().setInfo(info[i].infoText);

            cards.Add(objInfo);
        }
    }

    IEnumerator waitToShowCards(cardState s, List<StatModifier.useTimeType> relevantTimes) {
        if(cards.Count > 0)
            hideCards();
        while(cards.Count > 0)
            yield return new WaitForEndOfFrame();

        state = s;
        var count = createCardInfo(s, relevantTimes);
        spawnCards(count);

        StartCoroutine(animateShowingCards());
    }
    IEnumerator animateShowingCards() {
        float speed = 0.075f;

        for(int i = 0; i < cards.Count; i++) {
            cards[i].card.transform.DOLocalMove(cards[i].pos, speed);
            cards[i].card.transform.DOScale(cards[i].size, speed);
            cards[i].card.transform.DORotate(new Vector3(0.0f, 0.0f, cards[i].rot), speed);

            yield return new WaitForSeconds(.05f);
        }
    }
    IEnumerator animateHidingCards() {
        float speed = 0.15f;

        if(cards == null || cards.Count == 0)
            yield break;

        for(int i = cards.Count - 1; i >= 0; i--) {
            cards[i].card.transform.DOLocalMove(Vector2.zero, speed);
            cards[i].card.transform.DOScale(0.0f, speed);
            cards[i].card.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), speed);

            Destroy(cards[i].card.gameObject, speed);

            yield return new WaitForSeconds(.05f);
        }

        cards.Clear();
    }
}
