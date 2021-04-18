using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<Image> weaponImages, armorImages, itemImages, consumableImages;


    public void showCombatLocationEquipment() {
        var weapons = GameInfo.getCombatDetails().getAndAddWeaponReward(FindObjectOfType<PresetLibrary>());
        var armor = GameInfo.getCombatDetails().getAndAddArmorReward(FindObjectOfType<PresetLibrary>());
        var consumables = GameInfo.getCombatDetails().getAndAddConsumableReward(FindObjectOfType<PresetLibrary>());
        var items = GameInfo.getCombatDetails().getAndAddItemReward(FindObjectOfType<PresetLibrary>());

        for(int i = 0; i < weapons.Count; i++)
            weaponImages[i].sprite = weapons[i].w_sprite.getSprite();
        for(int i = 0; i < armor.Count; i++)
            armorImages[i].sprite = armor[i].a_sprite.getSprite();
        for(int i = 0; i < consumables.Count; i++)
            consumableImages[i].sprite = consumables[i].c_sprite.getSprite();
        for(int i = 0; i < items.Count; i++)
            itemImages[i].sprite = items[i].i_sprite.getSprite();
    }

    //  Buttons
    public void returnToMap() {
        SceneManager.LoadScene("Map");
    }
}
