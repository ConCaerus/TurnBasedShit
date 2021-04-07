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


    public void triggerTime(Item.useTimes time, UnitClass playingUnit) {
        foreach(var i in inplayItems) {
            if(canItemBeUsed(i, time, playingUnit))
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


    bool canItemBeUsed(ItemInfo itemInfo, Item.useTimes time, UnitClass playingUnit) {
        bool timeCondition = false;
        foreach(var i in itemInfo.item.i_useTimes) {
            if(i == Item.useTimes.beforeTurn && i == time && playingUnit == itemInfo.holder) {
                timeCondition = true;
                break;
            }

            else if(i == Item.useTimes.afterTurn && i == time && playingUnit == itemInfo.holder) {
                timeCondition = true;
                break;
            }

            else if(i == time) {
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
            switch(itemInfo.item.i_useEffects[i]) {
                //  recovers the holders health by the percentage of max health the mod is (Ex. mod = 1 - recovers 1% of max health)
                case Item.useEffects.recoverHealth:
                    float recoverPercentage = itemInfo.item.i_effectMods[i] / 100.0f;
                    itemInfo.holder.addHealth(itemInfo.holder.stats.u_maxHealth * recoverPercentage);
                    break;
            }
        }
    }
}
