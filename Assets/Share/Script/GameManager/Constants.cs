using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly string GOBLIN_ATTACK_TAG = "GoblinAttack";
    public static readonly string GOBLIN_TAG = "Goblin";
    public static readonly string STATUE_ATTACK_TAG = "StatueAttack";
    public static readonly string STATUE_TAG = "Statue";
    public static readonly string CRYSTAL_TAG = "Crystal";
    public static readonly string CRYSTAL_INATTACKAREA = "Crystal_attack";
    public static readonly float CAMERA_MIN_X = 6.43f;
    public static readonly float CAMERA_MAX_X = 33.24f;
    //スタチュー生成時のコスト
    public static readonly int[] STATUE_COST = { 10, 20, 30, 40, 50 };

    //シーン名の定数
    public static readonly string TITLE_SCNENE = "TitleScene";
    public static readonly string LOGIN_SCNENE = "LoginScene";
    public static readonly string MENU_SCNENE = "MenuScene";
    public static readonly string CONNECT_ROOM_SCNENE = "ConnectRoomScene";
    public static readonly string STAGE_SELECT_SCNENE = "StageSelectScene";
    public static readonly string GAME_SET_SCNENE = "GameSetScene";
    public static readonly string ONRINE_WAIT_SCENE = "OnlineWait";

     //カスタムメニューのパス名
    public static readonly string DEBUG_MODE_MENU_PATH = "CustomMenu/DebugMode";

    //広告表示変数。何回試合をやったか。
    public static readonly int ADVERTISE_LIMIT = 3;

    //ゴブリンを出すAIの性格
    public enum GoblinPersonality
    {
        Aggressive,
        Negative,
        Random,
        flexible,
        Num
    }

    //ゴブリンのタイプ
    public enum GoblinType
    {
        Knuckle,
        Axe,
        Lance,
        Num
    }

    //スタチューのタイプ
    public enum StatueType
    {
        zako,
        futuu,
        masi,
        tuyoi,
        saikyou,
        Num
    }

    public enum SceneName
    {
        TITLE_SCNENE,
        LOGIN_SCNENE,
        MENU_SCNENE,
        CONNECT_ROOM_SCNENE,
        STAGE_SELECT_SCNENE,
        GAME_SET_SCNENE,
        Num
    }


}
