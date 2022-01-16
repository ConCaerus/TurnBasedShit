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
        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            if(Party.getHolder().getObject<UnitStats>(i).weapon != null && !Party.getHolder().getObject<UnitStats>(i).weapon.isEmpty()) {
                if(index == 0)
                    return Party.getHolder().getObject<UnitStats>(i).weapon;
                index--;
            }
        }

        //  Inventory Equipment
        for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
            if(index == 0)
                return Inventory.getHolder().getObject<Weapon>(i);
            index--;
        }

        return null;
    }

    Armor getSelectedArmor() {
        int index = slot.getSelectedSlotIndex();

        //  Party Equipment
        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            if(Party.getHolder().getObject<UnitStats>(i).armor != null && !Party.getHolder().getObject<UnitStats>(i).armor.isEmpty()) {
                if(index == 0)
                    return Party.getHolder().getObject<UnitStats>(i).armor;
                index--;
            }
        }

        //  Inventory Equipment
        for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
            if(index == 0)
                return Inventory.getHolder().getObject<Armor>(i);
            index--;
        }

        return null;
    }


    void createSlots() {
        if(state == 0) {
            //  Party Equipment
            int index = 0;
            for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                if(Party.getHolder().getObject<UnitStats>(i).weapon != null && !Party.getHolder().getObject<UnitStats>(i).weapon.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getHolder().getObject<UnitStats>(i).weapon.rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getHolder().getObject<UnitStats>(i).u_name + "'s Weapon";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getHolder().getObject<UnitStats>(i).weapon).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getObject<Weapon>(i).rarity));
                s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Inv";
                s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getHolder().getObject<Weapon>(i)).sprite;
                index++;
            }
        }

        else if(state == 1) {
            //  Party Equipment
            int index = 0;
            for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                if(Party.getHolder().getObject<UnitStats>(i).armor != null && !Party.getHolder().getObject<UnitStats>(i).armor.isEmpty()) {
                    var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Party.getHolder().getObject<UnitStats>(i).armor.rarity));
                    s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Party.getHolder().getObject<UnitStats>(i).u_name + "'s Armor";
                    s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getHolder().getObject<UnitStats>(i).armor).sprite;
                    index++;
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
                var s = slot.createSlot(index, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getObject<Armor>(i).rarity));
                s.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Inv";
                s.transform.GetChild(1).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getHolder().getObject<Armor>(i)).sprite;
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
            for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                if(Party.getHolder().getObject<UnitStats>(i).weapon != null && !Party.getHolder().getObject<UnitStats>(i).weapon.isEmpty()) {
                    if(we.isTheSameInstanceAs(Party.getHolder().getObject<UnitStats>(i).weapon)) {
                        var stats = Party.getHolder().getObject<UnitStats>(i);
                        stats.weapon.setEqualTo(we, false);
                        Party.overrideUnitOfSameInstance(stats);

                        hasBeenUsed = true;
                        FindObjectOfType<RoomMovement>().deinteract();
                        hide();
                        return;
                    }
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
                if(we.isTheSameInstanceAs(Inventory.getHolder().getObject<Weapon>(i))) {
                    Inventory.overrideCollectable(i, we);

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
            for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
                if(Party.getHolder().getObject<UnitStats>(i).armor != null && !Party.getHolder().getObject<UnitStats>(i).armor.isEmpty()) {
                    if(ar.isTheSameInstanceAs(Party.getHolder().getObject<UnitStats>(i).armor)) {
                        var stats = Party.getHolder().getObject<UnitStats>(i);
                        stats.armor.setEqualTo(ar, false);
                        Party.overrideUnitOfSameInstance(stats);

                        hasBeenUsed = true;
                        FindObjectOfType<RoomMovement>().deinteract();
                        hide();
                        return;
                    }
                }
            }

            //  Inventory Equipment
            for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
                if(ar.isTheSameInstanceAs(Inventory.getHolder().getObject<Armor>(i))) {
                    Inventory.overrideCollectable(i, ar);

                    hasBeenUsed = true;
                    FindObjectOfType<RoomMovement>().deinteract();
                    hide();
                    return;
                }
            }
        }
    }
}
