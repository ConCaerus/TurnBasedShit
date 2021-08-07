using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] AudioSource player;


    List<AudioClip> playedClips = new List<AudioClip>();


    private void Awake() {
        StartCoroutine(refreshPlaylist());
    }

    public void playSound(AudioClip clip, bool randomize = true) {
        foreach(var i in playedClips) {
            if(i == clip)
                return;
        }

        if(randomize)
            randomizePitch();
        player.PlayOneShot(clip);
        playedClips.Add(clip);
    }

    public void randomizePitch() {
        player.pitch = Random.Range(0.6f, 1.25f);
    }

    IEnumerator refreshPlaylist() {
        yield return new WaitForEndOfFrame();

        playedClips.Clear();
        StartCoroutine(refreshPlaylist());
    }
}
