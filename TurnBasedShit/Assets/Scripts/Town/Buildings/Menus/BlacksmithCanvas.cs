using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BlacksmithCanvas : MonoBehaviour {
    public bool isShowing = false;

    [SerializeField] SlotMenu inventoryWeapons, inventoryArmor, partyWeapons, partyArmor;

    [SerializeField] GameObject selectedEquipment;

    [SerializeField] TextMeshProUGUI nameText, costText, coinText;

    BlacksmithBuilding reference;


    private void Start() {
        GameInfo.setCurrentLocationAsTown(MapLocationHolder.getRandomTownLocationWithBuilding(Building.type.Blacksmith));
        reference = GameInfo.getCurrentLocationAsTown().town.getBlacksmith();
        transform.GetChild(0).transform.localScale = new Vector3(0.0f, 0.0f);

        updateInfo();
    }

    private void Update() {
        if(inventoryWeapons.run())
            updateInfo();
        if(inventoryArmor.run())
            updateInfo();
        if(partyWeapons.run())
            updateInfo();
        if(partyArmor.run())
            updateInfo();
    }

    void updateSlots() {
        inventoryWeapons.destroySlots();
        inventoryArmor.destroySlots();
        partyWeapons.destroySlots();
        partyArmor.destroySlots();

        int slotIndex = 0;
        for(int i = 0; i < Inventory.getWeaponCount(); i++) {
            if(Inventory.getWeapon(i).wornAmount != GameInfo.wornState.perfect) {
                var temp = inventoryWeapons.createSlot(slotIndex, Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.wornAmount != GameInfo.wornState.perfect) {
                var temp = partyWeapons.createSlot(slotIndex, Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getMemberStats(i).equippedWeapon).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Inventory.getArmorCount(); i++) {
            if(Inventory.getArmor(i).wornAmount != GameInfo.wornState.perfect) {
                var temp = inventoryArmor.createSlot(slotIndex, Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.wornAmount != GameInfo.wornState.perfect) {
                var temp = partyArmor.createSlot(slotIndex, Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getMemberStats(i).equippedArmor).sprite;
                slotIndex++;
            }
        }

        updateInfo();
    }

    public void updateInfo() {
        coinText.text = Inventory.getCoinCount().ToString() + "c";
        if(getCurrentlyUsingMenu() == null || getCurrentlyUsingMenu().getSelectedSlotIndex() == -1) {
            resetInfo();
            return;
        }

        if(getCurrentlyUsingMenu() == inventoryWeapons) {
            int slotIndex = 0;
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                if(Inventory.getWeapon(i).wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        setWeaponInfo(Inventory.getWeapon(i));
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == partyWeapons) {
            int slotIndex = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        setWeaponInfo(Party.getMemberStats(i).equippedWeapon);
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == inventoryArmor) {
            int slotIndex = 0;
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                if(Inventory.getArmor(i).wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        setArmorInfo(Inventory.getArmor(i));
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == partyArmor) {
            int slotIndex = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        setArmorInfo(Party.getMemberStats(i).equippedArmor);
                        return;
                    }
                    slotIndex++;
                }
            }
        }
    }

    void resetInfo() {
        selectedEquipment.transform.GetChild(0).GetComponent<Image>().sprite = null;
        nameText.text = "";
        costText.text = "0c";
    }

    void setWeaponInfo(Weapon we) {
        selectedEquipment.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(we).sprite;
        nameText.text = we.name;
        costText.text = getCost(we.rarity, we.wornAmount).ToString("0") + "c";
    }
    void setArmorInfo(Armor ar) {
        selectedEquipment.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(ar).sprite;
        nameText.text = ar.name;
        costText.text = getCost(ar.rarity, ar.wornAmount).ToString("0") + "c";
    }

    int getCost(GameInfo.region rar, GameInfo.wornState state) {
        return ((((int)rar) + 1) * reference.chargeRate) * ((int)GameInfo.wornState.perfect - (int)state);
    }


    public void show() {
        transform.GetChild(0).DOScale(1.0f, 0.15f);
        updateSlots();
        isShowing = true;
    }
    public void hide(bool deinteractWalker) {
        transform.GetChild(0).DOScale(0.0f, 0.15f);
        isShowing = false;
        if(deinteractWalker)
            FindObjectOfType<LocationMovement>().deinteract();
    }

    SlotMenu getCurrentlyUsingMenu() {
        if(inventoryWeapons.getSelectedSlotIndex() != -1)
            return inventoryWeapons;
        if(inventoryArmor.getSelectedSlotIndex() != -1)
            return inventoryArmor;
        if(partyWeapons.getSelectedSlotIndex() != -1)
            return partyWeapons;
        if(partyArmor.getSelectedSlotIndex() != -1)
            return partyArmor;
        return null;
    }

    public void repairEquipment() {
        if(getCurrentlyUsingMenu() == null || getCurrentlyUsingMenu().getSelectedSlotIndex() == -1) {
            return;
        }

        if(getCurrentlyUsingMenu() == inventoryWeapons) {
            int slotIndex = 0;
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                if(Inventory.getWeapon(i).wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Inventory.getWeapon(i);
                        if(getCost(temp.rarity, temp.wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.rarity, temp.wornAmount));

                        temp.wornAmount = GameInfo.wornState.perfect;
                        Inventory.overrideWeapon(i, temp);

                        updateSlots();
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == partyWeapons) {
            int slotIndex = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Party.getMemberStats(i).equippedWeapon;
                        if(getCost(temp.rarity, temp.wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.rarity, temp.wornAmount));

                        temp.wornAmount = GameInfo.wornState.perfect;
                        var unit = Party.getMemberStats(i);
                        unit.equippedWeapon.setEqualTo(temp, true);
                        Party.overrideUnit(unit);

                        updateSlots();
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == inventoryArmor) {
            int slotIndex = 0;
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                if(Inventory.getArmor(i).wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Inventory.getArmor(i);
                        if(getCost(temp.rarity, temp.wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.rarity, temp.wornAmount));

                        temp.wornAmount = GameInfo.wornState.perfect;
                        Inventory.overrideArmor(i, temp);

                        updateSlots();
                        return;
                    }
                    slotIndex++;
                }
            }
        }

        else if(getCurrentlyUsingMenu() == partyArmor) {
            int slotIndex = 0;
            for(int i = 0; i < Party.getMemberCount(); i++) {
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.wornAmount != GameInfo.wornState.perfect) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Party.getMemberStats(i).equippedArmor;
                        if(getCost(temp.rarity, temp.wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.rarity, temp.wornAmount));

                        temp.wornAmount = GameInfo.wornState.perfect;
                        var unit = Party.getMemberStats(i);
                        unit.equippedArmor.setEqualTo(temp, true);
                        Party.overrideUnit(unit);

                        updateSlots();
                        return;
                    }
                    slotIndex++;
                }
            }
        }
    }
}
