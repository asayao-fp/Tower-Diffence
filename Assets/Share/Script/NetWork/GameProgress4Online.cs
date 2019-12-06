using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;


public class GameProgress4Online : GameProgress
{ 
    


    public GameObject photonView;

    public GameObject synObj;

    //親かどうか
    public bool isParent;
    void Awake(){

      game_status = 0;
      start = 5;
      gameset = false;
      photonView = GameObject.FindWithTag("NetObj");
      isParent = photonView.GetComponent<ConnectPlayer>().getParent();
      
      synObj = PhotonNetwork.Instantiate("NetWork/synchronizeObj", Vector3.zero, Quaternion.identity, 0) ;

      GameObject stobj = GameObject.FindWithTag("StaticObjects");
      gs = stobj.GetComponent<GameSettings>();

      //とりあえず親がスタチューで
      if(isParent){
        gs.setStatue(true);
      }else{
        gs.setStatue(false);
      }

      GameObject gameui = null;
      if(gs.isStatue()){
        gameui = (GameObject)ResourceManager.getObject("UI/GameUI4statue");
      }else{
        gameui = (GameObject)ResourceManager.getObject("UI/GameUI4gobrin");
      }
      GameObject ui = Instantiate(gameui) as GameObject;
      ui.name = "GameUI";

    }

    void Start()
    {

      GameObject stobj = GameObject.FindWithTag("StaticObjects");

      if(stobj.GetComponent<ResultData>() != null){
        Destroy(stobj.GetComponent<ResultData>());
      }

      fs = stobj.GetComponent<FacilitySetting>();

      if(sg_objs == null){
        sg_objs = new Dictionary<int,GameObject>();
      }
      count = 0;
      limittime = GameObject.FindWithTag("LimitTime").GetComponent<TextMeshProUGUI>();
      gs = stobj.GetComponent<GameSettings>();
      limit = gs.getLimitTime();
      if(limit < 10){
        limit = 50;
      }
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

      GameObject[] icons = GameObject.FindGameObjectsWithTag("GenerateIcon");
      for(int i=0;i<icons.Length;i++){
     //   icons[i].name = gs.getStatus(i).name;
      }

      GetComponent<InputManager4Online>().init(); //ビルドしたやつでやるとエラーが起きる
      
      Debug.Log("limit : " + limit + " : " + game_time);
    }

    public void setCDown(string text){
      if(isParent)return;
      starttime.text = text;
    }
    public void setTime(float time){
      if(isParent)return;
      game_time = (int)time;
      limittime.text = "" + (int)time;

    }

    public void setStatus(int type){
      if(isParent)return;
      game_status = type;
     // synObj.GetComponent<SynchronizeManager>().SetGameStatus(game_status);
    }
    void Update()
    {      


      if(game_status == AFTER_GAME && !gameset){
          StartCoroutine("GameSet",1);
          return;
      }else if(crystaldead){
          StartCoroutine("GameSet",2);
      }

      //子は無視
      if(!isParent){

        return;
      }

      if(gameset){
        return;
      }


      if(!isStart){
        String name = PlayerPrefs.GetString(UserData.USERDATA_NAME,"");
        String id = PlayerPrefs.GetString(UserData.USERDATA_ID,"");
        int level = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);
        int exp = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);

        GameSettings.printLog("gameprogress : " + name + " " + id + " " + level + " " + exp);

       // SoundManager.SoundPlay("bgm1",this.gameObject.name);

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

      }

      //親側でチェック
      checkObjs();

      synObj.GetComponent<SynchronizeManager>().SetGameStatus(game_status);
      synObj.GetComponent<SynchronizeManager>().SetGameTime(game_time);
      synObj.GetComponent<SynchronizeManager>().SetCountDown(starttime.text);
      synObj.GetComponent<SynchronizeManager>().setCrystalDead(crystaldead);

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
    
      synObj.GetComponent<SynchronizeManager>().Set4Manager(sg_objs);

        if(!crystaldead){
          if(crystalObj == null){
            crystalObj = GameObject.Find("crystal");
          }
          synObj.GetComponent<SynchronizeManager>().Set4CrystalHP(crystalObj.GetComponent<CrystalManager>().getHP());
          if(crystalObj.GetComponent<CrystalManager>().getHP() <= 0){
            crystalObj.GetComponent<CrystalManager>().Dead();
            crystaldead = true;
          }
          

        }

      //削除フラグが立ってるオブジェクトをtableから削除
      if(delete){
        synObj.GetComponent<SynchronizeManager>().Set4DeadObj(isdelete);
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
    public IEnumerator GameSet(int type){
        gameset = true;
        limittime.text = "GAME FINISH";


        ResultData rd = GameObject.FindWithTag("StaticObjects").AddComponent<ResultData>();

        bool result = false;
        
        if((isParent && (type == 1)) || (!isParent && (type == 2))){
          result = true;
        }

        rd.SetResult(result,10);        
        yield return new WaitForSeconds (5.0f); 


        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("GameSetScene");

    }

    public void setCrystalHP(float hp){
      if(crystalObj != null){
        crystalObj.GetComponent<CrystalManager>().setHP(hp);
      }
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

    public void deadManager(int unique_id){
      if(sg_objs.ContainsKey(unique_id)){
        FacilityManager fm = sg_objs[unique_id].GetComponent<FacilityManager>();
        fm.isEnd = true;
        fm.setNum(false);
        fm.Dead();
        sg_objs.Remove(unique_id);
      }
    }

    public void setCDead(bool iscdead){
      if(isParent)return;
      crystaldead = iscdead;
    }
    public void synchronizePosHP(int unique_id,float hp,float x,float y,float z){
      FacilityManager fm = sg_objs[unique_id].GetComponent<FacilityManager>();
      fm.setHP(hp);
      sg_objs[unique_id].gameObject.transform.position.Set(x,y,z);

      GameSettings.printLog("[GameProgress4Online] SETHP id : " + unique_id + " hp : " + fm.getHP() + " x : " + x + " y : " + y + " z : " + z);
    }

    public void TempGenerate(String name,Vector3 pos,bool isstatue,bool isparent){

      //親じゃなければsynObjに追加するだけ
      synObj.GetComponent<SynchronizeManager>().Set(name,pos);
      if(isparent){
         Generate(name,pos,isstatue,isparent,true);
      }
    }
    //召喚
    public void Generate(String name,Vector3 pos,bool isstatue,bool isparent,bool ismaster){

        GameObject obj = Instantiate (ResourceManager.getObject("Statue/" + name), pos, Quaternion.identity) as GameObject;
        obj.name = name;
        FacilityManager fm = obj.GetComponent<FacilityManager>();

        fm.setAddStatus(gs.getStatus(name));
        fm.setNum(true);
        fm.setId(count);        
        fm.Generate(pos,fm.getSData(),false);


        sg_objs.Add(count++,obj);

        
        if(isparent && ismaster){
          gcm.generateCost(fm.getSData().cost);
        }else if(!isparent && !ismaster){
          gcm.generateCost(fm.getSData().cost);
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

      FacilityManager fm = null;

      fm = sg_objs[obj_id].GetComponent<FacilityManager>();
      
      GameSettings.printLog("add hp " + obj_id + " " + hp);
      fm.addHP(hp);
    }

    public Stage setNowStage(Stage s){
      s = new Stage();
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
      //GameSettings.printLog("[GameProgress] MyObjNum :  " + myobj_num + "  MaxSetObj : " + MAX_SET_OBJ);
      return myobj_num < MAX_SET_OBJ;
    }

    //子が送ってきたスキル
    public void doSkill4Net(int skilltype){

      Skill(skilltype,false);
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


      //ローカルで使用回数は管理(怖い、、）
      skillnum = 0;
      //使用回数追加
      useskillnum++;

      if(!isParent){
        synObj.GetComponent<SynchronizeManager>().setSkillType(skilltype);
        return true;
      }

      //子は無視
      Skill(skilltype,true);

      return true;
    }

    public void Skill(int skilltype,bool isParent){
      switch(skilltype){
        case SKILL_RECOVERY:
          skillRecover(isParent);
          break;
        case SKILL_ENEMY_DEAD:
          skillEnemyDead(isParent);
          break;
      }

    }

    //自分のFacility全回復
    public void skillRecover(bool isParent){
      GameSettings.printLog("[GameProgress] SkillRecovery");
      GameObject[] objs = getObjs(isParent);

      for(int i=0;i<objs.Length;i++){
        FacilityManager fm = objs[i].GetComponent<FacilityManager>();
        fm.addHP(100000);
      }
    }

    //敵全滅
    public void skillEnemyDead(bool isParent){
      GameSettings.printLog("[GameProgress] SkillEnemyDead");
      GameObject[] objs = getObjs(isParent);

      for(int i=0;i<objs.Length;i++){
        FacilityManager fm = objs[i].GetComponent<FacilityManager>();
        fm.addHP(-100000);

      }

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
}
