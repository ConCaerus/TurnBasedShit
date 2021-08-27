using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class LocationMovement : MonoBehaviour {
    UnitStats reference;
    public GameObject armorPos;
    public GameObject unit;

    public AudioClip walkSound;


    float moveSpeed = 10.0f, flipSpeed = 0.15f;
    public float rightMost, leftMost;

    public bool movingDir = true;
    public bool canMove = true;
    public bool isMoving = false;

    Coroutine anim = null;

    private void Awake() {
        DOTween.Init();
        reference = Party.getLeaderStats();
        setVisuals();
        canMove = true;
    }


    private void Update() {
        if(FindObjectOfType<MenuCanvas>().isOpen())
            return;
        if(Input.GetKeyDown(KeyCode.W) && FindObjectOfType<TransitionCanvas>().loaded)
            interact();
        else if(Input.GetKeyDown(KeyCode.S) && FindObjectOfType<TransitionCanvas>().loaded)
            deinteract();
        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && canMove && FindObjectOfType<TransitionCanvas>().loaded) {
            isMoving = true;
            move();
            unit.GetComponent<UnitSpriteHandler>().setAnimState(3);
        }
        else {
            isMoving = false;
            unit.GetComponent<UnitSpriteHandler>().setAnimState(0);
        }
    }


    public void setVisuals() {
        reference = Party.getLeaderStats();
        unit.GetComponent<UnitSpriteHandler>().setColor(reference.u_sprite.color);

        unit.GetComponent<UnitSpriteHandler>().setEverything(reference.u_sprite, null, reference.equippedArmor);

        if(reference.equippedArmor != null && !reference.equippedArmor.isEmpty()) {
            armorPos.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(reference.equippedArmor).equippedSprite;
        }
    }

    public void move() {
        var sp = moveSpeed + (reference.u_speed / 10.0f);
        //  right
        if(Input.GetKey(KeyCode.D)) {
            if(!movingDir)
                flip();
        }

        //  left
        else if(Input.GetKey(KeyCode.A)) {
            if(movingDir)
                flip();
        }


        transform.Translate(Vector2.right * sp * Time.deltaTime);
        if(transform.position.x > rightMost && rightMost != 0.0f)
            transform.Translate(Vector2.left * sp * Time.deltaTime);
        else if(transform.position.x < leftMost && leftMost != 0.0f)
            transform.Translate(Vector2.left * sp * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        if(anim == null)
            anim = StartCoroutine(walkAnim());
    }

    public abstract void interact();
    public abstract void deinteract();

    public void flip() {
        if(movingDir) {
            transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), flipSpeed);
        }
        else if(!movingDir) {
            transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), flipSpeed);
        }
        movingDir = !movingDir;
    }


    IEnumerator walkAnim(bool jumpDir = true) {
        float time = 0.2f;
        unit.transform.DOPunchPosition(new Vector3(0.0f, 0.7f, 0.0f), time);
        FindObjectOfType<AudioManager>().playSound(walkSound);
        if(jumpDir)
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(5.0f, 25.0f)), time);
        else
            unit.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, Random.Range(-25.0f, -5.0f)), time);

        yield return new WaitForSeconds(time);

        while(FindObjectOfType<MenuCanvas>().isOpen())
            yield return new WaitForEndOfFrame();


        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            anim = StartCoroutine(walkAnim(!jumpDir));
        else
            anim = null;
    }
}
