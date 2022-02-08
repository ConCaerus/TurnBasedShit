using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTransformSetter : MonoBehaviour {
    public GameObject[] bodies, heads;
    public GameObject body, head;
    [Header("Weapon")]
    public GameObject weaponObj;
    public WeaponPreset wePreset;

    [Header("Armor")]
    public GameObject armorObj;
    public GameObject shoulderObj, hatObj;
    public ArmorPreset arPreset;
    [Range(0, 2)]
    public int armorBodyIndex;
    [Range(0, 2)]
    public int armorHeadIndex;

    public bool setTransform() {
        if(wePreset != null && arPreset == null && weaponObj != null && weaponObj.GetComponent<SpriteRenderer>().sprite != null) {  //  weapon 
            wePreset.preset.sprite.pos = weaponObj.transform.localPosition;
            wePreset.preset.sprite.size = weaponObj.transform.localScale;
            wePreset.preset.sprite.rot = weaponObj.transform.localRotation.eulerAngles.z;

            weaponObj.GetComponent<SpriteRenderer>().sprite = null;
            return true;
        }

        else if(arPreset != null && wePreset == null) { //  armor
            if(armorObj != null && armorObj.GetComponent<SpriteRenderer>().sprite != null) {
                arPreset.preset.sprite.pos[armorBodyIndex] = armorObj.transform.localPosition;
                arPreset.preset.sprite.size[armorBodyIndex] = armorObj.transform.localScale;

                armorObj.GetComponent<SpriteRenderer>().sprite = null;
            }

            if(shoulderObj != null && shoulderObj.GetComponent<SpriteRenderer>().sprite != null) {
                arPreset.preset.sprite.shoulderPos[armorBodyIndex] = shoulderObj.transform.localPosition;
                arPreset.preset.sprite.shoulderSize[armorBodyIndex] = shoulderObj.transform.localScale;
                arPreset.preset.sprite.shoulderRot = shoulderObj.transform.localRotation.eulerAngles.z;

                shoulderObj.GetComponent<SpriteRenderer>().sprite = null;
            }

            if(hatObj != null && hatObj.GetComponent<SpriteRenderer>().sprite != null) {
                arPreset.preset.sprite.hatPos[armorHeadIndex] = hatObj.transform.localPosition;
                arPreset.preset.sprite.hatSize[armorHeadIndex] = hatObj.transform.localScale;
                arPreset.preset.sprite.hatRot = hatObj.transform.localRotation.eulerAngles.z;
                arPreset.preset.sprite.hatInfrontOfHead = hatObj.GetComponent<SpriteRenderer>().sortingOrder > 0;

                hatObj.GetComponent<SpriteRenderer>().sprite = null;
            }
            return true;
        }
        return false;
    }
    public void veiwForAll() {
        clearSprites();
        var prevBody = body;
        var prevHead = head;
        if(wePreset != null || arPreset != null) {
            for(int i = 0; i < bodies.Length; i++) {
                body = bodies[i];
                head = heads[i];

                applyBody();
                applyHead();

                applySprites();
            }
        }

        body = prevBody;
        head = prevHead;
        applyBody();
        applyHead();
    }

    
    public void applySprites() {
        if(wePreset != null && wePreset.preset != null && wePreset.preset.name != "") {
            if(weaponObj != null) {
                weaponObj.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(wePreset.preset);
                var sp = FindObjectOfType<PresetLibrary>().getWeaponSprite(wePreset.preset);
                if(sp != null && sp.size.x != 0.0f) {
                    weaponObj.transform.localPosition = sp.pos;
                    weaponObj.transform.localScale = sp.size;
                    weaponObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.rot);
                }
            }
        }
        else if(arPreset != null && arPreset.preset != null && arPreset.preset.name != "") {
            var sp = FindObjectOfType<PresetLibrary>().getArmorSprite(arPreset.preset);
            if(armorObj != null) {
                armorObj.GetComponent<SpriteRenderer>().sprite = sp.equippedSprite;
                if(sp != null && sp.getRelevantSize(armorBodyIndex).x != 0.0f) {
                    armorObj.transform.localPosition = sp.getRelevantPos(armorBodyIndex);
                    armorObj.transform.localScale = sp.getRelevantSize(armorBodyIndex);
                }
            }
            if(shoulderObj != null) {
                shoulderObj.GetComponent<SpriteRenderer>().sprite = sp.equippedShoulder;
                if(sp != null && sp.getRelevantShoulderSize(armorBodyIndex).x != 0.0f) {
                    shoulderObj.transform.localPosition = sp.getRelevantShoulderPos(armorBodyIndex);
                    shoulderObj.transform.localScale = sp.getRelevantShoulderSize(armorBodyIndex);
                    shoulderObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.shoulderRot);
                }
            }
            if(hatObj != null) {
                hatObj.GetComponent<SpriteRenderer>().sprite = sp.equippedHat;
                if(sp != null && sp.getRelevantHatSize(armorHeadIndex).x != 0.0f) {
                    hatObj.transform.localPosition = sp.getRelevantHatPos(armorHeadIndex);
                    hatObj.transform.localScale = sp.getRelevantHatSize(armorHeadIndex);
                    hatObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.hatRot);
                }
            }
        }
    }
    public void clearSprites() {
        var prevBody = body;
        var prevHead = head;
        for(int i = 0; i < bodies.Length; i++) {
            body = bodies[i];
            head = heads[i];

            applyBody();
            applyHead();

            if(weaponObj != null)
                weaponObj.GetComponent<SpriteRenderer>().sprite = null;
            if(armorObj != null)
                armorObj.GetComponent<SpriteRenderer>().sprite = null;
            if(shoulderObj != null)
                shoulderObj.GetComponent<SpriteRenderer>().sprite = null;
            if(hatObj != null)
                hatObj.GetComponent<SpriteRenderer>().sprite = null;
        }

        body = prevBody;
        head = prevHead;
        applyBody();
        applyHead();
    }

    public void applyBody() {
        if(body.name == "Body0")
            armorBodyIndex = 0;
        else if(body.name == "Body1")
            armorBodyIndex = 1;
        else if(body.name == "Body2")
            armorBodyIndex = 2;

        foreach(var i in body.GetComponentsInChildren<Transform>()) {
            if(i.gameObject.name == "armor")
                armorObj = i.gameObject;
            else if(i.gameObject.name == "weapon")
                weaponObj = i.gameObject;
            else if(i.gameObject.name == "lArmArmor")
                shoulderObj = i.gameObject;
        }
    }
    public void applyHead() {
        if(head.name == "Head0")
            armorHeadIndex = 0;
        else if(head.name == "Head1")
            armorHeadIndex = 1;
        else if(head.name == "Head2")
            armorHeadIndex = 2;

        foreach(var i in head.GetComponentsInChildren<Transform>()) {
            if(i.gameObject.name == "Hat")
                hatObj = i.gameObject;
        }
    }
}
