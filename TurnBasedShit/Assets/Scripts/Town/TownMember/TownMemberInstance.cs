using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TownMemberInstance : MonoBehaviour {
    public TownMember reference;

    [SerializeField] GameObject dialog, acceptButton, declineButton;
    [SerializeField] Sprite exMark, quMark;
    [SerializeField] Color markColor;
    GameObject mark;
    float questIconSpeed = 1.0f;

    [SerializeField] TextMeshProUGUI dialogText;

    Coroutine dialogHider = null;
    public bool interacting = false;

    float dialogHeight = 2.73f, dialogArrowHeight = -0.75f;
    float dialogMoveSpeed = 2.5f;

    private void Start() {
        GetComponentInChildren<UnitSpriteHandler>().setEverything(reference.m_sprite, null, null, reference.m_sprite.layerOffset);

        if(reference.hasQuest) {
            mark = createMark(quMark);
            updateMark();
            StartCoroutine(waitToAnim(mark.gameObject));
        }

        dialog.SetActive(false);
    }


    private void Update() {
        if(interacting && inRangeOfPlayer()) {
            if(!dialog.activeInHierarchy) {
                showDialog();

                //  hide other member's dialogs
                foreach(var i in FindObjectsOfType<TownMemberInstance>()) {
                    if(i != this)
                        i.stopShowingDialog();
                }
            }
            updateDialogBox();
        }
        else if(interacting && !inRangeOfPlayer()) {
            interacting = false;
            FindObjectOfType<TownCameraMovement>().zoomOut();
        }
        if(!interacting && dialog.activeInHierarchy && dialogHider == null) {
            stopShowingDialog();
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

        if(reference.hasQuest) {
            if(!reference.isQuestActive()) {
                if(reference.m_questType == GameInfo.questType.bossFight)
                    dialogText.text = reference.m_bossQuest.bossUnit.u_name;
                else if(reference.m_questType == GameInfo.questType.kill)
                    dialogText.text = "Kill";
                else if(reference.m_questType == GameInfo.questType.delivery)
                    dialogText.text = "Delivery";
                else if(reference.m_questType == GameInfo.questType.pickup)
                    dialogText.text = "Pickup";
                acceptButton.SetActive(true);
                declineButton.SetActive(true);
            }
            else {
                dialogText.text = "Quest already accepted";
                acceptButton.SetActive(false);
                declineButton.SetActive(false);
            }
        }
        else {
            acceptButton.SetActive(false);
            declineButton.SetActive(false);
        }
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

    public void stopShowingDialog() {
        interacting = false;
        dialogHider = StartCoroutine(hideDialog());
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
            if(reference.m_bossQuest != null) {
                ActiveQuests.addQuest(reference.m_bossQuest);
            }
            else if(reference.m_killQuest != null) {
                ActiveQuests.addQuest(reference.m_killQuest);
            }
            else if(reference.m_deliveryQuest != null) {
                ActiveQuests.addQuest(reference.m_deliveryQuest);
            }
            else if(reference.m_pickupQuest != null) {
                ActiveQuests.addQuest(reference.m_pickupQuest);
            }
        }
        updateMark();
        stopShowingDialog();
        FindObjectOfType<TownCameraMovement>().zoomOut();
    }
    public void declineQuest() {
        stopShowingDialog();
        FindObjectOfType<TownCameraMovement>().zoomOut();
    }
}
