using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitSpriteHandler : MonoBehaviour {
    GameObject body, head;
    UnitStats reference;
    Color col;

    public bool initialized = false;

    public void setReference(UnitStats stats, bool shouldShowWeapon) {
        reference = stats;
        updateVisuals(shouldShowWeapon);
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

        updateVisuals(shouldShowWeapon);
    }


    public void updateVisuals(bool showWeapon) {
        for(int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        setBody();
        setHead();
        setFace();
        setWeapon();
        setArmor();
        setToNormalColor();
        offsetLayer();

        body.GetComponent<UnitBody>().setShowingWeapon(showWeapon);

        if(FindObjectOfType<PartyObject>() != null)
            FindObjectOfType<PartyObject>().repositionUnit(FindObjectOfType<PartyObject>().getInstantiatedMember(GetComponentInParent<UnitClass>().stats));

        initialized = true;
    }

    void setHead() {
        var h = FindObjectOfType<PresetLibrary>().getUnitHead(reference.u_sprite.headIndex);
        head = Instantiate(h.gameObject, body.transform).transform.GetChild(0).gameObject;
    }
    void setFace() {
        head.GetComponent<UnitHead>().setFace(FindObjectOfType<PresetLibrary>().getUnitFace(reference.u_sprite.faceIndex));
    }
    void setBody() {
        var b = FindObjectOfType<PresetLibrary>().getUnitBody(reference.u_sprite.bodyIndex);
        body = Instantiate(b.gameObject, transform);
    }

    public void setShowingFace(bool b) {
        head.GetComponent<UnitHead>().setShowingFace(b);
    }
    public bool isFaceShown() {
        return head.GetComponent<UnitHead>().showingFace;
    }

    public void setToNormalColor() {
        body.GetComponent<UnitBody>().setColor(reference.u_sprite.color);
        head.GetComponent<UnitHead>().setColor(reference.u_sprite.color);
    }
    public void setATempColor(Color c) {
        body.GetComponent<UnitBody>().setColor(c);
        head.GetComponent<UnitHead>().setColor(c);
    }
    public void tweenColorToNormal(float time) {
        body.GetComponent<UnitBody>().tweenToColor(reference.u_sprite.color, time);
        head.GetComponent<UnitHead>().tweenToColor(reference.u_sprite.color, time);
    }
    public void tweenColor(Color c, float time) {
        body.GetComponent<UnitBody>().tweenToColor(c, time);
        head.GetComponent<UnitHead>().tweenToColor(c, time);
    }
    public Color getColor() {
        return col;
    }

    public void setWeapon() {
        body.GetComponent<UnitBody>().setWeapon(reference.weapon);
    }
    public void setArmor() {
        body.GetComponent<UnitBody>().setArmor(reference.armor, reference.u_sprite.bodyIndex);
        head.GetComponent<UnitHead>().setArmor(reference.armor, reference.u_sprite.headIndex);
    }



    public void triggerAttackAnim() {
        body.GetComponent<UnitBody>().triggerAttackAnim();
        head.GetComponent<UnitHead>().triggerAttackAnim();
    }
    public void triggerDefendAnim() {
        body.GetComponent<UnitBody>().triggerDefendAnim();
        head.GetComponent<UnitHead>().triggerDefendAnim();
    }
    public void setWalkingAnim(bool b) {
        body.GetComponent<UnitBody>().setWalkingAnim(b);
        head.GetComponent<UnitHead>().setWalkingAnim(b);
    }
    public void setFishingAnim(bool b) {
        body.GetComponent<UnitBody>().setFishingAnim(b);
        head.GetComponent<UnitHead>().setFishingAnim(b);
    }

    public bool isAnimIdle() {
        return body.GetComponent<UnitBody>().isAnimIdle() && head.GetComponent<UnitHead>().isAnimIdle();
    }

    public void setAnimSpeed(float sp) {
        body.GetComponent<UnitBody>().setAnimSpeed(sp);
        head.GetComponent<UnitHead>().setAnimSpeed(sp);
    }


    void offsetLayer() {
        StartCoroutine(offsetWaiter());
    }

    IEnumerator offsetWaiter() {
        yield return new WaitForEndOfFrame();

        int normOffset = FindObjectOfType<PresetLibrary>().getRandomUnitBody().GetComponent<SpriteRenderer>().sortingOrder + reference.u_sprite.layerOffset;
        body.GetComponent<UnitBody>().offsetLayer(normOffset);
        head.GetComponent<UnitHead>().offsetLayer(normOffset);
    }


    public float getHeight() {
        return body.GetComponent<SpriteRenderer>().bounds.size.y + head.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    public Vector2 getCombatNormalPos() {
        switch(reference.u_sprite.bodyIndex) {
            case 0: return new Vector3(-.03f, .44f);
            case 1: return new Vector3(-.04f, -.03f);
            case 2: return new Vector3(-0.08f, .42f);
        }
        return Vector2.zero;
    }
    public Vector2 getCombatHighlightOffset() {
        switch(reference.u_sprite.bodyIndex) {
            case 0: return new Vector3(.093f, .45f);
            case 1: return new Vector3(.053f, .745f);
            case 2: return new Vector3(.085f, .482f);
        }
        return Vector2.zero;
    }
}
