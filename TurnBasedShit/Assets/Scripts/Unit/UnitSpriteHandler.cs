﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitSpriteHandler : MonoBehaviour {
    public GameObject head, face, body, rArm, lArm, weapon, armor, rShoulder, lShoulder, hat;
    bool showingFace = true, hatInfrontOfHead = true;
    UnitStats reference;
    int animState = 0;
    Color col;

    public bool showWeapon = true;

    Coroutine idler = null;


    public void setReference(UnitStats stats, bool shouldShowWeapon) {
        reference = stats;
        showWeapon = shouldShowWeapon;
        updateVisuals();
    }
    public void setReference(UnitSpriteInfo info, Weapon we, Armor ar, bool shouldShowWeapon) {
        reference = new UnitStats();
        reference.u_sprite.setEqualTo(info);
        
        if(we != null && !we.isEmpty()) {
            reference.weapon = new Weapon();
            reference.weapon.setEqualTo(we, false);
        }

        if(ar != null && !ar.isEmpty()) {
            reference.armor = new Armor();
            reference.armor.setEqualTo(ar, false);
        }

        showWeapon = shouldShowWeapon;
        updateVisuals();
    }


    public void updateVisuals() {
        StartCoroutine(waitToUpdate());
    }

    IEnumerator waitToUpdate() {
        yield return new WaitForEndOfFrame();

        setHead();
        setFace();
        setBody();
        setToNormalColor();
        offsetLayer();

        if(FindObjectOfType<PartyObject>() != null)
            FindObjectOfType<PartyObject>().repositionUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(GetComponentInParent<UnitClass>().stats));
    }

    public void setHead() {
        var hParent = FindObjectOfType<PresetLibrary>().getUnitHead(reference.u_sprite.headIndex);

        head.GetComponent<SpriteRenderer>().sprite = hParent.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        head.transform.parent.parent.localPosition = hParent.transform.localPosition;
        head.transform.parent.parent.localScale = hParent.transform.localScale;
        head.transform.parent.parent.localRotation = hParent.transform.localRotation;
        for(int i = 0; i < hParent.GetComponentsInChildren<Transform>().Length; i++) {
            head.transform.parent.GetComponentsInChildren<Transform>()[i].localPosition = hParent.GetComponentsInChildren<Transform>()[i].localPosition;
            head.transform.parent.GetComponentsInChildren<Transform>()[i].localScale = hParent.GetComponentsInChildren<Transform>()[i].localScale;
            head.transform.parent.GetComponentsInChildren<Transform>()[i].localRotation = hParent.GetComponentsInChildren<Transform>()[i].localRotation;
        }

        setFace();
        setArmor();
        setToNormalColor();
        offsetLayer();
    }
    public void setFace() {
        face.SetActive(showingFace);
        face.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(reference.u_sprite.faceIndex);
        setArmor();
        offsetLayer();
    }
    public void setBody() {
        var b = FindObjectOfType<PresetLibrary>().getUnitBody(reference.u_sprite.bodyIndex);

        body.GetComponent<SpriteRenderer>().sprite = b.GetComponent<SpriteRenderer>().sprite;
        body.transform.localPosition = b.transform.localPosition;
        body.transform.localScale = b.transform.localScale;
        body.transform.localRotation = b.transform.localRotation;
        for(int i = 0; i < b.GetComponentsInChildren<Transform>().Length; i++) {
            body.GetComponentsInChildren<Transform>()[i].transform.localPosition = b.GetComponentsInChildren<Transform>()[i].transform.localPosition;
            body.GetComponentsInChildren<Transform>()[i].transform.localScale = b.GetComponentsInChildren<Transform>()[i].transform.localScale;
            body.GetComponentsInChildren<Transform>()[i].transform.localRotation = b.GetComponentsInChildren<Transform>()[i].transform.localRotation;

            if(body.GetComponentsInChildren<Transform>()[i].GetComponent<SpriteRenderer>() != null)
                body.GetComponentsInChildren<Transform>()[i].GetComponent<SpriteRenderer>().sprite = b.GetComponentsInChildren<Transform>()[i].GetComponent<SpriteRenderer>().sprite;
        }
        setToNormalColor();
        setHead();

        setWeapon();
        setArmor();
        offsetLayer();
    }

    public void showFace() {
        showingFace = true;

        face.SetActive(true);
        face.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(reference.u_sprite.faceIndex);
    }
    public void hideFace() {
        showingFace = false;

        face.SetActive(false);
    }
    public bool isFaceShown() {
        return showingFace;
    }

    public void setToNormalColor() {
        var c = reference.u_sprite.color;
        //  body
        body.GetComponent<SpriteRenderer>().color = c;

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = c;
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = c;

        //  head
        head.GetComponent<SpriteRenderer>().color = c;
    }
    public void setATempColor(Color c) {
        //  body
        body.GetComponent<SpriteRenderer>().color = c;

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = c;
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = c;

        //  head
        head.GetComponent<SpriteRenderer>().color = c;
    }
    public void tweenColorToNormal(float time) {
        var c = reference.u_sprite.color;
        //  body
        body.GetComponent<SpriteRenderer>().DOColor(c, time);

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().DOColor(c, time);
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().DOColor(c, time);

        //  head
        head.GetComponent<SpriteRenderer>().DOColor(c, time);
    }
    public void tweenColor(Color c, float time) {
        //  body
        body.GetComponent<SpriteRenderer>().DOColor(c, time);

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().DOColor(c, time);
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().DOColor(c, time);

        //  head
        head.GetComponent<SpriteRenderer>().DOColor(c, time);
    }
    public Color getColor() {
        return col;
    }

    public void setWeapon() {
        var weapon = body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

        if(reference.weapon == null || reference.weapon.isEmpty() || !showWeapon) {
            weapon.GetComponent<SpriteRenderer>().sprite = null;
            weapon.GetComponent<SpriteRenderer>().color = Color.white;
            return;
        }

        var sp = FindObjectOfType<PresetLibrary>().getWeaponSprite(reference.weapon);
        weapon.transform.localPosition = new Vector2(sp.equippedX, sp.equippedY);
        weapon.transform.localScale = new Vector3(sp.equippedXSize, sp.equippedYSize, 0.0f);
        weapon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.equippedRot);
        weapon.GetComponent<SpriteRenderer>().sprite = sp.sprite;
    }
    public void setArmor() {
        var armor = body.transform.GetChild(2).gameObject;
        var rightShoulder = body.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        var leftShoulder = body.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        var hat = head.transform.GetChild(1).gameObject;

        if(reference.armor == null || reference.armor.isEmpty()) {
            armor.GetComponent<SpriteRenderer>().sprite = null;
            rightShoulder.GetComponent<SpriteRenderer>().sprite = null;
            leftShoulder.GetComponent<SpriteRenderer>().sprite = null;
            hat.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        var aSprite = FindObjectOfType<PresetLibrary>().getArmorSprite(reference.armor);

        if(aSprite.equippedSprite != null) {
            armor.GetComponent<SpriteRenderer>().sprite = aSprite.equippedSprite;
            armor.transform.localPosition = aSprite.getRelevantPos(reference.u_sprite.bodyIndex);
            armor.transform.localScale = aSprite.getRelevantSize(reference.u_sprite.bodyIndex);
        }

        if(aSprite.equippedShoulder != null) {
            rightShoulder.GetComponent<SpriteRenderer>().sprite = aSprite.equippedShoulder;
            rightShoulder.transform.localPosition = aSprite.getRelevantShoulderPos(reference.u_sprite.bodyIndex);
            rightShoulder.transform.localScale = aSprite.getRelevantShoulderSize(reference.u_sprite.bodyIndex);
            rightShoulder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.shoulderRot);

            leftShoulder.GetComponent<SpriteRenderer>().sprite = aSprite.equippedShoulder;
            leftShoulder.transform.localPosition = aSprite.getRelevantShoulderPos(reference.u_sprite.bodyIndex);
            leftShoulder.transform.localScale = aSprite.getRelevantShoulderSize(reference.u_sprite.bodyIndex);
            leftShoulder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.shoulderRot);
        }

        if(aSprite.equippedHat != null) {
            hatInfrontOfHead = aSprite.hatInfrontOfHead;
            hat.GetComponent<SpriteRenderer>().sprite = aSprite.equippedHat;
            hat.transform.localPosition = aSprite.getRelevantHatPos(reference.u_sprite.headIndex);
            hat.transform.localScale = aSprite.getRelevantHatSize(reference.u_sprite.headIndex);
            hat.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aSprite.hatRot);
        }
    }

    public Sprite getWeapon() {
        return body.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }


    public void triggerAttackAnim() {
        body.GetComponent<Animator>().SetTrigger("attack");
        head.GetComponent<Animator>().SetTrigger("attack");
    }
    public void triggerDefendAnim() {
        body.GetComponent<Animator>().SetTrigger("defend");
        head.GetComponent<Animator>().SetTrigger("defend");
    }
    public void setWalkingAnim(bool b) {
        body.GetComponent<Animator>().SetBool("walking", b);
        head.GetComponent<Animator>().SetBool("walking", b);
    }
    public void setFishingAnim(bool b) {
        body.GetComponent<Animator>().SetBool("fishing", b);
        head.GetComponent<Animator>().SetBool("fishing", b);
    }

    public bool isAnimIdle() {
        var b = body.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        var h = head.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);

        return b[0].clip.name == "BodyIdle" && h[0].clip.name == "HeadIdleAnim";
    }

    public void setAnimSpeed(float sp) {
        if(body != null && body.GetComponent<Animator>() != null)
            body.GetComponent<Animator>().speed = sp;
        if(head != null && head.GetComponent<Animator>() != null)
            head.GetComponent<Animator>().speed = sp;
    }


    void offsetLayer() {
        int normOffset = FindObjectOfType<PresetLibrary>().getRandomUnitBody().GetComponent<SpriteRenderer>().sortingOrder + reference.u_sprite.layerOffset;
        //  body
        body.GetComponent<SpriteRenderer>().sortingOrder = normOffset;

        //  arms
        body.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = normOffset - 2;
        body.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = normOffset + 4;

        //  arm armor
        rShoulder.GetComponent<SpriteRenderer>().sortingOrder = normOffset - 1;
        lShoulder.GetComponent<SpriteRenderer>().sortingOrder = normOffset + 5;


        //  weapon / armor
        weapon.GetComponent<SpriteRenderer>().sortingOrder = normOffset - 3;
        armor.GetComponent<SpriteRenderer>().sortingOrder = normOffset + 1;


        //  head / face
        head.GetComponent<SpriteRenderer>().sortingOrder = normOffset + 3;
        head.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = normOffset + 4;
        if(hatInfrontOfHead)
            head.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = normOffset + 5;
        else
            head.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = normOffset + 2;
    }


    public float getHeight() {
        float thing = body.GetComponent<SpriteRenderer>().bounds.size.y / 1.0f;
        return thing;
    }
    public Vector2 getCombatNormalPos() {
        if(GetComponentInChildren<UnitSpriteHandler>() != null) {
            switch(reference.u_sprite.bodyIndex) {
                case 0: return new Vector3(-.09f, .5f);
                case 1: return new Vector3(-.06f, .21f);
                case 2: return new Vector3(-0.09f, .51f);
            }
        }
        return Vector2.zero;
    }
}
