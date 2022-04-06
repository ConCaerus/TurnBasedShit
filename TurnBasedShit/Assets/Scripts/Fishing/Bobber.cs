using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bobber : MonoBehaviour {
    [SerializeField] Vector2 showPos;

    private void Start() {
        transform.position = new Vector3(transform.position.x, FindObjectOfType<FishingLineMover>().yPos);
        StartCoroutine(animateBob());
    }


    private void Update() {
        if(FindObjectOfType<FishingCanvas>().fishing)
            rotate();
    }


    void rotate() {
        //  make bobber look at the fish target
        if(FindObjectOfType<FishingLineMover>() == null)
            return;
        float area = FindObjectOfType<FishingLineMover>().maxXArea - FindObjectOfType<FishingLineMover>().minXArea;
        Vector3 diff = new Vector3(FindObjectOfType<FishingLineMover>().minXArea + area * FindObjectOfType<FishingCanvas>().fishTarget, transform.position.y - 5.0f) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90.0f);
        transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, (rot_z + 90.0f) / 2.0f);

        //  particle shits
        float maxRate = 1000.0f;
        float rate = (Mathf.Abs(diff.x) / area) * maxRate;
        transform.GetChild(1).GetComponent<ParticleSystem>().emissionRate = rate;
    }

    public void resetValues() {
        transform.GetChild(1).GetComponent<ParticleSystem>().emissionRate = 0;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void showSpoils(Collectable fish) {
        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getGenericSpriteForCollectable(fish);
        transform.GetChild(2).rotation = Quaternion.Euler(0.0f, 0.0f, fish.fishedData.hookedRot);
        transform.GetChild(2).transform.localScale = new Vector2(fish.fishedData.scale, fish.fishedData.scale);
        transform.GetChild(2).transform.localPosition = fish.fishedData.hookedPos;
        StartCoroutine(showSpoilsAnim());
    }




    IEnumerator animateBob() {
        float randTime = Random.Range(0.25f, 0.35f);

        transform.DOPunchPosition(new Vector3(0.0f, -Random.Range(0.01f, 0.1f)), randTime);
        yield return new WaitForSeconds(randTime);
        while(!FindObjectOfType<FishingCanvas>().fishing)
            yield return new WaitForSeconds(randTime);
        StartCoroutine(animateBob());
    }

    IEnumerator showSpoilsAnim() {
        yield return new WaitForSeconds(0.25f);
        transform.DOMove(showPos, 0.25f);
        transform.GetChild(2).transform.DOScale(3.0f, 0.25f);
    }
}
