using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UpgradeCanvas : MonoBehaviour {
    //  0 - weapon, 1 - armor
    public int state = 0;

    float statInc = 0.0f;
    Weapon.attribute weaponUpgrade;
    Armor.attributes armorUpgrade;
    [SerializeField] TextMeshProUGUI attributeText, statText;

    [SerializeField] TextMeshProUGUI title;

    [SerializeField] float slotTopY = 120, slotBotY = -120;
    [SerializeField] float slotBuffer = 10.0f;
    [SerializeField] float scrollSpeed = 35.0f;

    [SerializeField] GameObject slotHolder;
    [SerializeField] GameObject slotObject;
    List<GameObject> slots = new List<GameObject>();
    GameObject selectedSlot = null;

    private void Start() {
        if(GameInfo.getCurrentMapLocation() != null && GameInfo.getCurrentMapLocation().type == MapLocation.locationType.equipmentUpgrade)
            state = GameInfo.getCurrentMapLocationAsUpgrade().state;

        setUpgradeStats();

        switch(state) {
            //  Weapon
            case 0:
                title.text = "Upgrade Weapon";
                break;

            //  Armor
            case 1:
                title.text = "Upgrade Armor";
                break;
        }

        createSlots();
        updateInfo();
    }

    private void Update() {
        scrollThoughList();
        if(Input.GetMouseButtonDown(0) && getMousedOverSlot() != null) {
            selectedSlot = getMousedOverSlot();
            updateInfo();
        }
    }

    GameObject getMousedOverSlot() {
        //  return if their are no active slots
        if(slots.Count == 0)
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
            foreach(var i in slots) {
                if(hit.collider == i.GetComponent<Collider2D>()) {
                    foreach(var u in unwantedHits)
                        u.enabled = true;
                    return i.gameObject;
                }
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

    int getSelectedSlotIndex() {
        if(selectedSlot == null || slots.Count == 0)
            return -1;
        for(int i = 0; i < slots.Count; i++) {
            if(slots[i] == selectedSlot)
                return i;
        }
        return -1;
    }
    Weapon getWeaponInSelectedSlot() {
        int index = getSelectedSlotIndex();

        //  party weapons
        for(int i = 0; i < Party.getPartySize(); i++) {
            if(index <= 0)
                return Party.getMemberStats(i).equippedWeapon;
            if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty())
                index--;
        }


        //  Inventory weapons
        int inventoryWeaponCount = Inventory.getTypeCount(typeof(Weapon));

        if(inventoryWeaponCount > 0) {
            for(int i = 0; i < inventoryWeaponCount; i++) {
                if(index <= 0)
                    return Inventory.getWeapon(i);
                index--;
            }
        }
        return null;
    }
    Armor getArmorInSelectedSlot() {
        int index = getSelectedSlotIndex();

        //  party weapons
        for(int i = 0; i < Party.getPartySize(); i++) {
            if(index <= 0)
                return Party.getMemberStats(i).equippedArmor;
            if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty())
                index--;
        }


        //  Inventory weapons
        int inventoryArmorCount = Inventory.getTypeCount(typeof(Armor));

        if(inventoryArmorCount > 0) {
            for(int i = 0; i < inventoryArmorCount; i++) {
                if(index <= 0)
                    return Inventory.getArmor(i);
                index--;
            }
        }
        return null;
    }

    void updateInfo() {
        switch(state) {
            //  Weapon
            case 0:
                statText.text = "+ " + statInc.ToString("0.0") + " power";
                attributeText.text = "+ " + weaponUpgrade.ToString();
                break;

            //  Armor 
            case 1:
                statText.text = "+ " + statInc.ToString("0.0") + " defence";
                attributeText.text = "+ " + armorUpgrade.ToString();
                break;
        }

        foreach(var i in slots)
            i.GetComponent<Image>().color = Color.black;

        if(selectedSlot != null)
            selectedSlot.GetComponent<Image>().color = Color.grey;
    }

    void createSlots() {
        for(int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);
        slots.Clear();

        switch(state) {
            //  Weapons
            case 0:
                //  party weapons
                List<int> partyMembersWithWeapons = new List<int>();
                for(int i = 0; i < Party.getPartySize(); i++) {
                    if(Party.getMemberStats(i).equippedWeapon != null && !Party.getMemberStats(i).equippedWeapon.isEmpty())
                        partyMembersWithWeapons.Add(i);
                }

                if(partyMembersWithWeapons.Count > 0) {
                    for(int i = 0; i < partyMembersWithWeapons.Count; i++) {
                        createNewSlot(i, Party.getMemberStats(partyMembersWithWeapons[i]).equippedWeapon.w_sprite.getSprite(), Party.getMemberStats(partyMembersWithWeapons[i]).u_name);
                    }
                }

                //  Inventory weapons
                int inventoryWeaponCount = Inventory.getTypeCount(typeof(Weapon));

                if(inventoryWeaponCount > 0) {
                    for(int i = 0; i < inventoryWeaponCount; i++)
                        createNewSlot(i + partyMembersWithWeapons.Count, Inventory.getWeapon(i).w_sprite.getSprite());
                }
                break;

            //  Armor
            case 1:
                //  party armor
                List<int> partyMembersWithArmor = new List<int>();
                for(int i = 0; i < Party.getPartySize(); i++) {
                    if(Party.getMemberStats(i).equippedArmor != null && !Party.getMemberStats(i).equippedArmor.isEmpty())
                        partyMembersWithArmor.Add(i);
                }

                if(partyMembersWithArmor.Count > 0) {
                    for(int i = 0; i < partyMembersWithArmor.Count; i++) {
                        createNewSlot(i, Party.getMemberStats(partyMembersWithArmor[i]).equippedArmor.a_sprite.getSprite(), Party.getMemberStats(partyMembersWithArmor[i]).u_name);
                    }
                }


                //  Inventory Armor
                int armorCount = Inventory.getTypeCount(typeof(Armor));

                if(armorCount <= 0)
                    break;
                for(int i = 0; i < armorCount; i++)
                    createNewSlot(i + partyMembersWithArmor.Count, Inventory.getArmor(i).a_sprite.getSprite());
                break;
        }

        if(slots.Count > 0)
            scrollThoughList();
        updateInfo();
    }
    GameObject createNewSlot(int index, Sprite sp, string unitName = null) {
        var obj = Instantiate(slotObject, slotHolder.transform);

        //  position and scale
        float step = slotObject.GetComponent<RectTransform>().rect.height + slotBuffer;
        obj.transform.localPosition = new Vector3(0.0f, slotTopY - (step * index), 0.0f);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //  width and height
        var x = obj.GetComponent<RectTransform>().rect.width;
        var y = obj.GetComponent<RectTransform>().rect.height;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(x, y);

        //  text and images
        if(!string.IsNullOrEmpty(unitName))
            obj.GetComponentInChildren<TextMeshProUGUI>().text = unitName + "'s equipment";
        else
            obj.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
        obj.transform.GetChild(1).GetComponent<Image>().sprite = sp;
        obj.transform.GetChild(1).GetComponent<Image>().color = Color.white;

        slots.Add(obj);
        return obj;
    }

    void scrollThoughList() {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll == 0)
            return;

        scrollSlots(-scroll * scrollSpeed);

        //  Top bounds
        if(slots[0].transform.localPosition.y < slotTopY)
            scrollSlots(slotTopY - slots[0].transform.localPosition.y);

        //  Bot bounds
        else if(slots[slots.Count - 1].transform.localPosition.y > slotBotY)
            scrollSlots(slotBotY - slots[slots.Count - 1].transform.localPosition.y);
    }

    void scrollSlots(float val) {
        foreach(var i in slots) {
            i.transform.localPosition = new Vector3(0.0f, i.transform.localPosition.y + val, 0.0f);
        }
    }


    //  Buttons 
    public void upgrade() {
        if(selectedSlot != null) {
            switch(state) {
                //  Weapon
                case 0:
                    Weapon w = new Weapon();
                    Weapon weaponInSlot = getWeaponInSelectedSlot();
                    w.setEqualTo(weaponInSlot);
                    w.w_power += statInc;
                    w.w_attributes.Add(weaponUpgrade);

                    //  upgraded weapon is in the party
                    for(int i = 0; i < Party.getPartySize(); i++) {
                        if(Party.getMemberStats(i).equippedWeapon.isEqualTo(weaponInSlot)) {
                            var unit = Party.getMemberStats(i);
                            unit.equippedWeapon = w;
                            Party.overrideUnit(unit);
                            break;
                        }
                    }

                    //  upgraded weapon is in the inventory
                    for(int i = 0; i < Inventory.getTypeCount(typeof(Weapon)); i++) {
                        if(Inventory.getWeapon(i).isEqualTo(weaponInSlot)) {
                            Inventory.overrideWeapon(i, w);
                            break;
                        }
                    }
                    break;

                //  Armor
                case 1:
                    Armor a = new Armor();
                    Armor armorInSlot = getArmorInSelectedSlot();
                    a.setEqualTo(armorInSlot);
                    a.a_defence += statInc;
                    a.a_attributes.Add(armorUpgrade);

                    //  upgraded weapon is in the party
                    for(int i = 0; i < Party.getPartySize(); i++) {
                        if(Party.getMemberStats(i).equippedArmor.isEqualTo(armorInSlot)) {
                            var unit = Party.getMemberStats(i);
                            unit.equippedArmor = a;
                            Party.overrideUnit(unit);
                            break;
                        }
                    }

                    //  upgraded weapon is in the inventory
                    for(int i = 0; i < Inventory.getTypeCount(typeof(Armor)); i++) {
                        if(Inventory.getArmor(i).isEqualTo(armorInSlot)) {
                            Inventory.overrideArmor(i, a);
                            break;
                        }
                    }
                    break;
            }

            //  return to map
            SceneManager.LoadScene("Map");
        }
    }

    public void setUpgradeStats() {
        statInc = Random.Range(0.0f, 5.0f);
        weaponUpgrade = (Weapon.attribute)(Random.Range(0, Weapon.attributeCount));
        armorUpgrade = (Armor.attributes)(Random.Range(0, Armor.attributeCount));
        updateInfo();
    }
}
