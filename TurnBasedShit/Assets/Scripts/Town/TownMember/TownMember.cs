using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownMember {
    public bool hasQuest = false;
    public UnitSpriteInfo sprite = new UnitSpriteInfo();

    //  quests
    public GameInfo.questType questType;
    public BossFightQuest bossQust = null;
    public DeliveryQuest deliveryQuest = null;
    public KillQuest killQuest = null;
    public PickupQuest pickupQuest = null;

    public DialogInfo dialog = new DialogInfo();

    public Town home;

    public Weapon weapon;
    public Armor armor;

    public string name;
    public int m_instanceID = -1;
    public bool isNPC = false;

    public TownMember(PresetLibrary lib, bool setID, bool autoHasQuest = false) {
        if(setID)
            m_instanceID = GameInfo.getNextTownMemberInstanceID();

        bossQust = null;
        deliveryQuest = null;
        killQuest = null;
        pickupQuest = null;

        hasQuest = GameVariables.shouldMemberHaveQuest() || autoHasQuest || true;
        if(hasQuest) {
            var q = GameInfo.getRandomQuestType();

            questType = q;

            switch(q) {
                case GameInfo.questType.bossFight:
                    bossQust = lib.createRandomBossFightQuest(true);
                    break;

                case GameInfo.questType.delivery:
                    deliveryQuest = lib.createRandomDeliveryQuest(true);
                    break;

                case GameInfo.questType.kill:
                    killQuest = lib.createRandomKillQuest(true);
                    break;

                case GameInfo.questType.pickup:
                    pickupQuest = lib.createRandomPickupQuest(true);
                    break;
            }
        }

        name = NameLibrary.getRandomUsablePlayerName();

        sprite.color = getRandomColor();
        sprite.headIndex = Random.Range(0, lib.getHeadCount());
        sprite.faceIndex = Random.Range(0, lib.getFaceCount());
        sprite.bodyIndex = Random.Range(0, lib.getBodyCount());
    }


    public Color getRandomColor() {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    public void setEqualsTo(TownMember other, bool takeID) {
        if(other == null)
            return;
        hasQuest = other.hasQuest;
        sprite.setEqualTo(other.sprite);

        bossQust = null;
        deliveryQuest = null;
        killQuest = null;
        pickupQuest = null;

        if(other.hasQuest) {
            questType = other.questType;
            if(other.questType == GameInfo.questType.bossFight) {
                bossQust = other.bossQust;
            }
            else if(other.questType == GameInfo.questType.delivery) {
                deliveryQuest = other.deliveryQuest;
            }
            else if(other.questType == GameInfo.questType.kill) {
                killQuest = other.killQuest;
            }
            else if(other.questType == GameInfo.questType.pickup) {
                pickupQuest = other.pickupQuest;
            }
        }

        home = other.home;

        if(takeID)
            m_instanceID = other.m_instanceID;
    }


    public bool isQuestActive() {
        if(questType == GameInfo.questType.bossFight)
            return ActiveQuests.hasQuest(bossQust);
        if(questType == GameInfo.questType.kill)
            return ActiveQuests.hasQuest(killQuest);
        if(questType == GameInfo.questType.delivery)
            return ActiveQuests.hasQuest(deliveryQuest);
        if(questType == GameInfo.questType.pickup)
            return ActiveQuests.hasQuest(pickupQuest);
        return false;
    }
}
