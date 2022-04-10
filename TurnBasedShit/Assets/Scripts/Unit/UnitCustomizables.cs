using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCustomizables : MonoBehaviour {


    public abstract void setColor(Color c);
    public abstract void tweenToColor(Color c, float time);
    public abstract Color getColor();

    public abstract void setAllSpritesVisible(bool b);

    public abstract void offsetLayer(int norm);

    public abstract void triggerAttackAnim();
    public abstract void triggerDefendAnim();
    public abstract void setWalkingAnim(bool b);
    public abstract void setFishingAnim(bool b);
    public abstract bool isAnimIdle();
    public abstract void setAnimSpeed(float sp);
}
