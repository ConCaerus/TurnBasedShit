using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitBody : UnitCustomizables {
    public GameObject rArm, lArm, weapon, armor, rArmor, lArmor;
    public bool showingWeapon { get; private set; } = true;

    //  Overrides
    public override void setColor(Color c) {
        GetComponent<SpriteRenderer>().color = c;
        rArm.GetComponent<SpriteRenderer>().color = c;
        lArm.GetComponent<SpriteRenderer>().color = c;
    }
    public override void tweenToColor(Color c, float time) {
        GetComponent<SpriteRenderer>().DOColor(c, time);
        rArm.GetComponent<SpriteRenderer>().DOColor(c, time);
        lArm.GetComponent<SpriteRenderer>().DOColor(c, time);
    }
    public override Color getColor() {
        return GetComponent<SpriteRenderer>().color;
    }

    public override void setAllSpritesVisible(bool b) {
        GetComponent<SpriteRenderer>().enabled = b;
        rArm.GetComponent<SpriteRenderer>().enabled = b;
        lArm.GetComponent<SpriteRenderer>().enabled = b;
        weapon.GetComponent<SpriteRenderer>().enabled = b;
        armor.GetComponent<SpriteRenderer>().enabled = b;
        rArmor.GetComponent<SpriteRenderer>().enabled = b;
        lArmor.GetComponent<SpriteRenderer>().enabled = b;
    }

    public override void offsetLayer(int norm) {
        GetComponent<SpriteRenderer>().sortingOrder = norm;

        rArm.GetComponent<SpriteRenderer>().sortingOrder = norm - 2;
        lArm.GetComponent<SpriteRenderer>().sortingOrder = norm + 4;

        rArmor.GetComponent<SpriteRenderer>().sortingOrder = norm - 1;
        lArmor.GetComponent<SpriteRenderer>().sortingOrder = norm + 5;

        weapon.GetComponent<SpriteRenderer>().sortingOrder = norm - 3;
        armor.GetComponent<SpriteRenderer>().sortingOrder = norm + 1;
    }

    public override void triggerAttackAnim() {
        GetComponent<Animator>().SetTrigger("attack");
    }
    public override void triggerDefendAnim() {
        GetComponent<Animator>().SetTrigger("defend");
    }
    public override void setWalkingAnim(bool b) {
        GetComponent<Animator>().SetBool("walking", b);
    }
    public override void setFishingAnim(bool b) {
        GetComponent<Animator>().SetBool("fishing", b);
    }
    public override bool isAnimIdle() {
        return GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "BodyIdle";
    }
    public override void setAnimSpeed(float sp) {
        GetComponent<Animator>().speed = sp;
    }

    //  Unique functions
    public void setWeapon(Weapon we) {
        if(we == null || we.isEmpty()) {
            weapon.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        var sp = FindObjectOfType<PresetLibrary>().getWeaponSprite(we);
        weapon.transform.localPosition = sp.pos;
        weapon.transform.localScale = sp.size;
        weapon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.rot);
        weapon.GetComponent<SpriteRenderer>().sprite = sp.sprite;
    }
    public void setArmor(Armor ar, int bodyIndex) {
        if(ar == null || ar.isEmpty()) {
            armor.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        var sp = FindObjectOfType<PresetLibrary>().getArmorSprite(ar);
        if(sp == null) {
            Debug.Log(ar.name);
        }

        if(sp.equippedSprite != null) { //  body
            armor.GetComponent<SpriteRenderer>().sprite = sp.equippedSprite;
            armor.transform.localPosition = sp.getRelevantPos(bodyIndex);
            armor.transform.localScale = sp.getRelevantSize(bodyIndex);
        }

        if(sp.equippedShoulder != null) {   //  shoulders
            rArmor.GetComponent<SpriteRenderer>().sprite = sp.equippedShoulder;
            rArmor.transform.localPosition = sp.getRelevantShoulderPos(bodyIndex);
            rArmor.transform.localScale = sp.getRelevantShoulderSize(bodyIndex);
            rArmor.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.shoulderRot);

            lArmor.GetComponent<SpriteRenderer>().sprite = sp.equippedShoulder;
            lArmor.transform.localPosition = sp.getRelevantShoulderPos(bodyIndex);
            lArmor.transform.localScale = sp.getRelevantShoulderSize(bodyIndex);
            lArmor.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.shoulderRot);
        }
        //  hat is done in the head script
    }

    public void setShowingWeapon(bool b) {
        showingWeapon = b;
        weapon.gameObject.SetActive(b);
    }
}
