using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<Image> weaponImages, armorImages, itemImages, consumableImages;


    public void showCombatLocationEquipment() {
        var weapons = GameInfo.getCombatDetails().weapons;
        var armor = GameInfo.getCombatDetails().armor;
        var consumables = GameInfo.getCombatDetails().consumables;
        var items = GameInfo.getCombatDetails().items;

        for(int i = 0; i < weaponImages.Count; i++) {
            if(i < weapons.Count)
                weaponImages[i].sprite = weapons[i].w_sprite.getSprite();
            else
                weaponImages[i].enabled = false;
        }

        for(int i = 0; i < armorImages.Count; i++) {
            if(i < armor.Count)
                armorImages[i].sprite = armor[i].a_sprite.getSprite();
            else
                armorImages[i].enabled = false;
        }

        for(int i = 0; i < consumableImages.Count; i++) {
            if(i < consumables.Count)
                consumableImages[i].sprite = consumables[i].c_sprite.getSprite();
            else
                consumableImages[i].enabled = false;
        }

        for(int i = 0; i < itemImages.Count; i++) {
            if(i < items.Count)
                itemImages[i].sprite = items[i].i_sprite.getSprite();
            else
                itemImages[i].enabled = false;
        }
    }

    //  Buttons
    public void returnToMap() {
        SceneManager.LoadScene("Map");
    }
}
