using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<Image> weaponImages, armorImages, itemImages, consumableImages;
    [SerializeField] TextMeshProUGUI coinsText;


    public void showCombatLocationEquipment() {
        foreach(var i in weaponImages)
            i.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(var i in armorImages)
            i.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(var i in itemImages)
            i.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(var i in consumableImages)
            i.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

        StartCoroutine(showAndAnimateSlots());
    }



    IEnumerator showAndAnimateSlots() {
        var weapons = GameInfo.getCombatDetails().weapons;
        var armor = GameInfo.getCombatDetails().armor;
        var consumables = GameInfo.getCombatDetails().consumables;
        var items = GameInfo.getCombatDetails().items;
        GameInfo.getCombatDetails().addSpoils();

        coinsText.text = "Coins Earned: " + GameInfo.getCombatDetails().coinReward.ToString();

        float waitTime = 0.25f;

        for(int i = 0; i < weaponImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < weapons.Count) {
                weaponImages[i].transform.DOScale(1.0f, waitTime);
                weaponImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(weapons[i]).sprite;
            }
            else break;
        }

        for(int i = 0; i < armorImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < armor.Count) {
                armorImages[i].transform.DOScale(1.0f, waitTime);
                armorImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(armor[i]).sprite;
            }
            else break;
        }

        for(int i = 0; i < itemImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < items.Count) {
                itemImages[i].transform.DOScale(1.0f, waitTime);
                itemImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getItemSprite(items[i]).sprite;
            }
            else break;
        }

        for(int i = 0; i < consumableImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < consumables.Count) {
                consumableImages[i].transform.DOScale(1.0f, waitTime);
                consumableImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(consumables[i]).sprite;

                //  remove all consumables of this type from the list
                int count = 0;
                List<Usable> removables = new List<Usable>();
                foreach(var c in consumables) {
                    if(c.isTheSameTypeAs(consumables[i])) {
                        count++;
                        removables.Add(c);
                    }
                }

                foreach(var c in removables)
                    consumables.Remove(c);

                if(count > 1)
                    consumableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
                else
                    consumableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else break;
        }
    }

    //  Buttons
    public void returnToMap() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }
}
