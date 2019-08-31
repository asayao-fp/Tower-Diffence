using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;


public class FacilitySetting : MonoBehaviour
{
    private List<Facility> facilitylist = new List<Facility>();

    void Awake(){
      DontDestroyOnLoad(this);


      setFacility();
    }

    public Facility getFacilityList(int num){
      return facilitylist[num];
    }

    public Facility getFacility(String name){
      for(int i=0;i<facilitylist.Count;i++){
        if(facilitylist[i].facilityname.Equals(name)){
          return facilitylist[i];
        }
      }

      return null;
    }

    public void setFacility(){
      StreamReader sr = new StreamReader(System.IO.Path.GetFullPath("./Assets/takuma/Facility.txt"));
      System.Text.Encoding.GetEncoding("shift_jis");
      int num = 0;
      Boolean add = false;
      Facility f = null;
      while(sr.Peek() != -1) {
        String line = sr.ReadLine();
        add = false;
        if(line.StartsWith("facilityid")){
          num = int.Parse(line.Substring(11));
          f = new Facility();
          f.facilityid = num;
        }else if(line.StartsWith("facilityname")){
          f.facilityname=line.Substring(13);
        }else if(line.StartsWith("facilityendid")){
          if(int.Parse(line.Substring(14)) == num){
            add = true;
          }
        }else if(line.StartsWith("facilitypos")){
          String pstr = line.Substring(12);
          f.setpos = new Vector2(float.Parse(pstr.Substring(0,pstr.IndexOf(","))),float.Parse(pstr.Substring(pstr.IndexOf(",")+1,pstr.Length - (pstr.IndexOf(",") + 1))));
        }else if(line.StartsWith("facilityatk")){
          String pstr = line.Substring(12);
          float posx = float.Parse(pstr.Substring(0,pstr.IndexOf(",")));
          float posz = float.Parse(pstr.Substring(pstr.IndexOf(",") + 1,pstr.IndexOf(":") - (pstr.IndexOf(",") + 1)));
          String atkstr = pstr.Substring(pstr.IndexOf(":")+1);
          float size = float.Parse(atkstr.Substring(0,atkstr.IndexOf(";")));
        //  float atkz = float.Parse(atkstr.Substring(atkstr.IndexOf(",")+1,atkstr.Length - (atkstr.IndexOf(",")+1)));
        //  Debug.Log("atk : " + atkx + " " + atkz);
          f.attackpos = new Vector4(posx,posz,size);
        }else if(line.StartsWith("facilitytype")){
          f.settype = int.Parse(line.Substring(13));
        }else if(line.StartsWith("facilitycost")){
          f.cost = int.Parse(line.Substring(13));
        }else if(line.StartsWith("facilityhp")){
          f.hp = int.Parse(line.Substring(11));
          f.maxhp = f.hp;
        }else if(line.StartsWith("facilitytime")){
          f.time = int.Parse(line.Substring(13));
        }else if(line.StartsWith("facilityinterval")){
          f.atkInterval = int.Parse(line.Substring(17));
        }
        if(add){
          facilitylist.Add(f);
        }
      }
    }
}

public class Facility{
    public int facilityid; //ユニークid
    public String facilityname=""; //名前
    public Vector2 setpos; //召喚可能範囲
    public Vector3 attackpos; //攻撃範囲
    public int settype; //設置可能タイプ
    public int maxhp; //体力(変更不可能)
    public int cost; //召喚コスト
    public int hp; //体力
    public int time; //消滅までの時間
    public int atkInterval; //攻撃間隔
}
