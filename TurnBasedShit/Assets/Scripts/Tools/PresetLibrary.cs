using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Units
    [SerializeField] GameObject[] playerUnits;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] bosses;
    [SerializeField] UnitTraitPreset[] unitTraits;

    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] ConsumablePreset[] consumables;
    [SerializeField] ItemPreset[] items;

    //  Map
    [SerializeField] BuildingPreset[] buildings;


    //  Units
    public UnitStats createRandomUnit() {
        UnitStats stats = new UnitStats();
        stats.u_name = NameLibrary.getRandomUsablePlayerName();

        stats.setBaseMaxHealth(Random.Range(25, 125));
        stats.u_health = stats.getModifiedMaxHealth();
        stats.u_sprite.setSprite(playerUnits[Random.Range(0, playerUnits.Length)].GetComponent<SpriteRenderer>().sprite);
        stats.u_color = playerUnits[Random.Range(0, playerUnits.Length)].GetComponent<SpriteRenderer>().color;
        stats.u_power = Random.Range(1, 15);
        stats.u_defence = Random.Range(1, 15);
        stats.u_speed = Random.Range(-5, 10);

        return stats;
    }
    public GameObject getEnemy(string name) {
        foreach(var i in enemies) {
            if(i.GetComponent<UnitClass>().stats.u_name == name)
                return i;
        }
        return null;
    }
    public UnitStats getRandomEnemy(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        //  no specified difficulty level
        if(diff == (GameInfo.diffLvl)(-1))
            return enemies[Random.Range(0, enemies.Length)].GetComponent<UnitClass>().stats;

        List<UnitStats> useables = new List<UnitStats>();
        foreach(var i in enemies) {
            //  same difficulty
            if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff)
                useables.Add(i.GetComponent<EnemyUnitInstance>().stats);

            //  higher difficulty
            else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff + 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.GetComponent<EnemyUnitInstance>().stats);
            }

            //  lower difficulty
            else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff - 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.GetComponent<EnemyUnitInstance>().stats);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        //  not useable enemies, return random enemy from all enemies
        return enemies[Random.Range(0, enemies.Length)].GetComponent<EnemyUnitInstance>().stats;
    }
    public UnitStats getRandomBoss(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        //  no specified difficulty level
        if(diff == (GameInfo.diffLvl)(-1))
            return bosses[Random.Range(0, bosses.Length)].GetComponent<UnitClass>().stats;

        List<UnitStats> useables = new List<UnitStats>();
        foreach(var i in bosses) {
            //  same difficulty
            if(i.GetComponent<BossUnitInstance>().enemyDiff == diff)
                useables.Add(i.GetComponent<BossUnitInstance>().stats);

            //  higher difficulty
            else if(i.GetComponent<BossUnitInstance>().enemyDiff == diff + 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.GetComponent<BossUnitInstance>().stats);
            }

            //  lower difficulty
            else if(i.GetComponent<BossUnitInstance>().enemyDiff == diff - 1) {
                if(Random.Range(0, 101) <= 25)
                    useables.Add(i.GetComponent<BossUnitInstance>().stats);
            }
        }

        if(useables.Count > 0)
            return useables[Random.Range(0, useables.Count)];
        //  not useable enemies, return random enemy from all enemies
        return bosses[Random.Range(0, bosses.Length)].GetComponent<BossUnitInstance>().stats;
    }

    public UnitTrait getRandomGoodUnitTrait() {
        List<UnitTrait> goods = new List<UnitTrait>();
        foreach(var i in unitTraits) {
            if(i.preset.t_isGood)
                goods.Add(i.preset);
        }

        return goods[Random.Range(0, goods.Count)];
    }
    public UnitTrait getRandomBadUnitTrait() {
        List<UnitTrait> bads = new List<UnitTrait>();
        foreach(var i in unitTraits) {
            if(!i.preset.t_isGood)
                bads.Add(i.preset);
        }

        return bads[Random.Range(0, bads.Count)];
    }
    public UnitTrait getRandomUnitTrait() {
        return unitTraits[Random.Range(0, unitTraits.Length)].preset;
    }

    //  Equipment
    public Weapon getWeapon(string name) {
        Weapon temp = null;
        foreach(var i in weapons) {
            if(i.preset.w_name == name)
                temp.setEqualTo(i.preset);
        }
        return temp;
    }
    public Weapon getRandomWeapon(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        var temp = new Weapon();
        if(lvl == (GameInfo.rarityLvl)(-1)) {
            temp.setEqualTo(weapons[Random.Range(0, weapons.Length)].preset);
            return temp;
        }

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

        if(useables.Count > 0) {
            temp.setEqualTo(useables[Random.Range(0, useables.Count)]);
            return temp;
        }
        temp.setEqualTo(weapons[Random.Range(0, weapons.Length)].preset);
        return temp;
    }

    public Armor getArmor(string name) {
        Armor temp = null;
        foreach(var i in armor) {
            if(i.preset.a_name == name)
                temp.setEqualTo(i.preset);
        }
        return temp;
    }
    public Armor getRandomArmor(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        Armor temp = new Armor();

        if(lvl == (GameInfo.rarityLvl)(-1)) {
            temp.setEqualTo(armor[Random.Range(0, armor.Length)].preset);
            return temp;
        }

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

        if(useables.Count > 0) {
            temp.setEqualTo(useables[Random.Range(0, useables.Count)]);
            return temp;
        }
        //  if no useables, return random from all
        temp.setEqualTo(armor[Random.Range(0, armor.Length)].preset);
        return temp;
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
        return consumables[Random.Range(0, consumables.Length)].preset;
    }

    public Item getItem(string name) {
        foreach(var i in items) {
            if(i.preset.i_name == name)
                return i.preset;
        }
        return null;
    }
    public Item getRandomItem(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        var temp = new Item();

        if(lvl == (GameInfo.rarityLvl)(-1)) {
            temp.setEqualTo(items[Random.Range(0, items.Length)].preset);
            return temp;
        }

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

        if(useables.Count > 0) {
            temp.setEqualTo(useables[Random.Range(0, useables.Count)]);
            return temp;
        }
        temp.setEqualTo(items[Random.Range(0, items.Length)].preset);
        return temp;
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
        int waveCount = Random.Range(1, 4);
        var loc = new CombatLocation(lvl, FindObjectOfType<PresetLibrary>(), waveCount);
        loc.difficulty = lvl;

        loc.coinReward = 2 * ((int)lvl + 1) * waveCount; // default value
        loc.coinReward += (int)Random.Range(loc.coinReward * -0.1f, loc.coinReward * 0.1f);   //  randomizes it

        //  chance for weapon
        while(Random.Range(0, 101) < 20 && loc.weapons.Count < 3)
            loc.weapons.Add(Randomizer.randomizeWeapon(getRandomWeapon((GameInfo.rarityLvl)lvl), lvl));

        //  chance for armor
        while(Random.Range(0, 101) < 20 && loc.armor.Count < 3)
            loc.armor.Add(Randomizer.randomizeArmor(getRandomArmor((GameInfo.rarityLvl)lvl), lvl));

        //  chance for consumable
        while(Random.Range(0, 101) < 40 && loc.consumables.Count < 5)
            loc.consumables.Add(getRandomConsumable((GameInfo.rarityLvl)lvl));

        return loc;
    }
}
