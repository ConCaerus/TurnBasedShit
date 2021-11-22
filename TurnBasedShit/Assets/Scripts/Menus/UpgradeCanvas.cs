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

        transform.GetChild(0).transform.localScale = new Vector3(0.0f, 0.0f);

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
        createSlots();
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
            if(Party.getMemberStats(i).weapon != null && !Party.getMemberStats(i).weapon.isEmpty()) {
                if(index == 0)
                    return Party.getMemberStats(i).weapon;
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
            if(Party.getMemberStats(i).armor != null && !Party.getMemberStats(i).armor.isEmpty()) {
                if(index == 0)
                    return Party.getMemberStats(i).armor;
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
                if(Party.getMemberStats(i).weapon != null && !Party.getMemberStats(i).weapon.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getMemberStats(i).weapon.rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getMemberStats(i).u_name + "'s Weapon";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getMemberStats(i).weapon).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getWeapon(i).rarity));
                s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Inv";
                s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                index++;
            }
        }

        else if(state == 1) {
            //  Party Equipment
            int index = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).armor != null && !Party.getMemberStats(i).armor.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getMemberStats(i).armor.rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getMemberStats(i).u_name + "'s Armor";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getMemberStats(i).armor).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getArmor(i).rarity));
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
                foreach(var i in we.attributes)
                    attributeText.text += i.ToString() + ", ";
                attributeText.text += "  +  " + ((Weapon.attribute)attUpgrade).ToString();
            }

            statText.text = "";
            if(statUpgrade > 1.0f) {
                statText.text = we.power.ToString("0.0") + " >>> " + (statUpgrade * we.power).ToString("0.0");
            }
        }

        else if(state == 1) {
            if(getSelectedArmor() == null)
                return;

            var ar = getSelectedArmor();

            attributeText.text = "";
            if(attUpgrade != -1) {
                foreach(var i in ar.attributes)
                    attributeText.text += i.ToString() + ", ";
                attributeText.text += "  +  " + ((Armor.attribute)attUpgrade).ToString();
            }

            statText.text = "";
            if(statUpgrade > 1.0f) {
                statText.text = ar.defence.ToString("0.0") + " >>> " + (statUpgrade * ar.defence).ToString("0.0");
            }
        }
    }


    public void upgrade() {
        if(state == 0) {
            if(getSelectedWeapon() == null)
                return;

            var we = getSelectedWeapon();
            if(attUpgrade != -1)
                we.attributes.Add((Weapon.attribute)attUpgrade);

            if(statUpgrade > 1.0f)
                we.power *= statUpgrade;

            shrine.GetComponent<SpriteRenderer>().color = Color.grey;

            //  Party Equipment
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).weapon != null && !Party.getMemberStats(i).weapon.isEmpty()) {
                    if(we.isTheSameInstanceAs(Party.getMemberStats(i).weapon)) {
                        var stats = Party.getMemberStats(i);
                        stats.weapon.setEqualTo(we, false);
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
                if(we.isTheSameInstanceAs(Inventory.getWeapon(i))) {
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
                ar.attributes.Add((Armor.attribute)attUpgrade);

            if(statUpgrade > 1.0f)
                ar.defence *= statUpgrade;

            shrine.GetComponent<SpriteRenderer>().color = Color.grey;

            //  Party Equipment
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).armor != null && !Party.getMemberStats(i).armor.isEmpty()) {
                    if(ar.isTheSameInstanceAs(Party.getMemberStats(i).armor)) {
                        var stats = Party.getMemberStats(i);
                        stats.armor.setEqualTo(ar, false);
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
                if(ar.isTheSameInstanceAs(Inventory.getArmor(i))) {
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
