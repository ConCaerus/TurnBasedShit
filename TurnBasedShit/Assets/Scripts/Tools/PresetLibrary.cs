using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Units
    [SerializeField] EnemyUnitInstance[] enemies;

    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] ConsumablePreset[] consumables;
    [SerializeField] ItemPreset[] items;

    //  Map
    [SerializeField] BuildingPreset[] buildings;


    //  Units
    public UnitClassStats getRandomEnemy(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        //  no specified difficulty level
        if(diff == (GameInfo.diffLvl)(-1))
            return enemies[Random.Range(0, enemies.Length)].stats;

        List<UnitClassStats> useables = new List<UnitClassStats>();
        foreach(var i in enemies) {
            //  same difficulty
            if(i.enemyDiff == diff)
                useables.Add(i.stats);

            //  higher difficulty
            else if(i.enemyDiff == diff + 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.stats);
            }

            //  lower difficulty
            else if(i.enemyDiff == diff - 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.stats);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        return null;
    }

    //  Equipment
    public Weapon getWeapon(string name) {
        foreach(var i in weapons) {
            if(i.preset.w_name == name)
                return i.preset;
        }
        return null;
    }
    public Weapon getRandomWeapon(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1))
            return weapons[Random.Range(0, weapons.Length)].preset;

        List<Weapon> useables = new List<Weapon>();
        foreach(var i in weapons) {
            //  same lvl
            if(i.preset.w_rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.w_rarity > lvl) {
                int lvlOffset = i.preset.w_rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    useables.Add(i.preset);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        return null;
    }

    public Armor getArmor(string name) {
        foreach(var i in armor) {
            if(i.preset.a_name == name)
                return i.preset;
        }
        return null;
    }
    public Armor getRandomArmor(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1))
            return armor[Random.Range(0, armor.Length)].preset;

        List<Armor> useables = new List<Armor>();
        foreach(var i in armor) {
            //  same lvl
            if(i.preset.a_rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.a_rarity > lvl) {
                int lvlOffset = i.preset.a_rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    useables.Add(i.preset);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        return null;
    }

    public Consumable getConsumable(string name) {
        foreach(var i in consumables) {
            if(i.preset.c_name == name)
                return i.preset;
        }
        return null;
    }
    public Consumable getRandomConsumable(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1))
            return consumables[Random.Range(0, consumables.Length)].preset;

        List<Consumable> useables = new List<Consumable>();
        foreach(var i in consumables) {
            //  same lvl
            if(i.preset.c_rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.c_rarity > lvl) {
                int lvlOffset = i.preset.c_rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    useables.Add(i.preset);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        return null;
    }

    public Item getItem(string name) {
        foreach(var i in items) {
            if(i.preset.i_name == name)
                return i.preset;
        }
        return null;
    }
    public Item getRandomItem(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1))
            return items[Random.Range(0, items.Length)].preset;

        List<Item> useables = new List<Item>();
        foreach(var i in items) {
            //  same lvl
            if(i.preset.i_rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.i_rarity > lvl) {
                int lvlOffset = i.preset.i_rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    useables.Add(i.preset);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        return null;
    }

    //  Map
    public Building getBuilding(Building.type t) {
        foreach(var i in buildings) {
            if(i.preset.b_type == t)
                return i.preset;
        }
        return null;
    }
    public Building getRandomBuilding() {
        return buildings[Random.Range(0, buildings.Length)].preset;
    }

    public CombatLocation createCombatLocation(GameInfo.diffLvl lvl) {
        var loc = new CombatLocation();
        loc.difficulty = lvl;

        //  enemies
        int enemyCount = Random.Range(2, 6);
        for(int i = 0; i < enemyCount; i++) {
            loc.enemies.Add(Randomizer.randomizeUnitStats(getRandomEnemy(lvl), true));
        }

        loc.coinReward = (2 * (int)lvl); // default value
        loc.coinReward += (int)Random.Range(-loc.coinReward * 0.1f, (loc.coinReward * 0.1f));   //  randomizes it

        //  chance for weapon
        while(Random.Range(0, 101) < 20 && loc.weapons.Count < 3)
            loc.weapons.Add(Randomizer.randomizeWeapon(getRandomWeapon((GameInfo.rarityLvl)lvl)));

        //  chance for armor
        while(Random.Range(0, 101) < 20 && loc.armor.Count < 3)
            loc.armor.Add(Randomizer.randomizeArmor(getRandomArmor((GameInfo.rarityLvl)lvl)));

        //  chance for consumable
        while(Random.Range(0, 101) < 40 && loc.consumables.Count < 5)
            loc.consumables.Add(getRandomConsumable((GameInfo.rarityLvl)lvl));

        return loc;
    }
}
