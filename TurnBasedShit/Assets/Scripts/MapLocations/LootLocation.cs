using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootLocation : MapLocation {

    public ObjectHolder collectables;
    public int coins;

    public LootLocation(Vector2 p, GameInfo.region reg, PresetLibrary lib) {
        type = locationType.loot;
        region = reg;
        pos = p;

        collectables = new ObjectHolder();

        int maxLootCount = 5;
        var pool = lib.getAllCollectables(GameInfo.getCurrentRegion());

        for(int i = 0; i < maxLootCount; i++) {
            var thing = pool[Random.Range(0, pool.Count)];
            //  thing comes in singles, so just add of of the thing
            if(thing.maxStackCount == 1) {
                collectables.addObject<Collectable>(thing);
            }

            //  thing comes in a stack, so add a random amount from 1 to have of the max stack
            else if(thing.maxStackCount > 1) {
                var randStackCount = Random.Range(1, (int)(thing.maxStackCount / 2f) + 1);
                for(int j = 0; j < randStackCount; j++)
                    collectables.addObject<Collectable>(thing);
            }

            else
                Debug.Log("fuck");
        }

        coins = Random.Range(1, 4) * ((int)GameInfo.getCurrentRegion() + 1);
    }


    public void activate(MapLootCanvas canvas, PresetLibrary lib, FullInventoryCanvas fic) {
        //  show canvas that shows what collectables were just gotted

        Inventory.addCollectables(collectables.getCollectables(), lib, fic);
        Inventory.addCoins(coins);

        canvas.show(collectables.getCollectables(), coins);

        MapLocationHolder.removeLocation(this);
    }

    public override void enterLocation(TransitionCanvas tc) {
    }
}
