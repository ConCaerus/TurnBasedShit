using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour {
    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] GameObject weaponInventoryCanvas, armorInventoryCanvas;
    [SerializeField] GameObject supportItemInventoryCanvas;
    [SerializeField] GameObject weaponImage, armorImage;

    [SerializeField] GameObject weaponInventorySlots, armorInventorySlots;
    [SerializeField] GameObject equipmentUI;

    [SerializeField] GameObject supportItemInventorySlots;

    [SerializeField] TextMeshProUGUI unitName;
    [SerializeField] GameObject unitImage;
    [SerializeField] Slider unitHealthSlider;

    [SerializeField] GameObject unitStatsMenu;
    [SerializeField] TextMeshProUGUI unitsSpeedText;
    [SerializeField] Vector2 statsOffset;

    GameObject shownUnit = null;

    int clickCount = 0;
    Coroutine clickCountDecreaser = null;


    abstract class InventoryObject {
        public GameObject icon;
        public GameObject slot;


        protected Vector2 getMousePos() {
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
            }
        }

        public void destory() {
            if(icon.gameObject != null)
                Destroy(icon.gameObject);
        }
    }
    InventoryObject heldObject = null;

    class InventoryWeapon : InventoryObject {
        public Weapon weapon;
    }
    List<InventoryWeapon> inventoryWeapons = new List<InventoryWeapon>();

    class InventoryArmor : InventoryObject {
        public Armor armor;
    }
    List<InventoryArmor> inventoryArmors = new List<InventoryArmor>();

    class InventoryItem : InventoryObject {
        public Item item;
    }
    class InventoryItemStack : InventoryObject {
        public List<InventoryItem> items = new List<InventoryItem>();

        public TextMeshProUGUI countText;

        public InventoryItemStack(GameObject slot, Transform parent) {
            items.Clear();
            countText = Instantiate(slot.GetComponentInChildren<TextMeshProUGUI>());
            countText.text = items.Count.ToString();
            countText.transform.SetParent(parent);
            countText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            countText.enabled = slot.GetComponentInChildren<TextMeshProUGUI>().enabled;
        }

        public void textFollowMouse(GameObject slot) {
            if(countText.enabled) {
                var target = getMousePos();
                target += (Vector2)slot.GetComponentInChildren<TextMeshProUGUI>().transform.localScale;
                countText.transform.position = target;
            }
        }

        public void setCountText() {
            countText.text = items.Count.ToString();
            if(items.Count <= 1)
                countText.enabled = false;
            else
                countText.enabled = true;
        }

        public void destroyStack() {
            foreach(var i in items)
                i.destory();
        }
    }
    List<InventoryItem> inventoryItems = new List<InventoryItem>();



    private void Awake() {
        inventoryCanvas.SetActive(false);
    }


    private void Update() {
        manageClickCount();

        if(inventoryCanvas.activeInHierarchy && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))) {
            selectItemFromInventory(slotThatMouseIsOver(supportItemInventorySlots));
        }
        if(heldObject != null) {
            heldObject.followMouse();
            if(heldObject.GetType() == typeof(InventoryItemStack)) {
                ((InventoryItemStack)heldObject).countText.transform.localPosition = supportItemInventorySlots.GetComponentInChildren<TextMeshProUGUI>().transform.localPosition;
                foreach(var i in ((InventoryItemStack)heldObject).items)
                    i.followMouse();
                ((InventoryItemStack)heldObject).textFollowMouse(supportItemInventorySlots.GetComponentInChildren<Collider2D>().gameObject);
            }
        }
        if(isMouseOverUnitImage() && heldObject == null) {
            unitStatsMenu.SetActive(true);
            unitStatsMenu.transform.position = getMousePos() - statsOffset;
        }
        else {
            unitStatsMenu.SetActive(false);
        }

        if(isMouseOverUnitImage() && Input.GetMouseButtonDown(0) && heldObject != null) {
            useHeldObject();
            Debug.Log(true);
        }
    }

    void manageClickCount() {
        if(Input.GetMouseButtonDown(0)) {
            clickCount++;
            clickCountDecreaser = StartCoroutine(decreaseClickCount());
        }
    }


    Vector2 getMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    bool isMouseOverUnitImage() {
        Ray ray;
        RaycastHit2D hit;

        List<Collider2D> unwantedHits = new List<Collider2D>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);


        if(hit.collider == null)
            return false;
        while(hit.collider != null) {
            if(hit.collider == unitImage.GetComponent<Collider2D>()) {
                foreach(var i in unwantedHits)
                    i.enabled = true;
                return true;
            }

            //  hit is not a wanted object
            hit.collider.enabled = false;
            unwantedHits.Add(hit.collider);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hit.collider == null) {
                foreach(var i in unwantedHits)
                    i.enabled = true;
                return false;
            }
        }
        return false;
    }
    GameObject slotThatMouseIsOver(GameObject slotHolder) {
        Ray ray;
        RaycastHit2D hit;

        List<Collider2D> unwantedHits = new List<Collider2D>();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if(hit.collider == null)
            return null;
        bool isHittingWantedObject = false;
        while(!isHittingWantedObject) {
            foreach(var i in slotHolder.GetComponentsInChildren<Collider2D>()) {
                if(hit.collider == i) {
                    isHittingWantedObject = true;
                    break;
                }
            }
            if(isHittingWantedObject)
                break;

            //  hit is not a wanted object
            hit.collider.enabled = false;
            unwantedHits.Add(hit.collider);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hit.collider == null)
                break;
        }

        if(hit.collider != null) {
            foreach(var i in slotHolder.GetComponentsInChildren<Collider2D>()) {
                if(hit.collider == i)
                    return i.gameObject;
            }
        }

        foreach(var i in unwantedHits)
            i.enabled = true;
        return null;
    }


    public void showInventory() {
        if(shownUnit == null)
            setShownUnit(0);
        inventoryCanvas.SetActive(true);
        updateUI();
    }
    public void closeInventory() {
        inventoryCanvas.SetActive(false);
    }


    public void cycleUnitsRight() {
        int index = shownUnit.GetComponent<UnitClass>().stats.u_order;
        index++;
        if(index >= Party.getPartySize())
            index = 0;

        setShownUnit(index);
        updateUI();
    }
    public void cycleUnitsLeft() {
        int index = shownUnit.GetComponent<UnitClass>().stats.u_order;
        index--;
        if(index < 0) {
            index = Party.getPartySize() - 1;
        }

        setShownUnit(index);
        updateUI();
    }


    public void updateUI() {
        updateUnitImages();

        if(heldObject != null) {
            if(heldObject.GetType() != typeof(InventoryItemStack))
                heldObject.destory();
            else
                ((InventoryItemStack)heldObject).destroyStack();
        }
        heldObject = null;
        foreach(var i in inventoryArmors)
            i.destory();
        foreach(var i in inventoryWeapons)
            i.destory();
        foreach(var i in inventoryItems)
            i.destory();


        populateInventorySlots();
    }

    void updateUnitImages() {
        unitName.text = shownUnit.GetComponent<UnitClass>().stats.u_name;
        unitImage.GetComponent<Image>().sprite = shownUnit.GetComponent<SpriteRenderer>().sprite;
        unitImage.GetComponent<Image>().color = shownUnit.GetComponent<SpriteRenderer>().color;
        unitHealthSlider.maxValue = shownUnit.GetComponent<UnitClass>().stats.u_maxHealth;
        unitHealthSlider.value = shownUnit.GetComponent<UnitClass>().stats.u_health;

        unitsSpeedText.text = "Speed: " + shownUnit.GetComponent<UnitClass>().stats.u_speed.ToString();

        weaponImage.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_sprite.getSprite();
        armorImage.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedArmor.a_sprite.getSprite();
    }
    void setShownUnit(int partyIndex) {
        var stats = Party.getMemberStats(partyIndex);

        foreach(var i in FindObjectsOfType<PlayerUnitInstance>()) {
            if(i.stats.u_order == stats.u_order) {
                shownUnit = i.gameObject;

                return;
            }
        }
    }

    void populateInventorySlots() {
        heldObject = null;
        foreach(var i in inventoryWeapons) {
            i.destory();
        }
        inventoryWeapons.Clear();
        foreach(var i in inventoryArmors) {
            i.destory();
        }
        inventoryArmors.Clear();
        foreach(var i in inventoryItems) {
            i.destory();
        }
        inventoryItems.Clear();

        //  Weapon shit
        if(weaponInventoryCanvas.activeInHierarchy) {
            var slots = weaponInventorySlots.GetComponentsInChildren<Button>();
            for(int i = 0; i < Inventory.getWeaponCount(); i++) {
                var temp = slots[i];
                GameObject other = Instantiate(temp.gameObject);
                other.transform.position = temp.gameObject.transform.position;
                Destroy(other.GetComponent<Button>());
                other.transform.SetParent(weaponInventorySlots.gameObject.transform);
                other.GetComponent<Image>().raycastTarget = false;
                other.GetComponent<Image>().sprite = Inventory.getWeapon(i).w_sprite.getSprite();
                other.GetComponent<Image>().color = Color.white;
                other.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                InventoryWeapon w = new InventoryWeapon();
                w.weapon = Inventory.getWeapon(i);
                w.icon = other;
                w.slot = temp.gameObject;

                inventoryWeapons.Add(w);
            }
        }

        //  Armor shit
        else if(armorInventorySlots.activeInHierarchy) {
            var slots = armorInventorySlots.GetComponentsInChildren<Button>();
            for(int i = 0; i < Inventory.getArmorCount(); i++) {
                var temp = slots[i];
                GameObject other = Instantiate(temp.gameObject);
                other.transform.position = temp.gameObject.transform.position;
                Destroy(other.GetComponent<Button>());
                other.transform.SetParent(armorInventorySlots.gameObject.transform);
                other.GetComponent<Image>().raycastTarget = false;
                other.GetComponent<Image>().sprite = Inventory.getArmor(i).a_sprite.getSprite();
                other.GetComponent<Image>().color = Color.white;
                other.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

                InventoryArmor a = new InventoryArmor();
                a.armor = Inventory.getArmor(i);
                a.icon = other;
                a.slot = temp.gameObject;

                inventoryArmors.Add(a);
            }
        }

        //  Item shit
        else if(supportItemInventoryCanvas.activeInHierarchy) {
            var slots = supportItemInventorySlots.GetComponentsInChildren<Collider2D>();
            foreach(var i in slots) {
                if(i.GetComponentInChildren<TextMeshProUGUI>() != null)
                    i.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
            for(int i = 0; i < Inventory.getItemCount(); i++) {
                GameObject slot = null;
                Item newItem = Inventory.getItem(i);

                //  sets the slot object to a slot that isn't full but is populated with the same type of item
                foreach(var j in inventoryItems) {
                    if(j.item.isEqualTo(newItem) && getItemCountForSlot(j.slot) < newItem.i_maxStackCount) {
                        slot = j.slot;
                        slot.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                        slot.GetComponentInChildren<TextMeshProUGUI>().text = (getItemCountForSlot(slot) + 1).ToString();
                        break;
                    }
                }
                //  there wasn't a slot that could stack this item
                //  sets the slot object to the first empty slot location
                if(slot == null) {
                    for(int s = 0; s < slots.Length; s++) {
                        if(getItemCountForSlot(slots[s].gameObject) == 0) {
                            slot = slots[s].gameObject;
                            break;
                        }
                    }
                }


                GameObject other = Instantiate(slot.gameObject);
                other.transform.position = slot.gameObject.transform.position;
                Destroy(other.transform.GetChild(0).gameObject);
                Destroy(other.GetComponent<Collider2D>());
                other.transform.SetParent(supportItemInventorySlots.gameObject.transform);
                other.GetComponent<Image>().raycastTarget = false;
                other.GetComponent<Image>().sprite = Inventory.getItem(i).i_sprite.getSprite();
                other.GetComponent<Image>().color = Color.white;
                other.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

                InventoryItem it = new InventoryItem();
                it.item = newItem;
                it.icon = other;
                it.slot = slot.gameObject;

                inventoryItems.Add(it);
            }
        }
    }

    int getItemCountForSlot(GameObject slot) {
        int count = 0;
        foreach(var i in inventoryItems) {
            if(i.slot == slot)
                count++;
        }
        return count;
    }
    bool sameAsStackType(Item item, GameObject slot) {
        foreach(var i in inventoryItems) {
            if(i.slot == slot)
                return item.isEqualTo(i.item);
        }
        return false;
    }
    bool canFitInStack(Item item, GameObject slot) {
        return getItemCountForSlot(slot) == 0 ||
            (sameAsStackType(item, slot) &&
            getItemCountForSlot(slot) < item.i_maxStackCount);
    }
    void setItemCountTextForSlot(GameObject slot) {
        if(slot.GetComponentInChildren<TextMeshProUGUI>() == null)
            return;
        if(getItemCountForSlot(slot) > 1) {
            slot.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            int count = getItemCountForSlot(slot.gameObject);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
        }
        else
            slot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }


    public void selectWeaponFromInventory(GameObject pressedButton) {
        InventoryWeapon prevWeapon = null;
        if(heldObject != null) {
            heldObject.setNewSlot(pressedButton);
            prevWeapon = (InventoryWeapon)heldObject;
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
        InventoryArmor prevArmor = null;
        if(heldObject != null) {
            heldObject.setNewSlot(pressedButton);
            prevArmor = (InventoryArmor)heldObject;
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

    public void selectItemFromInventory(GameObject pressedButton) {
        if(pressedButton == null)
            return;

        //  Takes just one item from stack (right click)
        if(Input.GetMouseButton(1)) {
            //  adds the held object to the slot stack if there is enough room and the items are the same type
            if(heldObject != null) {
                if(heldObject.GetType() != typeof(InventoryItemStack)) {
                    //  checks if the stack is populated by the same item type or if the stack is empty
                    if(canFitInStack(((InventoryItem)heldObject).item, pressedButton)) {
                        heldObject.setNewSlot(pressedButton);
                        inventoryItems.Add((InventoryItem)heldObject);
                        heldObject = null;
                    }
                }
                else {
                    //  adds one item from the held stack to the stack
                    if(canFitInStack(((InventoryItemStack)heldObject).items[0].item, pressedButton)) {
                        ((InventoryItemStack)heldObject).items[0].setNewSlot(pressedButton);
                        inventoryItems.Add(((InventoryItemStack)heldObject).items[0]);
                        ((InventoryItemStack)heldObject).items.RemoveAt(0);
                    }
                    if(((InventoryItemStack)heldObject).items.Count > 0) {
                        ((InventoryItemStack)heldObject).setCountText();
                    }
                    else {
                        if(((InventoryItemStack)heldObject).countText != null)
                            Destroy(((InventoryItemStack)heldObject).countText.gameObject);
                        heldObject = null;
                    }
                }
            }
            else {
                //  taking an item from the inventory
                foreach(var i in inventoryItems) {
                    if(i.slot == pressedButton) {
                        heldObject = i;
                        inventoryItems.Remove(i);
                        break;
                    }
                }
            }
        }

        //  Takes the entire stack (left click)
        else if(Input.GetMouseButton(0) && clickCount < 2) {
            //  addes the held object to the slot stack if there is enough room and the items are the same type
            if(heldObject != null) {
                if(heldObject.GetType() != typeof(InventoryItemStack)) {
                    //  checks if the stack is populated by the same item type or if the stack is empty
                    if(canFitInStack(((InventoryItem)heldObject).item, pressedButton)) {
                        heldObject.setNewSlot(pressedButton);
                        inventoryItems.Add((InventoryItem)heldObject);
                        heldObject = null;
                    }
                }
                else {
                    //  adds as many items to the stack as possible
                    while(((InventoryItemStack)heldObject).items.Count > 0 && canFitInStack(((InventoryItemStack)heldObject).items[0].item, pressedButton)) {
                        ((InventoryItemStack)heldObject).items[0].setNewSlot(pressedButton);
                        inventoryItems.Add(((InventoryItemStack)heldObject).items[0]);
                        ((InventoryItemStack)heldObject).items.RemoveAt(0);
                        setItemCountTextForSlot(pressedButton);
                    }
                    if(((InventoryItemStack)heldObject).items.Count > 1) {
                        ((InventoryItemStack)heldObject).countText.text = ((InventoryItemStack)heldObject).items.Count.ToString();
                    }
                    else if(((InventoryItemStack)heldObject).items.Count == 1) {
                        ((InventoryItemStack)heldObject).countText.enabled = false;
                    }
                    else {
                        if(((InventoryItemStack)heldObject).countText != null)
                            Destroy(((InventoryItemStack)heldObject).countText.gameObject);
                        heldObject = null;
                    }
                }
            }
            else {
                //  taking a stack from the inventory
                var temp = new InventoryItemStack(pressedButton, supportItemInventorySlots.transform);

                foreach(var i in inventoryItems) {
                    if(i.slot == pressedButton) {
                        temp.items.Add(i);
                    }
                }
                if(temp.items.Count > 0) {
                    foreach(var i in temp.items)
                        inventoryItems.Remove(i);

                    temp.setCountText();
                    heldObject = temp;
                }
            }
        }

        //  Takes half of the stack (middle mouse)
        else if(Input.GetMouseButton(2)) {
            //  addes the held object to the slot stack if there is enough room and the items are the same type
            if(heldObject != null) {
                if(heldObject.GetType() != typeof(InventoryItemStack)) {
                    //  checks if the stack is populated by the same item type or if the stack is empty
                    if(canFitInStack(((InventoryItem)heldObject).item, pressedButton)) {
                        heldObject.setNewSlot(pressedButton);
                        inventoryItems.Add((InventoryItem)heldObject);
                        heldObject = null;
                    }
                }
                else {
                    //  adds as many items to the stack as possible
                    while(((InventoryItemStack)heldObject).items.Count > 0 && canFitInStack(((InventoryItemStack)heldObject).items[0].item, pressedButton)) {
                        ((InventoryItemStack)heldObject).items[0].setNewSlot(pressedButton);
                        inventoryItems.Add(((InventoryItemStack)heldObject).items[0]);
                        ((InventoryItemStack)heldObject).items.RemoveAt(0);
                        setItemCountTextForSlot(pressedButton);
                    }
                    if(((InventoryItemStack)heldObject).items.Count > 1) {
                        ((InventoryItemStack)heldObject).countText.text = ((InventoryItemStack)heldObject).items.Count.ToString();
                    }
                    else if(((InventoryItemStack)heldObject).items.Count == 1) {
                        ((InventoryItemStack)heldObject).countText.enabled = false;
                    }
                    else {
                        if(((InventoryItemStack)heldObject).countText != null)
                            Destroy(((InventoryItemStack)heldObject).countText.gameObject);
                        heldObject = null;
                    }
                }
            }
            else {
                if(getItemCountForSlot(pressedButton) > 0) {
                    //  taking half a stack from the inventory
                    var temp = new InventoryItemStack(pressedButton, supportItemInventorySlots.transform);
                    int numberToAdd = getItemCountForSlot(pressedButton);
                    if(numberToAdd % 2 != 0)
                        numberToAdd++;
                    numberToAdd /= 2;
                    foreach(var i in inventoryItems) {
                        if(numberToAdd == 0)
                            break;
                        else if(i.slot == pressedButton) {
                            temp.items.Add(i);
                            numberToAdd--;
                        }
                    }
                    if(temp.items.Count > 0) {
                        foreach(var i in temp.items)
                            inventoryItems.Remove(i);

                        temp.setCountText();
                        heldObject = temp;
                    }
                }
            }
        }


        //  autofills and takes a stack (double click)
        else if(Input.GetMouseButton(0) && clickCount > 1) {
            //  held object will already be populated with the items added from when the click count was 1
            if(heldObject != null) {
                

                if(((InventoryItemStack)heldObject).items.Count > 0) {
                    //  fills the stack with more items
                    List<GameObject> slotsTakenFrom = new List<GameObject>();

                    foreach(var i in inventoryItems) {
                        if(((InventoryItemStack)heldObject).items.Count >= ((InventoryItemStack)heldObject).items[0].item.i_maxStackCount)
                            break;
                        else if(i.item.isEqualTo(((InventoryItemStack)heldObject).items[0].item)) {
                            ((InventoryItemStack)heldObject).items.Add(i);
                            slotsTakenFrom.Add(i.slot);
                        }
                    }
                    foreach(var i in ((InventoryItemStack)heldObject).items)
                        inventoryItems.Remove(i);

                    foreach(var i in slotsTakenFrom)
                        setItemCountTextForSlot(i);

                    ((InventoryItemStack)heldObject).setCountText();
                }
            }
        }


        setItemCountTextForSlot(pressedButton);
    }


    public void equipHeldWeapon() {
        if(heldObject != null) {
            var prev = shownUnit.GetComponent<UnitClass>().stats.equippedWeapon;

            var newUnitWeapon = new InventoryWeapon();
            newUnitWeapon = (InventoryWeapon)heldObject;

            var newInvWeapon = new InventoryWeapon();
            newInvWeapon.weapon = prev;
            heldObject.icon.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedWeapon.w_sprite.getSprite();
            newInvWeapon.icon = heldObject.icon;

            shownUnit.GetComponent<UnitClass>().setEquippedWeapon(newUnitWeapon.weapon);
            Inventory.removeWeapon(newUnitWeapon.weapon);
            Inventory.addNewWeapon(prev);
            heldObject = newInvWeapon;

            updateUnitImages();
        }
    }

    public void equipHeldArmor() {
        if(heldObject != null) {
            var prev = shownUnit.GetComponent<UnitClass>().stats.equippedArmor;

            var newUnitArmor = new InventoryArmor();
            newUnitArmor = (InventoryArmor)heldObject;

            var newInvArmor = new InventoryArmor();
            newInvArmor.armor = prev;
            heldObject.icon.GetComponent<Image>().sprite = shownUnit.GetComponent<UnitClass>().stats.equippedArmor.a_sprite.getSprite();
            newInvArmor.icon = heldObject.icon;

            shownUnit.GetComponent<UnitClass>().setEquippedArmor(newUnitArmor.armor);
            Inventory.overrideArmor(Inventory.getArmorIndex(newUnitArmor.armor), prev);
            heldObject = newInvArmor;

            updateUnitImages();
        }
    }

    public void useHeldItemOnUnit() {
        if(heldObject != null) {
            if(heldObject.GetType() == typeof(InventoryItem)) {
                var temp = new InventoryItem();
                temp = (InventoryItem)heldObject;

                temp.item.applyEffect(shownUnit);
                Inventory.removeItem(temp.item);
                temp.destory();

                heldObject = null;
            }
            else if(heldObject.GetType() == typeof(InventoryItemStack)) {
                ((InventoryItemStack)heldObject).items[0].item.applyEffect(shownUnit);
                Inventory.removeItem(((InventoryItemStack)heldObject).items[0].item);
                ((InventoryItemStack)heldObject).items[0].destory();
                ((InventoryItemStack)heldObject).items.RemoveAt(0);
                ((InventoryItemStack)heldObject).setCountText();
            }

            updateUnitImages();
        }
    }


    public void useHeldObject() {
        if(heldObject != null) {
            if(heldObject.GetType() == typeof(InventoryWeapon))
                equipHeldWeapon();
            else if(heldObject.GetType() == typeof(InventoryArmor))
                equipHeldArmor();
            else if(heldObject.GetType() == typeof(InventoryItem) || heldObject.GetType() == typeof(InventoryItemStack))
                useHeldItemOnUnit();
        }
    }


    IEnumerator decreaseClickCount() {
        yield return new WaitForSeconds(0.35f);

        clickCount = 0;
        clickCountDecreaser = null;
    }
}
