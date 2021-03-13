using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData {
    public static int saveIndex = 0;
    static string saveIndexTag() {
        return "Save" + saveIndex.ToString() + " ";
    }


    public static void setString(string tag, string save) {
        PlayerPrefs.SetString(saveIndexTag() + tag, save);
    }
    public static string getString(string tag) {
        return PlayerPrefs.GetString(saveIndexTag() + tag, null);
    }


    public static void setInt(string tag, int save) {
        PlayerPrefs.SetInt(saveIndexTag() + tag, save);
    }
    public static int getInt(string tag) {
        return PlayerPrefs.GetInt(saveIndexTag() + tag, 0);
    }


    public static void setFloat(string tag, float save) {
        PlayerPrefs.SetFloat(saveIndexTag() + tag, save);
    }
    public static float getFloat(string tag) {
        return PlayerPrefs.GetFloat(saveIndexTag() + tag, 0.0f);
    }


    public static void deleteKey(string tag) {
        PlayerPrefs.DeleteKey(saveIndexTag() + tag);
    }
    public static void save() {
        PlayerPrefs.Save();
    }


    public static void deleteSave(int i) {
        saveIndex = i;
        Inventory.clearInventory();
        Party.clearParty();
        saveIndex = 0;
    }
}
