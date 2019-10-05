using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using TMPro;


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

    private TextMeshProUGUI limittime;
    private TextMeshProUGUI starttime;
    private GenerateCostManager gcm;
    public GameObject Debug_obj;//デバッグ用のgobrin

    [SerializeField]
    private bool isDebug;
    private GameObject debugObj;
    private GameObject crystalObj;
    private bool crystaldead;

    /*** 攻撃の種類  ***/
    public const int ATK_MAGICBALL = 1;
    public const int ATK_LASER = 2;
    public const int ATK_THUNDER = 3;
    public const int ATK_POISON = 4;
    public const int ATK_EXPLODE = 5;

    [SerializeField]
    private int MAX_SET_OBJ;
    private int myobj_num;
    private bool isStatue;

    void Awake(){
      game_status = 0;
      start = 5;
    }

    void Start()
    {
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();

      if(sg_objs == null){
        sg_objs = new Dictionary<int,GameObject>();
      }
      count = 0;
      limittime = GameObject.FindWithTag("LimitTime").GetComponent<TextMeshProUGUI>();
      gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();
      limit = gs.getLimitTime();
      game_time = limit;
      limittime.text = "" + (int)game_time;
      starttime = GameObject.FindWithTag("StartTime").GetComponent<TextMeshProUGUI>();
      starttime.text = "" + (int)start;
      gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
      crystaldead = false;
      myobj_num = 0;
      isStatue = gs.isStatue();
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
      int[] isdelete = new int[sg_objs.Count()];
      int count = 0;
      bool delete = false;
      myobj_num = 0;
      foreach(KeyValuePair<int,GameObject> pair in sg_objs){
        isdelete[count] = -1;
        if(pair.Value == null){
        }else{    
          FacilityManager fm = pair.Value.GetComponent<FacilityManager>();
          if((isStatue && fm.isStatue) || (!isStatue && !fm.isStatue)){
              myobj_num++;
          }
          if(fm.getHP() <= 0){
            fm.isEnd = true;
            fm.setNum(false);
            fm.Dead();
            isdelete[count] = pair.Key;
            delete = true;
          }
        }
        count++;
        if(!crystaldead){
          if(crystalObj == null){
            crystalObj = GameObject.Find("crystal");
          }
          if(crystalObj.GetComponent<CrystalManager>().getHP() <= 0){
            crystalObj.GetComponent<CrystalManager>().Dead();
            crystaldead = true;
          }
        }
      }
      //削除フラグが立ってるオブジェクトをtableから削除
      if(delete){
        for(int i=0;i<isdelete.Length;i++){
          if(isdelete[i] != -1){
            sg_objs.Remove(isdelete[i]);
            if(isdelete[i] == 1000000){
              debugObj = null;
            }
          }
        }
      }
      
      if(isDebug){
        if(debugObj == null){
          debugObj = ResourceManager.getObject("Statue/debugGobrin");
          Generate("debugGobrin",debugObj.transform.position);
        }
      }

      
    }

    public GameObject[] getObjs(){
      GameObject[] vals = new GameObject[sg_objs.Values.Count];
      sg_objs.Values.CopyTo(vals,0);
      return vals;
    }
    //現在のステータスを取得
    public int getStatus(){
      return game_status;
    }

    //召喚 (デバッグ用)
    public void Generate(GameObject obj){
        obj.GetComponent<FacilityManager>().setId(1000000);
        if(sg_objs == null){
            sg_objs = new Dictionary<int,GameObject>();

        }
        sg_objs.Add(1000000,obj);
    }
    //召喚
    public void Generate(String name,Vector3 pos){
//        GameObject obj = Instantiate (ResourceManager.getObject("Statue/" + name,getStatueType()), pos, Quaternion.identity) as GameObject;
        GameObject obj = Instantiate (ResourceManager.getObject("Statue/" + name), pos, Quaternion.identity) as GameObject;
        obj.name = name;
        FacilityManager fm = obj.GetComponent<FacilityManager>();
        fm.init();
        
        if(!name.Equals("debugGobrin")){
          gcm.generateCost(fm.getSData().cost);
          fm.Generate(pos,obj.transform.localScale,fm.getSData());

          fm.setId(count);
          sg_objs.Add(count++,obj);

        }else{
          fm.setId(1000000);
          sg_objs.Add(1000000,obj); 
        }
        fm.setNum(true);
    }

    //ダメージ計算
    public void calcDamage(int obj_id,int type){
       int hp = 0;
       String debugstr = "";
       //攻撃の種類
       //攻撃を受けたキャラクターの状態
       //パラメータとかで計算？（これはいずれ）
       switch(type){
         case ATK_MAGICBALL:
           debugstr = "ATK_MAGICBALL";
           hp = -1;
           break;
         case ATK_LASER:
           debugstr = "ATK_LASER";           
           hp = -2;
           break;
         case ATK_THUNDER:
           debugstr = "ATK_THUNDER";
           hp = -3;
           break;
         case ATK_POISON:
           debugstr = "ATK_POISON";
           hp = -4;
           break;
         case ATK_EXPLODE:
           debugstr = "ATK_EXPLODE";
           hp = -5;
           break;
       }
       GameSettings.printLog("[CALC DAMAGE] TYPE : " + debugstr);
       
       AddHP(obj_id,hp,false);
    }
    //攻撃受けた
    public void AddHP(int obj_id,int hp,Boolean isDebug){
      FacilityManager fm = null;

      fm = sg_objs[obj_id].GetComponent<FacilityManager>();
      GameSettings.printLog("add hp " + obj_id + " " + hp);
      fm.addHP(hp);
    }

    public Stage setNowStage(Stage s){
      s.enablelist = new List<float[]>();
      s.enablelistv = new List<Vector2[]>();
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
          
          s.enablelist.Add(sinfo);
          
          Vector2[] posv = new Vector2[4];
          posv[0] = new Vector2(sinfo[0],sinfo[3]); //左上
          posv[1] = new Vector2(sinfo[1],sinfo[3]); //右上
          posv[2] = new Vector2(sinfo[1],sinfo[2]); //右下
          posv[3] = new Vector2(sinfo[0],sinfo[2]); //左下

          s.enablelistv.Add(posv);
        }
      }
      return s;
    }

    //テーマを取得
    public bool getStatueType(){
      return gs.getStatueType();
    }

    //デバックモードか
    public bool getDebug(){
      return isDebug;
    }

    //召喚コストが足りてるか
    public bool hasCost(int cost){
      return gcm.getCost() >= cost;
    }

    //オブジェクトを設置可能か
    public bool canObjSet(){
      GameSettings.printLog("[GameProgress] MyObjNum :  " + myobj_num + "  MaxSetObj : " + MAX_SET_OBJ);
      return myobj_num < MAX_SET_OBJ;
    }
}
