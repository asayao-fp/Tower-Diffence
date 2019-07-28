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
        }

        if(add){
          facilitylist.Add(f);
        }

      }

/*      for(int i=0;i<facilitylist.Count;i++){
        Debug.Log("facilitylist :"
           + facilitylist[i].facilityid + "¥n"
          + facilitylist[i].facilityname + "¥n"+
           facilitylist[i].setpos + "¥n"
           + facilitylist[i].attackpos + "¥n"
           + facilitylist[i].settype);
      }*/
    }


}

public class Facility{
    public int facilityid;
    public String facilityname="";
    public Vector2 setpos;
    public Vector3 attackpos;
    public int settype;
}
