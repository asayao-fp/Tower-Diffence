using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{

    private DrawPosition dp;
    private Vector3 setpos;
    private GameObject generatePrefab;
    GameObject prefab;
    public Boolean isset;
    private GameObject stage;

    private FacilityPosition fp;
    public Toggle[] tgls = new Toggle[4];
    public Vector2[] spos = new Vector2[4];
    void Start()
    {
      setpos = new Vector3();
    }

    void Update()
    {
       if(dp == null){
         dp = GameObject.Find("StageSelect").GetComponent<DrawPosition>();
         stage = GameObject.Find("Stage");
       }
       if(fp == null){
         fp = GameObject.Find("StageSelect").GetComponent<FacilityPosition>();
       }

       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       RaycastHit hit;
       if (Physics.Raycast(ray,out hit,10.0f))
       {
         setpos = hit.point;
         String name = "";
         for(int i=0;i<tgls.Length;i++){
           if(tgls[i].isOn){
             name = tgls[i].name;
           }
         }
         if (Input.GetMouseButtonDown(0)) {
           //設置可能範囲内なら置ける
           spos = getSetPosition(fp.getFacility(name),setpos);
           if(checkPosition()){
              // プレハブを取得
              prefab = (GameObject)Resources.Load ("takuma/Prefabs/" + name);
              // プレハブからインスタンスを生成
              generatePrefab = Instantiate (prefab, setpos, Quaternion.identity) as GameObject;
            }
          }

        if (Input.GetMouseButtonUp(0)) {
           if(generatePrefab != null){
             Instantiate (prefab, generatePrefab.transform.position, Quaternion.identity);
             Destroy(generatePrefab);
           }
        }

        if (Input.GetMouseButton(0)) {
            spos = getSetPosition(fp.getFacility(name),setpos);
        }
        if(generatePrefab != null){
          if(checkPosition()){
            generatePrefab.transform.position = setPosition(generatePrefab.transform.position,setpos);
          }
          //  dp.isShow = true;
        }
       }else{
        // dp.isShow = false;
       }
    }

    //マウス座標から、設置物の置ける範囲を取得
    public Vector2[] getSetPosition(Facility f,Vector3 pos){
      return new Vector2[]{
        new Vector2(pos.x-f.setpos.x/2,pos.z-f.setpos.y/2), //左下
        new Vector2(pos.x+f.setpos.x/2,pos.z-f.setpos.y/2), //右下
        new Vector2(pos.x-f.setpos.x/2,pos.z+f.setpos.y/2), //左上
        new Vector2(pos.x+f.setpos.x/2,pos.z+f.setpos.y/2) //右上
      };
    }

    //設置物を可能範囲内に置けるか
    public Boolean checkPosition(){

      int incount = 0;//設置可能域にいるか
      for(int i=0;i<spos.Length;i++){
        Boolean isin = false;
        for(int j=0;j<dp.stage.enablelist.Count;j++){

          if(dp.stage.enablelist[j][4] > dp.setType){
            continue;
          }

          float[] area = dp.stage.enablelist[j];
          //Debug.Log(i + " area : " + area[0] + " " + area[1] + " " + area[2] + " " + area[3]
          //+ " " + spos[i].x + " " + spos[i].y
          // + (area[0] <= spos[i].x) + " " + (spos[i].x <= area[1]) + " " + (area[2] <= spos[i].y) + " " + (spos[i].y <= area[3]));
          if((area[0] <= spos[i].x) && (spos[i].x <= area[1]) && (area[2] <= spos[i].y) && (spos[i].y <= area[3])){
            isin = true;
            break;
          }
        }
        if(isin)incount++;
      }

      return incount == spos.Length;
    }
    public Vector3 setPosition(Vector3 nowpos,Vector3 setpos){

      //setposがステージ外だったらダメだよ
      Vector3 sp = new Vector3(0,0.01f,0);
      float ssizex = stage.GetComponent<Renderer>().bounds.size.x;
      float ssizez = stage.GetComponent<Renderer>().bounds.size.z;
      Boolean ischeck = checkPosition();

      //Stageの範囲内にいるか
      if((setpos.x < 0) || (setpos.x > ssizex)){
        sp.x = nowpos.x;
      }else{
        sp.x = setpos.x;
      }
      if((setpos.z < 0) || (setpos.z > ssizez)){
        sp.z = nowpos.z;
      }else{
        sp.z = setpos.z;
      }

      if(ischeck){
        sp.x = setpos.x;
        sp.z = setpos.z;
      }else{
        sp.x = nowpos.x;
        sp.z = nowpos.z;
      }

      return sp;
    }
}
