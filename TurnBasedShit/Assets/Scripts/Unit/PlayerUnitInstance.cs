using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnitInstance : UnitClass {
    public GameObject equippedWeaponPosition;
    public GameObject equippedArmorPosition;

    private void Awake() {
        isPlayerUnit = true;
    }

    private void Start() {
        updateShownEquipment();
    }


    private void Update() {
        attackingLogic();
    }


    void attackingLogic() {
        if(FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject && attackingTarget == null) {
            setAttackingTarget();
        }

        else if(attackingTarget != null && FindObjectOfType<TurnOrderSorter>().playingUnit == gameObject) {
            attack(attackingTarget);
        }
    }

    void setAttackingTarget() {
        if(Input.GetMouseButtonDown(0)) {
            foreach(var i in FindObjectsOfType<UnitClass>()) {
                if(i.isMouseOverUnit) {
                    attackingTarget = i.gameObject;
                    return;
                }
            }
        }
    }

    public void updateShownEquipment() {
        //  weapon shit
        var w = stats.equippedWeapon;
        if(w != null && !w.isEmpty()) {
            equippedWeaponPosition.transform.localPosition = new Vector2(FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedX, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedY);
            equippedWeaponPosition.transform.localScale = new Vector3(-FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedSize, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedSize, 0.0f);
            equippedWeaponPosition.transform.rotation = Quaternion.Euler(0.0f, 0.0f, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedRot);
            equippedWeaponPosition.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(w).sprite;
        }
        else {
            equippedWeaponPosition.GetComponent<SpriteRenderer>().sprite = null;
        }

        //  armor shit
        var a = stats.equippedArmor;
        if(a != null && !a.isEmpty()) {
            equippedArmorPosition.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(a).equippedSprite;
        }
        else
            equippedArmorPosition.GetComponent<SpriteRenderer>().sprite = null;
    }
}
