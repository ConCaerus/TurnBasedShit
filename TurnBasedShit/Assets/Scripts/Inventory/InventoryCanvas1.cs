using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCanvas1 : MonoBehaviour {
    [SerializeField] GameObject heldObjectPicture;
    object heldObject = null;
    [SerializeField] List<GameObject> activeSlots = new List<GameObject>();
    [SerializeField] List<GameObject> slotHolders = new List<GameObject>();
    List<object> activeObjects = new List<object>();

    [SerializeField] GameObject weaponCanvas, armorCanvas, consumableCanvas;
    [SerializeField] GameObject unitWeaponImage, unitArmorImage;

    [SerializeField] GameObject shownUnitInformation;
    UnitClassStats shownUnit = null;


    private void Update() {
        if(FindObjectOfType<InfoBox>() != null)
            manageInfoBox();

        heldObjectPicture.transform.position = GameState.getMousePos();

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            selectObjectFromSlot();
    }


    //  useful getters
    GameObject getMousedOverCollider() {
        //  return if their are no active slots
        if(activeSlots.Count == 0)
            return null;

        Ray ray;
        RaycastHit2D hit;

        List<Collider2D> unwantedHits = new List<Collider2D>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        //  return if the ray did not hit anything
        if(hit.collider == null)
            return null;

        while(true) {
            //  if the hit is hitting an inventory slot
            foreach(var i in activeSlots) {
                if(hit.collider == i.GetComponent<Collider2D>()) {
                    foreach(var u in unwantedHits)
                        u.enabled = true;
                    return i.gameObject;
                }
            }

            GameObject wantedHit = null;

            //  if the hit is hittign the shown unit picture
            if(hit.collider == shownUnitInformation.transform.GetChild(2).GetComponent<Collider2D>())
                wantedHit = shownUnitInformation.transform.GetChild(2).transform.gameObject;

            //  if the hit is hitting a unit equipped slot
            if(weaponCanvas.activeInHierarchy && hit.collider == unitWeaponImage.transform.parent.GetComponent<Collider2D>())
                wantedHit = unitWeaponImage.transform.parent.gameObject;
            if(armorCanvas.activeInHierarchy && hit.collider == unitArmorImage.transform.parent.GetComponent<Collider2D>())
                wantedHit = unitArmorImage.transform.parent.gameObject;

            //  hit is a wanted object
            if(wantedHit != null) {
                foreach(var u in unwantedHits)
                    u.enabled = true;
                return wantedHit.gameObject;
            }

            //  hit is not a wanted object
            hit.collider.enabled = false;
            unwantedHits.Add(hit.collider);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            //  hit has run out of hit objects
            if(hit.collider == null) {
                foreach(var i in unwantedHits)
                    i.enabled = true;
                return null;
            }
        }
    }

    object getObjectInSlot(GameObject slot) {
        if(slot == unitWeaponImage.transform.parent.gameObject)
            return shownUnit.equippedWeapon;
        if(slot == unitArmorImage.transform.parent.gameObject)
            return shownUnit.equippedArmor;

        int index = getSlotIndex(slot);
        if(index >= activeSlots.Count || index < 0)
            return null;

        return activeObjects[index];
    }

    int getConsumableCountInSlot(GameObject slot) {
        if(slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>() == null)
            return -1;

        int index = getSlotIndex(slot);

        if(activeObjects[index] == null)
            return 0;
        else if(activeObjects[index] != null && !slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled)
            return 1;
        else
            return int.Parse(slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
    }

    int getSlotIndex(GameObject slot) {
        for(int i = 0; i < activeSlots.Count; i++) {
            if(slot == activeSlots[i])
                return i;
        }
        return -1;
    }



    //  useful setters
    //  this is also used as a button
    public void setActiveSlots() {
        activeSlots.Clear();
        foreach(var i in slotHolders) {
            if(i.activeInHierarchy) {
                foreach(var s in i.GetComponentsInChildren<Collider2D>()) {
                    activeSlots.Add(s.gameObject);
                }
            }
        }
    }

    public void setHeldObject(object thing) {
        heldObject = thing;
        if(thing == null)
            heldObject = null;
        else if(thing.GetType() == typeof(Weapon) && ((Weapon)thing).isEmpty()) {
            heldObject = null;
        }
        else if(thing.GetType() == typeof(Armor) && ((Armor)thing).isEmpty())
            heldObject = null;
        heldObjectPicture.SetActive(thing != null);

        if(heldObject != null) {
            if(thing.GetType() == typeof(Weapon))
                heldObjectPicture.GetComponent<Image>().sprite = ((Weapon)thing).w_sprite.getSprite();

            else if(thing.GetType() == typeof(Armor))
                heldObjectPicture.GetComponent<Image>().sprite = ((Armor)thing).a_sprite.getSprite();

            else if(thing.GetType() == typeof(Consumable))
                heldObjectPicture.GetComponent<Image>().sprite = ((Consumable)thing).c_sprite.getSprite();
        }
    }

    Sprite getHeldObjectSprite() {
        if(heldObject.GetType() == typeof(Weapon))
            return ((Weapon)heldObject).w_sprite.getSprite();

        else if(heldObject.GetType() == typeof(Armor))
            return ((Armor)heldObject).a_sprite.getSprite();

        else if(heldObject.GetType() == typeof(Consumable))
            return ((Consumable)heldObject).c_sprite.getSprite();

        return null;
    }


    //  managers
    public void manageInfoBox() {
        var temp = getMousedOverCollider();
        if(temp == null) {
            FindObjectOfType<InfoBox>().hide();
            return;
        }
        //  if temp is not a slot
        if(temp == shownUnitInformation.transform.GetChild(2).transform.gameObject) {
            FindObjectOfType<InfoBox>().setAndShowInfoBox(shownUnit);
            return;
        }

        //  if temp is a slot
        var obj = getObjectInSlot(temp);
        if(obj == null) {
            FindObjectOfType<InfoBox>().hide();
            return;
        }

        FindObjectOfType<InfoBox>().setAndShowInfoBox(getObjectInSlot(temp));
    }

    void updateUnitChange() {
        shownUnitInformation.transform.GetChild(2).GetComponent<Image>().sprite = shownUnit.u_sprite.getSprite();
        shownUnitInformation.transform.GetChild(2).GetComponent<Image>().color = shownUnit.u_color;
        shownUnitInformation.GetComponentInChildren<Slider>().maxValue = shownUnit.u_maxHealth;
        shownUnitInformation.GetComponentInChildren<Slider>().value = shownUnit.u_health;
        shownUnitInformation.GetComponentInChildren<TextMeshProUGUI>().text = shownUnit.u_name;

        //  equipped weapon
        if(weaponCanvas.activeInHierarchy) {
            if(shownUnit.equippedWeapon == null || shownUnit.equippedWeapon.isEmpty()) {
                unitWeaponImage.GetComponent<Image>().sprite = null;
                unitWeaponImage.GetComponent<Image>().enabled = false;
            }
            else {
                unitWeaponImage.GetComponent<Image>().enabled = true;
                unitWeaponImage.GetComponent<Image>().sprite = shownUnit.equippedWeapon.w_sprite.getSprite();
            }
        }
        else if(armorCanvas.activeInHierarchy) {
            if(shownUnit.equippedArmor == null || shownUnit.equippedArmor.isEmpty()) {
                unitArmorImage.GetComponent<Image>().sprite = null;
                unitArmorImage.GetComponent<Image>().enabled = false;
            }
            else {
                unitArmorImage.GetComponent<Image>().enabled = true;
                unitArmorImage.GetComponent<Image>().sprite = shownUnit.equippedArmor.a_sprite.getSprite();
            }
        }
    }

    public void selectObjectFromSlot() {
        var slot = getMousedOverCollider();
        var index = getSlotIndex(slot);
        var obj = getObjectInSlot(slot);

        //  used slot is not in the active slots list
        if(index < 0 && slot != unitWeaponImage.transform.parent.gameObject && slot != unitArmorImage.transform.parent.gameObject && slot != shownUnitInformation.transform.GetChild(2).transform.gameObject)
            return;

        //  nothing to do
        if(obj == null && heldObject == null)
            return;

        if((obj == null || obj.GetType() != typeof(Consumable)) && (heldObject == null || heldObject.GetType() != typeof(Consumable))) {
            //  put down the current held object
            if(heldObject != null) {
                //  slot is not a unit equipment
                if(slot != unitWeaponImage.transform.parent.gameObject && slot != unitArmorImage.transform.parent.gameObject) {
                    activeObjects[index] = heldObject;
                    activeSlots[index].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    activeSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = getHeldObjectSprite();
                }

                //  slot is a unit equipment
                else {
                    if(weaponCanvas.activeInHierarchy) {
                        //  sets the unit's weapon to the held object
                        shownUnit.equippedWeapon = (Weapon)heldObject;
                        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(shownUnit);

                        if(obj != null && !((Weapon)obj).isEmpty())
                            Inventory.addNewWeapon((Weapon)obj);
                        else
                            obj = null;

                        //  removes the held object from the inventory
                        Inventory.removeWeapon((Weapon)heldObject);

                        //  sets picture
                        unitWeaponImage.GetComponent<Image>().enabled = true;
                        unitWeaponImage.GetComponent<Image>().sprite = getHeldObjectSprite();
                    }
                    else if(armorCanvas.activeInHierarchy) {
                        shownUnit.equippedArmor = (Armor)heldObject;
                        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(shownUnit);

                        if(obj != null && !((Armor)obj).isEmpty())
                            Inventory.addNewArmor((Armor)obj);
                        else
                            obj = null;

                        Inventory.removeArmor((Armor)heldObject);

                        unitArmorImage.GetComponent<Image>().enabled = true;
                        unitArmorImage.GetComponent<Image>().sprite = getHeldObjectSprite();
                    }
                }
            }

            //   takes object in slot
            else {
                //  slot is not a unit equipment
                if(slot != unitWeaponImage.transform.parent.gameObject && slot != unitArmorImage.transform.parent.gameObject) {
                    activeObjects[index] = null;
                    slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                    slot.transform.GetChild(0).GetComponent<Image>().enabled = false;
                }

                //  slot is a unit equipment
                else {
                    if(weaponCanvas.activeInHierarchy) {
                        shownUnit.equippedWeapon = null;
                        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(shownUnit);

                        //  picture
                        unitWeaponImage.GetComponent<Image>().sprite = null;
                        unitWeaponImage.GetComponent<Image>().enabled = false;

                        if(obj != null && !((Weapon)obj).isEmpty())
                            Inventory.addNewWeapon((Weapon)obj);
                    }
                    else if(armorCanvas.activeInHierarchy) {
                        shownUnit.equippedArmor = null;
                        FindObjectOfType<PartyObject>().resaveInstantiatedUnit(shownUnit);

                        unitArmorImage.GetComponent<Image>().sprite = null;
                        unitArmorImage.GetComponent<Image>().enabled = false;

                        if(obj != null && !((Armor)obj).isEmpty())
                            Inventory.addNewArmor((Armor)obj);
                    }
                }
            }


            //  set held object to the object in the slot
            setHeldObject(obj);
        }

        //  consumable things
        else if((obj == null || obj.GetType() == typeof(Consumable)) || (heldObject == null || heldObject.GetType() == typeof(Consumable))) {
            //  put down the current held object
            if(heldObject != null) {
                //  player is applying the held consumable to the unit
                if(slot == shownUnitInformation.transform.GetChild(2).transform.gameObject) {
                    var unit = FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit);
                    if(unit != null) {
                        shownUnit = ((Consumable)heldObject).applyEffect(unit);
                        updateUnitChange();
                    }

                    Inventory.removeConsumable((Consumable)heldObject);
                }
                else {
                    int slotStackCount = getConsumableCountInSlot(slot);

                    //  held object does not fit in stack so nothing happens
                    bool roomInStack = obj == null || (slotStackCount >= 0 && slotStackCount < ((Consumable)obj).c_maxStackCount);
                    bool sameTypeAsStack = obj == null || ((Consumable)obj).isEqualTo((Consumable)heldObject);

                    if(!(roomInStack && sameTypeAsStack))
                        return;

                    //  held object fits in the stack
                    slotStackCount++;
                    if(slotStackCount > 1) {
                        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slotStackCount.ToString();
                    }

                    activeObjects[index] = heldObject;
                    activeSlots[index].transform.GetChild(1).GetComponent<Image>().enabled = true;
                    activeSlots[index].transform.GetChild(1).GetComponent<Image>().sprite = getHeldObjectSprite();
                }

                setHeldObject(null);
            }

            //  runs for auto use and for setting the object into a slot
            else {
                int slotStackCount = getConsumableCountInSlot(slot) - 1;
                if(slotStackCount > 1) {
                    slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                    slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slotStackCount.ToString();
                }
                else
                    slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

                if(slotStackCount == 0) {
                    activeObjects[index] = null;
                    slot.transform.GetChild(1).GetComponent<Image>().sprite = null;
                    slot.transform.GetChild(1).GetComponent<Image>().enabled = false;
                }

                //  if the player did not auto use the object
                if(!Input.GetMouseButton(1))
                    setHeldObject(obj);

                //  player wants to auto use the consumable
                else {
                    var unit = FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit);
                    if(unit != null) {
                        //  use the consumable
                        shownUnit = ((Consumable)obj).applyEffect(unit);
                        updateUnitChange();

                        Inventory.removeConsumable((Consumable)obj);
                    }
                }
            }
        }
    }


    public void populateInventorySlots() {
        //  just put this here for simplicity.
        setActiveSlots();

        //  sets the shown unit
        if(shownUnit == null || shownUnit.isEmpty())
            shownUnit = Party.getMemberStats(0);
        updateUnitChange();

        setHeldObject(null);
        activeObjects.Clear();

        //  weapon
        if(weaponCanvas.activeInHierarchy) {
            for(int i = 0; i < activeSlots.Count; i++) {
                activeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                activeObjects.Add(null);

                Weapon we = Inventory.getWeapon(i);
                if(we != null) {
                    activeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    activeSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = we.w_sprite.getSprite();

                    activeObjects[i] = we;
                }
            }
        }

        //  armor
        else if(armorCanvas.activeInHierarchy) {
            for(int i = 0; i < activeSlots.Count; i++) {
                activeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                activeObjects.Add(null);

                Armor ar = Inventory.getArmor(i);
                if(ar != null) {
                    activeSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    activeSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = ar.a_sprite.getSprite();

                    activeObjects[i] = ar;
                }
            }
        }

        //  consumable
        else if(consumableCanvas.activeInHierarchy) {
            List<List<Consumable>> groups = new List<List<Consumable>>();
            //  sorts the different consumables into groups
            for(int i = 0; i < Inventory.getConsumableCount(); i++) {
                var con = Inventory.getConsumable(i);

                //  checks if the consumable can be put into an existing group
                bool fitAGroup = false;
                foreach(var c in groups) {
                    if(c[0].isEqualTo(con) && c.Count < c[0].c_maxStackCount) {
                        c.Add(con);
                        fitAGroup = true;
                        break;
                    }
                }
                if(fitAGroup)
                    continue;

                //  if no list contains that type of consumable, create a new group
                var temp = new List<Consumable>();
                temp.Add(con);
                groups.Add(temp);
            }


            for(int i = 0; i < activeSlots.Count; i++) {
                activeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                activeSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                activeObjects.Add(null);

                if(groups.Count > i) {
                    List<Consumable> cons = groups[i];

                    if(cons.Count > 1) {
                        activeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                        activeSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cons.Count.ToString();
                    }
                    activeSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                    activeSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = cons[0].c_sprite.getSprite();

                    activeObjects[i] = cons[0];
                }
            }
        }
    }


    //  Buttons
    public void cycleUnits(bool right) {
        int index = shownUnit.u_order;

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= Party.getPartySize())
            index = 0;
        else if(index < 0)
            index = Party.getPartySize() - 1;

        shownUnit = Party.getMemberStats(index);
        updateUnitChange();
    }
}
