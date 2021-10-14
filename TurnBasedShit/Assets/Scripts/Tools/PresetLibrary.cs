using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Units
    [SerializeField] GameObject playerUnit;
    [SerializeField] GameObject[] startingUnits;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] bosses;
    [SerializeField] UnitTraitPreset[] unitTraits;

    //  unit customs
    [SerializeField] GameObject[] unitHeads;
    [SerializeField] Sprite[] unitFaces;
    [SerializeField] GameObject[] unitBodies;

    [SerializeField] CombatScarSpriteHolder[] combatScars;

    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] ConsumablePreset[] consumables;
    [SerializeField] ItemPreset[] items;
    [SerializeField] EquipmentPair[] equipmentPairs;
    [SerializeField] SummonPair[] summonParis;

    //  Map
    [SerializeField] GameObject[] buildings;
    [SerializeField] GameObject townMember;

    [SerializeField] Color[] rarityColors;


    //  Units
    public UnitStats createRandomPlayerUnitStats(bool setID) {
        var stats = new UnitStats(playerUnit.GetComponent<UnitClass>().stats);
        stats = Randomizer.randomizePlayerUnitStats(stats, this);

        if(setID)
            stats.u_instanceID = GameInfo.getNextUnitInstanceID();

        stats.setBaseMaxHealth(100.0f);
        stats.u_health = stats.getModifiedMaxHealth();
        return stats;
    }
    public GameObject getPlayerUnitObject() {
        return playerUnit.gameObject;
    }
    public GameObject getEnemy(int index) {
        return enemies[index].gameObject;
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
                    useables.Add(i.gameObject);

                //  higher difficulty
                else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff + 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i.gameObject);
                }

                //  lower difficulty
                else if(i.GetComponent<EnemyUnitInstance>().enemyDiff == diff - 1) {
                    if(Random.Range(0, 101) <= 25)
                        useables.Add(i.gameObject);
                }
            }

            if(useables.Count > 0)
                enemy = useables[Random.Range(0, useables.Count)].gameObject;
        }
        enemy = enemies[Random.Range(0, enemies.Length)].gameObject;
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

        boss.GetComponent<UnitClass>().stats.u_name = NameLibrary.getRandomEnemyName();
        return boss;
    }
    public GameObject getSummonForWeapon(Weapon we) {
        GameObject temp = null;
        foreach(var i in summonParis) {
            if(i.weapon.preset.isTheSameTypeAs(we)) {
                temp = i.summon.gameObject;
                break;
            }
        }

        return temp.gameObject;
    }

    public int getRandomEnemyIndex(GameInfo.diffLvl diff = (GameInfo.diffLvl)(-1)) {
        //  no specified difficulty level
        if(diff != (GameInfo.diffLvl)(-1)) {
            List<int> useables = new List<int>();
            useables.Clear();
            for(int i = 0; i < enemies.Length; i++) {
                if(enemies[i].GetComponent<EnemyUnitInstance>().enemyDiff == diff)
                    useables.Add(i);

                else if(enemies[i].GetComponent<EnemyUnitInstance>().enemyDiff == diff + 1) {
                    if(GameVariables.chanceOutOfHundred(25))
                        useables.Add(i);
                }

                else if(enemies[i].GetComponent<EnemyUnitInstance>().enemyDiff == diff - 1)
                    if(GameVariables.chanceOutOfHundred(25))
                        useables.Add(i);
            }

            if(useables.Count > 0)
                return useables[Random.Range(0, useables.Count)];
        }
        return Random.Range(0, enemies.Length);
    }
    public int getBossIndex(GameObject boss) {
        for(int i = 0; i < bosses.Length; i++) {
            if(bosses[i] == boss)
                return i;
        }
        return -1;
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
    public UnitTrait getRandomUnusedUnitTrait(UnitStats stats) {
        List<UnitTrait> temp = new List<UnitTrait>();

        foreach(var i in unitTraits) {
            bool hasTrait = false;
            foreach(var s in stats.u_traits) {
                if(s == i.preset) {
                    hasTrait = true;
                    break;
                }
            }
            if(!hasTrait)
                temp.Add(i.preset);
        }

        return temp[Random.Range(0, temp.Count)];
    }
    public UnitTrait getUnitTrait(string name) {
        foreach(var i in unitTraits) {
            if(i.preset.t_name == name)
                return i.preset;
        }
        return null;
    }


    //  unit customs
    public GameObject getUnitHead(int index) {
        return unitHeads[index];
    }
    public Sprite getUnitFace(int index) {
        return unitFaces[index];
    }
    public GameObject getUnitBody(int index) {
        return unitBodies[index];
    }
    public GameObject getUnitArm(int index) {
        return getUnitBody(index).transform.GetChild(0).GetChild(0).gameObject;
    }

    public GameObject getRandomUnitHead() {
        return getUnitHead(Random.Range(0, unitHeads.Length));
    }
    public Sprite getRandomUnitFace() {
        return getUnitFace(Random.Range(0, unitFaces.Length));
    }
    public GameObject getRandomUnitBody() {
        return getUnitBody(Random.Range(0, unitBodies.Length));
    }
    public GameObject getRandomUnitArm() {
        return getUnitArm(Random.Range(0, unitBodies.Length));
    }

    public Sprite getUnitHeadSprite(int index) {
        return getUnitHead(index).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
    public Sprite getUnitBodySprite(int index) {
        return getUnitBody(index).GetComponent<SpriteRenderer>().sprite;
    }
    public Sprite gettUnitArmSprite(int index) {
        return getUnitArm(index).GetComponent<SpriteRenderer>().sprite;
    }

    public int getHeadCount() {
        return unitHeads.Length;
    }
    public int getFaceCount() {
        return unitFaces.Length;
    }
    public int getBodyCount() {
        return unitBodies.Length;
    }

    //  Equipment
    public Weapon getWeapon(string name, GameInfo.element ele) {
        Weapon temp = new Weapon();
        foreach(var i in weapons) {
            if(i.preset.w_name == name && ele == i.preset.w_element) {
                temp.setEqualTo(i.preset, false);
                temp.w_instanceID = GameInfo.getNextWeaponInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Weapon getWeapon(Weapon we) {
        return getWeapon(we.w_name, we.w_element);
    }
    public Weapon getRandomWeapon(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {

        if(lvl == (GameInfo.rarityLvl)(-1)) {
            return getWeapon(weapons[Random.Range(0, weapons.Length)].preset);
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
            return getWeapon(useables[Random.Range(0, useables.Count)]);
        }
        return getWeapon(weapons[Random.Range(0, weapons.Length)].preset);
    }

    public Armor getArmor(string name) {
        Armor temp = new Armor();
        foreach(var i in armor) {
            if(i.preset.a_name == name) {
                temp.setEqualTo(i.preset, false);
                temp.a_instanceID = GameInfo.getNextArmorInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Armor getArmor(Armor a) {
        return getArmor(a.a_name);
    }
    public Armor getRandomArmor(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1)) {
            return getArmor(armor[Random.Range(0, armor.Length)].preset);
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
            return getArmor(useables[Random.Range(0, useables.Count)]);
        }
        //  if no useables, return random from all
        return getArmor(armor[Random.Range(0, armor.Length)].preset);
    }

    public Consumable getConsumable(string name) {
        Consumable temp = new Consumable();
        foreach(var i in consumables) {
            if(i.preset.c_name == name) {
                temp.setEqualTo(i.preset, false);
                temp.c_instanceID = GameInfo.getNextConsumableInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Consumable getConsumable(Consumable c) {
        return getConsumable(c.c_name);
    }
    public Consumable getRandomConsumable(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1)) {
            return getConsumable(consumables[Random.Range(0, consumables.Length)].preset);
        }

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

        if(useables.Count > 0) {
            return getConsumable(useables[Random.Range(0, useables.Count)]);
        }
        return getConsumable(consumables[Random.Range(0, consumables.Length)].preset);
    }

    public Item getItem(string name) {
        Item temp = new Item();
        foreach(var i in items) {
            if(i.preset.i_name == name) {
                temp.setEqualTo(i.preset, false);
                temp.i_instanceID = GameInfo.getNextItemInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Item getItem(Item i) {
        return getItem(i.i_name);
    }
    public Item getRandomItem(GameInfo.rarityLvl lvl = (GameInfo.rarityLvl)(-1)) {
        if(lvl == (GameInfo.rarityLvl)(-1)) {
            return getItem(items[Random.Range(0, items.Length)].preset);
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
            return getItem(useables[Random.Range(0, useables.Count)]);
        }
        return getItem(items[Random.Range(0, items.Length)].preset);
    }

    public EquipmentPair getRelevantPair(UnitStats stats) {
        foreach(var i in equipmentPairs) {
            if(i.checkIfApplys(stats))
                return i;
        }
        return null;
    } 

    //  Map
    public TownMember createRandomTownMember(bool autoHasQuest = false) {
        return new TownMember(this, true, autoHasQuest);
    }
    public GameObject getTownMemberObj(bool createNewStats, bool autoHasQuest = false) {
        var member = townMember;
        if(createNewStats)
            member.GetComponentInChildren<TownMemberInstance>().reference.setEqualsTo(createRandomTownMember(), true);
        return member;
    }

    public GameObject getBuilding(Building.type t) {
        foreach(var i in buildings) {
            if(t == Building.type.Hospital && i.GetComponent<HospitalInstance>() != null)
                return i.gameObject;

            if(t == Building.type.Church && i.GetComponent<ChurchInstance>() != null)
                return i.gameObject;

            if(t == Building.type.Shop && i.GetComponent<ShopInstance>() != null)
                return i.gameObject;

            if(t == Building.type.Casino && i.GetComponent<CasinoInstance>() != null)
                return i.gameObject;

            if(t == Building.type.Blacksmith && i.GetComponent<BlacksmithInstance>() != null)
                return i.gameObject;
        }
        return null;
    }
    public GameObject getRandomBuilding() {
        return buildings[Random.Range(0, buildings.Length)].gameObject;
    }

    //  special locations
    public CombatLocation createCombatLocation(GameInfo.diffLvl lvl) {
        int waveCount = Random.Range(1, 4);
        var loc = new CombatLocation(lvl, this, waveCount);
        loc.difficulty = lvl;

        loc.coinReward = 2 * ((int)lvl + 1) * waveCount; // default value
        loc.coinReward += (int)Random.Range(loc.coinReward * -0.1f, loc.coinReward * 0.1f);   //  randomizes it

        //  chance for weapon
        while(GameVariables.chanceWeaponDrop())
            loc.weapons.Add(Randomizer.randomizeWeapon(getRandomWeapon((GameInfo.rarityLvl)lvl), lvl));

        //  chance for armor
        while(GameVariables.chanceArmorDrop())
            loc.armor.Add(Randomizer.randomizeArmor(getRandomArmor((GameInfo.rarityLvl)lvl), lvl));

        //  chance for item
        while(GameVariables.chanceItemDrop())
            loc.items.Add(getRandomItem((GameInfo.rarityLvl)lvl));

        //  chance for consumable
        while(GameVariables.chanceConsumableDrop())
            loc.consumables.Add(getRandomConsumable((GameInfo.rarityLvl)lvl));

        return loc;
    }
    public BossLocation createRandomBossLocation() {
        var boss = getRandomBoss();
        return new BossLocation(Map.getRandPos(), boss, boss.GetComponent<BossUnitInstance>().enemyDiff, this);
    }
    public PickupLocation createRandomPickupLocation() {
        int type = Random.Range(0, 4);
        if(type == 0)
            return new PickupLocation(Map.getRandPos(), getRandomWeapon(), this, GameInfo.getCurrentDiff());
        else if(type == 1)
            return new PickupLocation(Map.getRandPos(), getRandomArmor(), this, GameInfo.getCurrentDiff());
        else if(type == 2)
            return new PickupLocation(Map.getRandPos(), getRandomConsumable(), Random.Range(1, 16), this, GameInfo.getCurrentDiff());
        else
            return new PickupLocation(Map.getRandPos(), getRandomItem(), this, GameInfo.getCurrentDiff());
    }
    public RescueLocation createRandomRescueLocation() {
        return new RescueLocation(Map.getRandPos(), this);
    }
    public UpgradeLocation createRandomUpgradeLocation() {
        return new UpgradeLocation(Map.getRandPos(), Random.Range(0, 2));
    }


    public BossFightQuest createRandomBossFightQuest(bool setID) {
        return new BossFightQuest(createRandomBossLocation(), setID);
    }
    public PickupQuest createRandomPickupQuest(bool setID) {
        return new PickupQuest(createRandomPickupLocation(), setID);
    }
    public KillQuest createRandomKillQuest(bool setID) {
        return new KillQuest(Random.Range(5, 36), getRandomEnemy().GetComponent<EnemyUnitInstance>().enemyType, setID);   //  change this number too
    }
    public DeliveryQuest createRandomDeliveryQuest(bool setID) {
        int townInd = 0;
        //  no towns
        if(MapLocationHolder.getTownCount() == 0) {
            Vector2 pos = Map.getRandPos();
            MapLocationHolder.addLocation(new TownLocation(pos, Map.getDiffForX(pos.x), this));
        }
        else {
            townInd = Random.Range(0, MapLocationHolder.getTownCount());
            if(GameInfo.getCurrentLocationAsTown() != null) {
                while(MapLocationHolder.getTownLocation(townInd).isEqualTo(GameInfo.getCurrentLocationAsTown()))
                    townInd = Random.Range(0, MapLocationHolder.getTownCount());
            }
        }

        //  create delivery objecs
        int rand = Random.Range(0, 5);
        if(rand == 0) {
            var things = new List<Weapon>();
            for(int i = 0; i < Random.Range(1, 4); i++)
                things.Add(getRandomWeapon());

            return new DeliveryQuest(MapLocationHolder.getTownLocation(townInd), things, setID);
        }
        if(rand == 1) {
            var things = new List<Armor>();
            for(int i = 0; i < Random.Range(1, 4); i++)
                things.Add(getRandomArmor());

            return new DeliveryQuest(MapLocationHolder.getTownLocation(townInd), things, setID);
        }
        if(rand == 2) {
            var things = new List<Consumable>();
            var con = getRandomConsumable();
            for(int i = 0; i < Random.Range(1, 26); i++)
                things.Add(con);

            return new DeliveryQuest(MapLocationHolder.getTownLocation(townInd), things, setID);
        }
        if(rand == 3) {
            var things = new List<Item>();
            for(int i = 0; i < Random.Range(0, 4); i++)
                things.Add(getRandomItem());

            return new DeliveryQuest(MapLocationHolder.getTownLocation(townInd), things, setID);
        }
        if(rand == 4) {
            var things = new List<UnitStats>();
            for(int i = 0; i < Random.Range(1, 3); i++)
                things.Add(createRandomPlayerUnitStats(true));

            return new DeliveryQuest(MapLocationHolder.getTownLocation(townInd), things, setID);
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
            if(i.preset.isTheSameTypeAs(con))
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

    public Color getRarityColor(GameInfo.rarityLvl lvl) {
        return rarityColors[(int)lvl];
    }
}


[System.Serializable]
public class CombatScarSpriteHolder {
    public Sprite sprite;

    public float scale;
    public float rot;
}

[System.Serializable]
public class SummonPair {
    public WeaponPreset weapon;
    public GameObject summon;
}
