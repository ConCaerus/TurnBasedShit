using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUnit : MonoBehaviour {
    [SerializeField] GameObject fishingRodPreset;

    GameObject currentRod = null;


    private void Start() {
        GetComponent<UnitSpriteHandler>().setReference(Party.getLeaderStats(), false);
        setVisuals();
    }

    private void Update() {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f * FindObjectOfType<FishingCanvas>().fishSlider.value);
        if(currentRod != null) {
            currentRod.transform.parent.transform.rotation = Quaternion.Euler(0.0f, 0.0f, (-45/2.0f) + 45.0f * FindObjectOfType<FishingCanvas>().reelSlider.value);
        }
    }


    public void setVisuals() {
        GetComponent<UnitSpriteHandler>().updateVisuals();
        GetComponent<UnitSpriteHandler>().setFishingAnim(true);

        StartCoroutine(addRod());
    }

    IEnumerator addRod() {
        yield return new WaitForEndOfFrame();
        var rodHolder = Instantiate(fishingRodPreset.gameObject, transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0));
        rodHolder.transform.localPosition = Vector2.zero;

        var rod = rodHolder.transform.GetChild(0).gameObject;

        var we = FindObjectOfType<PresetLibrary>().getWeapon("Fishing Rod");
        WeaponSpriteHolder sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(we);
        rod.transform.localPosition = new Vector3(0.31f, 0.76f);   //  from the animation
        rod.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        rod.transform.localScale = new Vector3(sprite.equippedXSize, sprite.equippedYSize);

        if(currentRod != null)
            Destroy(currentRod.transform.parent.gameObject);
        currentRod = rod;
    }
}
