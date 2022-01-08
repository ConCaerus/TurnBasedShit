using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownMember {
    public bool hasQuest = false;
    public UnitSpriteInfo sprite = new UnitSpriteInfo();

    public Quest.questType questType = (Quest.questType)(-1);
    public ObjectHolder questHolder;    //  only should hold one quest

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

        hasQuest = GameVariables.shouldMemberHaveQuest() || autoHasQuest;
        if(hasQuest) {
            questHolder = new ObjectHolder();
            var q = GameInfo.getRandomQuestType();

            switch(q) {
                case Quest.questType.bossFight:
                    questHolder.addObject<BossFightQuest>(lib.createRandomBossFightQuest(true, true, home.region));
                    questType = Quest.questType.bossFight;
                    break;

                case Quest.questType.delivery:
                    questHolder.addObject<DeliveryQuest>(lib.createRandomDeliveryQuest(true, home.region));
                    questType = Quest.questType.delivery;
                    break;

                case Quest.questType.kill:
                    questHolder.addObject<KillQuest>(lib.createRandomKillQuest(true, home.region));
                    questType = Quest.questType.kill;
                    break;

                case Quest.questType.pickup:
                    questHolder.addObject<PickupQuest>(lib.createRandomPickupQuest(true, true, home.region));
                    questType = Quest.questType.pickup;
                    break;

                case Quest.questType.rescue:
                    questHolder.addObject<RescueQuest>(lib.createRandomRescueQuest(true, true, home.region));
                    questType = Quest.questType.pickup;
                    break;

                case Quest.questType.fishing:
                    questHolder.addObject<FishingQuest>(lib.createRandomFishingQuest(true, home.region));
                    questType = Quest.questType.fishing;
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
            questHolder = new ObjectHolder();
            questHolder.addObject<Quest>(other.getQuest());
        }

        home = other.home;

        if(takeID)
            m_instanceID = other.m_instanceID;
    }


    public Quest getQuest() {
        switch(questType) {
            case Quest.questType.bossFight: return questHolder.getObject<BossFightQuest>(0);
            case Quest.questType.pickup: return questHolder.getObject<PickupQuest>(0);
            case Quest.questType.delivery: return questHolder.getObject<DeliveryQuest>(0);
            case Quest.questType.kill: return questHolder.getObject<KillQuest>(0);
            case Quest.questType.rescue: return questHolder.getObject<RescueQuest>(0);
            case Quest.questType.fishing: return questHolder.getObject<FishingQuest>(0);
        }
        return null;
    }
    public void setQuest(Quest other) {
        questType = other.getQuestType();
        questHolder = new ObjectHolder();
        questHolder.addObject<Quest>(other);
    }

    public bool isQuestActive() {
        return ActiveQuests.hasQuest(getQuest());
    }
}
