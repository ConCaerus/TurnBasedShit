using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class MapLootCanvas : MonoBehaviour {
    public SlotMenu slot;

    [SerializeField] TextMeshProUGUI coinsText;

    private void Start() {
        hide();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.G))
            hide();
    }


    public void show(List<Collectable> loot, int coins) {
        gameObject.transform.GetChild(0).DOScale(1.0f, 0.05f);
        StartCoroutine(populateSlots(loot, coins));
    }

    public void hide() {
        gameObject.transform.GetChild(0).DOScale(0.0f, 0.15f);
    }



    IEnumerator populateSlots(List<Collectable> loot, int coins) {
        slot.destroySlots();
        if(coins != 0)
            coinsText.text = "Coins: " + coins.ToString() + "c";
        else
            coinsText.text = "";

        List<int> skipIndexes = new List<int>();
        for(int i = 0; i < loot.Count; i++) {
            bool skip = false;
            foreach(var s in skipIndexes) {
                if(i == s) {
                    skip = true;
                    break;
                }
            }

            if(skip)
                continue;

            yield return new WaitForSeconds(0.1f);

            var obj = slot.instantiateNewSlot(FindObjectOfType<PresetLibrary>().getRarityColor(loot[i].rarity));
            obj.GetComponent<SlotObject>().grow(.15f);
            obj.GetComponent<SlotObject>().setImage(1, FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(loot[i]));
            obj.GetComponent<SlotObject>().setImageColor(0, FindObjectOfType<PresetLibrary>().getRarityColor(loot[i].rarity));
            obj.GetComponent<SlotObject>().setInfo(loot[i].name);
            if(loot[i].maxStackCount == 1)
                obj.GetComponent<SlotObject>().setText(0, "");
            else {
                var count = 0;

                for(int s = i + 1; s < loot.Count; s++) {
                    if(loot[s].isTheSameTypeAs(loot[i])) {
                        skipIndexes.Add(s);
                        count++;
                        if(count >= loot[i].maxStackCount)
                            break;
                    }
                }

                if(count > 0)
                    obj.GetComponent<SlotObject>().setText(0, (count + 1).ToString());
                else
                    obj.GetComponent<SlotObject>().setText(0, "");
            }
        }
    }
}
