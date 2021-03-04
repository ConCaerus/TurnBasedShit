using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] AudioClip hitSound, swaySound, dieSound;

    AudioSource player;

    private void Awake() {
        player = GetComponent<AudioSource>();
    }


    public void playHitSound() {
        randomizePitch();
        player.PlayOneShot(hitSound);
    }
    public void playSwaySound() {
        randomizePitch();
        player.PlayOneShot(swaySound);
    }
    public void playDieSound() {
        randomizePitch();
        player.PlayOneShot(dieSound);
    }

    void randomizePitch() {
        player.pitch = Random.Range(0.6f, 1.25f);
    }
}
