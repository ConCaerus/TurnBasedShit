using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<Image> weaponImages, armorImages, itemImages, usableImages, unusableImages;
    [SerializeField] TextMeshProUGUI coinsText;


    public void showCombatLocationEquipment() {
        foreach(var i in weaponImages)
            i.transform.localScale = Vector3.zero;
        foreach(var i in armorImages)
            i.transform.localScale = Vector3.zero;
        foreach(var i in itemImages)
            i.transform.localScale = Vector3.zero;
        foreach(var i in usableImages)
            i.transform.localScale = Vector3.zero;
        foreach(var i in unusableImages)
            i.transform.localScale = Vector3.zero;

        StartCoroutine(showAndAnimateSlots());
    }



    IEnumerator showAndAnimateSlots() {
        var weapons = new List<Weapon>();
        var armor = new List<Armor>();
        var usables = new List<Usable>();
        var unusables = new List<Unusable>();
        var items = new List<Item>();


        foreach(var i in GameInfo.getCombatDetails().collectables) {
            if(i.type == Collectable.collectableType.weapon)
                weapons.Add((Weapon)i);
            else if(i.type == Collectable.collectableType.armor)
                armor.Add((Armor)i);
            else if(i.type == Collectable.collectableType.usable)
                usables.Add((Usable)i);
            else if(i.type == Collectable.collectableType.unusable)
                unusables.Add((Unusable)i);
            else if(i.type == Collectable.collectableType.item)
                items.Add((Item)i);
        }


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

        for(int i = 0; i < usableImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < usables.Count) {
                usableImages[i].transform.DOScale(1.0f, waitTime);
                usableImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUsableSprite(usables[i]).sprite;

                //  remove all consumables of this type from the list
                int count = 0;
                List<Usable> removables = new List<Usable>();
                foreach(var c in usables) {
                    if(c.isTheSameTypeAs(usables[i])) {
                        count++;
                        removables.Add(c);
                    }
                }

                foreach(var c in removables)
                    usables.Remove(c);

                if(count > 1)
                    usableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
                else
                    usableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else break;
        }

        for(int i = 0; i < unusableImages.Count; i++) {
            yield return new WaitForSeconds(waitTime);
            if(i < usables.Count) {
                unusableImages[i].transform.DOScale(1.0f, waitTime);
                unusableImages[i].transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnusableSprite(unusables[i]).sprite;

                //  remove all consumables of this type from the list
                int count = 0;
                List<Unusable> removables = new List<Unusable>();
                foreach(var c in unusables) {
                    if(c.isTheSameTypeAs(unusables[i])) {
                        count++;
                        removables.Add(c);
                    }
                }

                foreach(var c in removables)
                    unusables.Remove(c);

                if(count > 1)
                    unusableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
                else
                    unusableImages[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else break;
        }
    }

    //  Buttons
    public void returnToMap() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }
}
