using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs
{
    public static int Coins
    {
        set => PlayerPrefs.SetInt(Const.COIN_KEY, value);
        get => PlayerPrefs.GetInt(Const.COIN_KEY);
    }
    
    public static int LevelDone1
    {
        set => PlayerPrefs.SetInt(Const.LEVEL_ZONE1, value);
        get => PlayerPrefs.GetInt(Const.LEVEL_ZONE1);
    }
    public static int LevelDone2
    {
        set => PlayerPrefs.SetInt(Const.LEVEL_ZONE2, value);
        get => PlayerPrefs.GetInt(Const.LEVEL_ZONE2);
    }
    public static int LevelDone3
    {
        set => PlayerPrefs.SetInt(Const.LEVEL_ZONE3, value);
        get => PlayerPrefs.GetInt(Const.LEVEL_ZONE3);
    }
    public static int ZoneLevel1
    {
        set => PlayerPrefs.SetInt(Const.ZONE_1, value);
        get => PlayerPrefs.GetInt(Const.ZONE_1);
    }
    public static int ZoneLevel2
    {
        set => PlayerPrefs.SetInt(Const.ZONE_2, value);
        get => PlayerPrefs.GetInt(Const.ZONE_2);
    }
    public static int ZoneLevel3
    {
        set => PlayerPrefs.SetInt(Const.ZONE_3, value);
        get => PlayerPrefs.GetInt(Const.ZONE_3);
    }
    

}
