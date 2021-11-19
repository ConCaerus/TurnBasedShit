using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitCanvas : MonoBehaviour {
    public TextMeshProUGUI nameText, powerText, defenceText, speedText, critText, firstTraitText, secondTraitText, levelText;
    public Slider healthSlider, expSlider;
    public Image faceImage, headImage, bodyImage, rArmImage, lArmImage, weaponImage, armorImage, itemImage;
    public Button leaderButton;
    public GameObject edgedSlider, bluntSlider, summonSlider;
    public List<GameObject> lockImages = new List<GameObject>();

    public float equippmentTransitionTime = 0.01f;
    public float timeBtwTransition = 0.05f;

    const string indexTag = "UnitCanvas shown unit index";
    public UnitStats shownUnit;

    public Color leaderColor, nonLeaderColor;

    public Slider rSlider, gSlider, bSlider;
    bool lockColor = true;


    List<GameObject> traits = new List<GameObject>();


    private void Start() {
        setup();
        rSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
        gSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
        bSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
    }


    public void setup() {
        lockColor = true;
        if(SaveData.getInt(indexTag) < Party.getMemberCount())
            shownUnit = Party.getMemberStats(SaveData.getInt(indexTag));
        else {
            shownUnit = Party.getMemberStats(0);
            SaveData.setInt(indexTag, 0);
        }
        updateLockedState();
        updateUnitWindow();
    }

    public void updateUnitWindow() {
        if(!FindObjectOfType<MenuCanvas>().isOpen())
            return;
        nameText.text = shownUnit.u_name;
        levelText.text = shownUnit.u_level.ToString();
        healthSlider.maxValue = shownUnit.getModifiedMaxHealth();
        healthSlider.value = shownUnit.u_health;
        expSlider.maxValue = shownUnit.u_expCap;
        expSlider.value = shownUnit.u_exp;
        healthSlider.GetComponent<InfoBearer>().setInfo(shownUnit.u_health.ToString("0.0") + " / " + shownUnit.getModifiedMaxHealth().ToString("0.0"));
        expSlider.GetComponent<InfoBearer>().setInfo("EXP: " + shownUnit.u_exp.ToString("0.0") + " / " + shownUnit.u_expCap.ToString("0.0"));

        rSlider.value = shownUnit.u_sprite.color.r;
        gSlider.value = shownUnit.u_sprite.color.g;
        bSlider.value = shownUnit.u_sprite.color.b;

        if(shownUnit.isTheSameInstanceAs(Party.getLeaderStats()))
            leaderButton.GetComponent<Image>().color = leaderColor;
        else
            leaderButton.GetComponent<Image>().color = nonLeaderColor;

        //  stats
        powerText.text = shownUnit.u_power.ToString("0.0");
        defenceText.text = shownUnit.u_defence.ToString("0.0");
        speedText.text = shownUnit.u_speed.ToString("0.0");
        critText.text = shownUnit.u_critChance.ToString("0.00");


        //  traits
        foreach(var i in traits)
            Destroy(i.gameObject);
        traits.Clear();

        int traitCount = shownUnit.u_traits.Count;
        firstTraitText.text = "";
        secondTraitText.text = "";
        if(traitCount > 0) {
            //  does not need second trait preset
            if(traitCount == 1) {
                firstTraitText.text = shownUnit.u_traits[0].t_name;
                firstTraitText.GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitTrait(shownUnit.u_traits[0]));
                secondTraitText.text = "";
            }
            else {
                Vector2 prevPos = secondTraitText.transform.position;
                for(int i = 0; i < traitCount; i++) {
                    if(i == 0) {
                        firstTraitText.text = shownUnit.u_traits[i].t_name;
                        firstTraitText.GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitTrait(shownUnit.u_traits[i]));
                    }
                    else if(i == 1) {
                        secondTraitText.text = shownUnit.u_traits[i].t_name;
                        secondTraitText.GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitTrait(shownUnit.u_traits[i]));
                    }

                    //  create a new trait text
                    else {
                        float buffer = firstTraitText.transform.position.y - secondTraitText.transform.position.y;
                        var t = Instantiate(firstTraitText, firstTraitText.transform.parent);
                        t.transform.position = prevPos - new Vector2(0.0f, buffer);
                        t.text = shownUnit.u_traits[i].t_name;
                        t.GetComponent<InfoBearer>().setInfo(InfoTextCreator.createForUnitTrait(shownUnit.u_traits[i]));
                        traits.Add(t.gameObject);


                        prevPos = t.transform.position;
                    }
                }
            }
        }

        //  images
        faceImage.sprite = FindObjectOfType<PresetLibrary>().getUnitFace(shownUnit.u_sprite.faceIndex);
        faceImage.SetNativeSize();
        headImage.sprite = FindObjectOfType<PresetLibrary>().getUnitHead(shownUnit.u_sprite.headIndex).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        headImage.SetNativeSize();
        bodyImage.sprite = FindObjectOfType<PresetLibrary>().getUnitBody(shownUnit.u_sprite.bodyIndex).GetComponent<SpriteRenderer>().sprite;
        bodyImage.SetNativeSize();
        rArmImage.sprite = FindObjectOfType<PresetLibrary>().getUnitArm(shownUnit.u_sprite.bodyIndex).GetComponent<SpriteRenderer>().sprite;
        rArmImage.SetNativeSize();
        lArmImage.sprite = FindObjectOfType<PresetLibrary>().getUnitArm(shownUnit.u_sprite.bodyIndex).GetComponent<SpriteRenderer>().sprite;
        lArmImage.SetNativeSize();

        headImage.color = shownUnit.u_sprite.color;
        bodyImage.color = shownUnit.u_sprite.color;
        rArmImage.color = shownUnit.u_sprite.color;
        lArmImage.color = shownUnit.u_sprite.color;


        var weaponInfo = weaponImage.transform.parent.GetComponent<InfoBearer>();
        var armorInfo = armorImage.transform.parent.GetComponent<InfoBearer>();
        var itemInfo = itemImage.transform.parent.GetComponent<InfoBearer>();

        weaponInfo.setInfo(InfoTextCreator.createForCollectable(shownUnit.equippedWeapon));
        armorInfo.setInfo(InfoTextCreator.createForCollectable(shownUnit.equippedArmor));
        itemInfo.setInfo(InfoTextCreator.createForCollectable(shownUnit.equippedItem));

        weaponImage.enabled = true;
        armorImage.enabled = true;
        itemImage.enabled = true;
        if(shownUnit.equippedWeapon != null && !shownUnit.equippedWeapon.isEmpty()) {
            weaponImage.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(shownUnit.equippedWeapon).sprite;
            weaponImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownUnit.equippedWeapon.rarity);
        }
        else {
            weaponImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;
            weaponImage.enabled = false;
        }
        if(shownUnit.equippedArmor != null && !shownUnit.equippedArmor.isEmpty()) {
            armorImage.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(shownUnit.equippedArmor).sprite;
            armorImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownUnit.equippedArmor.rarity);
        }
        else {
            armorImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;
            armorImage.enabled = false;
        }
        if(shownUnit.equippedItem != null && !shownUnit.equippedItem.isEmpty()) {
            itemImage.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownUnit.equippedItem).sprite;
            if(shownUnit.equippedItem.isTheSameTypeAs(FindObjectOfType<PresetLibrary>().getItem("Dummy")))
                itemImage.color = shownUnit.u_sprite.color;
            else
                itemImage.color = Color.white;
            itemImage.transform.parent.GetChild(0).GetComponent<Image>().color = FindObjectOfType<PresetLibrary>().getRarityColor(shownUnit.equippedItem.rarity);
        }
        else {
            itemImage.transform.parent.GetChild(0).GetComponent<Image>().color = Color.gray;
            itemImage.color = Color.white;
            itemImage.enabled = false;
        }


        //  skill sliders
        edgedSlider.GetComponent<CircularSlider>().setText(shownUnit.getEdgedLevel().ToString());
        edgedSlider.GetComponent<CircularSlider>().setValue((shownUnit.u_edgedExp / shownUnit.u_skillExpCap) - shownUnit.getEdgedLevel() + 1);

        bluntSlider.GetComponent<CircularSlider>().setText(shownUnit.getBluntLevel().ToString());
        bluntSlider.GetComponent<CircularSlider>().setValue((shownUnit.u_bluntExp / shownUnit.u_skillExpCap) - shownUnit.getBluntLevel() + 1);

        summonSlider.GetComponent<CircularSlider>().setText(shownUnit.getSummonedLevel().ToString());
        summonSlider.GetComponent<CircularSlider>().setValue((shownUnit.u_summonedExp / shownUnit.u_skillExpCap) - shownUnit.getSummonedLevel() + 1);


        if(FindObjectOfType<PartyObject>() != null && FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit) != null) {
            FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit).GetComponent<PlayerUnitInstance>().updateSprites();
        }
        if(FindObjectOfType<MapMovement>() != null) {
            FindObjectOfType<MapMovement>().setVisuals();
            FindObjectOfType<MapMovement>().setSideUnitVisuals();
        }
        if(FindObjectOfType<LocationMovement>() != null && shownUnit != null && !shownUnit.isEmpty()) {
            FindObjectOfType<LocationMovement>().setVisuals();
        }
        if(FindObjectOfType<FishingUnit>() != null && shownUnit != null && !shownUnit.isEmpty())
            FindObjectOfType<FishingUnit>().setVisuals();

        lockColor = false;
    }


    public void updateUnitColor() {
        if(lockColor)
            return;
        shownUnit.u_sprite.color = new Color(rSlider.value, gSlider.value, bSlider.value);
        saveShownUnit();
        updateUnitWindow();
    }

    void updateLockedState() {
        if(FindObjectOfType<PartyObject>() != null) {
            foreach(var i in lockImages)
                i.SetActive(true);
            FindObjectOfType<WeaponSelectionCanvas>().enabled = false;
            FindObjectOfType<ArmorSelectionCanvas>().enabled = false;
            FindObjectOfType<ItemSelectionCanvas>().enabled = false;
        }
        else {
            foreach(var i in lockImages)
                i.SetActive(false);
        }
    }


    public void setUnitWeapon(Weapon w) {
        if(w == null || w.isEmpty()) {
            shownUnit.equippedWeapon = null;
            saveShownUnit();
            updateUnitWindow();
            return;
        }
        shownUnit.equippedWeapon = new Weapon();
        shownUnit.equippedWeapon.setEqualTo(w, true);
        saveShownUnit();
        updateUnitWindow();
    }
    public void setUnitArmor(Armor a) {
        if(a == null || a.isEmpty()) {
            shownUnit.equippedArmor = null;
            saveShownUnit();
            updateUnitWindow();
            return;
        }
        shownUnit.equippedArmor = new Armor();
        shownUnit.equippedArmor.setEqualTo(a, true);
        saveShownUnit();
        updateUnitWindow();
    }
    public void setUnitItem(Item i) {
        if(i == null || i.isEmpty()) {
            shownUnit.equippedItem = null;
            saveShownUnit();
            updateUnitWindow();
            return;
        }
        shownUnit.equippedItem = new Item();
        shownUnit.equippedItem.setEqualTo(i, true);
        saveShownUnit();
        updateUnitWindow();
    }


    //  buttons
    public void cycleUnit(bool right) {
        lockColor = true;
        int index = Party.getUnitIndex(shownUnit);

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= Party.getMemberCount())
            index = 0;
        else if(index < 0)
            index = Party.getMemberCount() - 1;

        SaveData.setInt(indexTag, index);
        shownUnit = Party.getMemberStats(index);
        updateUnitWindow();
    }
    public void setLeader() {
        Party.setLeader(shownUnit);
        updateUnitWindow();
    }

    public void cycleFace(bool right) {
        int index = shownUnit.u_sprite.faceIndex;

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= FindObjectOfType<PresetLibrary>().getFaceCount())
            index = 0;
        else if(index < 0)
            index = FindObjectOfType<PresetLibrary>().getFaceCount() - 1;

        shownUnit.u_sprite.faceIndex = index;
        saveShownUnit();
        updateUnitWindow();
    }
    public void cycleHead(bool right) {
        int index = shownUnit.u_sprite.headIndex;

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= FindObjectOfType<PresetLibrary>().getHeadCount())
            index = 0;
        else if(index < 0)
            index = FindObjectOfType<PresetLibrary>().getHeadCount() - 1;

        shownUnit.u_sprite.headIndex = index;
        saveShownUnit();
        updateUnitWindow();
    }
    public void cycleBody(bool right) {
        int index = shownUnit.u_sprite.bodyIndex;

        //  right
        if(right) {
            index++;
        }
        //  left
        else {
            index--;
        }

        if(index >= FindObjectOfType<PresetLibrary>().getBodyCount())
            index = 0;
        else if(index < 0)
            index = FindObjectOfType<PresetLibrary>().getBodyCount() - 1;

        shownUnit.u_sprite.bodyIndex = index;
        saveShownUnit();
        updateUnitWindow();
    }

    public void renameUnit() {
        nameText.alignment = TextAlignmentOptions.TopLeft;
        FindObjectOfType<TextInputReader>().startReading(GameVariables.getMaxPlayerUnitNameLength(), updateNameText, setUnitName);
    }

    public void updateNameText(string newName) {
        shownUnit.u_name = newName;
        nameText.text = newName;
    }
    public void setUnitName(string newName) {
        nameText.alignment = TextAlignmentOptions.TopGeoAligned;
        shownUnit.u_name = newName;
        saveShownUnit();
    }


    public void saveShownUnit() {
        if(FindObjectOfType<PartyObject>() != null)
            FindObjectOfType<PartyObject>().resaveInstantiatedUnit(shownUnit);
        else
            Party.overrideUnit(shownUnit);
    }
}
