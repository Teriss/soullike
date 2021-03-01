using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatabsase
{
    private string weaponDatabaseFileName = "weaponData";
    public readonly JSONObject DB;

    public GameDatabsase() {
        try {
        TextAsset weaponText = Resources.Load("weapon/"+weaponDatabaseFileName) as TextAsset;
        DB = new JSONObject(weaponText.text);
        }
        catch (System.Exception) {
            throw new Exception("缺少数据文件！");
        }
        
    }
}
