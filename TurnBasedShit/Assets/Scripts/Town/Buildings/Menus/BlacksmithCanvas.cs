using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BlacksmithCanvas : MonoBehaviour {
    public bool isShowing = false;

    [SerializeField] SlotMenu inventoryWeapons, inventoryArmor, partyWeapons, partyArmor;
    SlotMenu runningMenu = null;
    [SerializeField] GameObject slotPreset;

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
        if(runningMenu != null) {
            if(runningMenu.run()) {
                if(runningMenu.getSelectedSlot() != null) {
                    if(runningMenu != inventoryWeapons)
                        inventoryWeapons.setSelectedSlotIndex(-1);
                    if(runningMenu != inventoryArmor)
                        inventoryArmor.setSelectedSlotIndex(-1);
                    if(runningMenu != partyWeapons)
                        partyWeapons.setSelectedSlotIndex(-1);
                    if(runningMenu != partyArmor)
                        partyArmor.setSelectedSlotIndex(-1);
                    updateInfo();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                var temp = Inventory.getWeapon(i);
                temp.w_wornAmount--;
                Inventory.overrideWeapon(i, temp);
            }
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                var temp = Inventory.getArmor(i);
                temp.a_wornAmount--;
                Inventory.overrideArmor(i, temp);
            }
            updateSlots();
        }
    }

    public void setCurrentSlotMenu(SlotMenu men) {
        runningMenu = men;
    }

    void updateSlots() {
        inventoryWeapons.destroySlots();
        inventoryArmor.destroySlots();
        partyWeapons.destroySlots();
        partyArmor.destroySlots();

        int slotIndex = 0;
        for(int i = 0; i < Inventory.getWeaponCount(); i++) {
            if(Inventory.getWeapon(i).w_wornAmount != GameInfo.wornState.New) {
                var temp = inventoryWeapons.createNewSlot(slotIndex, slotPreset.gameObject, inventoryWeapons.transform.GetChild(0), Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Inventory.getWeapon(i)).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.w_wornAmount != GameInfo.wornState.New) {
                var temp = partyWeapons.createNewSlot(slotIndex, slotPreset.gameObject, partyWeapons.transform.GetChild(0), Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(Party.getMemberStats(i).equippedWeapon).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Inventory.getArmorCount(); i++) {
            if(Inventory.getArmor(i).a_wornAmount != GameInfo.wornState.New) {
                var temp = inventoryArmor.createNewSlot(slotIndex, slotPreset.gameObject, inventoryArmor.transform.GetChild(0), Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Inventory.getArmor(i)).sprite;
                slotIndex++;
            }
        }

        slotIndex = 0;
        for(int i = 0; i < Party.getMemberCount(); i++) {
            if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.a_wornAmount != GameInfo.wornState.New) {
                var temp = partyArmor.createNewSlot(slotIndex, slotPreset.gameObject, partyArmor.transform.GetChild(0), Color.white);
                temp.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(Party.getMemberStats(i).equippedArmor).sprite;
                slotIndex++;
            }
        }

        updateInfo();
    }

    void updateInfo() {
        coinText.text = Inventory.getCoinCount().ToString() + "c";
        if(getCurrentlyUsingMenu() == null || getCurrentlyUsingMenu().getSelectedSlotIndex() == -1) {
            resetInfo();
            return;
        }

        if(getCurrentlyUsingMenu() == inventoryWeapons) {
            int slotIndex = 0;
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                if(Inventory.getWeapon(i).w_wornAmount != GameInfo.wornState.New) {
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
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.w_wornAmount != GameInfo.wornState.New) {
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
                if(Inventory.getArmor(i).a_wornAmount != GameInfo.wornState.New) {
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
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.a_wornAmount != GameInfo.wornState.New) {
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
        nameText.text = we.w_name;
        costText.text = getCost(we.w_rarity, we.w_wornAmount).ToString("0") + "c";
    }
    void setArmorInfo(Armor ar) {
        selectedEquipment.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(ar).sprite;
        nameText.text = ar.a_name;
        costText.text = getCost(ar.a_rarity, ar.a_wornAmount).ToString("0") + "c";
    }
    
    int getCost(GameInfo.rarityLvl rar, GameInfo.wornState state) {
        return ((((int)rar) + 1) * reference.chargeRate) * ((int)GameInfo.wornState.New - (int)state);
    }


    public void show() {
        transform.GetChild(0).DOScale(1.0f, 0.15f);
        updateSlots();
        isShowing = true;
    }
    public void hide() {
        transform.GetChild(0).DOScale(0.0f, 0.15f);
        isShowing = false;
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
                if(Inventory.getWeapon(i).w_wornAmount != GameInfo.wornState.New) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Inventory.getWeapon(i);
                        if(getCost(temp.w_rarity, temp.w_wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.w_rarity, temp.w_wornAmount));

                        temp.w_wornAmount = GameInfo.wornState.New;
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
                if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty() && Party.getMemberStats(i).equippedWeapon.w_wornAmount != GameInfo.wornState.New) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Party.getMemberStats(i).equippedWeapon;
                        if(getCost(temp.w_rarity, temp.w_wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.w_rarity, temp.w_wornAmount));

                        temp.w_wornAmount = GameInfo.wornState.New;
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
                if(Inventory.getArmor(i).a_wornAmount != GameInfo.wornState.New) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Inventory.getArmor(i);
                        if(getCost(temp.a_rarity, temp.a_wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.a_rarity, temp.a_wornAmount));

                        temp.a_wornAmount = GameInfo.wornState.New;
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
                if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty() && Party.getMemberStats(i).equippedArmor.a_wornAmount != GameInfo.wornState.New) {
                    if(slotIndex == getCurrentlyUsingMenu().getSelectedSlotIndex()) {
                        var temp = Party.getMemberStats(i).equippedArmor;
                        if(getCost(temp.a_rarity, temp.a_wornAmount) > Inventory.getCoinCount())
                            return;
                        Inventory.addCoins(-getCost(temp.a_rarity, temp.a_wornAmount));

                        temp.a_wornAmount = GameInfo.wornState.New;
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
