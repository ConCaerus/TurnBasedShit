using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultsCanvas : MonoBehaviour {
    [SerializeField] List<GameObject> equipmentIcons = new List<GameObject>();


    public void showCombatLocationEquipment() {
        int index = 0;

        //  weapons
        foreach(var i in GameInfo.getCombatDetails().weapons) {
            var obj = Instantiate(equipmentIcons[index]);
            obj.GetComponent<Image>().sprite = i.w_sprite.getSprite();

            obj.transform.position = equipmentIcons[index].transform.position;
            obj.transform.SetParent(equipmentIcons[index].transform);

            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            index++;
        }
    }

    //  Buttons
    public void returnToMap() {
        SceneManager.LoadScene("Map");
    }
}
