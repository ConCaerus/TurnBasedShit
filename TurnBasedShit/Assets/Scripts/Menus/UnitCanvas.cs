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

    public float equippmentTransitionTime = 0.15f;
    public float timeBtwTransition = 0.05f;

    public UnitStats shownUnit;

    public Color worthlessColor, commonColor, uncommonColor, unusualColor, rareColor, legendaryColor, mythicalColor;
    public Color leaderColor, nonLeaderColor;

    public Slider rSlider, gSlider, bSlider;


    List<GameObject> traits = new List<GameObject>();


    private void Start() {
        setup();
        rSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
        gSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
        bSlider.onValueChanged.AddListener(delegate { updateUnitColor(); });
    }


    public void setup() {
        shownUnit = Party.getMemberStats(0);
        updateUnitWindow();
    }

    public void updateUnitWindow() {
        nameText.text = shownUnit.u_name;
        levelText.text = shownUnit.u_level.ToString();
        healthSlider.maxValue = shownUnit.getModifiedMaxHealth();
        healthSlider.value = shownUnit.u_health;
        expSlider.maxValue = shownUnit.u_expCap;
        expSlider.value = shownUnit.u_exp;

        rSlider.value = shownUnit.u_sprite.color.r;
        gSlider.value = shownUnit.u_sprite.color.g;
        bSlider.value = shownUnit.u_sprite.color.b;

        weaponImage.transform.parent.GetChild(0).GetComponent<Image>().color = getRarityColor(shownUnit.equippedWeapon.w_rarity);
        armorImage.transform.parent.GetChild(0).GetComponent<Image>().color = getRarityColor(shownUnit.equippedArmor.a_rarity);
        itemImage.transform.parent.GetChild(0).GetComponent<Image>().color = getRarityColor(shownUnit.equippedItem.i_rarity);

        if(shownUnit.u_order == Party.getLeaderID())
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
                secondTraitText.text = "";
            }
            else {
                Vector2 prevPos = secondTraitText.transform.position;
                for(int i = 0; i < traitCount; i++) {
                    if(i == 0)
                        firstTraitText.text = shownUnit.u_traits[i].t_name;
                    else if(i == 1)
                        secondTraitText.text = shownUnit.u_traits[i].t_name;

                    //  create a new trait text
                    else {
                        float buffer = firstTraitText.transform.position.y - secondTraitText.transform.position.y;
                        var t = Instantiate(firstTraitText, firstTraitText.transform.parent);
                        t.transform.position = prevPos - new Vector2(0.0f, buffer);
                        t.text = shownUnit.u_traits[i].t_name;
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

        weaponImage.enabled = true;
        armorImage.enabled = true;
        itemImage.enabled = true;
        if(shownUnit.equippedWeapon != null && !shownUnit.equippedWeapon.isEmpty())
            weaponImage.sprite = FindObjectOfType<PresetLibrary>().getWeaponSprite(shownUnit.equippedWeapon).sprite;
        else
            weaponImage.enabled = false;
        if(shownUnit.equippedArmor != null && !shownUnit.equippedArmor.isEmpty())
            armorImage.sprite = FindObjectOfType<PresetLibrary>().getArmorSprite(shownUnit.equippedArmor).sprite;
        else
            armorImage.enabled = false;
        if(shownUnit.equippedItem != null && !shownUnit.equippedItem.isEmpty())
            itemImage.sprite = FindObjectOfType<PresetLibrary>().getItemSprite(shownUnit.equippedItem).sprite;
        else
            itemImage.enabled = false;

        if(FindObjectOfType<PartyObject>() != null && FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit) != null) {
            FindObjectOfType<PartyObject>().getInstantiatedMember(shownUnit).GetComponent<PlayerUnitInstance>().updateSprites();
        }
    }


    public void updateUnitColor() {
        shownUnit.u_sprite.color = new Color(rSlider.value, gSlider.value, bSlider.value);
        Party.overrideUnit(shownUnit, FindObjectOfType<PartyObject>());
        updateUnitWindow();
    }


    public Color getRarityColor(GameInfo.rarityLvl rar) {
        switch(rar) {
            case GameInfo.rarityLvl.Worthless:
                return worthlessColor;

            case GameInfo.rarityLvl.Common:
                return commonColor;

            case GameInfo.rarityLvl.Uncommon:
                return uncommonColor;

            case GameInfo.rarityLvl.Unusual:
                return unusualColor;

            case GameInfo.rarityLvl.Rare:
                return rareColor;

            case GameInfo.rarityLvl.Legendary:
                return legendaryColor;

            case GameInfo.rarityLvl.Mythical:
                return mythicalColor;
        }

        return new Color();
    }


    public void setUnitWeapon(Weapon w) {
        shownUnit.equippedWeapon = new Weapon();
        shownUnit.equippedWeapon.setEqualTo(w, true);
        Party.overrideUnit(shownUnit, FindObjectOfType<PartyObject>());
        updateUnitWindow();
    }
    public void setUnitArmor(Armor a) {
        shownUnit.equippedArmor.setEqualTo(a, true);
        Party.overrideUnit(shownUnit, FindObjectOfType<PartyObject>());
        updateUnitWindow();
    }
    public void setUnitItem(Item i) {
        shownUnit.equippedItem.setEqualTo(i, true);
        Party.overrideUnit(shownUnit, FindObjectOfType<PartyObject>());
        updateUnitWindow();
    }


    //  buttons
    public void cycleUnit(bool right) {
        int index = shownUnit.u_order;

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
        Party.overrideUnit(shownUnit);
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
        Party.overrideUnit(shownUnit, FindObjectOfType<PartyObject>());
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
        Party.overrideUnit(shownUnit);
        updateUnitWindow();
    }
}
