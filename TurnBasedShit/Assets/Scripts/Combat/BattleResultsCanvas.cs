using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<Image> weaponImages, armorImages, itemImages, consumableImages;
    [SerializeField] TextMeshProUGUI coinsText;


    public void showCombatLocationEquipment() {
        var weapons = GameInfo.getCombatDetails().weapons;
        var armor = GameInfo.getCombatDetails().armor;
        var consumables = GameInfo.getCombatDetails().consumables;
        var items = GameInfo.getCombatDetails().items;

        coinsText.text = "Coins Earned: " + GameInfo.getCombatDetails().coinReward.ToString();

        for(int i = 0; i < weaponImages.Count; i++) {
            if(i < weapons.Count)
                weaponImages[i].sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(weapons[i]).sprite;
            else
                weaponImages[i].enabled = false;
        }

        for(int i = 0; i < armorImages.Count; i++) {
            if(i < armor.Count)
                armorImages[i].sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(armor[i]).sprite;
            else
                armorImages[i].enabled = false;
        }

        for(int i = 0; i < consumableImages.Count; i++) {
            if(i < consumables.Count)
                consumableImages[i].sprite = FindObjectOfType<PresetLibrary>().getConsumableSprite(consumables[i]).sprite;
            else
                consumableImages[i].enabled = false;
        }

        for(int i = 0; i < itemImages.Count; i++) {
            if(i < items.Count)
                itemImages[i].sprite = FindObjectOfType<PresetLibrary>().getItemSprite(items[i]).sprite;
            else
                itemImages[i].enabled = false;
        }
    }

    //  Buttons
    public void returnToMap() {
        FindObjectOfType<TransitionCanvas>().loadSceneWithTransition("Map");
    }
}
