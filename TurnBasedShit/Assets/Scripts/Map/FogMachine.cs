using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FogMachine : MonoBehaviour {
    [SerializeField] GameObject[] windPresets;

    private void Start() {
        DOTween.Init();

        for(int i = 0; i < 50; i++)
            StartCoroutine(createWind());
    }


    IEnumerator createWind() {
        var pos = Map.getRandPos();
        var obj = Instantiate(windPresets[Random.Range(0, windPresets.Length)], pos, Quaternion.identity, transform);
        var main = obj.GetComponent<ParticleSystem>().main;

        yield return new WaitForSeconds(Random.Range(0, main.startLifetime.constant));

        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, main.startLifetime.constant + 0.1f);

        yield return new WaitForSeconds(main.startLifetime.constant);

        StartCoroutine(createWind());
    }
}
