using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetLibrary : MonoBehaviour {
    //  Units
    [SerializeField] GameObject playerUnit;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] bosses;
    [SerializeField] UnitTraitPreset[] unitTraits;

    //  unit customs
    [SerializeField] GameObject[] unitHeads;
    [SerializeField] Sprite[] unitFaces;
    [SerializeField] GameObject[] unitBodies;

    //  profiles
    [SerializeField] GameObject playerUnitProfile;
    [SerializeField] GameObject[] enemyProfiles;

    [SerializeField] CombatScarSpriteHolder[] combatScars;

    //  Equipment
    [SerializeField] WeaponPreset[] weapons;
    [SerializeField] ArmorPreset[] armor;
    [SerializeField] UsablePreset[] usables;
    [SerializeField] UnusablePreset[] unusables;
    [SerializeField] ItemPreset[] items;
    [SerializeField] EquipmentPair[] equipmentPairs;
    [SerializeField] SummonPair[] summonParis;

    //  starting equipment
    [SerializeField] int startingUnitCount;
    [SerializeField] WeaponPreset[] startingWeapons;
    [SerializeField] ArmorPreset[] startingArmor;
    [SerializeField] ItemPreset[] startingItems;


    //  Map
    [SerializeField] GameObject[] buildings;
    [SerializeField] GameObject townMember;

    [SerializeField] Color[] rarityColors;


    //  Units
    public void addStartingUnits() {
        Party.clearParty(true);
        Party.clearPartyEquipment();
        for(int i = 0; i < startingUnitCount; i++) {
            var stats = createRandomPlayerUnitStats(true);

            if(i < startingWeapons.Length && startingWeapons[i] != null && !startingWeapons[i].preset.isEmpty())
                stats.equippedWeapon = startingWeapons[i].preset;
            if(i < startingArmor.Length && startingArmor[i] != null && !startingArmor[i].preset.isEmpty())
                stats.equippedArmor = startingArmor[i].preset;
            if(i < startingItems.Length && startingItems[i] != null && !startingItems[i].preset.isEmpty())
                stats.equippedItem = startingItems[i].preset;

            Party.addUnit(stats);
        }
    }

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
    public GameObject getRandomEnemy(GameInfo.region diff = (GameInfo.region)(-1)) {
        GameObject enemy = null;
        //  no specified difficulty level
        if(diff != (GameInfo.region)(-1)) {
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
    public GameObject getRandomBoss(GameInfo.region diff = (GameInfo.region)(-1)) {
        GameObject boss = null;

        if(diff != (GameInfo.region)(-1)) {
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

        else if(diff == (GameInfo.region)(-1) || boss == null) {
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

    public int getRandomEnemyIndex(GameInfo.region diff = (GameInfo.region)(-1)) {
        //  no specified difficulty level
        if(diff != (GameInfo.region)(-1)) {
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
                if(s.isTheSameTypeAs(i.preset)) {
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


    //  Profiles
    public GameObject getProfileForUnit(UnitClass unit) {
        if(unit.isPlayerUnit)
            return getPlayerUnitProfile();

        return getEnemyProfile(unit.GetComponent<EnemyUnitInstance>().enemyType);
    }
    public GameObject getPlayerUnitProfile() {
        return playerUnitProfile;
    }
    public GameObject getEnemyProfile(EnemyUnitInstance.type type) {
        switch(type) {
            case EnemyUnitInstance.type.groundBird: return enemyProfiles[0];
            case EnemyUnitInstance.type.slime: return enemyProfiles[1];
            case EnemyUnitInstance.type.stumpSpider: return enemyProfiles[2];
            default: return playerUnitProfile;
        }
    }

    //  Equipment
    public List<Collectable> getAllCollectables(GameInfo.region lvl = (GameInfo.region)(-1)) {
        List<Collectable> temp = new List<Collectable>();
        if(lvl == (GameInfo.region)(-1)) {
            foreach(var i in weapons)
                temp.Add(i.preset);
            foreach(var i in armor)
                temp.Add(i.preset);
            foreach(var i in items)
                temp.Add(i.preset);
            foreach(var i in usables)
                temp.Add(i.preset);
            foreach(var i in unusables)
                temp.Add(i.preset);
            return temp;
        }

        foreach(var i in weapons) {
            if(i.preset.rarity == lvl)
                temp.Add(i.preset);
        }
        foreach(var i in armor) {
            if(i.preset.rarity == lvl)
                temp.Add(i.preset);
        }
        foreach(var i in items) {
            if(i.preset.rarity == lvl)
                temp.Add(i.preset);
        }
        foreach(var i in usables) {
            if(i.preset.rarity == lvl)
                temp.Add(i.preset);
        }
        foreach(var i in unusables) {
            if(i.preset.rarity == lvl)
                temp.Add(i.preset);
        }
        return temp;
    }
    public Weapon getWeapon(string name) {
        Weapon temp = new Weapon();
        foreach(var i in weapons) {
            if(i.preset.name == name) {
                temp.setEqualTo(i.preset, false);
                temp.instanceID = GameInfo.getNextWeaponInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Weapon getWeapon(Weapon we) {
        return getWeapon(we.name);
    }
    public Weapon getRandomWeapon(GameInfo.region lvl = (GameInfo.region)(-1)) {
        if(lvl == (GameInfo.region)(-1)) {
            return getWeapon(weapons[Random.Range(0, weapons.Length)].preset);
        }

        List<Weapon> useables = new List<Weapon>();
        foreach(var i in weapons) {
            //  same lvl
            if(i.preset.rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.rarity > lvl) {
                int lvlOffset = i.preset.rarity - lvl;
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
            if(i.preset.name == name) {
                temp.setEqualTo(i.preset, false);
                temp.instanceID = GameInfo.getNextArmorInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Armor getArmor(Armor a) {
        return getArmor(a.name);
    }
    public Armor getRandomArmor(GameInfo.region lvl = (GameInfo.region)(-1)) {
        if(lvl == (GameInfo.region)(-1)) {
            return getArmor(armor[Random.Range(0, armor.Length)].preset);
        }

        List<Armor> useables = new List<Armor>();
        foreach(var i in armor) {
            //  same lvl
            if(i.preset.rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.rarity > lvl) {
                int lvlOffset = i.preset.rarity - lvl;
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

    public Usable getUsable(string name) {
        Usable temp = new Usable();
        foreach(var i in usables) {
            if(i.preset.name == name) {
                temp.setEqualTo(i.preset, false);
                temp.instanceID = GameInfo.getNextUsableInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Usable getUsable(Usable c) {
        return getUsable(c.name);
    }
    public Usable getRandomUsable(GameInfo.region lvl = (GameInfo.region)(-1)) {
        if(lvl == (GameInfo.region)(-1)) {
            return getUsable(usables[Random.Range(0, usables.Length)].preset);
        }

        List<Usable> right = new List<Usable>();
        foreach(var i in usables) {
            //  same lvl
            if(i.preset.rarity == lvl)
                right.Add(i.preset);

            //  higher lvl
            if(i.preset.rarity > lvl) {
                int lvlOffset = i.preset.rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    right.Add(i.preset);
            }
        }

        if(right.Count > 0) {
            return getUsable(right[Random.Range(0, right.Count)]);
        }
        return getUsable(usables[Random.Range(0, usables.Length)].preset);
    }

    public Unusable getUnusable(string name) {
        Unusable temp = new Unusable();
        foreach(var i in unusables) {
            if(i.preset.name == name) {
                temp.setEqualTo(i.preset, false);
                temp.instanceID = GameInfo.getNextUsableInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Unusable getUnusable(Unusable c) {
        return getUnusable(c.name);
    }
    public Unusable getRandomUnusable(GameInfo.region lvl = (GameInfo.region)(-1)) {
        if(lvl == (GameInfo.region)(-1)) {
            return getUnusable(unusables[Random.Range(0, unusables.Length)].preset);
        }

        List<Unusable> right = new List<Unusable>();
        foreach(var i in unusables) {
            //  same lvl
            if(i.preset.rarity == lvl)
                right.Add(i.preset);

            //  higher lvl
            if(i.preset.rarity > lvl) {
                int lvlOffset = i.preset.rarity - lvl;
                float threshold = 75.0f / (float)lvlOffset;
                if(Random.Range(0, 101) < threshold)
                    right.Add(i.preset);
            }
        }

        if(right.Count > 0) {
            return getUnusable(right[Random.Range(0, right.Count)]);
        }
        return getUnusable(unusables[Random.Range(0, unusables.Length)].preset);
    }

    public Item getItem(string name) {
        Item temp = new Item();
        foreach(var i in items) {
            if(i.preset.name == name) {
                temp.setEqualTo(i.preset, false);
                temp.instanceID = GameInfo.getNextItemInstanceID();
                return temp;
            }
        }

        return null;
    }
    public Item getItem(Item i) {
        return getItem(i.name);
    }
    public Item getRandomItem(GameInfo.region lvl = (GameInfo.region)(-1)) {
        if(lvl == (GameInfo.region)(-1)) {
            return getItem(items[Random.Range(0, items.Length)].preset);
        }

        List<Item> useables = new List<Item>();
        foreach(var i in items) {
            //  same lvl
            if(i.preset.rarity == lvl)
                useables.Add(i.preset);

            //  higher lvl
            if(i.preset.rarity > lvl) {
                int lvlOffset = i.preset.rarity - lvl;
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

    public List<Collectable> getFishableCollectables(GameInfo.region diff = (GameInfo.region)(-1)) {
        List<Collectable> fishables = new List<Collectable>();
        foreach(var i in getAllCollectables(diff)) {
            if(i.canBeFished)
                fishables.Add(i);
        }

        return fishables;
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
    public CombatLocation createCombatLocation(GameInfo.region lvl) {
        int waveCount = Random.Range(1, 4);
        var loc = new CombatLocation(lvl, this, waveCount);
        loc.difficulty = lvl;

        loc.coinReward = 2 * ((int)lvl + 1) * waveCount; // default value
        loc.coinReward += (int)Random.Range(loc.coinReward * -0.1f, loc.coinReward * 0.1f);   //  randomizes it

        return loc;
    }
    public BossLocation createRandomBossLocation() {
        var boss = getRandomBoss();
        return new BossLocation(Map.getRandPos(), boss, boss.GetComponent<BossUnitInstance>().enemyDiff, this);
    }
    public PickupLocation createRandomPickupLocation() {
        int type = Random.Range(0, 4);
        if(type == 0)
            return new PickupLocation(Map.getRandPos(), getRandomWeapon(), this, GameInfo.getCurrentRegion());
        else if(type == 1)
            return new PickupLocation(Map.getRandPos(), getRandomArmor(), this, GameInfo.getCurrentRegion());
        else if(type == 2)
            return new PickupLocation(Map.getRandPos(), getRandomUsable(), Random.Range(1, 16), this, GameInfo.getCurrentRegion());
        else
            return new PickupLocation(Map.getRandPos(), getRandomItem(), this, GameInfo.getCurrentRegion());
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
            var things = new List<Usable>();
            var con = getRandomUsable();
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

    public Sprite getGenericSpriteForCollectable(Collectable c) {
        switch(c.type) {
            case Collectable.collectableType.weapon:
                return getWeaponSprite((Weapon)c).sprite;
            case Collectable.collectableType.armor:
                return getArmorSprite((Armor)c).sprite;
            case Collectable.collectableType.item:
                return getItemSprite((Item)c).sprite;
            case Collectable.collectableType.usable:
                return getUsableSprite((Usable)c).sprite;
            case Collectable.collectableType.unusable:
                return getUnusableSprite((Unusable)c).sprite;
            default:
                return null;
        }
    }
    public WeaponSpriteHolder getWeaponSprite(Weapon we) {
        foreach(var i in weapons) {
            if(i.preset.isTheSameTypeAs(we))
                return i.preset.getSpriteHolder();
        }
        Debug.Log("here");
        return null;
    }
    public ArmorSpriteHolder getArmorSprite(Armor ar) {
        foreach(var i in armor) {
            if(i.preset.name == ar.name)
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public UsableSpriteHolder getUsableSprite(Usable con) {
        foreach(var i in usables) {
            if(i.preset.isTheSameTypeAs(con))
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public UnusableSpriteHolder getUnusableSprite(Unusable con) {
        foreach(var i in unusables) {
            if(i.preset.isTheSameTypeAs(con))
                return i.preset.getSpriteHolder();
        }
        return null;
    }
    public ItemSpriteHolder getItemSprite(Item it) {
        foreach(var i in items) {
            if(i.preset.name == it.name)
                return i.preset.getSpriteHolder();
        }
        return null;
    }

    public Usable getUsableFromSprite(Sprite s) {
        foreach(var i in usables) {
            if(i.preset.getSpriteHolder().sprite == s)
                return i.preset;
        }
        return null;
    }
    public Unusable getUnusableFromSprite(Sprite s) {
        foreach(var i in unusables) {
            if(i.preset.getSpriteHolder().sprite == s)
                return i.preset;
        }
        return null;
    }

    public Color getRarityColor(GameInfo.region lvl) {
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
