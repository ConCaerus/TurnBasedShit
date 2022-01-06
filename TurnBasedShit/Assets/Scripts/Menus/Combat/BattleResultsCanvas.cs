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
        StartCoroutine(showSpoils());
    }

    IEnumerator showSpoils() {
        coinsText.text = "Coins Earned: " + GameInfo.getCombatDetails().coins.ToString() + "c";
        List<Collectable> spoils = new List<Collectable>();
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

            var obj = slot.instantiateNewSlot(Color.white);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(spoils[i]);
            obj.GetComponent<InfoBearer>().setInfo(spoils[i].name);
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
    }

    //  Buttons
    public void returnToMap() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }
}
