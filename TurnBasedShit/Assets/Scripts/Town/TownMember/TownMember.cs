using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownMember {
    public bool hasQuest = false;
    public Color m_color;
    public Quest quest;

    public Town home;

    public int m_instanceID = -1;

    public TownMember(PresetLibrary lib, bool setID, bool autoHasQuest = false) {
        if(setID)
            m_instanceID = GameInfo.getNextTownMemberInstanceID();

        hasQuest = GameVariables.shouldMemberHaveQuest() || autoHasQuest;
        if(hasQuest) {
            quest = lib.createRandomQuest();
        }

        m_color = getRandomColor();
    }


    public Color getRandomColor() {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    public void setEqualsTo(TownMember other, bool takeID) {
        if(other == null)
            return;
        hasQuest = other.hasQuest;
        m_color = other.m_color;
        quest = other.quest;

        home = other.home;

        if(takeID)
            m_instanceID = other.m_instanceID;
    }
}
