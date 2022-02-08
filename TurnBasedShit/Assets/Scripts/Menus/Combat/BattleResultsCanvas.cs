using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] SlotMenu slot;


    public void showCombatLocationEquipment() {
        Time.timeScale = 1.0f;
        StartCoroutine(showSpoils());
    }

    IEnumerator showSpoils() {
        coinsText.text = "Coins Earned: " + GameInfo.getCombatDetails().coins.ToString() + "c";
        List<Collectable> spoils = new List<Collectable>();
        List<UnitStats> rescues = new List<UnitStats>();
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<Weapon>())
            spoils.Add(i);
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<Armor>())
            spoils.Add(i);
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<Item>())
            spoils.Add(i);
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<Usable>())
            spoils.Add(i);
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<Unusable>())
            spoils.Add(i);
        foreach(var i in GameInfo.getCombatDetails().spoils.getObjects<UnitStats>())
            rescues.Add(i);

        List<int> skipIndexes = new List<int>();
        for(int i = 0; i < spoils.Count; i++) {
            bool skip = false;
            foreach(var s in skipIndexes) {
                if(i == s) {
                    skip = true;
                    break;
                }
            }

            if(skip)
                continue;

            yield return new WaitForSeconds(0.15f);

            var obj = slot.instantiateNewSlot(FindObjectOfType<PresetLibrary>().getRarityColor(spoils[i].rarity));
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(spoils[i]);
            obj.GetComponent<SlotObject>().setInfo(spoils[i].name);
            if(spoils[i].maxStackCount == 1)
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            else {
                var count = 0;

                for(int s = i + 1; s < spoils.Count; s++) {
                    if(spoils[s].isTheSameTypeAs(spoils[i])) {
                        skipIndexes.Add(s);
                        count++;
                        if(count >= spoils[i].maxStackCount)
                            break;
                    }
                }

                if(count > 0)
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (count + 1).ToString();
                else
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        for(int i = 0; i < rescues.Count; i++) {
            yield return new WaitForSeconds(0.15f);

            var obj = slot.instantiateNewSlot(rescues[i].u_sprite.color * 2.0f);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(rescues[i].u_sprite.headIndex);
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(rescues[i].u_sprite.faceIndex);
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
            obj.transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(.4f, .4f);
            obj.transform.GetChild(0).GetChild(0).transform.localPosition = Vector3.zero;
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
            obj.transform.GetChild(0).GetComponent<Image>().color = rescues[i].u_sprite.color;
            obj.GetComponent<InfoBearer>().setInfo(rescues[i].u_name);
        }
    }

    //  Buttons
    public void returnToMap() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }
}
