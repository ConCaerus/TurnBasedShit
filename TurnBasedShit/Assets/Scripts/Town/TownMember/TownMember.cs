using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownMember {
    public bool hasQuest = false;
    public UnitSpriteInfo m_sprite = new UnitSpriteInfo();

    //  quests
    public GameInfo.questType m_questType;
    public BossFightQuest m_bossQuest = null;
    public DeliveryQuest m_deliveryQuest = null;
    public KillQuest m_killQuest = null;
    public PickupQuest m_pickupQuest = null;

    public Town home;

    public int m_instanceID = -1;

    public TownMember(PresetLibrary lib, bool setID, bool autoHasQuest = false) {
        if(setID)
            m_instanceID = GameInfo.getNextTownMemberInstanceID();

        m_bossQuest = null;
        m_deliveryQuest = null;
        m_killQuest = null;
        m_pickupQuest = null;

        hasQuest = GameVariables.shouldMemberHaveQuest() || autoHasQuest || true;
        if(hasQuest) {
            var q = GameInfo.getRandomQuestType();

            m_questType = q;

            switch(q) {
                case GameInfo.questType.bossFight:
                    m_bossQuest = lib.createRandomBossFightQuest(true);
                    break;

                case GameInfo.questType.delivery:
                    m_deliveryQuest = lib.createRandomDeliveryQuest(true);
                    break;

                case GameInfo.questType.kill:
                    m_killQuest = lib.createRandomKillQuest(true);
                    break;

                case GameInfo.questType.pickup:
                    m_pickupQuest = lib.createRandomPickupQuest(true);
                    break;
            }
        }

        m_sprite.color = getRandomColor();
        m_sprite.headIndex = Random.Range(0, lib.getHeadCount());
        m_sprite.faceIndex = Random.Range(0, lib.getFaceCount());
        m_sprite.bodyIndex = Random.Range(0, lib.getBodyCount());
    }


    public Color getRandomColor() {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    public void setEqualsTo(TownMember other, bool takeID) {
        if(other == null)
            return;
        hasQuest = other.hasQuest;
        m_sprite.setEqualTo(other.m_sprite);

        m_bossQuest = null;
        m_deliveryQuest = null;
        m_killQuest = null;
        m_pickupQuest = null;

        if(other.hasQuest) {
            m_questType = other.m_questType;
            if(other.m_questType == GameInfo.questType.bossFight) {
                m_bossQuest = other.m_bossQuest;
            }
            else if(other.m_questType == GameInfo.questType.delivery) {
                m_deliveryQuest = other.m_deliveryQuest;
            }
            else if(other.m_questType == GameInfo.questType.kill) {
                m_killQuest = other.m_killQuest;
            }
            else if(other.m_questType == GameInfo.questType.pickup) {
                m_pickupQuest = other.m_pickupQuest;
            }
        }

        home = other.home;

        if(takeID)
            m_instanceID = other.m_instanceID;
    }


    public bool isQuestActive() {
        if(m_questType == GameInfo.questType.bossFight)
            return ActiveQuests.hasQuest(m_bossQuest);
        if(m_questType == GameInfo.questType.kill)
            return ActiveQuests.hasQuest(m_killQuest);
        if(m_questType == GameInfo.questType.delivery)
            return ActiveQuests.hasQuest(m_deliveryQuest);
        if(m_questType == GameInfo.questType.pickup)
            return ActiveQuests.hasQuest(m_pickupQuest);
        return false;
    }
}
