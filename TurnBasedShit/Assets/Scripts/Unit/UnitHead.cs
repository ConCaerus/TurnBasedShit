using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitHead : UnitCustomizables {
    public GameObject face, hat;
    bool hatInfrontOfHead = true;
    public bool showingFace { get; private set; } = true;

    //  Overrides
    public override void setColor(Color c) {
        GetComponent<SpriteRenderer>().color = c;
    }
    public override void tweenToColor(Color c, float time) {
        GetComponent<SpriteRenderer>().DOColor(c, time);
    }
    public override Color getColor() {
        return GetComponent<SpriteRenderer>().color;
    }

    public override void offsetLayer(int norm) {
        GetComponent<SpriteRenderer>().sortingOrder = norm + 3;
        face.GetComponent<SpriteRenderer>().sortingOrder = norm + 4;

        if(hatInfrontOfHead)
            hat.GetComponent<SpriteRenderer>().sortingOrder = norm + 5;
        else                                                     
            hat.GetComponent<SpriteRenderer>().sortingOrder = norm + 2;
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
        return GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "HeadIdleAnim";
    }
    public override void setAnimSpeed(float sp) {
        GetComponent<Animator>().speed = sp;
    }


    //  Unique functions
    public void setShowingFace(bool b) {
        showingFace = b;
        face.gameObject.SetActive(b);
    }
    public void setFace(Sprite f) {
        face.GetComponent<SpriteRenderer>().sprite = f;
    }

    public void setArmor(Armor ar, int headIndex) {
        if(ar == null || ar.isEmpty()) {
            hat.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }
        var sp = FindObjectOfType<PresetLibrary>().getArmorSprite(ar);
        hatInfrontOfHead = sp.hatInfrontOfHead;
        hat.GetComponent<SpriteRenderer>().sprite = sp.equippedHat;
        hat.transform.localPosition = sp.getRelevantHatPos(headIndex);
        hat.transform.localScale = sp.getRelevantHatSize(headIndex);
        hat.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, sp.hatRot);
    }
}
