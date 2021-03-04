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
    UnitClassStats affectedUnit = null;

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
            next.affectedUnit = Randomizer.createRandomUnitStats();
        }

        //  kills a random unit from the party
        else if(next.affectsParty && next.affect == partyAffectors.killRandUnit) {
            next.affectedUnit = Party.getMemberStats(Random.Range(0, Party.getPartySize()));
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
                    affectedUnit.u_health = Mathf.Clamp(affectedUnit.u_health + affectedUnitMod, -100.0f, affectedUnit.u_maxHealth);

                    c1.text = affectedUnit.u_name + " gained " + affectedUnitMod.ToString() + " HP";
                }
                else {
                    affectedUnit.u_health += affectedUnitMod;
                    c1.text = affectedUnit.u_name + " lost " + Mathf.Abs(affectedUnitMod).ToString() + " HP";
                }

                Party.resaveUnit(affectedUnit);

                //  checks if the unit died
                if(affectedUnit.u_health <= 0.0f) {
                    c2.text = affectedUnit.u_name + " died";
                    Party.removeUnitAtIndex(affectedUnit.u_order);
                }
                break;

            case partyAffectors.addUnit:
                Party.addNewUnit(affectedUnit);

                c1.text = affectedUnit.u_name + " was added to the party";
                break;

            case partyAffectors.killUnit:
                Party.removeUnitAtIndex(affectedUnit.u_order);

                c1.text = affectedUnit.u_name + " has died";
                break;

            case partyAffectors.killRandUnit:
                Party.removeUnitAtIndex(affectedUnit.u_order);

                c1.text = affectedUnit.u_name + " has died";
                break;
        }
        affectedUnit = null;
    }

    public Story setupChooseUnitPrompt() {
        //  needs an affected unit
        Story chooseUnitStory = new Story();
        chooseUnitStory.mainText = chooseUnitToAffectPrompt;

        for(int i = 0; i < Party.getPartySize(); i++) {
            Choice ch = new Choice();
            ch.title = Party.getMemberStats(i).u_name + getRelevantUnitInformation(Party.getMemberStats(i));
            ch.stories.Add(this);
            chooseUnitStory.choices.Add(ch);
        }
        return chooseUnitStory;
    }
    public string getRelevantUnitInformation(UnitClassStats stats) {
        switch(affect) {
            case partyAffectors.unitHealth:
                return ": " + stats.u_health.ToString() + " / " + stats.u_maxHealth.ToString();
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
/*
  public class Story : ScriptableObject {
    [TextArea]
    public string story;
    public StoryUnitCause cause;

    public List<StoryChoice> choices;

    
    public Story getNextStory(int choice) {
        Story next = null;


        //  no choices and the player is selecting a unit to be affected
        if(choices[choice].probables.Count == 0 && !cause.isEmpty()) {
            cause.setAffectedUnit(choice);
            return this;
        }

        //  only one choice
        else if(choices[choice].probables.Count == 1)
            next = choices[choice].probables[0].outcome;

        //  more than one choice
        else {
            int rand = Random.Range(0, 101);

            float closest = 100.0f;
            for(int i = 0; i < choices[choice].probables.Count; i++) {
                float prob = choices[choice].probables[i].probThreshold;

                //  checks if the prob is better than the closest prob
                if(rand < prob && prob - rand < closest) {
                    closest = prob - rand;
                    next = choices[choice].probables[i].outcome;
                }
            }
        }


        //  start the select unit story if the next story requires a unit.
        if(next.cause != null && next.cause.chooseAffectedUnitStory != null && !next.cause.hasAffectedUnit()) {
            next.cause.setDifferentUnitSelectionOptions();
            var c = next.cause;
            next = next.cause.chooseAffectedUnitStory;
            next.cause = c;
        }
        return next;
    }
    public string getChoiceText(int choice) {
        return choices[choice].story;
    }
    public int getChoiceCount() {
        return choices.Count;
    }
    public bool isEnd() {
        return story == "END";
    }
}


[System.Serializable]
public class StoryChoice {
    [TextArea]
    public string story;
    public List<ProbableStory> probables;

    public StoryChoice(string s = "") {
        if(s != "")
            story = s;
    }
}

[System.Serializable]
public class ProbableStory {
    public Story outcome;
    public float probThreshold;
}

[System.Serializable]
public class StoryUnitCause {
    public enum causeType {
        unitHealth
    }

    public Story chooseAffectedUnitStory;
    UnitClassStats affectedUnit;
    public float modAmount;
    public causeType modType;


    public void affectUnit(TextMeshProUGUI causeText, TextMeshProUGUI otherCauseText) {
        switch(modType) {
            case causeType.unitHealth:
                Debug.Log(affectedUnit.u_health + " " + modAmount);
                Mathf.Clamp(affectedUnit.u_health += modAmount, -100, affectedUnit.u_maxHealth);
                Debug.Log(affectedUnit.u_health);
                causeText.text = getCauseString();

                //  unit fucking died
                if(affectedUnit.u_health <= 0.0f) {
                    otherCauseText.text = affectedUnit.u_name + " has died";
                    Party.removeUnit(affectedUnit);
                }
                break;
        }
        Party.resaveUnit(affectedUnit);
    }

    public string getCauseString() {
        switch(modType) {
            case causeType.unitHealth:
                if(modAmount > 0.0f)
                    return affectedUnit.u_name + " gained " + modAmount.ToString() + " health";
                else
                    return affectedUnit.u_name + " lost " + Mathf.Abs(modAmount).ToString() + " health";
        }
        return "";
    }

    public string getRelevantUnitInformation(UnitClassStats stats) {
        switch(modType) {
            case causeType.unitHealth:
                return stats.u_health.ToString() + " / " + stats.u_maxHealth.ToString() + " HP";
        }
        return "";
    }


    public void setAffectedUnit(int choice) {
        affectedUnit = Party.getMemberStats(choice);
    }
    public void setDifferentUnitSelectionOptions() {
        chooseAffectedUnitStory.choices.Clear();
        for(int i = 0; i < Party.getPartySize(); i++) {
            var choiceStory = Party.getMemberStats(i).u_name + ": " + getRelevantUnitInformation(Party.getMemberStats(i));

            var s = new StoryChoice(choiceStory);
            s.probables = new List<ProbableStory>();
            s.probables.Add(new ProbableStory());

            chooseAffectedUnitStory.choices.Add(s);
        }
    }

    public void setRandAffectedUnit() {
        int rand = Random.Range(0, Party.getPartySize());
        affectedUnit = Party.getMemberStats(rand);
    }
    public bool isEmpty() {
        return modAmount == 0;
    }
    public bool hasAffectedUnit() {
        return affectedUnit == null;
    }
}

*/
