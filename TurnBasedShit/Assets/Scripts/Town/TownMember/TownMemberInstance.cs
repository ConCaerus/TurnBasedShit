using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TownMemberInstance : MonoBehaviour {
    public TownMember reference;

    [SerializeField] Sprite exMark, quMark;
    [SerializeField] Color markColor;
    GameObject mark;
    float questIconSpeed = 1.0f;

    public bool interacting = false;

    private void Start() {
        GetComponentInChildren<UnitSpriteHandler>().setReference(reference.sprite, reference.weapon, reference.armor, true);
        if(reference.hasQuest) {
            GetComponentInChildren<DialogBox>().runWhenDoneAndAccepted = acceptQuest;
            GetComponentInChildren<DialogBox>().setName(reference.name);
            GetComponentInChildren<DialogBox>().setDialog(DialogLibrary.getDialogForTownMember(reference));
        }
        gameObject.name = reference.name;


        if(reference.hasQuest) {
            mark = createMark(quMark);
            updateMark();
            StartCoroutine(waitToAnim(mark.gameObject));
        }
    }


    private void Update() {
        if(interacting && inRangeOfPlayer()) {
            if(!GetComponentInChildren<DialogBox>().showing) {
                GetComponentInChildren<DialogBox>().showDialog();
            }
        }
        else if(interacting && !inRangeOfPlayer()) {
            GetComponentInChildren<DialogBox>().hideDialog(1);
            interacting = false;
        }
    }


    bool inRangeOfPlayer() {
        return Mathf.Abs(transform.position.x - FindObjectOfType<LocationMovement>().transform.position.x) < 5.0f;
    }


    GameObject createMark(Sprite s) {
        var thing = new GameObject("mark");
        var sr = thing.AddComponent<SpriteRenderer>();
        sr.sprite = s;
        sr.sortingLayerName = "Unit";
        sr.sortingOrder = 1;
        sr.color = markColor;
        thing.transform.SetParent(transform);
        thing.transform.localPosition = new Vector3(0.0f, 1.25f, 0.0f);

        return thing;
    }

    void updateMark() {
        if(reference.hasQuest) {
            if(reference.isQuestActive())
                mark.GetComponent<SpriteRenderer>().sprite = exMark;
            else
                mark.GetComponent<SpriteRenderer>().sprite = quMark;
        }
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



    //  dialog buttons
    public void acceptQuest() {
        if(!reference.isQuestActive()) {
            ActiveQuests.addQuest(reference.getQuest());
        }
        updateMark();
        interacting = false;
    }
    public void declineQuest() {
        GetComponentInChildren<DialogBox>().hideDialog(0);
        interacting = false;
    }
}