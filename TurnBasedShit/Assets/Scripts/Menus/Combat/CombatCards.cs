using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CombatCards : MonoBehaviour {
    [SerializeField] GameObject cardPreset;

    [SerializeField] Color goodColor, badColor;


    List<cardInfo> cards = new List<cardInfo>();

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

    List<cardInfo> createCardInfo(cardState state, List<StatModifier.useTimeType> relevantTimes) {
        var temp = new List<cardInfo>();
        var unit = GetComponentInParent<UnitClass>();
        switch(state) {
            case cardState.attacking:
                //  adds for the initial weapon that the unit uses to attack
                if(unit.stats.weapon != null && !unit.stats.weapon.isEmpty()) {
                    var t = new cardInfo();
                    t.color = goodColor;
                    t.profText = "W";
                    if(unit.stats.u_type == GameInfo.combatUnitType.player || unit.stats.u_type == GameInfo.combatUnitType.deadUnit)
                        t.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(unit.stats.weapon).sprite;
                    else
                        t.infoText = InfoTextCreator.createForCombatCardWeapon(unit.stats.weapon);
                    temp.Add(t);
                }

                if(unit.stats.armor != null && !unit.stats.armor.isEmpty() && unit.stats.armor.getPowerAttCount() != 0) {
                    var t = new cardInfo();
                    t.color = goodColor;
                    t.profText = "A";
                    if(unit.stats.u_type == GameInfo.combatUnitType.player || unit.stats.u_type == GameInfo.combatUnitType.deadUnit)
                        t.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(unit.stats.armor).sprite;
                    else
                        t.infoText = InfoTextCreator.createForCombatCardArmor(unit.stats.armor);
                    temp.Add(t);
                }

                //  check for item shit

                //  goes through all of the passive mods of all of the traits, talent, and item's passive modifiers
                foreach(var i in unit.stats.getAllPassiveMods()) {
                    //  specific for weapon attack type
                    if(unit.stats.weapon != null && !unit.stats.weapon.isEmpty()) {
                        switch(unit.stats.weapon.aType) {
                            case Weapon.attackType.Edged:
                                if(i.getMod(StatModifier.passiveModifierType.edgedExpMod, unit.stats, false) != 0f) {
                                    var t = new cardInfo();
                                    t.color = i.getMod(StatModifier.passiveModifierType.edgedExpMod, unit.stats, false) > 0f ? goodColor : badColor;
                                    t.profText = "P";
                                    temp.Add(t);
                                }
                                if(i.getMod(StatModifier.passiveModifierType.modEdgedPower, unit.stats, false) != 0f) {
                                    var t = new cardInfo();
                                    t.color = i.getMod(StatModifier.passiveModifierType.modEdgedPower, unit.stats, false) > 0f ? goodColor : badColor;
                                    t.profText = "P";
                                    temp.Add(t);
                                }
                                break;

                            case Weapon.attackType.Blunt:
                                if(i.getMod(StatModifier.passiveModifierType.bluntExpMod, unit.stats, false) != 0f) {
                                    var t = new cardInfo();
                                    t.color = i.getMod(StatModifier.passiveModifierType.bluntExpMod, unit.stats, false) > 0f ? goodColor : badColor;
                                    t.profText = "P";
                                    temp.Add(t);
                                }
                                if(i.getMod(StatModifier.passiveModifierType.modBluntPower, unit.stats, false) != 0f) {
                                    var t = new cardInfo();
                                    t.color = i.getMod(StatModifier.passiveModifierType.modBluntPower, unit.stats, false) > 0f ? goodColor : badColor;
                                    t.profText = "P";
                                    temp.Add(t);
                                }
                                break;
                        }
                    }

                    if(i.getMod(StatModifier.passiveModifierType.modPower, unit.stats, false) != 0f) {
                        var t = new cardInfo();
                        t.color = i.getMod(StatModifier.passiveModifierType.modPower, unit.stats, false) > 0f ? goodColor : badColor;
                        t.profText = "T";
                        temp.Add(t);
                    }
                }

                //  goes through all of the timed modifiers
                foreach(var i in unit.stats.getAllTimedMods()) {
                    foreach(var t in relevantTimes) {
                        if(i.useTimes.Contains(t) && i.getMod(StatModifier.timedModifierType.addPower, unit, false) != 0f) {
                            var te = new cardInfo();
                            te.color = i.useTimes.Contains(t) && i.getMod(StatModifier.timedModifierType.addPower, unit, false) > 0f ? goodColor : badColor;
                            temp.Add(te);
                        }
                    }
                }
                break;

            case cardState.defending:
                break;

            case cardState.charging:
                break;

            case cardState.special:
                break;
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

        foreach(var i in cards) {
            i.card.transform.DOLocalMove(Vector2.zero, speed);
            i.card.transform.DOScale(0.0f, speed);
            i.card.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), speed);

            Destroy(i.card.gameObject, speed);

            yield return new WaitForSeconds(.05f);
        }

        cards.Clear();
    }
}
