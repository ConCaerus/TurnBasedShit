using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheelShower : MonoBehaviour {
    [SerializeField] GameObject wheelPreset;
    GameObject wheel = null;
    GameObject usedParent = null;

    float spinSpeed = 180.0f;

    private void Update() {
        if(wheel != null)
            wheel.GetComponent<RectTransform>().Rotate(new Vector3(0.0f, 0.0f, spinSpeed * Time.deltaTime));
    }

    public void showWheel(GameObject caller, Color c) {
        if(usedParent != null && caller == usedParent)
            return;

        GetComponent<Canvas>().sortingOrder = caller.GetComponentInParent<Canvas>().sortingOrder + 1;

        usedParent = caller.gameObject;
        if(wheel != null)
            wheel.GetComponent<SpinWheelObject>().startHiding();
        wheel = Instantiate(wheelPreset.gameObject, transform);

        wheel.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 0.0f);
        wheel.GetComponent<RectTransform>().transform.position = caller.GetComponent<RectTransform>().transform.position;
        wheel.GetComponent<Image>().color = c;

        wheel.GetComponent<SpinWheelObject>().startShowing();
    }
    public void hideWheel() {
        if(wheel != null)
            wheel.GetComponent<SpinWheelObject>().startHiding();
        wheel = null;
        usedParent = null;
    }

    public bool alreadyShowingForGameObject(GameObject caller) {
        if(usedParent == null || wheel == null)
            return false;
        return caller == usedParent;
    }
}
