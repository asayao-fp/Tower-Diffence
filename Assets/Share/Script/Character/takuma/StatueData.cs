using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable/Create StatueData")]
public class StatueData : ScriptableObject
{
   public int id; //ユニークID
   public String name; //名前
   public Vector2 setpos; //召喚可能範囲
   public Vector3 attackpos; //攻撃範囲
   public byte settype; //設置可能タイプ
   public int hp; //体力
   public int time; //消滅までの時間
   public int atkInterval; //攻撃間隔
   public int cost; //召喚コスト
   public String name4Preview; //表示用の名前

}
