using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Units
    [SerializeField] GameObject playerUnit;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] bosses;
    [SerializeField] UnitTraitPreset[] unitTraits;

    [SerializeField] CombatScarSpriteHolder[] combatScars;

    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] ConsumablePreset[] consumables;
    [SerializeField] ItemPreset[] items;

    //  Map
    [SerializeField] GameObject[] buildings;
    [SerializeField] Sprite weaponUpgradeIcon, armorUpgradeIcon, nestIcon, townIcon;


    //  Units
    public GameObject createPlayerUnit(bool isSlave = false) {
        var temp = playerUnit;
        temp.GetComponent<UnitClass>().stats.u_slaveStats.isSlave = isSlave;
        return playerUnit;
    }
    public GameObject getEnemy(string name) {
        foreach(var i in enemies) {
            if(i.GetComponent<UnitClass>().stats.u_name == name)
                return i;
        }
        return null;
    }
    public GameObject getRandomEnemy(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        GameObject enemy = null;
        //  no specified difficulty level
        if(diff != (GameInfo.diffLvl)(-1)) {
            List<GameObject> useables = new List<GameObject>();
            useables.Clear();
            foreach(var i in enemies) {
                //  same difficulty
                if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff)
                    useables.Add(i);

                //  higher difficulty
                else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff + 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i);
                }

                //  lower difficulty
                else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff - 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i);
                }
            }

            if(useables.Count > 0)
                enemy = useables[Random.Range(0, useables.Count)];
        }
        else if(diff == (GameInfo.diffLvl)(-1) || enemy == null) {
            //  not useable enemies, return random enemy from all enemies
            enemy = enemies[Random.Range(0, enemies.Length)];
        }

        //  makes it so this enemy's stats don't change the preset's stats
        return enemy;
    }
    public GameObject getRandomBoss(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        GameObject boss = null;

        if(diff != (GameInfo.diffLvl)(-1)) {
            List<GameObject> useables = new List<GameObject>();
            foreach(var i in bosses) {
                //  same difficulty
                if(i.GetComponent<BossUnitInstance>().enemyDiff == diff)
                    useables.Add(i);

                //  higher difficulty
                else if(i.GetComponent<BossUnitInstance>().enemyDiff == diff + 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i);
                }

                //  lower difficulty
                else if(i.GetComponent<BossUnitInstance>().enemyDiff == diff - 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i);
                }
            }

            if(useables.Count > 0)
                boss = useables[Random.Range(0, useables.Count)];
        }

        else if(diff == (GameInfo.diffLvl)(-1) || boss == null) {
            //  not useable enemies, return random enemy from all enemies
            boss = bosses[Random.Range(0, bosses.Length)];
        }

        return boss;
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
    public Town createRandomTown(GameInfo.diffLvl diff) {
        var town = new Town(Random.Range(3, 9), diff, this);
        town.shopPriceMod = Random.Range(-0.1f, 0.1f);
        town.shopSellReduction = Random.Range(0.0f, 0.15f);

        return town;
    }

    public Building getBuilding(Building.type t) {
        foreach(var i in buildings) {
            if(i.GetComponent<Building>().b_type == t)
                return i.GetComponent<Building>();
        }
        return null;
    }
    public Building getRandomBuilding() {
        return buildings[Random.Range(0, buildings.Length)].GetComponent<BuildingInstance>().building;
    }

    //  special locations
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
    public BossLocation createRandomBossLocation() {
        var boss = getRandomBoss();
        return new BossLocation(Map.getRandPos(), boss, boss.GetComponent<BossUnitInstance>().enemyDiff, this);
    }
    public PickupLocation createRandomPickupLocation() {
        int type = Random.Range(0, 5);
        if(type == 0)
            return new PickupLocation(Map.getRandPos(), getRandomWeapon(), this, GameInfo.getDiffRegion());
        if(type == 1)
            return new PickupLocation(Map.getRandPos(), getRandomArmor(), this, GameInfo.getDiffRegion());
        if(type == 2)
            return new PickupLocation(Map.getRandPos(), getRandomConsumable(), Random.Range(1, 16), this, GameInfo.getDiffRegion());
        if(type == 3)
            return new PickupLocation(Map.getRandPos(), getRandomItem(), this, GameInfo.getDiffRegion());
        return null;
    }


    public Quest createRandomQuest(Quest.questType type = (Quest.questType)(-1)) {
        if(type == (Quest.questType)(-1)) {
            //  change when adding new quests
            type = (Quest.questType)Random.Range(0, 4);
        }

        //  for debugging shit
        type = Quest.questType.equipmentPickup;

        switch(type) {
            case Quest.questType.bossFight:
                return new BossFightQuest(createRandomBossLocation());

            case Quest.questType.equipmentPickup:
                return new PickupQuest(createRandomPickupLocation());

            case Quest.questType.accumulative:
                return new AccumulativeQuest((AccumulativeQuest.type)Random.Range(0, 1));   //  change this number too

            case Quest.questType.delivery:
                //  get a random town
                int townInd = Random.Range(0, TownLibrary.getTownCount());
                if(FindObjectOfType<TownInstance>() != null) {
                    while(townInd == FindObjectOfType<TownInstance>().town.t_index)
                        townInd = Random.Range(0, TownLibrary.getTownCount());
                }

                //  create delivery objecs
                int rand = Random.Range(0, 5);
                if(rand == 0) {
                    var things = new List<Weapon>();
                    for(int i = 0; i < Random.Range(1, 4); i++)
                        things.Add(getRandomWeapon());

                    return new DeliveryQuest(TownLibrary.getTown(townInd), things);
                }
                if(rand == 1) {
                    var things = new List<Armor>();
                    for(int i = 0; i < Random.Range(1, 4); i++)
                        things.Add(getRandomArmor());

                    return new DeliveryQuest(TownLibrary.getTown(townInd), things);
                }
                if(rand == 2) {
                    var things = new List<Consumable>();
                    var con = getRandomConsumable();
                    for(int i = 0; i < Random.Range(1, 26); i++)
                        things.Add(con);

                    return new DeliveryQuest(TownLibrary.getTown(townInd), things);
                }
                if(rand == 3) {
                    var things = new List<Item>();
                    for(int i = 0; i < Random.Range(0, 4); i++)
                        things.Add(getRandomItem());

                    return new DeliveryQuest(TownLibrary.getTown(townInd), things);
                }
                if(rand == 4) {
                    var things = new List<UnitStats>();
                    for(int i = 0; i < Random.Range(1, 3); i++)
                        things.Add(createPlayerUnit(true).GetComponent<UnitClass>().stats);

                    return new DeliveryQuest(TownLibrary.getTown(townInd), things);
                }
                break;
        }

        return null;
    }

    //  sprite holders
    public UnitSpriteHolder getPlayerUnitSprite() {
        return playerUnit.GetComponent<UnitClass>().getSpriteHolder();
    }
    public UnitSpriteHolder getEnemyUnitSprite(EnemyUnitInstance enemy) {
        foreach(var i in enemies) {
            if(i.GetComponent<EnemyUnitInstance>().enemyType == enemy.enemyType) {
                return i.GetComponent<EnemyUnitInstance>().getSpriteHolder();
            }
        }
        return null;
    }

    public CombatScarSpriteHolder getCombatScarSprite(int index) {
        return combatScars[index];
    }
    public int getCombatScarSpriteCount() {
        return combatScars.Length;
    }

    public WeaponSpriteHolder getWeaponSprite(Weapon we) {
        foreach(var i in weapons) {
            if(i.preset.w_name == we.w_name && i.preset.w_element == we.w_element)
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public ArmorSpriteHolder getArmorSprite(Armor ar) {
        foreach(var i in armor) {
            if(i.preset.a_name == ar.a_name)
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public ConsumableSpriteHolder getConsumableSprite(Consumable con) {
        foreach(var i in consumables) {
            if(i.preset.c_name == con.c_name)
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public ItemSpriteHolder getItemSprite(Item it) {
        foreach(var i in items) {
            if(i.preset.i_name == it.i_name)
                return i.preset.getSpriteHolder();
        }
        return null;
    }

    public Sprite getMapLocationSprite(MapLocation loc) {
        switch(loc.type) {
            case MapLocation.locationType.town:
                return getTownLocationSprite();

            case MapLocation.locationType.equipmentUpgrade:
                return getUpgradeLocationSprite((UpgradeLocation)loc);

            case MapLocation.locationType.nest:
                return getNestLocationSprite();

            case MapLocation.locationType.boss:
                return getBossFightLocationSprite((BossLocation)loc);

            case MapLocation.locationType.equipmentPickup:
                return getPickupLocationSprite((PickupLocation)loc);

            case MapLocation.locationType.rescue:
                return getRescueLocationSprite();
        }

        return null;
    }
    public Sprite getBossFightLocationSprite(BossLocation loc) {
        foreach(var i in loc.combatLocation.waves) {
            foreach(var j in i.enemies) {
                if(j.GetComponent<BossUnitInstance>() != null)
                    return j.GetComponent<BossUnitInstance>().getSpriteHolder().sprite;
            }
        }

        return null;
    }
    public Sprite getPickupLocationSprite(PickupLocation loc) {
        if(loc.pickupWeapon != null && !loc.pickupWeapon.isEmpty())
            return getWeaponSprite(loc.pickupWeapon).sprite;
        if(loc.pickupArmor != null && !loc.pickupArmor.isEmpty())
            return getArmorSprite(loc.pickupArmor).sprite;
        if(loc.pickupConsumable != null && !loc.pickupConsumable.isEmpty())
            return getConsumableSprite(loc.pickupConsumable).sprite;
        if(loc.pickupItem != null && !loc.pickupItem.isEmpty())
            return getItemSprite(loc.pickupItem).sprite;
        return null;
    }
    public Sprite getUpgradeLocationSprite(UpgradeLocation loc) {
        if(loc.state == 0)
            return weaponUpgradeIcon;
        else if(loc.state == 1)
            return armorUpgradeIcon;
        return null;
    }
    public Sprite getNestLocationSprite() {
        return nestIcon;
    }
    public Sprite getRescueLocationSprite() {
        return getPlayerUnitSprite().sprite;
    }
    public Sprite getTownLocationSprite() {
        return townIcon;
    }
}


[System.Serializable]
public class CombatScarSpriteHolder {
    public Sprite sprite;

    public float scale;
    public float rot;
}
