using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class temp : MonoBehaviour {
    bool attacking = false;

    [SerializeField] float time = 0.25f;

    [SerializeField] Sprite attackSprite, normalSprite;

    private void Awake() {
        DOTween.Init();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && !attacking)
            StartCoroutine(attackAnim());

    }


    IEnumerator attackAnim() {
        attacking = true;

        gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite;

        gameObject.transform.DOScale(1.25f, time);
        gameObject.transform.DOMove(new Vector3(gameObject.transform.position.x + 2.0f, gameObject.transform.position.y, 0.0f), time / 4.0f);

        yield return new WaitForSeconds(time);

        gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        gameObject.transform.DOMove(new Vector3(gameObject.transform.position.x - 2.0f, gameObject.transform.position.y, 0.0f), time);

        gameObject.transform.DOScale(0.75f, time / 4.0f);

        yield return new WaitForSeconds(time / 4.0f);

        attacking = false;
    }
}
