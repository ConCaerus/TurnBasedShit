using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum partyAffectors {
    unitHealth, addUnit, killUnit, killRandUnit
}

[CreateAssetMenu(fileName = "Story", menuName = "Presets/Story")]
public class Story : ScriptableObject {
    [TextArea]
    public string mainText = "";

    public bool affectsParty = false;
    public string chooseUnitToAffectPrompt = "";
    public float affectedUnitMod = 0.0f;
    public partyAffectors affect;
    UnitStats affectedUnit = null;

    public bool isEnd = false;
    public List<Choice> choices = new List<Choice>();


    public Story getNextStory(int choice) {
        Story next = choices[choice].stories[0];
        //  no more stories
        if(next == null)
            return null;

        //  if there are more than one possible outcomes, choose one
        if(choices[choice].stories.Count > 1) {
            next = chooseAStoryFromPossibles(choice);
        }

        //  resets the saved affected unit
        next.affectedUnit = null;
        //  sets up a unit selection prompt if the next story affects a unit
        if(next.needsAffectedUnit()) {
            //  if the unit was just selected
            if(mainText == next.chooseUnitToAffectPrompt) {
                next.affectedUnit = Party.getMemberStats(choice);
            }

            //  else create a new unit select prompt
            else
                next = next.setupChooseUnitPrompt();
        }

        //  adds a new unit to the party
        else if(next.affectsParty && next.affect == partyAffectors.addUnit) {
            next.affectedUnit = Randomizer.createRandomUnitStats(false);
        }

        //  kills a random unit from the party
        else if(next.affectsParty && next.affect == partyAffectors.killRandUnit) {
            next.affectedUnit = Party.getMemberStats(Random.Range(0, Party.getMemberCount()));
        }

        //  sets up an "OK" choice for the end of the story
        if(next.isEnd) {
            Choice ch = new Choice();
            ch.title = "Okay";
            next.choices.Clear();
            next.choices.Add(ch);
        }
        return next;
    }


    public bool needsAffectedUnit() {
        if(affectsParty) {
            switch(affect) {
                case partyAffectors.unitHealth:
                    return true;
                case partyAffectors.addUnit:
                    return false;
                case partyAffectors.killUnit:
                    return true;
                case partyAffectors.killRandUnit:
                    return false;
            }
        }
        return false;
    }


    public void affectParty(TextMeshProUGUI c1, TextMeshProUGUI c2) {
        switch(affect) {
            case partyAffectors.unitHealth:
                //  adds or removes the health
                if(affectedUnitMod > 0.0f) {
                    affectedUnit.u_health = Mathf.Clamp(affectedUnit.u_health + affectedUnitMod, -100.0f, affectedUnit.getModifiedMaxHealth());

                    c1.text = affectedUnit.u_name + " gained " + affectedUnitMod.ToString() + " HP";
                }
                else {
                    affectedUnit.u_health += affectedUnitMod;
                    c1.text = affectedUnit.u_name + " lost " + Mathf.Abs(affectedUnitMod).ToString() + " HP";
                }

                Party.overrideUnit(affectedUnit);

                //  checks if the unit died
                if(affectedUnit.u_health <= 0.0f) {
                    c2.text = affectedUnit.u_name + " died";
                    Party.removeUnit(affectedUnit.u_order);
                }
                break;

            case partyAffectors.addUnit:
                Party.addNewUnit(affectedUnit);

                c1.text = affectedUnit.u_name + " was added to the party";
                break;

            case partyAffectors.killUnit:
                Party.removeUnit(affectedUnit.u_order);

                c1.text = affectedUnit.u_name + " has died";
                break;

            case partyAffectors.killRandUnit:
                Party.removeUnit(affectedUnit.u_order);

                c1.text = affectedUnit.u_name + " has died";
                break;
        }
        affectedUnit = null;
    }

    public Story setupChooseUnitPrompt() {
        //  needs an affected unit
        Story chooseUnitStory = new Story();
        chooseUnitStory.mainText = chooseUnitToAffectPrompt;

        for(int i = 0; i < Party.getMemberCount(); i++) {
            Choice ch = new Choice();
            ch.title = Party.getMemberStats(i).u_name + getRelevantUnitInformation(Party.getMemberStats(i));
            ch.stories.Add(this);
            chooseUnitStory.choices.Add(ch);
        }
        return chooseUnitStory;
    }
    public string getRelevantUnitInformation(UnitStats stats) {
        switch(affect) {
            case partyAffectors.unitHealth:
                return ": " + stats.u_health.ToString() + " / " + stats.getModifiedMaxHealth().ToString();
        }
        return "";
    }

    public Story chooseAStoryFromPossibles(int index) {
        List<Story> stories = new List<Story>();
        int storyCount = choices[index].stories.Count;

        for(int i = 0; i < storyCount; i++) {
            for(int p = 0; p < choices[index].probables[i]; p++) {
                stories.Add(choices[index].stories[i]);
            }
        }

        int rand = Random.Range(0, stories.Count);
        return stories[Random.Range(0, rand)];
    }

    public string getChoiceTitle(int index) {
        return choices[index].title;
    }
    public int getChoiceCount() {
        return choices.Count;
    }

    public void setAffectedUnit(int index) {
        affectedUnit = Party.getMemberStats(index);
    }
}

[System.Serializable]
public class Choice {
    public string title;
    public List<Story> stories = new List<Story>();
    public List<int> probables = new List<int>();
}