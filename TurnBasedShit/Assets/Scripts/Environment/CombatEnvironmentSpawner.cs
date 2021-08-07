using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnvironmentSpawner : MonoBehaviour {
    [SerializeField] int maxObjCount = 25, minObjCount = 15;
    [SerializeField] List<GameObject> spriteObjPresets = new List<GameObject>();
    [SerializeField] float battleFeildX, battleFeildY;

    List<GameObject> instantiatedObjs = new List<GameObject>();



    public void spawnEnivronmentObjects() {
        foreach(var i in instantiatedObjs)
            Destroy(i.gameObject);
        instantiatedObjs.Clear();
        Camera camera = Camera.main;
        // gets top-right coord
        Vector2 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector2 botLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        for(int i = 0; i < Random.Range(minObjCount, maxObjCount + 1); i++) {
            var temp = Instantiate(spriteObjPresets[Random.Range(0, spriteObjPresets.Count)]);

            var randX = Random.Range(botLeft.x, topRight.x);
            var randY = Random.Range(botLeft.y, topRight.y);
            temp.transform.position = new Vector2(randX, randY);

            //  if the obj is placed inside the battlefield
            if(Mathf.Abs(randX) < battleFeildX || Mathf.Abs(randY) < battleFeildY)
                temp.transform.localScale /= 2.0f;
            temp.transform.SetParent(gameObject.transform);

            instantiatedObjs.Add(temp);
        }
    }
}
