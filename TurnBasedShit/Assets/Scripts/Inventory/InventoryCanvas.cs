using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour {
    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] GameObject weaponInventoryCanvas, armorInventoryCanvas;
    [SerializeField] GameObject weaponImage, armorImage;

    [SerializeField] GameObject weaponInventorySlots, armorInventorySlots;

    [SerializeField] TextMeshProUGUI unitName;
    [SerializeField] GameObject unitImage;

    GameObject shownUnit = null;



    abstract class inventoryObject {
        public int index;
        public GameObject icon;
        public GameObject slot;


        Vector2 getMousePos() {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        public void followMouse() {
            if(icon != null)
                icon.transform.position = new Vector3(getMousePos().x, getMousePos().y, icon.transform.position.z);
        }

        public void setNewSlot(GameObject s) {
            slot = s;
            if(icon != null) {
                icon.transform.position = s.transform.position;
                icon.transform.SetParent(s.transform);
            }
        }

        public void destory() {
            Destroy(icon.gameObject);
        }

        public abstract char getType();
    }
    inventoryObject heldObject = null;

    class inventoryWeapon : inventoryObject {

        public override char getType() {
            return 'w';
        }
    }
    List<inventoryWeapon> inventoryWeapons = new List<inventoryWeapon>();

    class inventoryArmor : inventoryObject {

        public override char getType() {
            return 'a';
        }
    }
    List<inventoryArmor> inventoryArmors = new List<inventoryArmor>();



    private void Awake() {
        inventoryCanvas.SetActive(false);
    }


    private void Update() {
        if(heldObject != null) {
            heldObject.followMouse();
        }
    }


    public void showWeaponInventory() {
        weaponInventoryCanvas.SetActive(true);
        armorInventoryCanvas.SetActive(false);
        updateUI();
    }
    public void showArmorInventory() {
        weaponInventoryCanvas.SetActive(false);
        armorInventoryCanvas.SetActive(true);
        updateUI();
    }


    public void showInventory() {
        if(shownUnit == null)
            shownUnit = FindObjectOfType<Party>().getPartyMember(0);
        inventoryCanvas.SetActive(true);
        updateUI();
        Time.timeScale = 0;
    }
    public void closeInventory() {
        inventoryCanvas.SetActive(false);
        Time.timeScale = 1;
    }


    public void cycleUnitsRight() {
        int index = shownUnit.GetComponent<UnitClass>().stats.u_order;
        index++;
        if(index >= FindObjectOfType<Party>().getPartyCount())
            index = 0;

        shownUnit = FindObjectOfType<Party>().getPartyMember(index);
        updateUI();
    }
    public void cycleUnitsLeft() {
        int index = shownUnit.GetComponent<UnitClass>().stats.u_order;
        index--;
        if(index < 0) {
            index = FindObjectOfType<Party>().getPartyCount() - 1;
        }

        shownUnit = FindObjectOfType<Party>().getPartyMember(index);
        updateUI();
    }


    void updateUI() {
        updateUnitImages();

        if(heldObject != null) {
            heldObject.destory();
            heldObject = null;
        }
        populateInventorySlots();
    }

    void updateUnitImages() {
        unitName.text = shownUnit.GetComponent<UnitClass>().stats.u_name;
        unitImage.GetComponent<Image>().sprite = shownUnit.GetComponent<SpriteRenderer>().sprite;
        unitImage.GetComponent<Image>().color = shownUnit.GetComponent<SpriteRenderer>().color;
        shownUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_sprite.setLocation();
        shownUnit.GetComponent<UnitClass>().stats.equippedArmor.a_sprite.setLocation();
        shownUnit.GetComponent<UnitClass>().populateEmptyEquippment();

        weaponImage.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_sprite.getSprite();
        armorImage.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedArmor.a_sprite.getSprite();
    }

    void populateInventorySlots() {
        Inventory.loadAllEquippment();
        if(weaponInventoryCanvas.activeInHierarchy) {
            var slots = weaponInventorySlots.GetComponentsInChildren<Button>();
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                bool alreadyInInventory = false;
                foreach(var wea in inventoryWeapons) {
                    if(wea.index == i) {
                        alreadyInInventory = true;
                        break;
                    }
                }
                if(alreadyInInventory)
                    continue;
                var temp = slots[i];
                GameObject other = Instantiate(temp.gameObject);
                other.transform.position = temp.gameObject.transform.position;
                Destroy(other.GetComponent<Button>());
                other.transform.SetParent(temp.gameObject.transform);
                other.GetComponent<Image>().raycastTarget = false;
                other.GetComponent<Image>().sprite = Inventory.loadWeapon(i).w_sprite.getSprite();
                other.GetComponent<Image>().color = Color.white;
                other.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

                inventoryWeapon w = new inventoryWeapon();
                w.index = i;
                w.icon = other;
                w.slot = temp.gameObject;

                inventoryWeapons.Add(w);
            }
        }

        else if(armorInventorySlots.activeInHierarchy) {
            var slots = armorInventorySlots.GetComponentsInChildren<Button>();
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                bool alreadyInInventory = false;
                foreach(var arm in inventoryArmors) {
                    if(arm.index == i) {
                        alreadyInInventory = true;
                        break;
                    }
                }
                if(alreadyInInventory)
                    continue;
                var temp = slots[i];
                GameObject other = Instantiate(temp.gameObject);
                other.transform.position = temp.gameObject.transform.position;
                Destroy(other.GetComponent<Button>());
                other.transform.SetParent(temp.gameObject.transform);
                other.GetComponent<Image>().raycastTarget = false;
                other.GetComponent<Image>().sprite = Inventory.loadArmor(i).a_sprite.getSprite();
                other.GetComponent<Image>().color = Color.white;
                other.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

                inventoryArmor a = new inventoryArmor();
                a.index = i;
                a.icon = other;
                a.slot = temp.gameObject;

                inventoryArmors.Add(a);
            }
        }
    }


    public void selectWeaponFromInventory(GameObject pressedButton) {
        inventoryWeapon prevWeapon = null;
        if(heldObject != null) {
            heldObject.setNewSlot(pressedButton);
            prevWeapon = (inventoryWeapon)heldObject;
        }
        heldObject = null;
        foreach(var i in inventoryWeapons) {
            if(i.slot == pressedButton) {
                heldObject = i;
                inventoryWeapons.Remove(i);
                break;
            }
        }

        if(prevWeapon != null)
            inventoryWeapons.Add(prevWeapon);
    }

    public void selectArmorFromInventory(GameObject pressedButton) {
        inventoryArmor prevArmor = null;
        if(heldObject != null) {
            heldObject.setNewSlot(pressedButton);
            prevArmor = (inventoryArmor)heldObject;
        }
        heldObject = null;
        foreach(var i in inventoryArmors) {
            if(i.slot == pressedButton) {
                heldObject = i;
                inventoryArmors.Remove(i);
                break;
            }
        }

        if(prevArmor != null)
            inventoryArmors.Add(prevArmor);
    }

    public void equipHeldWeapon() {
        if(heldObject != null) {
            var prev = shownUnit.GetComponent<UnitClass>().stats.equippedWeapon;
            shownUnit.GetComponent<UnitClass>().setEquippedWeapon(Inventory.takeWeaponAtIndex(heldObject.index));
            Inventory.addWeaponToInventory(prev);
            heldObject.index = Inventory.getWeaponCount() - 1;
            heldObject.icon.GetComponent<Image>().sprite = prev.w_sprite.getSprite();

            updateUnitImages();
        }
    }
}
