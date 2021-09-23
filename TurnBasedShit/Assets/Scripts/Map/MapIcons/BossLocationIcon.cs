using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossLocationIcon : MapIcon {
    float mSkullOffset = 15.0f, mBonesOffset = 5.0f;

    public override void mouseOver() {
        if(isMouseOver)
            return;
        isMouseOver = true;

        resetIconVisuals();
        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.75f, enterSpeed);

        //  skull
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.25f, enterSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y + mSkullOffset, enterSpeed);

        //  bones
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.15f, enterSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y - mBonesOffset, enterSpeed);
    }

    public override void mouseExit() {
        isMouseOver = false;

        var preset = FindObjectOfType<MapLocationSpawner>().bossLocationPreset.transform;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();

        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);

        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y, exitSpeed);

        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y, exitSpeed);
    }

    public override void resetIconVisuals() {
        if(idler != null)
            StopCoroutine(idler);
        idler = null;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();

        var preset = FindObjectOfType<MapLocationSpawner>().bossLocationPreset.transform;

        transform.GetChild(0).GetComponent<RectTransform>().localScale = preset.GetChild(0).GetComponent<RectTransform>().localScale;

        transform.GetChild(1).GetComponent<RectTransform>().localScale = preset.GetChild(1).GetComponent<RectTransform>().localScale;
        transform.GetChild(1).GetComponent<RectTransform>().localPosition = preset.GetChild(1).GetComponent<RectTransform>().localPosition;

        transform.GetChild(2).GetComponent<RectTransform>().localScale = preset.GetChild(2).GetComponent<RectTransform>().localScale;
        transform.GetChild(2).GetComponent<RectTransform>().localPosition = preset.GetChild(2).GetComponent<RectTransform>().localPosition;
    }


    public override IEnumerator idleAnim() {
        var speed = exitSpeed * 4.0f;

        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.85f, speed);

        //  skull
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.15f, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y + mSkullOffset / 2.0f, speed);

        //  bones
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.05f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y - mBonesOffset / 2.0f, speed);


        yield return new WaitForSeconds(speed);

        var preset = FindObjectOfType<MapLocationSpawner>().bossLocationPreset.transform;


        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, speed);

        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.0f, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y, speed);

        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.0f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y, speed);

        yield return new WaitForSeconds(speed);

        idler = null;
    }
}
