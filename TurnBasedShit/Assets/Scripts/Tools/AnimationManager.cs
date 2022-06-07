using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationManager {
    static List<animInstance> animInstances = new List<animInstance>();

    public static string bodyIdle = "BodyIdle";
    public static string headIdle = "HeadIdle";
    public static string bodyWalk = "Body0WalkAnim";

    class animInstance {
        public GameObject parent = null;
        public string anim = "";
    }

    public static void ChangeAnimationState(GameObject parent, string animName) {
        //  checks if relevant
        animInstance parentInstance = null;
        foreach(var i in animInstances) {
            if(i.parent == parent) {
                parentInstance = i;
                if(i.anim == animName)
                    return;
            }
        }



        Animator anim = null;
        if(parent.GetComponent<Animator>() != null) //  checks if on parent
            anim = parent.GetComponent<Animator>();
        else if(parent.GetComponentInChildren<Animator>() != null)  //  checks if on children
            anim = parent.GetComponentInChildren<Animator>();
        else return;    //  no animator?

        anim.Play(animName);

        //  saves the info
        if(parentInstance == null) {
            var inst = new animInstance();
            inst.parent = parent;
            inst.anim = animName;
            animInstances.Add(inst);
        }
        else {
            parentInstance.anim = animName;
        }
    }
}
