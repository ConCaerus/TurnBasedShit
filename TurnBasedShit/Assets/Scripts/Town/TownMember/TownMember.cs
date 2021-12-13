using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownMember {
    public bool hasQuest = false;
    public UnitSpriteInfo sprite = new UnitSpriteInfo();

    public Quest quest;

    public DialogInfo dialog = new DialogInfo();

    public Town home;

    public Weapon weapon;
    public Armor armor;

    public string name;
    public int m_instanceID = -1;
    public bool isNPC = false;

    public TownMember(PresetLibrary lib, Town t, bool setID, bool autoHasQuest = false) {
        if(setID)
            m_instanceID = GameInfo.getNextTownMemberInstanceID();
        isNPC = false;

        quest = null;

        weapon = new Weapon();
        var wTemp = lib.getRandomWeapon(t.region);
        if(wTemp == null || wTemp.isEmpty())
            weapon = null;
        else
            weapon.setEqualTo(wTemp, true);

        armor = new Armor();
        var aTemp = lib.getRandomArmor(t.region);
        if(aTemp == null || aTemp.isEmpty())
            armor = null;
        else
            armor.setEqualTo(aTemp, true);

        home = t;

        hasQuest = GameVariables.shouldMemberHaveQuest() || autoHasQuest || true;
        if(hasQuest) {
            var q = GameInfo.getRandomQuestType();

            switch(q) {
                case Quest.questType.bossFight:
                    quest = lib.createRandomBossFightQuest(true);
                    break;

                case Quest.questType.delivery:
                    quest = lib.createRandomDeliveryQuest(true);
                    break;

                case Quest.questType.kill:
                    quest = lib.createRandomKillQuest(true);
                    break;

                case Quest.questType.pickup:
                    quest = lib.createRandomPickupQuest(true);
                    break;

                case Quest.questType.fishing:
                    break;
            }
        }

        name = NameLibrary.getRandomUsablePlayerName();

        sprite.color = getRandomColor();
        sprite.headIndex = Random.Range(0, lib.getHeadCount());
        sprite.faceIndex = Random.Range(0, lib.getFaceCount());
        sprite.bodyIndex = Random.Range(0, lib.getBodyCount());
    }

    public TownMember(PresetLibrary lib, NPCPreset preset, Town t, bool setID) {
        setEqualsTo(preset.preset, false);


        if(preset.weapon != null) {
            weapon = new Weapon();
            weapon.setEqualTo(lib.getWeapon(preset.weapon.preset.name), true);
        }
        if(preset.armor != null) {
            armor = new Armor();
            armor.setEqualTo(lib.getArmor(preset.armor.preset.name), true);
        }

        home = t;

        if(setID)
            m_instanceID = GameInfo.getNextTownMemberInstanceID();
    }


    public Color getRandomColor() {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    public void setEqualsTo(TownMember other, bool takeID) {
        if(other == null)
            return;
        hasQuest = other.hasQuest;
        sprite.setEqualTo(other.sprite);

        quest = null;

        isNPC = other.isNPC;

        //  equipment
        if(other.weapon == null || other.weapon.isEmpty())
            weapon = null;
        else {
            weapon = new Weapon();
            weapon.setEqualTo(other.weapon, true);
        }

        if(other.armor == null || other.armor.isEmpty())
            armor = null;
        else {
            armor = new Armor();
            armor.setEqualTo(other.armor, true);
        }


        dialog = other.dialog;
        name = other.name;


        if(other.hasQuest) {
            quest = other.quest;
        }

        home = other.home;

        if(takeID)
            m_instanceID = other.m_instanceID;
    }


    public bool isQuestActive() {
        return ActiveQuests.hasQuest(quest);
        return false;
    }
}
