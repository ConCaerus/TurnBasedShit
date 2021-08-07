using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TownMemberInstance : MonoBehaviour {
    public TownMember reference;

    [SerializeField] GameObject dialog;
    [SerializeField] Sprite exMark, quMark;
    [SerializeField] Color markColor;
    GameObject mark;
    float questIconSpeed = 1.0f;

    Coroutine dialogHider = null;
    public bool interacting = false;

    float dialogHeight = 2.73f, dialogArrowHeight = -0.75f;
    float dialogMoveSpeed = 2.5f;

    private void Start() {
        GetComponent<SpriteRenderer>().color = reference.m_color;

        if(reference.hasQuest) {
            if(!ActiveQuests.hasQuest(reference.quest)) {
                mark = createMark(quMark);
            }
            else {
                mark = createMark(exMark);
            }
            StartCoroutine(waitToAnim(mark.gameObject));
        }

        dialog.SetActive(false);
    }


    private void Update() {
        if(interacting && inRangeOfPlayer()) {
            if(!dialog.activeInHierarchy)
                showDialog();
            updateDialogBox();
        }
        else if(interacting && !inRangeOfPlayer()) {
            interacting = false;
            FindObjectOfType<TownCameraMovement>().zoomOut();
        }
        if(!interacting && dialog.activeInHierarchy && dialogHider == null) {
            dialogHider = StartCoroutine(hideDialog());
        }
    }


    bool inRangeOfPlayer() {
        return Mathf.Abs(transform.position.x - FindObjectOfType<LocationMovement>().transform.position.x) < 5.0f;
    }
    void updateDialogBox() {
        var dialogPos = new Vector2((FindObjectOfType<LocationMovement>().transform.position.x - transform.position.x) / 4.0f, dialogHeight);
        var dialogArrowX = transform.position.x;

        dialog.transform.localPosition = Vector3.Lerp(dialog.transform.localPosition, dialogPos, dialogMoveSpeed * Time.deltaTime);
        dialog.transform.GetChild(1).transform.position = Vector3.Lerp(dialog.transform.GetChild(1).transform.position, new Vector3(dialogArrowX, dialog.transform.GetChild(1).transform.position.y, 0.0f), dialogMoveSpeed * 2.0f * Time.deltaTime);
        dialog.transform.GetChild(1).transform.localPosition = new Vector3(dialog.transform.GetChild(1).transform.localPosition.x, dialogArrowHeight, 0.0f);
    }

    void showDialog() {
        dialog.SetActive(true);
        dialog.transform.DOScale(2.0f, 0.15f);
    }

    IEnumerator hideDialog() {
        dialog.transform.DOScale(0.0f, 0.25f);

        yield return new WaitForSeconds(0.25f);

        dialog.SetActive(false);
        dialogHider = null;
    }


    GameObject createMark(Sprite s) {
        var thing = new GameObject(s.name);
        var sr = thing.AddComponent<SpriteRenderer>();
        sr.sprite = s;
        sr.sortingLayerName = "Unit";
        sr.sortingOrder = 1;
        sr.color = markColor;
        thing.transform.SetParent(transform.parent);
        thing.transform.localPosition = new Vector3(0.0f, 1.25f, 0.0f);

        return thing;
    }


    IEnumerator waitToAnim(GameObject thing) {
        yield return new WaitForSeconds(Random.Range(0.0f, questIconSpeed));

        StartCoroutine(questIconAnim(thing));
    }

    IEnumerator questIconAnim(GameObject thing) {
        float height = 0.5f;
        thing.transform.DOPunchPosition(new Vector3(0.0f, height, 0.0f), questIconSpeed, 2, 0.0f);
        yield return new WaitForSeconds(questIconSpeed);

        StartCoroutine(questIconAnim(thing));
    }
}
