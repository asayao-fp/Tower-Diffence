using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly string GOBLIN_ATTACK_TAG = "GoblinAttack";
    public static readonly string GOBLIN_TAG = "Goblin";
    public static readonly string STATUE_ATTACK_TAG = "StatueAttack";
    public static readonly string STATUE_TAG = "Statue";
    public static readonly float CAMERA_MIN_X = 6.43f;
    public static readonly float CAMERA_MAX_X = 33.24f;
    //スタチュー生成時のコスト
    public static readonly int[] STATUE_COST = { 10, 20, 30, 40, 50 };

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


}
