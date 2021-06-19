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

                //  adds to the max health
                case Item.useEffectTypes.modMaxHealth:
                    float gainedHealth = itemInfo.holder.stats.getBaseMaxHealth() * itemInfo.item.getMaxHealthMod();
                    gainedHealth += itemInfo.holder.stats.getBaseMaxHealth();
                    if(gainedHealth <= 0.0f)
                        break;

                    itemInfo.holder.stats.setBaseMaxHealth(gainedHealth);
                    break;

                //  adds to power
                case Item.useEffectTypes.modDamageGiven:
                    float gainedPower = itemInfo.holder.stats.u_power * itemInfo.item.getDamageGivenMod();
                    itemInfo.holder.stats.addPower(gainedPower);
                    break;

                //  adds to defence
                case Item.useEffectTypes.modDamageTaken:
                    float gainedDefence = itemInfo.holder.stats.u_defence * itemInfo.item.getDamageTakenMod();
                    itemInfo.holder.stats.addDefence(gainedDefence);
                    break;

                //  adds to the speed
                case Item.useEffectTypes.modSpeed:
                    float gainedSpeed = itemInfo.holder.stats.u_speed * itemInfo.item.getSpeedMod();
                    itemInfo.holder.stats.addSpeed(gainedSpeed);
                    break;
            }
        }
    }
}
