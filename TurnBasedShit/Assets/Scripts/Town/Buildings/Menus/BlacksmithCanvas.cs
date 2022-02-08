using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BlacksmithCanvas : MonoBehaviour {
    public bool isShowing = false;

    [SerializeField] SlotMenu slot;

    [SerializeField] GameObject mainSlot;

    [SerializeField] TextMeshProUGUI nameText, costText, wornText;
    [SerializeField] CoinCount coinCount;

    BlacksmithBuilding reference;


    private void Start() {
        GameInfo.setCurrentLocationAsTown(MapLocationHolder.getRandomTownLocationWithBuilding(Building.type.Blacksmith));
        reference = GameInfo.getCurrentLocationAsTown().town.holder.getObject<BlacksmithBuilding>(0);
        transform.GetChild(0).transform.localScale = new Vector3(0.0f, 0.0f);
        coinCount.updateCount(false);

        for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
            var obj = Inventory.getHolder().getObject<Weapon>(i);
            obj.wornAmount = GameInfo.wornState.Old;
            Inventory.overrideCollectable(i, obj);
        }
    }

    private void Update() {
        if(slot.run())
            updateInfo();
    }

    void populateSlots() {
        slot.destroySlots();

        int slotIndex = 0;
        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            if(Party.getHolder().getObject<UnitStats>(i).weapon != null && !Party.getHolder().getObject<UnitStats>(i).weapon.isEmpty()) {
                var temp = slot.createSlot(slotIndex, Color.white);
                temp.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getHolder().getObject<UnitStats>(i).weapon).sprite);
                temp.GetComponent<SlotObject>().setImageColor(0, Party.getHolder().getObject<UnitStats>(i).u_sprite.color);
                temp.GetComponent<SlotObject>().setInfo(Party.getHolder().getObject<UnitStats>(i).u_name + "'s Weapon");
                if(Party.getHolder().getObject<UnitStats>(i).weapon.wornAmount == GameInfo.wornState.Perfect)
                    temp.GetComponent<SlotObject>().setInteractibility(false);
                slotIndex++;
            }
        }

        for(int i = 0; i < Party.getHolder().getObjectCount<UnitStats>(); i++) {
            if(Party.getHolder().getObject<UnitStats>(i).armor != null && !Party.getHolder().getObject<UnitStats>(i).armor.isEmpty()) {
                var temp = slot.createSlot(slotIndex, Color.white);
                temp.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getHolder().getObject<UnitStats>(i).armor).sprite);
                temp.GetComponent<SlotObject>().setImageColor(0, Party.getHolder().getObject<UnitStats>(i).u_sprite.color);
                temp.GetComponent<SlotObject>().setInfo(Party.getHolder().getObject<UnitStats>(i).u_name + "'s Armor");
                if(Party.getHolder().getObject<UnitStats>(i).armor.wornAmount == GameInfo.wornState.Perfect)
                    temp.GetComponent<SlotObject>().setInteractibility(false);
                slotIndex++;
            }
        }

        for(int i = 0; i < Inventory.getHolder().getObjectCount<Weapon>(); i++) {
            var temp = slot.createSlot(slotIndex, Color.white);
            temp.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getHolder().getObject<Weapon>(i)).sprite);
            temp.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getObject<Weapon>(i).rarity));
            temp.GetComponent<SlotObject>().setInfo(Inventory.getHolder().getObject<Weapon>(i).name);
            if(Inventory.getHolder().getObject<Weapon>(i).wornAmount == GameInfo.wornState.Perfect)
                temp.GetComponent<SlotObject>().setInteractibility(false);
            slotIndex++;
        }

        for(int i = 0; i < Inventory.getHolder().getObjectCount<Armor>(); i++) {
            var temp = slot.createSlot(slotIndex, Color.white);
            temp.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getHolder().getObject<Armor>(i)).sprite);
            temp.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(Inventory.getHolder().getObject<Armor>(i).rarity));
            temp.GetComponent<SlotObject>().setInfo(Inventory.getHolder().getObject<Armor>(i).name);
            if(Inventory.getHolder().getObject<Armor>(i).wornAmount == GameInfo.wornState.Perfect)
                temp.GetComponent<SlotObject>().setInteractibility(false);
            slotIndex++;
        }
    }

    public void updateInfo() {
        resetInfo();
        if(slot.getSelectedSlotIndex() < 0)
            return;
        var col = getSelectedCollectable();
        if(col == null)
            return;

        mainSlot.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(col));
        mainSlot.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(col.rarity));
        mainSlot.GetComponent<SlotObject>().setInfo(col.name);
        nameText.text = col.name;
        costText.text = col.type == Collectable.collectableType.Weapon ? costText.text = getCost(col.rarity, ((Weapon)col).wornAmount).ToString("0") : getCost(col.rarity, ((Armor)col).wornAmount).ToString("0");
        costText.text += "c";
        wornText.text = col.type == Collectable.collectableType.Weapon ? ((Weapon)col).wornAmount.ToString() : ((Armor)col).wornAmount.ToString();
    }


    Collectable getSelectedCollectable() {
        int index = slot.getSelectedSlotIndex();
        if(index < 0)
            return null;
        if(index >= Party.getWeaponCountInParty())
            index -= Party.getWeaponCountInParty();
        else
            return Party.getWeaponInParty(index);
        if(index >= Party.getArmorCountInParty())
            index -= Party.getArmorCountInParty();
        else
            return Party.getArmorInParty(index);
        if(index >= Inventory.getHolder().getObjectCount<Weapon>())
            index -= Inventory.getHolder().getObjectCount<Weapon>();
        else
            return Inventory.getHolder().getObject<Weapon>(index);
        if(index < Inventory.getHolder().getObjectCount<Armor>())
            return Inventory.getHolder().getObject<Armor>(index);
        return null;
    }

    void resetInfo() {
        mainSlot.GetComponent<SlotObject>().setImage(1, null);
        mainSlot.GetComponent<SlotObject>().setImageColor(0, Color.gray);
        mainSlot.GetComponent<SlotObject>().setInfo("");
        nameText.text = "";
        costText.text = "0c";
    }

    int getCost(GameInfo.region rar, GameInfo.wornState state) {
        return ((((int)rar) + 1) * reference.chargeRate) * ((int)GameInfo.wornState.Perfect - (int)state);
    }


    public void show() {
        transform.GetChild(0).DOScale(1.0f, 0.15f);
        populateSlots();
        isShowing = true;
    }
    public void hide(bool deinteractWalker) {
        transform.GetChild(0).DOScale(0.0f, 0.15f);
        isShowing = false;
        if(deinteractWalker)
            FindObjectOfType<LocationMovement>().deinteract();
    }

    public void repairEquipment() {
        //  find the right piece of equipment and run this code
        var col = getSelectedCollectable();

        //  party shit
        foreach(var i in Party.getHolder().getObjects<UnitStats>()) {
            if(col.type == Collectable.collectableType.Weapon) {
                if(i.weapon != null && !i.weapon.isEmpty() && i.weapon.isTheSameInstanceAs(col)) {
                    if(getCost(col.rarity, i.weapon.wornAmount) > Inventory.getCoinCount())
                        return;
                    Inventory.addCoins(-getCost(col.rarity, i.weapon.wornAmount), coinCount, true);
                    var temp = i.weapon;
                    temp.wornAmount = GameInfo.wornState.Perfect;
                    Party.overrideUnitOfSameInstance(i);
                    populateSlots();
                    return;
                }
            }
            else if(col.type == Collectable.collectableType.Armor) {
                if(i.armor != null && !i.armor.isEmpty() && i.armor.isTheSameInstanceAs(col)) {
                    if(getCost(col.rarity, i.armor.wornAmount) > Inventory.getCoinCount())
                        return;
                    Inventory.addCoins(-getCost(col.rarity, i.armor.wornAmount), coinCount, true);
                    var temp = i.armor;
                    temp.wornAmount = GameInfo.wornState.Perfect;
                    Party.overrideUnitOfSameInstance(i);
                    populateSlots();
                    return;
                }
            }
        }

        //  Inventory shit
        if(!Inventory.hasCollectable(col))
            return;
        var index = Inventory.getHolder().getCollectableIndex(col);
        if(col.type == Collectable.collectableType.Weapon) {
            if(getCost(col.rarity, ((Weapon)col).wornAmount) > Inventory.getCoinCount())
                return;
            Inventory.addCoins(-getCost(col.rarity, ((Weapon)col).wornAmount), coinCount, true);
            var temp = (Weapon)col;
            temp.wornAmount = GameInfo.wornState.Perfect;
        }
        else if(col.type == Collectable.collectableType.Armor) {
            if(getCost(col.rarity, ((Armor)col).wornAmount) > Inventory.getCoinCount())
                return;
            Inventory.addCoins(-getCost(col.rarity, ((Armor)col).wornAmount), coinCount, true);
            var temp = (Armor)col;
            temp.wornAmount = GameInfo.wornState.Perfect;
        }

        //  NOTE: this doesn't run if the collectable is in the party because of the returns
        Inventory.overrideCollectable(index, col);
        populateSlots();
    }
}
