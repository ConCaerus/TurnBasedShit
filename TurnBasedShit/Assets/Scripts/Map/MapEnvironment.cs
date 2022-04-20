using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEnvironment : MonoBehaviour {
    [SerializeField] GameObject background;
    [SerializeField] Tilemap environmentMap, backgroundMap;
    [SerializeField] Color[] regionColors = new Color[5];
    [SerializeField] List<Tile> grasslandTiles = new List<Tile>();
    [SerializeField] List<Tile> forestTiles = new List<Tile>();
    [SerializeField] List<Tile> swampTiles = new List<Tile>();
    [SerializeField] List<Tile> mountainTiles = new List<Tile>();
    [SerializeField] List<Tile> hellTiles = new List<Tile>();
    [SerializeField] List<Tile> backgroundTiles = new List<Tile>();

    [System.Serializable]
    public class MapEnvironmentalInfo {
        public List<Vector3Int> info = new List<Vector3Int>();
    }

    string envTag(GameInfo.region reg) {
        switch(reg) {
            case GameInfo.region.grassland: return "GrasslandEnvironmentals";
            case GameInfo.region.forest: return "ForestEnvironmentals";
            case GameInfo.region.swamp: return "SwampEnvironmentals";
            case GameInfo.region.mountains: return "MountainEnvironmentals";
            case GameInfo.region.hell: return "HellEnvironmentals";
            default: return "";
        }
    }




    private void Start() {
        background.transform.localScale = new Vector3(Map.width * 2, Map.height * 2);
        background.GetComponent<SpriteRenderer>().color = regionColors[(int)GameInfo.getCurrentRegion()];

        spawnEnvironment();

        backgroundMap.BoxFill(backgroundMap.origin, backgroundTiles[(int)GameInfo.getCurrentRegion()], backgroundMap.cellBounds.min.x, backgroundMap.cellBounds.min.y, backgroundMap.cellBounds.max.x, backgroundMap.cellBounds.max.y);
    }

    void spawnEnvironment() {
        List<Tile> tiles = getRelevantTiles();
        if(!string.IsNullOrEmpty(SaveData.getString(envTag(GameInfo.getCurrentRegion())))) { //  load
            var data = SaveData.getString(envTag(GameInfo.getCurrentRegion()));
            var info = JsonUtility.FromJson<MapEnvironmentalInfo>(data);
            foreach(var i in info.info) {
                environmentMap.SetTile(new Vector3Int(i.x, i.y, 0), tiles[i.z]);
            }
        }
        else {  //  save
            var info = new MapEnvironmentalInfo();
            for(int i = 0; i < getEnvCount(GameInfo.getCurrentRegion()); i++) {
                Vector2Int pos = new Vector2Int((int)Map.getRandPos().x, (int)Map.getRandPos().y);
                int index = Random.Range(0, tiles.Count);
                environmentMap.SetTile((Vector3Int)pos, tiles[index]);
                info.info.Add(new Vector3Int(pos.x, pos.y, index));
            }
            var data = JsonUtility.ToJson(info);
            SaveData.setString(envTag(GameInfo.getCurrentRegion()), data);
        }
    }

    int getEnvCount(GameInfo.region reg) {
        switch(reg) {
            case GameInfo.region.grassland: return 150;
            case GameInfo.region.forest: return 250;
            case GameInfo.region.swamp: return 200;
            case GameInfo.region.mountains: return 150;
            case GameInfo.region.hell: return 150;
            default: return 0;
        }
    }
    List<Tile> getRelevantTiles() {
        return (int)GameInfo.getCurrentRegion() == 0 ? grasslandTiles : (int)GameInfo.getCurrentRegion() == 1 ? forestTiles : (int)GameInfo.getCurrentRegion() == 2 ? swampTiles : (int)GameInfo.getCurrentRegion() == 3 ? mountainTiles : hellTiles;
    }
}
