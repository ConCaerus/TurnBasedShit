using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] AudioSource effectPlayer, musicPlayer;
    [SerializeField] AudioClip levelUpSound, tatterSound;


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
        effectPlayer.PlayOneShot(clip);
        playedClips.Add(clip);
    }
    public void playMusic(AudioClip clip, bool repeat) {
        if(!repeat)
            effectPlayer.PlayOneShot(clip);
        else 
            StartCoroutine(musicRepeater(clip));
    }

    public void randomizePitch() {
        effectPlayer.pitch = Random.Range(0.6f, 1.25f);
    }

    IEnumerator refreshPlaylist() {
        yield return new WaitForEndOfFrame();

        playedClips.Clear();
        StartCoroutine(refreshPlaylist());
    }
    IEnumerator musicRepeater(AudioClip clip) {
        musicPlayer.PlayOneShot(clip);

        yield return new WaitForSeconds(clip.length);

        StartCoroutine(musicRepeater(clip));
    }

    //  specific players
    public void playLevelUpSound() {
        playSound(levelUpSound);
    }
    public void playTatterSound() {
        playSound(tatterSound);
    }
}
