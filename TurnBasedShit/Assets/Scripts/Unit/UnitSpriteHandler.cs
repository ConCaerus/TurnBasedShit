using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpriteHandler : MonoBehaviour {
    public GameObject head, face, body;
    int faceIndex = -1, headIndex;
    int animState = 0;
    int orderOffset = 0;
    Color col;

    Coroutine idler = null;


    public void setEverything(int h, int f, int b, Color c, Weapon w, Armor a) {
        setHead(h);
        setFace(f);
        setBody(b, w, a); 
        setColor(c);
        offsetLayer();
    }
    public void setEverything(UnitSpriteInfo info, Weapon w, Armor a) {
        setEverything(info.headIndex, info.faceIndex, info.bodyIndex, info.color, w, a);
    }
    public void setHead(int index) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getHeadCount());
        Destroy(head.gameObject);
        GameObject obj = FindObjectOfType<PresetLibrary>().getUnitHead(index);

        head = Instantiate(obj, body.transform).transform.GetChild(0).gameObject;
        face = head.transform.GetChild(0).gameObject;

        setFace(faceIndex);
        setColor(col);
        offsetLayer();
        headIndex = index;
    }
    public void setFace(int index) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getFaceCount());
        face.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getUnitFace(index);
        faceIndex = index;
        offsetLayer();
    }
    public void setBody(int index, Weapon w, Armor a) {
        if(index == -1)
            index = Random.Range(0, FindObjectOfType<PresetLibrary>().getBodyCount());
        Destroy(body.gameObject);
        body = Instantiate(FindObjectOfType<PresetLibrary>().getUnitBody(index), transform);
        setColor(col);
        setHead(headIndex);

        setWeapon(w);
        setArmor(a);
        offsetLayer();
    }

    public void setColor(Color c) {
        foreach(var i in GetComponentsInChildren<SpriteRenderer>()) {
            if(i.gameObject != face && i.gameObject != body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject && i.gameObject != body.transform.GetChild(2).gameObject)
                i.color = c;
            else
                i.color = Color.white;
        }
        col = c;
    }

    public void setWeapon(Weapon w) {
        var weapon = body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

        if(w == null || w.isEmpty()) {
            weapon.GetComponent<SpriteRenderer>().sprite = null;
            weapon.GetComponent<SpriteRenderer>().color = Color.white;
            return;
        }

        weapon.transform.localPosition = new Vector2(FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedX, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedY);
        weapon.transform.localScale = new Vector3(-FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedSize, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedSize, 0.0f);
        weapon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, FindObjectOfType<PresetLibrary>().getWeaponSprite(w).equippedRot);
        weapon.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(w).sprite;
    }
    public void setArmor(Armor a) {
        var armor = body.transform.GetChild(2).gameObject;

        if(a == null || a.isEmpty()) {
            armor.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        armor.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(a).equippedSprite;
        armor.transform.localPosition = new Vector2(FindObjectOfType<PresetLibrary>().getArmorSprite(a).xPos, FindObjectOfType<PresetLibrary>().getArmorSprite(a).yPos);
        armor.transform.localScale = new Vector3(FindObjectOfType<PresetLibrary>().getArmorSprite(a).scale, FindObjectOfType<PresetLibrary>().getArmorSprite(a).scale, 0.0f);
    }

    public Sprite getWeapon() {
        return body.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }


    public void setAnimState(int i) {
        body.GetComponent<Animator>().SetInteger("state", i);
        head.GetComponent<Animator>().SetInteger("state", i);
        animState = i;

        if(idler != null)
            StopCoroutine(idler);
        if(i > 0) {
            idler = StartCoroutine(returnToIdle());
        }
    }
    public int getAnimState() {
        return animState;
    }
    public bool isAnimDone() {
        if(body.GetComponent<Animator>().GetInteger("state") == 0 || head.GetComponent<Animator>().GetInteger("state") == 0) {
            return false;
        }

        if(body.GetComponent<Animator>().GetInteger("state") != head.GetComponent<Animator>().GetInteger("state"))
            return false;
        bool h = head.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !head.GetComponent<Animator>().IsInTransition(0);
        bool b = body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !body.GetComponent<Animator>().IsInTransition(0);

        return h && b;
    }

    public void setAnimSpeed(float sp) {
        body.GetComponent<Animator>().speed = sp;
        head.GetComponent<Animator>().speed = sp;
    }


    public void setLayerOffset(int offset) {
        orderOffset = offset;
    }

    void offsetLayer() {
        if(orderOffset == 0)
            return;
        //  body
        body.GetComponent<SpriteRenderer>().sortingOrder = FindObjectOfType<PresetLibrary>().getRandomUnitBody().GetComponent<SpriteRenderer>().sortingOrder + orderOffset;

        //  arms
        body.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 1;
        body.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder - 1;

        //  weapon / armor
        body.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder - 2;
        body.transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = body.GetComponent<SpriteRenderer>().sortingOrder + 2;


        //  head / face
        head.GetComponent<SpriteRenderer>().sortingOrder = FindObjectOfType<PresetLibrary>().getRandomUnitHead().GetComponent<SpriteRenderer>().sortingOrder + orderOffset;
        face.GetComponent<SpriteRenderer>().sortingOrder = FindObjectOfType<PresetLibrary>().getRandomUnitHead().GetComponent<SpriteRenderer>().sortingOrder + orderOffset + 1;
    }



    IEnumerator returnToIdle() {
        yield return new WaitForEndOfFrame();

        if(isAnimDone()) {
            setAnimState(0);
            idler = null;
        }
        else
            idler = StartCoroutine(returnToIdle());
    }
}
