using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RescueLocationIcon : MapIcon {
    float mHeadOffset = 15.0f, mTearOffsetY = 12.5f, mTearOffsetX = 5.0f;

    public override void mouseOver() {
        if(isMouseOver)
            return;
        isMouseOver = true;

        resetIconVisuals();

        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.75f, enterSpeed);

        //  head
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.25f, enterSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().localPosition.y + mHeadOffset, enterSpeed);

        //  lTear
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.75f, enterSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().localPosition.y + mTearOffsetY, enterSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveX(transform.GetChild(2).GetComponent<RectTransform>().localPosition.x - mTearOffsetX, enterSpeed);

        //  rTear
        transform.GetChild(3).GetComponent<RectTransform>().DOScale(1.75f, enterSpeed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(3).GetComponent<RectTransform>().localPosition.y + mTearOffsetY, enterSpeed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveX(transform.GetChild(3).GetComponent<RectTransform>().localPosition.x + mTearOffsetX, enterSpeed);
    }

    public override void mouseExit() {
        isMouseOver = false;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(3).GetComponent<RectTransform>().DOComplete();

        var preset = FindObjectOfType<MapLocationSpawner>().rescueLocationPreset.transform;

        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);

        //  head
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().localPosition.y, exitSpeed);

        //  lTear
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().localPosition.y, exitSpeed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveX(preset.GetChild(2).GetComponent<RectTransform>().localPosition.x, exitSpeed);

        //  rTear
        transform.GetChild(3).GetComponent<RectTransform>().DOScale(1.0f, exitSpeed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(3).GetComponent<RectTransform>().localPosition.y, exitSpeed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveX(preset.GetChild(3).GetComponent<RectTransform>().localPosition.x, exitSpeed);
    }

    public override void resetIconVisuals() {
        if(idler != null)
            StopCoroutine(idler);
        idler = null;

        transform.GetChild(0).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(1).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(2).GetComponent<RectTransform>().DOComplete();
        transform.GetChild(3).GetComponent<RectTransform>().DOComplete();

        var preset = FindObjectOfType<MapLocationSpawner>().rescueLocationPreset.transform;

        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().localScale = preset.GetChild(0).GetComponent<RectTransform>().localScale;

        //  head
        transform.GetChild(1).GetComponent<RectTransform>().localScale = preset.GetChild(1).GetComponent<RectTransform>().localScale;
        transform.GetChild(1).GetComponent<RectTransform>().localPosition = preset.GetChild(1).GetComponent<RectTransform>().localPosition;

        //  lTear
        transform.GetChild(2).GetComponent<RectTransform>().localScale = preset.GetChild(2).GetComponent<RectTransform>().localScale;
        transform.GetChild(2).GetComponent<RectTransform>().localPosition = preset.GetChild(2).GetComponent<RectTransform>().localPosition;

        //  rTear
        transform.GetChild(3).GetComponent<RectTransform>().localScale = preset.GetChild(3).GetComponent<RectTransform>().localScale;
        transform.GetChild(3).GetComponent<RectTransform>().localPosition = preset.GetChild(3).GetComponent<RectTransform>().localPosition;
    }

    public override IEnumerator idleAnim() {
        var speed = exitSpeed * 4.0f;

        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(0.85f, speed);

        //  head
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.15f, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(1).GetComponent<RectTransform>().localPosition.y + mHeadOffset / 2.0f, speed);

        //  lTear
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.5f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(2).GetComponent<RectTransform>().localPosition.y + mTearOffsetY / 2.0f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveX(transform.GetChild(2).GetComponent<RectTransform>().localPosition.x - mTearOffsetX, speed);

        //  rTear
        transform.GetChild(3).GetComponent<RectTransform>().DOScale(1.5f, speed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveY(transform.GetChild(3).GetComponent<RectTransform>().localPosition.y + mTearOffsetY / 2.0f, speed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveX(transform.GetChild(3).GetComponent<RectTransform>().localPosition.x + mTearOffsetX, speed);

        yield return new WaitForSeconds(speed);

        var preset = FindObjectOfType<MapLocationSpawner>().rescueLocationPreset.transform;
        //  spot
        transform.GetChild(0).GetComponent<RectTransform>().DOScale(1.0f, speed);

        //  head
        transform.GetChild(1).GetComponent<RectTransform>().DOScale(1.0f, speed);
        transform.GetChild(1).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(1).GetComponent<RectTransform>().localPosition.y, speed);

        //  lTear
        transform.GetChild(2).GetComponent<RectTransform>().DOScale(1.0f, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(2).GetComponent<RectTransform>().localPosition.y, speed);
        transform.GetChild(2).GetComponent<RectTransform>().DOLocalMoveX(preset.GetChild(2).GetComponent<RectTransform>().localPosition.x, speed);

        //  rTear
        transform.GetChild(3).GetComponent<RectTransform>().DOScale(1.0f, speed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveY(preset.GetChild(3).GetComponent<RectTransform>().localPosition.y, speed);
        transform.GetChild(3).GetComponent<RectTransform>().DOLocalMoveX(preset.GetChild(3).GetComponent<RectTransform>().localPosition.x, speed);

        yield return new WaitForSeconds(speed);

        idler = null;
    }
}
