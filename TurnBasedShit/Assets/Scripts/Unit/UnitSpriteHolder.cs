﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSpriteHolder {
    public Sprite sprite, attackingSprite, defendingSprite;
    public AnimationClip idleAnim, attackAnim;
    public Animator anim;
}
