using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryCanvas : MonoBehaviour {
    public int state = 0;
    [SerializeField] GameObject mainSlot, unitSlot;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI nameText, itemNameText;
    [SerializeField] SlotMenu slot;

    UnitStats shownUnit;

    private void Start() {
        slot.init();
        state = 4;
        shownUnit = Party.getMemberStats(0);

        //populateSlots();
    }

    private void Update() {
        if(slot.run()) {
            updateInfo();
            if(getSelectedCollectable() != null)
                itemNameText.text = getSelectedCollectable().name;
            else
                itemNameText.text = "";
        }
    }

    public void populateSlots() {
        //slot.destroySlots();
        switch(state) {
            //  weapon
            case 0:
                for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                    var obj = makeSlot(i, FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite, InfoTextCreator.createForCollectable(Inventory.getWeapon(i)));
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                slot.deleteSlotsAfterIndex(Inventory.getWeaponCount());
                break;

            //  armor
            case 1:
                for(int i = 0; i < Inventory.getArmorCount(); i++) {
                    var obj = makeSlot(i, FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite, InfoTextCreator.createForCollectable(Inventory.getArmor(i)));
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                slot.deleteSlotsAfterIndex(Inventory.getArmorCount());
                break;

            //  item
            case 2:
                for(int i = 0; i < Inventory.getItemCount(); i++) {
                    var obj = makeSlot(i, FindObjectOfType<PresetLibrary>().getItemSprite(Inventory.getItem(i)).sprite, InfoTextCreator.createForCollectable(Inventory.getItem(i)));
                    obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                slot.deleteSlotsAfterIndex(Inventory.getItemCount());
                break;

            //  usable
            case 3:
                slot.destroySlots();
                makeUsableSlots();
                break;

            //  unusable
            case 4:
                slot.destroySlots();
                makeUnusableSlots();
                break;
        }

        updateInfo();
    }



    void updateInfo() {
        if(shownUnit == null || shownUnit.isEmpty())
            shownUnit = Party.getMemberStats(0);
        unitSlot.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(shownUnit.u_sprite.headIndex);
        unitSlot.transform.GetChild(0).GetComponent<Image>().color = shownUnit.u_sprite.color;
        unitSlot.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        unitSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(shownUnit.u_sprite.faceIndex);
        unitSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();

        healthSlider.maxValue = shownUnit.getModifiedMaxHealth();
        healthSlider.value = shownUnit.u_health;

        nameText.text = shownUnit.u_name;
        if(getSelectedCollectable() != null)
            itemNameText.text = getSelectedCollectable().name;
        else
            itemNameText.text = "";

        if(slot.getSelectedSlot() == null)
            mainSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        else
            mainSlot.transform.GetChild(0).GetComponent<Image>().sprite = slot.getSelectedSlot().transform.GetChild(0).GetComponent<Image>().sprite;
    }


    int makeUsableSlots() {
        var l = Inventory.getUniqueUsables();
        int numberOfSlots = 0;
        int slotIndex = 0;
        for(int i = 0; i < l.Count; i++) {
            var obj = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUsableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            slotIndex++;

            int count = Inventory.getNumberOfMatchingUsables(l[i]);
            if(count > l[i].maxStackCount) {
                while(count > l[i].maxStackCount) {
                    if(count == Inventory.getNumberOfMatchingUsables(l[i])) {
                        if(l[i].maxStackCount > 1)
                            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    }
                    else {
                        var temp = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUsableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));
                        if(l[i].maxStackCount > 1)
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                        slotIndex++;
                        numberOfSlots++;
                    }

                    count -= l[i].maxStackCount;
                }

                var catcher = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUsableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));
                if(count > 1)
                    catcher.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
                else
                    catcher.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                slotIndex++;
            }
            else if(count > 1)
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
            else
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            numberOfSlots++;
        }
        return numberOfSlots;
    }
    int makeUnusableSlots() {
        var l = Inventory.getUniqueUnusables();
        int numberOfSlots = 0;
        int slotIndex = 0;
        for(int i = 0; i < l.Count; i++) {
            var obj = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUnusableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));

            slotIndex++;

            int count = Inventory.getNumberOfMatchingUnusables(l[i]);
            if(count > l[i].maxStackCount) {
                while(count > l[i].maxStackCount) {
                    if(count == Inventory.getNumberOfMatchingUnusables(l[i])) {
                        if(l[i].maxStackCount > 1)
                            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    }
                    else {
                        var temp = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUnusableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));
                        if(l[i].maxStackCount > 1)
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                        slotIndex++;
                        numberOfSlots++;
                    }

                    count -= l[i].maxStackCount;
                }

                var catcher = makeSlot(slotIndex, FindObjectOfType<PresetLibrary>().getUnusableSprite(l[i]).sprite, InfoTextCreator.createForCollectable(l[i]));
                if(count > 1)
                    catcher.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
                else
                    catcher.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                slotIndex++;
            }
            else if(count > 1)
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
            else
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            numberOfSlots++;
        }

        return numberOfSlots;
    }

    GameObject makeSlot(int i, Sprite sp, string info) {
        var obj = slot.createSlot(i, Color.white, info);
        obj.transform.GetChild(0).GetComponent<Image>().sprite = sp;

        return obj;
    }

    Collectable getSelectedCollectable() {
        if(slot.getSelectedSlotIndex() == -1)
            return null;

        if(state == 0)
            return Inventory.getWeapon(slot.getSelectedSlotIndex());
        else if(state == 1)
            return Inventory.getArmor(slot.getSelectedSlotIndex());
        else if(state == 2)
            return Inventory.getItem(slot.getSelectedSlotIndex());
        else if(state == 3)
            return FindObjectOfType<PresetLibrary>().getUsableFromSprite(mainSlot.transform.GetChild(0).GetComponent<Image>().sprite);
        else if(state == 4)
            return FindObjectOfType<PresetLibrary>().getUnusableFromSprite(mainSlot.transform.GetChild(0).GetComponent<Image>().sprite);

        return null;
    }



    //  button shit
    public void setState(int i) {
        if(i == state)
            return;
        state = i;
        populateSlots();
        slot.setSelectedSlotIndex(-1);
        updateInfo();
    }

    public void cycleUnit(bool right) {
        int unitIndex = Party.getUnitIndex(shownUnit);

        //  right
        if(right) {
            unitIndex++;
        }
        //  left
        else {
            unitIndex--;
        }

        if(unitIndex >= Party.getMemberCount())
            unitIndex = 0;
        else if(unitIndex < 0)
            unitIndex = Party.getMemberCount() - 1;

        shownUnit = Party.getMemberStats(unitIndex);
        updateInfo();
    }

    public void use() {
        var obj = getSelectedCollectable();
        if(obj == null || obj.type == Collectable.collectableType.unusable)
            return;

        if(obj.type == Collectable.collectableType.weapon)
            swapWeapon((Weapon)obj);
        else if(obj.type == Collectable.collectableType.armor)
            swapArmor((Armor)obj);
        else if(obj.type == Collectable.collectableType.item)
            swapItem((Item)obj);
        else if(obj.type == Collectable.collectableType.usable && ((Usable)obj).applyStatsEffect(shownUnit))
            Inventory.removeUsable((Usable)obj);

        populateSlots();
    }

    void swapWeapon(Weapon w) {
        var uWeapon = new Weapon();
        uWeapon.setEqualTo(shownUnit.equippedWeapon, true);
        shownUnit.equippedWeapon = new Weapon();
        shownUnit.equippedWeapon.setEqualTo(w, true);
        Inventory.removeWeapon(w);

        if(uWeapon != null && !uWeapon.isEmpty())
            Inventory.addWeapon(uWeapon);

        Party.overrideUnit(shownUnit);
    }
    void swapArmor(Armor a) {
        var uArmor = new Armor();
        uArmor.setEqualTo(shownUnit.equippedArmor, true);
        shownUnit.equippedArmor = new Armor();
        shownUnit.equippedArmor.setEqualTo(a, true);
        Inventory.removeArmor(a);

        if(uArmor != null && !uArmor.isEmpty())
            Inventory.addArmor(uArmor);

        Party.overrideUnit(shownUnit);
    }
    void swapItem(Item i) {
        var uItem = new Item();
        uItem.setEqualTo(shownUnit.equippedItem, true);
        shownUnit.equippedItem = new Item();
        shownUnit.equippedItem.setEqualTo(i, true);
        Inventory.removeItem(i);

        if(uItem != null && !uItem.isEmpty())
            Inventory.addItem(uItem);

        Party.overrideUnit(shownUnit);
    }
}
