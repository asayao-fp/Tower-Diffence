using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class GameProgress : MonoBehaviour
{
    private Dictionary<int,GameObject> sg_objs;
    private int count;
    FacilitySetting fs;
    GameSettings gs;

    public int BEFORE_GAME = 0;
    public int NOW_GAME = 1;
    public int AFTER_GAME = 2;

    private int game_status;
    private float game_time;
    private int limit;
    private float start;

    private Text limittime;
    private Text starttime;
    private GenerateCostManager gcm;
    public GameObject Debug_obj;//デバッグ用のgobrin

    void Awake(){
      game_status = 0;
      start = 5;
    }

    void Start()
    {
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      sg_objs = new Dictionary<int,GameObject>();
      count = 0;
      limittime = GameObject.FindWithTag("LimitTime").GetComponent<Text>();
      gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();
      limit = gs.getLimitTime();
      game_time = limit;
      limittime.text = "" + (int)game_time;
      starttime = GameObject.FindWithTag("StartTime").GetComponent<Text>();
      starttime.text = "" + (int)start;
      gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
    }

    void Update()
    {

      if(start > -3){
        start -= Time.deltaTime;
        if(start > 1 ){
          starttime.text = "" + (int)start;
        }else if(start > -2){
          starttime.text = "START !!!!!!";
        }else{
          starttime.text = "";
        }
        return;
      }

      if(game_time > 0){
        game_time -= Time.deltaTime;
        game_status = NOW_GAME;
        limittime.text = "" + (int)game_time;
      }else{
        game_status = AFTER_GAME;
        limittime.text = "GAME FINISH";
      }

      checkObjs();

    }

    public void checkObjs(){
      if(Debug_obj != null){
        if(Debug_obj.GetComponent<FacilityManager>().fInfo.hp <= 0){
          Debug.Log("dead debug_obj");
          Destroy(Debug_obj);
        }
      }
      foreach(KeyValuePair<int,GameObject> pair in sg_objs){
        if(pair.Value == null){
          //Debug.Log("null : " + pair.Key);
        }else{
          //Debug.Log (pair.Key + " " + pair.Value.name);
          if(pair.Value.GetComponent<FacilityManager>().fInfo.hp <= 0){
            Debug.Log("dead");
            Dead(pair.Key);
          }
        }        
      }
    }

    public Facility getFM(int obj_id,Boolean isDebug){
      if(isDebug){
        return Debug_obj.GetComponent<FacilityManager>().fInfo;
      }else{
        return sg_objs[obj_id].GetComponent<FacilityManager>().fInfo;
      }
    }

    //現在のステータスを取得
    public int getStatus(){
      return game_status;
    }

    //死んだ
    public void Dead(int obj_id){
      sg_objs.Remove(obj_id);
      
    }

    //召喚された
    public void Generate(GameObject obj){
      if(gcm == null){
        gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
      }

      gcm.generateCost(obj.GetComponent<FacilityManager>().fInfo.cost);

      {
        obj.GetComponent<FacilityManager>().setId(count);
        sg_objs.Add(count++,obj);

      }

    }

    //攻撃受けた
    public void AddHP(int obj_id,int hp,Boolean isDebug){
      FacilityManager fm = null;

      if(isDebug){
        Debug_obj.GetComponent<FacilityManager>().fInfo.hp += hp;
        fm = Debug_obj.GetComponent<FacilityManager>();
      }else{
       fm = sg_objs[obj_id].GetComponent<FacilityManager>();
       fm.fInfo.hp += hp;
      }
    //  Debug.Log("fm attack : " + fm.fInfo.hp + " " + fm.name);
    }


    public Stage setNowStage(Stage s){
      s.enablelist = new List<float[]>();
      GameObject stage = GameObject.FindWithTag("Stage");
      foreach (Transform child in stage.transform)
      {
          if(child.gameObject.tag.StartsWith("Type_")){
            int value = int.Parse(child.gameObject.tag.Substring(5));
            Vector3 pos = child.position;
            Vector3 scale = child.localScale;
            float [] sinfo = new float[5];
            sinfo[0] = pos.x - scale.x/2.0f;//xの開始座標
            sinfo[1] = pos.x + scale.x/2.0f;//xの終了座標
            sinfo[2] = pos.z - scale.z/2.0f;//zの開始座標
            sinfo[3] = pos.z + scale.z/2.0f;//zの終了座標
            sinfo[4] = value;               //座標の値
            sinfo[0] = sinfo[0] < 0 ? 0 : sinfo[0];
            sinfo[2] = sinfo[2] < 0 ? 0 : sinfo[2];
            
            Debug.Log("info : " + child.gameObject.name + " " + sinfo[0] + " " + sinfo[1] + " " + sinfo[2] + " " + sinfo[3] + " " + sinfo[4]);
            s.enablelist.Add(sinfo);
          }
      }
      return s;
    }

}
