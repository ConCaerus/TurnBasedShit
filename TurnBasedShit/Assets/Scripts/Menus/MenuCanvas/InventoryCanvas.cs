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
    [SerializeField] TextMeshProUGUI nameText, itemNameText, flavorText, coinText;
    [SerializeField] SlotMenu slot;
    [SerializeField] AudioClip usable;
    float waitTime = .035f;

    UnitStats shownUnit;

    private void Start() {
        state = 4;
        shownUnit = Party.getHolder().getObject<UnitStats>(0);

        //populateSlots();
    }

    private void Update() {
        if(slot.run()) {
            updateInfo();
        }
    }

    public IEnumerator populateSlots() {
        slot.destroySlots();
        switch(state) {
            //  weapon
            case 0:
                for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
                    var obj = makeSlot(i, Inventory.getHolder().getObject<Weapon>(i), InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Weapon>(i)));
                    obj.GetComponent<SlotObject>().setText(0, "");
                    yield return new WaitForSeconds(waitTime);
                }
                slot.deleteSlotsAfterIndex(Inventory.getHolder().getObjectCount<Weapon>());
                break;

            //  armor
            case 1:
                for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
                    var obj = makeSlot(i, Inventory.getHolder().getObject<Armor>(i), InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Armor>(i)));
                    obj.GetComponent<SlotObject>().setText(0, "");
                    yield return new WaitForSeconds(waitTime);
                }
                slot.deleteSlotsAfterIndex(Inventory.getHolder().getObjectCount<Armor>());
                break;

            //  item
            case 2:
                for(int i = 0; i < Inventory.getHolder().getObjectCount<Item>(); i++) {
                    var obj = makeSlot(i, Inventory.getHolder().getObject<Item>(i), InfoTextCreator.createForCollectable(Inventory.getHolder().getObject<Item>(i)));
                    obj.GetComponent<SlotObject>().setText(0, "");
                    yield return new WaitForSeconds(waitTime);
                }
                slot.deleteSlotsAfterIndex(Inventory.getHolder().getObjectCount<Item>());
                break;

            //  usable
            case 3:
                slot.destroySlots();
                StartCoroutine(makeUsableSlots());
                break;

            //  unusable
            case 4:
                slot.destroySlots();
                StartCoroutine(makeUnusableSlots());
                break;
        }

        updateInfo();
    }



    void updateInfo() {
        coinText.text = Inventory.getCoinCount().ToString() + "c";

        if(shownUnit == null || shownUnit.isEmpty())
            shownUnit = Party.getHolder().getObject<UnitStats>(0);
        unitSlot.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitHeadSprite(shownUnit.u_sprite.headIndex);
        unitSlot.transform.GetChild(0).GetComponent<Image>().color = shownUnit.u_sprite.color;
        unitSlot.GetComponent<Image>().color = shownUnit.u_sprite.color * 2.0f;
        unitSlot.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        unitSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(shownUnit.u_sprite.faceIndex);
        unitSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();

        healthSlider.maxValue = shownUnit.getModifiedMaxHealth();
        healthSlider.value = shownUnit.u_health;

        nameText.text = shownUnit.u_name;
        if(getSelectedCollectable() != null) {
            itemNameText.text = getSelectedCollectable().name;
            flavorText.text = getSelectedCollectable().flavor;
        }
        else {
            itemNameText.text = "";
            flavorText.text = "";
        }

        if(slot.getSelectedSlot() == null)
            mainSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        else
            mainSlot.transform.GetChild(0).GetComponent<Image>().sprite = slot.getSelectedSlot().transform.GetChild(0).GetComponent<Image>().sprite;
    }


    IEnumerator makeUsableSlots() {
        var l = Inventory.getUniqueUsables();
        int slotIndex = 0;
        for(int i = 0; i < l.Count; i++) {
            var obj = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));
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
                        var temp = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));
                        if(l[i].maxStackCount > 1)
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                        slotIndex++;
                    }

                    count -= l[i].maxStackCount;
                }

                var catcher = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));
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
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator makeUnusableSlots() {
        var l = Inventory.getUniqueUnusables();
        int slotIndex = 0;
        for(int i = 0; i < l.Count; i++) {
            var obj = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));

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
                        var temp = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));
                        if(l[i].maxStackCount > 1)
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = l[i].maxStackCount.ToString();
                        else
                            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                        slotIndex++;
                    }

                    count -= l[i].maxStackCount;
                }

                var catcher = makeSlot(slotIndex, l[i], InfoTextCreator.createForCollectable(l[i]));
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
            yield return new WaitForSeconds(waitTime);
        }
    }

    GameObject makeSlot(int i, Collectable c, string info) {
        var obj = slot.createSlot(i, Color.white, info);
        obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(c));
        obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(c.rarity));

        return obj;
    }

    Collectable getSelectedCollectable() {
        if(slot.getSelectedSlotIndex() == -1)
            return null;

        if(state == 0)
            return Inventory.getHolder().getObject<Weapon>(slot.getSelectedSlotIndex());
        else if(state == 1)
            return Inventory.getHolder().getObject<Armor>(slot.getSelectedSlotIndex());
        else if(state == 2)
            return Inventory.getHolder().getObject<Item>(slot.getSelectedSlotIndex());
        else if(state == 3)
            return Inventory.getFirstMatchingUsable(FindObjectOfType<PresetLibrary>().getUsableFromSprite(mainSlot.transform.GetChild(0).GetComponent<Image>().sprite));
        else if(state == 4)
            return Inventory.getFirstMatchingUnusable(FindObjectOfType<PresetLibrary>().getUnusableFromSprite(mainSlot.transform.GetChild(0).GetComponent<Image>().sprite));

        return null;
    }



    //  button shit
    public void setState(int i) {
        if(i == state)
            return;
        state = i;
        StartCoroutine(populateSlots());
        slot.setSelectedSlotIndex(-1);
        updateInfo();
    }

    public void cycleUnit(bool right) {
        int unitIndex = Party.getHolder().getUnitStatsIndex(shownUnit);

        //  right
        if(right) {
            unitIndex++;
        }
        //  left
        else {
            unitIndex--;
        }

        if(unitIndex >= Party.getHolder().getObjectCount<UnitStats>())
            unitIndex = 0;
        else if(unitIndex < 0)
            unitIndex = Party.getHolder().getObjectCount<UnitStats>() - 1;

        shownUnit = Party.getHolder().getObject<UnitStats>(unitIndex);
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
        else if(obj.type == Collectable.collectableType.usable && ((Usable)obj).applyStatsEffect(shownUnit, FindObjectOfType<PartyObject>())) {
            Inventory.removeCollectable((Usable)obj);
            FindObjectOfType<AudioManager>().playSound(usable);
            shownUnit = Party.getHolder().getObject<UnitStats>(Party.getHolder().getUnitStatsIndex(shownUnit));
        }

        StartCoroutine(populateSlots());
    }

    void swapWeapon(Weapon w) {
        var uWeapon = new Weapon();
        uWeapon.setEqualTo(shownUnit.weapon, true);
        shownUnit.weapon = new Weapon();
        shownUnit.weapon.setEqualTo(w, true);
        Inventory.removeCollectable(w);

        if(uWeapon != null && !uWeapon.isEmpty())
            Inventory.addCollectable(uWeapon, FindObjectOfType<PresetLibrary>());

        Party.overrideUnitOfSameInstance(shownUnit);
    }
    void swapArmor(Armor a) {
        var uArmor = new Armor();
        uArmor.setEqualTo(shownUnit.armor, true);
        shownUnit.armor = new Armor();
        shownUnit.armor.setEqualTo(a, true);
        Inventory.removeCollectable(a);

        if(uArmor != null && !uArmor.isEmpty())
            Inventory.addCollectable(uArmor, FindObjectOfType<PresetLibrary>());

        Party.overrideUnitOfSameInstance(shownUnit);
    }
    void swapItem(Item i) {
        var uItem = new Item();
        uItem.setEqualTo(shownUnit.item, true);
        shownUnit.item = new Item();
        shownUnit.item.setEqualTo(i, true);
        Inventory.removeCollectable(i);

        if(uItem != null && !uItem.isEmpty())
            Inventory.addCollectable(uItem, FindObjectOfType<PresetLibrary>());

        Party.overrideUnitOfSameInstance(shownUnit);
    }
}
