using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UpgradeCanvas : MonoBehaviour {
    //  0 - weapon, 1 - armor
    public int state = 0;

    int attUpgrade = -1;
    float statUpgrade = 0.0f;

    [SerializeReference] GameObject shrine;
    [SerializeField] TextMeshProUGUI attributeText, statText;
    public SlotMenu slot;

    public bool isShowing = false;
    public bool hasBeenUsed = false;


    private void Start() {
        state = GameInfo.getCurrentLocationAsUpgrade().state;
        attUpgrade = GameInfo.getCurrentLocationAsUpgrade().attUpgrade;
        statUpgrade = GameInfo.getCurrentLocationAsUpgrade().statUpgrade;

        createSlots();
        attributeText.text = "";
        statText.text = "";
    }


    private void Update() {
        if(slot.run())
            updateInfo();
    }


    public void show() {
        float speed = 0.15f;
        transform.GetChild(0).DOScale(new Vector2(1.0f, 1.0f), speed);
        isShowing = true;
    }

    public void hide() {
        float speed = 0.25f;
        transform.GetChild(0).DOScale(new Vector2(0.0f, 0.0f), speed);
        FindObjectOfType<LocationMovement>().deinteract();
        isShowing = false;
    }


    Weapon getSelectedWeapon() {
        int index = slot.getSelectedSlotIndex();

        //  Party Equipment
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty()) {
                if(index == 0)
                    return Party.getMemberStats(i).equippedWeapon;
                index--;
            }
        }

        //  Inventory Equipment
        for(int i = 0; i < Inventory.getWeaponCount(); i++) {
            if(index == 0)
                return Inventory.getWeapon(i);
            index--;
        }

        return null;
    }

    Armor getSelectedArmor() {
        int index = slot.getSelectedSlotIndex();

        //  Party Equipment
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty()) {
                if(index == 0)
                    return Party.getMemberStats(i).equippedArmor;
                index--;
            }
        }

        //  Inventory Equipment
        for(int i = 0; i < Inventory.getArmorCount(); i++) {
            if(index == 0)
                return Inventory.getArmor(i);
            index--;
        }

        return null;
    }


    void createSlots() {
        if(state == 0) {
            //  Party Equipment
            int index = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getMemberStats(i).equippedWeapon.w_rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getMemberStats(i).u_name + "'s Weapon";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getMemberStats(i).equippedWeapon).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getWeapon(i).w_rarity));
                s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Inv";
                s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                index++;
            }
        }

        else if(state == 1) {
            //  Party Equipment
            int index = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getMemberStats(i).equippedArmor.a_rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getMemberStats(i).u_name + "'s Armor";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getMemberStats(i).equippedArmor).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getArmor(i).a_rarity));
                s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Inv";
                s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                index++;
            }
        }
    }

    public void updateInfo() {
        if(state == 0) {
            if(getSelectedWeapon() == null)
                return;

            var we = getSelectedWeapon();

            attributeText.text = "";
            if(attUpgrade != -1) {
                foreach(var i in we.w_attributes)
                    attributeText.text += i.ToString() + ", ";
                attributeText.text += "  +  " + ((Weapon.attribute)attUpgrade).ToString();
            }

            statText.text = "";
            if(statUpgrade > 1.0f) {
                statText.text = we.w_power.ToString("0.0") + " >>> " + (statUpgrade * we.w_power).ToString("0.0");
            }
        }

        else if(state == 1) {
            if(getSelectedArmor() == null)
                return;

            var ar = getSelectedArmor();

            attributeText.text = "";
            if(attUpgrade != -1) {
                foreach(var i in ar.a_attributes)
                    attributeText.text += i.ToString() + ", ";
                attributeText.text += "  +  " + ((Armor.attribute)attUpgrade).ToString();
            }

            statText.text = "";
            if(statUpgrade > 1.0f) {
                statText.text = ar.a_defence.ToString("0.0") + " >>> " + (statUpgrade * ar.a_defence).ToString("0.0");
            }
        }
    }


    public void upgrade() {
        if(state == 0) {
            if(getSelectedWeapon() == null)
                return;

            var we = getSelectedWeapon();
            if(attUpgrade != -1)
                we.w_attributes.Add((Weapon.attribute)attUpgrade);

            if(statUpgrade > 1.0f)
                we.w_power *= statUpgrade;

            shrine.GetComponent<SpriteRenderer>().color = Color.grey;

            //  Party Equipment
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty()) {
                    if(we.isEqualTo(Party.getMemberStats(i).equippedWeapon)) {
                        var stats = Party.getMemberStats(i);
                        stats.equippedWeapon.setEqualTo(we, false);
                        Party.overrideUnit(stats);

                        hasBeenUsed = true;
                        FindObjectOfType<RoomMovement>().deinteract();
                        hide();
                        return;
                    }
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                if(we.isEqualTo(Inventory.getWeapon(i))) {
                    Inventory.overrideWeapon(i, we);

                    hasBeenUsed = true;
                    FindObjectOfType<RoomMovement>().deinteract();
                    hide();
                    return;
                }
            }
        }

        else if(state == 1) {
            if(getSelectedArmor() == null)
                return;

            var ar = getSelectedArmor();
            if(attUpgrade != -1)
                ar.a_attributes.Add((Armor.attribute)attUpgrade);

            if(statUpgrade > 1.0f)
                ar.a_defence *= statUpgrade;

            shrine.GetComponent<SpriteRenderer>().color = Color.grey;

            //  Party Equipment
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty()) {
                    if(ar.isEqualTo(Party.getMemberStats(i).equippedArmor)) {
                        var stats = Party.getMemberStats(i);
                        stats.equippedArmor.setEqualTo(ar, false);
                        Party.overrideUnit(stats);

                        hasBeenUsed = true;
                        FindObjectOfType<RoomMovement>().deinteract();
                        hide();
                        return;
                    }
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                if(ar.isEqualTo(Inventory.getArmor(i))) {
                    Inventory.overrideArmor(i, ar);

                    hasBeenUsed = true;
                    FindObjectOfType<RoomMovement>().deinteract();
                    hide();
                    return;
                }
            }
        }
    }
}
