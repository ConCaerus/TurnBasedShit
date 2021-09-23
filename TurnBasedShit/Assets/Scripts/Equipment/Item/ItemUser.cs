using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour {
    struct ItemInfo {
        public Item item;
        public UnitClass holder;

        public ItemInfo(Item it, UnitClass stats) {
            item = it;
            holder = stats;
        }
    }
    List<ItemInfo> inplayItems = new List<ItemInfo>();


    public void triggerTime(Item.useTimes time, UnitClass relevantUnit, bool needsRelevantUnitToBeHolder) {
        foreach(var i in inplayItems) {
            if(canItemBeUsed(i, time, relevantUnit, needsRelevantUnitToBeHolder))
                useItem(i);
        }
    }

    public void resetInplayItems() {
        inplayItems.Clear();

        //  gets player items
        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.equippedItem != null) {
                inplayItems.Add(new ItemInfo(i.stats.equippedItem, i));
            }
        }
    }


    bool canItemBeUsed(ItemInfo itemInfo, Item.useTimes time, UnitClass relevantUnit, bool needsRelevantUnitToBeHolder) {
        bool timeCondition = false;
        foreach(var i in itemInfo.item.i_useTimes) {
            if(i == time && needsRelevantUnitToBeHolder && relevantUnit == itemInfo.holder) {
                timeCondition = true;
                break;
            }
            else if(i == time && !needsRelevantUnitToBeHolder) {
                timeCondition = true;
                break;
            }
        }

        if(!timeCondition)
            return false;

        //  eventually check for use conditions too
        return true;
    }

    void useItem(ItemInfo itemInfo) {
        for(int i = 0; i < itemInfo.item.i_useEffects.Count; i++) {
            switch(itemInfo.item.i_useEffects[i].effect) {
                //  recovers the holders health by the percentage of max health the mod is (Ex. mod = 0.01 - recovers 1% of max health)
                case Item.useEffectTypes.modHealth:
                    float healedAmount = itemInfo.holder.stats.getModifiedMaxHealth() * itemInfo.item.getHealthMod();
                    float emptyHealth = itemInfo.holder.stats.getModifiedMaxHealth() - itemInfo.holder.stats.u_health;
                    if(emptyHealth <= 0.0f) {
                        itemInfo.holder.stats.u_health = itemInfo.holder.stats.getModifiedMaxHealth();
                        break;
                    }
                    if(healedAmount > emptyHealth)
                        healedAmount = emptyHealth;
                    itemInfo.holder.addHealth(healedAmount);
                    FindObjectOfType<DamageTextCanvas>().showTextForUnit(itemInfo.holder.gameObject, healedAmount, DamageTextCanvas.damageType.healed);
                    break;

                //  adds to power
                case Item.useEffectTypes.modDamageGiven:
                    itemInfo.holder.tempPowerMod = itemInfo.item.getDamageGivenMod();
                    break;

                //  adds to defence
                case Item.useEffectTypes.modDamageTaken:
                    itemInfo.holder.tempDefenceMod = itemInfo.item.getDamageTakenMod();
                    break;

                //  adds to the speed
                case Item.useEffectTypes.modSpeed:
                    itemInfo.holder.tempSpeedMod = itemInfo.item.getSpeedMod();
                    break;
            }
        }
    }
}
