using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownLocationIcon : MapIcon {
    float mHouseOffset = 5.0f, mWindowOffset = 6.0f;

    public override void mouseOver() {
        if(isMouseOver)
            return;
        isMouseOver = true;

        resetIconVisuals();
        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.75f, enterSpeed);

        //  skull
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.05f, enterSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y + mHouseOffset, enterSpeed);

        //  bones
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.15f, enterSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y + mWindowOffset, enterSpeed);
    }

    public override void mouseExit() {
        isMouseOver = false;

        var preset = FindObjectOfType<MapLocationSpawner>().townLocationPreset.transform;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();

        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);

        transform.GetChild(1).GetComponent<RectTransform>().DOScale(preset.GetChild(1).GetComponent<RectTransform>().localScale, exitSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y, exitSpeed);

        transform.GetChild(2).GetComponent<RectTransform>().DOScale(preset.GetChild(2).GetComponent<RectTransform>().localScale, exitSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y, exitSpeed);
    }

    public override void resetIconVisuals() {
        if(idler != null)
            StopCoroutine(idler);
        idler = null;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();

        var preset = FindObjectOfType<MapLocationSpawner>().townLocationPreset.transform;

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
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(0.95f, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y + mHouseOffset / 2.0f, speed);

        //  bones
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(0.85f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y + mWindowOffset / 2.0f, speed);


        yield return new WaitForSeconds(speed);

        var preset = FindObjectOfType<MapLocationSpawner>().townLocationPreset.transform;


        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, speed);

        transform.GetChild(1).GetComponent<RectTransform>().DOScale(preset.GetChild(1).GetComponent<RectTransform>().localScale, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().transform.localPosition.y, speed);

        transform.GetChild(2).GetComponent<RectTransform>().DOScale(preset.GetChild(2).GetComponent<RectTransform>().localScale, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().transform.localPosition.y, speed);

        yield return new WaitForSeconds(speed);

        idler = null;
    }
}
