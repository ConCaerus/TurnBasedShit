using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlaveStats {
    public bool isSlave = false;
    public float escapeDesire = 0.0f;

    //  Values that affect chance of escape
    public float scaredModifier = 1.5f;
    public GameInfo.rarityLvl idealWeathLevel = GameInfo.rarityLvl.Uncommon;


    public void modifyEscapeDesire(float f) {
        escapeDesire += f;
    }

    public float calculateEscapeChance() {
        //  slave no longer wants to escape
        if(escapeDesire <= 0.0f) {
            isSlave = false;
            return 0.0f;
        }
        //  somehow this function ran for a non-slave
        if(!isSlave)
            return 0.0f;

        float temp = escapeDesire;
        UnitStats stats = null;
        for(int i = 0; i < Party.getPartySize(); i++) {
            if(Party.getMemberStats(i).u_slaveStats == this) {
                stats = Party.getMemberStats(i);
                break;
            }
        }
        //  somehow this function ran for a non-party member
        if(stats == null)
            return -1.0f;

        //  Intimidation
        float intimidationAmount = stats.u_power * scaredModifier;
        for(int i = 0; i < Party.getPartySize(); i++) {
            //  big boi in party
            if(Party.getMemberStats(i) != stats && Party.getMemberStats(i).u_power > intimidationAmount) {
                temp -= Party.getMemberStats(i).u_power / intimidationAmount;
            }

            //  smol boi in party
            else if(Party.getMemberStats(i) != stats && Party.getMemberStats(i).u_power <= stats.u_power) {
                temp += 2.0f * (stats.u_power / Party.getMemberStats(i).u_power);
            }
        }


        //  Wealthy
        int weaponAffect = (int)stats.equippedWeapon.w_rarity - (int)idealWeathLevel;
        int armorAffect = (int)stats.equippedArmor.a_rarity - (int)idealWeathLevel;
        float avgWealth = (weaponAffect + armorAffect) / 2.0f;
        temp += avgWealth / 2.0f;

        return temp;
    }
}
