using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NPCPreset", menuName = "Presets/NPCPreset")]
public class NPCPreset : ScriptableObject {
    public TownMember member;
    public WeaponPreset weapon;
    public ArmorPreset armor;

    public DialogInfo dialog;
}
