using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MapCollectCanvas : MonoBehaviour {
    [SerializeField] GameObject holder;
    [SerializeField] Image sprite;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float moveOffset;
    Coroutine mover = null;

    List<info> next = new List<info>();

    struct info {
        public bool adding;
        public Collectable col;
    }

    private void Start() {
        holder.SetActive(false);
    }

    public void showCanvas(bool adding, Collectable col) {
        info temp = new info();
        temp.adding = adding;
        temp.col = col;
        next.Add(temp);
        if(mover == null) 
            mover = StartCoroutine(moveObj());
    }


    IEnumerator moveObj() {
        float time = 0.5f;
        var obj = Instantiate(holder, holder.transform.parent);
        obj.transform.position = FindObjectOfType<MapMovement>().transform.position + new Vector3(0.0f, .3f, 0.0f);
        obj.SetActive(true);
        text.text = (next[0].adding) ? "+" : "-";
        sprite.sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(next[0].col);
        next.RemoveAt(0);

        obj.GetComponent<RectTransform>().DOMoveY(obj.transform.position.y + moveOffset, time);
        Destroy(obj.gameObject, time + 0.1f);

        yield return new WaitForSeconds(time * .25f);
        obj.transform.DOScale(0.0f, time * .75f);

        if(next.Count > 0) {
            mover = StartCoroutine(moveObj());
        }
        else
            mover = null;
    }
}
