using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DialogBox : MonoBehaviour {
    [SerializeField] TextMeshProUGUI mainText, nameText;
    [SerializeField] TextMeshProUGUI[] responseTexts = new TextMeshProUGUI[2];
    [SerializeField] GameObject arrow;

    public float boxHeight = 2.0f;
    float maxArrowX = 1.9f;
    float speed = 10.0f;

    public bool showing = false;

    public GameObject talker;

    DialogInfo referenceInfo = null;


    public delegate void func();
    public func runWhenDoneAndAccepted = null;


    private void Start() {
        DOTween.Init();

        hideDialog(0);
        responseTexts[0].transform.parent.GetComponent<Button>().onClick.AddListener(delegate { advanceDialog(1); });
        responseTexts[1].transform.parent.GetComponent<Button>().onClick.AddListener(delegate { advanceDialog(2); });
    }


    private void Update() {
        //  Keyboard input
        if(referenceInfo != null) {
            if(Input.GetKeyDown(KeyCode.Alpha1) && referenceInfo.firstResponseDialog != null) {
                advanceDialog(1);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2) && referenceInfo.secondResponseDialog != null) {
                advanceDialog(2);
            }
        }
    }

    private void LateUpdate() {
        //  follow talker
        transform.position = Vector3.Lerp(transform.position, new Vector3(talker.transform.position.x, talker.transform.position.y + boxHeight), speed * Time.deltaTime);
        var aXPos = talker.transform.position.x;
        aXPos = Mathf.Clamp(aXPos, transform.position.x - maxArrowX, transform.position.x + maxArrowX);
        arrow.transform.position = Vector2.Lerp(arrow.transform.position, new Vector2(aXPos, arrow.transform.position.y), speed * 2.0f * Time.deltaTime);
    }


    public void setName(string name) {
        nameText.text = name;
    }

    public void hideDialog(int choiceAnswered) {
        transform.DOComplete();
        transform.DOScale(new Vector3(0.0f, 0.0f), .25f);
        if(FindObjectOfType<TownCameraMovement>() != null) {
            FindObjectOfType<TownCameraMovement>().zoomOut();
        }
        showing = false;

        if(runWhenDoneAndAccepted != null && choiceAnswered == 1)
            runWhenDoneAndAccepted();
    }
    public void showDialog() {
        transform.DOComplete();
        transform.DOScale(new Vector3(1.0f, 1.0f), .15f);
        if(FindObjectOfType<TownCameraMovement>() != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn(2.0f);
        }
        showing = true;

        advanceDialog(0);
    }

    public void setDialog(DialogInfo tree) {
        referenceInfo = tree;
    }

    public void advanceDialog(int choiceAnswered = 0) {    //  chiceAnswered: 0 - catchall, 1- first, 2- second
        var info = referenceInfo;
        if(choiceAnswered == 1)
            info = info.firstResponseDialog;
        else if(choiceAnswered == 2)
            info = info.secondResponseDialog;

        //  done with dialog
        if(info == null) {
            hideDialog(choiceAnswered);
            return;
        }


        showing = true;
        animate();

        if(FindObjectOfType<TownCameraMovement>() != null) {
            FindObjectOfType<TownCameraMovement>().zoomIn();
        }
        //  sets the main text
        FindObjectOfType<TextCreator>().animateText(info.mainText, mainText);

        //  sets the response texts
        if(string.IsNullOrEmpty(info.firstOption))
            responseTexts[0].transform.parent.gameObject.SetActive(false);
        else {
            responseTexts[0].transform.parent.gameObject.SetActive(true);
            responseTexts[0].text = "1. " + info.firstOption;
        }

        if(string.IsNullOrEmpty(info.secondOption))
            responseTexts[1].transform.parent.gameObject.SetActive(false);
        else {
            responseTexts[1].transform.parent.gameObject.SetActive(true);
            responseTexts[1].text = "2. " + info.secondOption;
        }

        //  remove previous dialog's listeners
        if(referenceInfo != null) {
            responseTexts[0].transform.parent.GetComponent<Button>().onClick.RemoveListener(delegate { referenceInfo.firstAction(); });
            responseTexts[1].transform.parent.GetComponent<Button>().onClick.RemoveListener(delegate { referenceInfo.secondAction(); });
        }

        //  adds new dialog's listeners
        if(info.firstAction != null)
            responseTexts[0].transform.parent.GetComponent<Button>().onClick.AddListener(delegate { info.firstAction(); });
        if(info.secondAction != null)
            responseTexts[1].transform.parent.GetComponent<Button>().onClick.AddListener(delegate { info.secondAction(); });


        referenceInfo = info;
    }


    void animate() {
        float time = 0.15f, rotAmt = 5.0f;
        transform.DOComplete();
        transform.DOPunchScale(new Vector3(.05f, .05f), time);
        transform.DOPunchPosition(new Vector3(0.0f, 0.25f), time);
        transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(-rotAmt, rotAmt)), time);
    }
}
