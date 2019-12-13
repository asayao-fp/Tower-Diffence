using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;


public class GameProgress : MonoBehaviour
{
    protected Dictionary<int,GameObject> sg_objs;

    protected int count;
    protected FacilitySetting fs;
    protected GameSettings gs;

    public int BEFORE_GAME = 0;
    public int NOW_GAME = 1;
    public int AFTER_GAME = 2;

    protected int game_status;
    protected float game_time;
    protected int limit;
    protected float start;
    protected float skillnum; //スキル用の数
    protected int useskillnum; //使用したスキルの回数
    protected TextMeshProUGUI limittime;
    protected TextMeshProUGUI starttime;
    protected GenerateCostManager gcm;
    public GameObject Debug_obj;//デバッグ用のgobrin

    [SerializeField]
    protected bool isDebug;
    protected GameObject debugObj;
    protected GameObject crystalObj;
    protected bool crystaldead;

    /*** 攻撃の種類  ***/
    public const int ATK_MAGICBALL = 1;
    public const int ATK_LASER = 2;
    public const int ATK_THUNDER = 3;
    public const int ATK_POISON = 4;
    public const int ATK_EXPLODE = 5;
    /*** スキルの種類  ***/
    public const int SKILL_RECOVERY = 0; 
    public const int SKILL_ENEMY_DEAD = 1;
    public const int SKILL_CYCROPS_GENERATE = 2;

    [SerializeField]
    protected int MAX_SKILL_NUM = 50;

    [SerializeField]
    protected int MAX_SET_OBJ;
    protected int myobj_num;
    protected bool isStatue;
    protected bool isStart;
    protected bool gameset; 

    [SerializeField]
    protected int roottype4cyc; //サイクロプス用ルートタイプ
    [SerializeField]
    protected Vector3 genepos4cyc; //サイクロプス用召喚ポジション

    void Awake(){
      game_status = 0;
      start = 5;
      gameset = false;

      GameObject stobj = GameObject.FindWithTag("StaticObjects");

      if(stobj.GetComponent<ResultData>() != null){
        Destroy(stobj.GetComponent<ResultData>());
      }

      fs = stobj.GetComponent<FacilitySetting>();

      gs = stobj.GetComponent<GameSettings>();

      GameObject gameui = null;
      if(gs.isStatue()){
        gameui = (GameObject)ResourceManager.getObject("UI/GameUI4statue");
      }else{
        gameui = (GameObject)ResourceManager.getObject("UI/GameUI4gobrin");
      }
      GameObject ui = Instantiate(gameui) as GameObject;
      ui.name = "GameUI";


      if(sg_objs == null){
        sg_objs = new Dictionary<int,GameObject>();
      }
      count = 0;
      limittime = GameObject.FindWithTag("LimitTime").GetComponent<TextMeshProUGUI>();
      limit = gs.getLimitTime();
      game_time = limit;
      limittime.text = "" + (int)game_time;
      starttime = GameObject.FindWithTag("StartTime").GetComponent<TextMeshProUGUI>();
      starttime.text = "" + (int)start;
      gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
      crystaldead = false;
      myobj_num = 0;
      isStatue = gs.isStatue();
      isStart = false;
      skillnum = 0;
      useskillnum = 0;

      GameObject[] icons = ui.GetComponent<ObjectReference>().objects;
      for(int i=0;i<icons.Length;i++){
        icons[i].name = gs.getStatus(i).name;
      }
      GetComponent<InputManager>().init(); //ビルドしたやつでやるとエラーが起きる
    }


    void Update()
    {

      if(gameset){
        return;
      }
      if(!isStart){
        String name = PlayerPrefs.GetString(UserData.USERDATA_NAME,"");
        String id = PlayerPrefs.GetString(UserData.USERDATA_ID,"");
        int level = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);
        int exp = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);

        //GameSettings.printLog("gameprogress : " + name + " " + id + " " + level + " " + exp);

        SoundManager.SoundPlay("bgm1",this.gameObject.name);

        isStart = true;
        return;
      }


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
        StartCoroutine("GameSet",1);

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
      }
        if(!crystaldead){
          if(crystalObj == null){
            crystalObj = GameObject.Find("crystal");
          }
          if(crystalObj.GetComponent<CrystalManager>().getHP() <= 0){
            crystalObj.GetComponent<CrystalManager>().Dead();
            crystaldead = true;
            StartCoroutine("GameSet",2);
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
    }

/*試合終了 1 -> 時間切れ　2 -> クリスタル破壊 */
    public IEnumerator GameSet(int type)
    {
        gameset = true;
        limittime.text = "GAME FINISH";
        ResultData rd = GameObject.FindWithTag("StaticObjects").AddComponent<ResultData>();

        bool result = false;
        if ((gs.isStatue() && (type == 1)) || (!gs.isStatue() && (type == 2)))
        {
            result = true;
        }
        rd.SetResult(result, 10);


        yield return new WaitForSeconds(1.0f);

        int ac = gs.getAdvertiseCount();
        ac++;
        SceneManager.LoadScene("GameSetScene");

        //nullぽになった
/*
        if (ac == Constants.ADVERTISE_LIMIT)
        {
            /*
            video:デフォルト。5秒後にスキップ可
            rewardedVideo:スキップ不可。
            
            if (Advertisement.IsReady("video"))
            {
                ShowOptions options = new ShowOptions
                {
                    resultCallback = advertisementResult
                };

                Advertisement.Show("video", options);
            }
        }
        else
        {
            gs.setAdvertiseCount(ac);
            SceneManager.LoadScene("GameSetScene");
        }
        */
    }

    //全てのFacilityを取得
    public GameObject[] getObjs(){
      GameObject[] vals = new GameObject[sg_objs.Values.Count];
      sg_objs.Values.CopyTo(vals,0);
      return vals;
    }

    //自分、または相手のFacilityを取得
    public GameObject[] getObjs(bool isown){
      List<GameObject> vlist = new List<GameObject>();

      GameObject[] objs = getObjs();
      bool isStatue = gs.isStatue();
      for(int i=0;i<objs.Length;i++){
        if(objs[i] == null)continue;
        if(objs[i].gameObject.tag.Equals("Statue")){
          if((isown && isStatue) || (!isown && !isStatue)){
            vlist.Add(objs[i]);
          }
        }else if(objs[i].gameObject.tag.Equals("Goblin")){
          if((isown && !isStatue) || (!isown && isStatue)){
            vlist.Add(objs[i]);
          }
        }
      }

      return vlist.ToArray();
    }

    //現在のステータスを取得
    public int getStatus(){
      return game_status;
    }

    public void Generate(String name,Vector3 pos,bool isai,bool isstatue){
      Generate(name,pos,isai,isstatue,-1);
    }

    //召喚
    public void Generate(String name,Vector3 pos,bool isai,bool isstatue,int roottype){
        GameObject obj = Instantiate (ResourceManager.getObject("Statue/" + name), pos, Quaternion.identity) as GameObject;
        obj.name = name;

        FacilityManager fm = obj.GetComponent<FacilityManager>();

        bool iscyc = name.Equals("Cyc");
        if(iscyc){
          ((GobrinManager)fm).setRoot(roottype4cyc);
        }else if(!isstatue && !isai){
          ((GobrinManager)fm).setRoot(GetComponent<InputManager>().roottype);
        }else if(!isstatue && isai){
          ((GobrinManager)fm).setRoot(roottype);
        }

        fm.setAddStatus(gs.getStatus(name));        
        fm.setId(count);
        fm.Generate(pos,fm.getSData(),isai);
        fm.setNum(true);

        sg_objs.Add(count++,obj);

        if(!iscyc){
          gcm.generateCost(isai ? 0 : fm.getSData().cost);      
        }

        GameSettings.printLog("[GameProgress] Generate obj : " + obj.name + " id : " + (count - 1));
    }

    //ダメージ計算
    public void calcDamage(int obj_id,int type,int attack){
       int hp = -attack;
       String debugstr = "";
       //攻撃の種類
       //攻撃を受けたキャラクターの状態
       //パラメータとかで計算？（これはいずれ）
       switch(type){
         case ATK_MAGICBALL:
           debugstr = "ATK_MAGICBALL";
           break;
         case ATK_LASER:
           debugstr = "ATK_LASER";           
           break;
         case ATK_THUNDER:
           debugstr = "ATK_THUNDER";
           break;
         case ATK_POISON:
           debugstr = "ATK_POISON";
           break;
         case ATK_EXPLODE:
           debugstr = "ATK_EXPLODE";
           break;
       }
       GameSettings.printLog("[CALC DAMAGE] TYPE : " + debugstr);
       
       AddHP(obj_id,hp,false);
    }
    //攻撃受けた
    public void AddHP(int obj_id,int hp,Boolean isDebug){
      if(obj_id == 1000000){
        crystalObj.GetComponent<CrystalManager>().AddHP(hp);
      }else{
        FacilityManager fm = null;

        fm = sg_objs[obj_id].GetComponent<FacilityManager>();
      
        GameSettings.printLog("add hp " + obj_id + " " + hp);
        fm.addHP(hp);
      }
    }

    public Stage setNowStage(Stage s){
      s = new Stage();
      s.enablelist = new List<float[]>();
      s.enablelistv = new List<Vector2[]>();
      GameObject stage = GameObject.FindWithTag("Stage");

      string stagetype = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>().isStatue() ? "statue" : "gobrin";

      foreach (Transform child in stage.transform)
      {

        if(child.gameObject.tag.StartsWith("Type_")){
          
          if(!child.gameObject.name.StartsWith(stagetype)){
            Destroy(child.gameObject);
            continue;
          }
          int value = int.Parse(child.gameObject.tag.Substring(5));
          int gv = int.Parse(child.gameObject.name.Substring(6)); //ゴブリンのルート結びつけるよう
          

          Vector3 pos = child.position;
          Vector3 scale = child.localScale;
          float [] sinfo = new float[6];
          sinfo[0] = pos.x - scale.x/2.0f;//xの開始座標
          sinfo[1] = pos.x + scale.x/2.0f;//xの終了座標
          sinfo[2] = pos.z - scale.z/2.0f;//zの開始座標
          sinfo[3] = pos.z + scale.z/2.0f;//zの終了座標
          sinfo[4] = value;               //座標の値
          sinfo[0] = sinfo[0] < 0 ? 0 : sinfo[0];
          sinfo[2] = sinfo[2] < 0 ? 0 : sinfo[2];
          sinfo[5] = gv;
          
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
      //GameSettings.printLog("[GameProgress] MyObjNum :  " + myobj_num + "  MaxSetObj : " + MAX_SET_OBJ);
      return myobj_num < MAX_SET_OBJ;
    }

    public bool doSkill(){
      if(getStatus() != NOW_GAME)return false;

      int skilltype = gs.getSkillType();

      //スキル使用回数が最大を超えてたらできない
      if(gs.isUseSkill(useskillnum)){
        GameSettings.printLog("[GameProgress] doSkill UseSkill over!");
        return false;
      }

      //スキル用ゲージが溜まってなかったらできない
      if(skillnum < MAX_SKILL_NUM){
        GameSettings.printLog("[GameProgress] doSkill Skillgage not!");
        return false;
      }


      switch(skilltype){
        case SKILL_RECOVERY:
          skillRecover();
          break;
        case SKILL_ENEMY_DEAD:
          skillEnemyDead();
          break;
        case SKILL_CYCROPS_GENERATE:
          skillCycropsGenerate();
          break;
      }
      skillnum = 0;
      //使用回数追加
      useskillnum++;

      return true;
    }

    //自分のFacility全回復
    public void skillRecover(){
      GameSettings.printLog("[GameProgress] SkillRecovery");
      GameObject[] objs = getObjs(true);

      for(int i=0;i<objs.Length;i++){
        FacilityManager fm = objs[i].GetComponent<FacilityManager>();
        fm.addHP(100000);
      }
    }

    //敵全滅
    public void skillEnemyDead(){
      GameSettings.printLog("[GameProgress] SkillEnemyDead");
      GameObject[] objs = getObjs(false);

      for(int i=0;i<objs.Length;i++){
        FacilityManager fm = objs[i].GetComponent<FacilityManager>();
        fm.addHP(-100000);

      }

    }

    //サイクロプス召喚ゴブリン専用)
    public void skillCycropsGenerate(){
      GameSettings.printLog("[GameProgress] SkillCycropsGenerate");
      Generate("Cyc",genepos4cyc,false,false);
    }

    public bool addSkillCost(int num){

      if(getStatus() != NOW_GAME)return false;

      skillnum += num;

      if(skillnum >= MAX_SKILL_NUM){
        skillnum = MAX_SKILL_NUM;
      }
      GameSettings.printLog("[GameProgress] addSkillCost num : " + num + " now : " + skillnum);

      return skillnum >= MAX_SKILL_NUM;
    }

/*
    private void advertisementResult(ShowResult result)
    {
        gs.setAdvertiseCount(0);
        SceneManager.LoadScene("GameSetScene");
    }
    */

}
