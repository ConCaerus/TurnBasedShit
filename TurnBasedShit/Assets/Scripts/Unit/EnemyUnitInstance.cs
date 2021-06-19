using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitInstance : UnitClass {
    public GameInfo.diffLvl enemyDiff = 0;
    public GameInfo.element weakTo, strongTo;

    public type enemyType;

    public List<int> combatScars = new List<int>();
    [SerializeField] List<GameObject> combatScarsObjects = new List<GameObject>();

    public enum type {
        bug, goblin, slime, snake, spider
    }


    private void Awake() {
        isPlayerUnit = false;
        giveRandomScars();
        showCombatScars();
    }


    public IEnumerator combatTurn() {
        yield return new WaitForSeconds(0.5f);
        if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setRandomAttackingTarget();

            yield return new WaitForSeconds(0.5f);

            if(this != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
                attack(attackingTarget);
            }
        }
    }


    public void giveRandomScars() {
        combatScars = new List<int>();
        combatScars.Clear();

        //  max scar count is 3
        int count = Random.Range(0, 4);

        for(int i = 0; i < count; i++) {
            combatScars.Add(Random.Range(0, FindObjectOfType<PresetLibrary>().getCombatScarSpriteCount()));
        }
    }


    public void showCombatScars() {
        if(combatScars == null || combatScars.Count == 0)
            return;
        List<GameObject> unusedObjects = new List<GameObject>();
        foreach(var i in combatScarsObjects)
            unusedObjects.Add(i);
        for(int i = 0; i < combatScars.Count; i++) {
            int randIndex = Random.Range(0, unusedObjects.Count);
            var s = FindObjectOfType<PresetLibrary>().getCombatScarSprite(combatScars[i]);
            unusedObjects[randIndex].GetComponent<SpriteRenderer>().sprite = s.sprite;
            unusedObjects.RemoveAt(randIndex);
        }
    }
}
