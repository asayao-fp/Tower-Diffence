using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InputManager : MonoBehaviour
{
    protected GameObject stage;　//ステージ
    protected FacilitySetting fs; //施設セッティング
    protected StageSetting ss; //ステージセッティング
    protected Stage nowStage; //現在のステージ情報
    protected StatueData nowStatue; //現在の施設情報

    protected GameSettings gs;

    protected Vector3 setpos; //マウス座標
    protected GameObject generatePrefab; //ドラッグ中に使用する施設用オブジェクト
    protected GameObject prefab; //生成用のプレハブ
    protected GameObject atkPrefab; //攻撃範囲用のプレハブ
    protected GameObject setPrefab; //設置範囲用のプレハブ
    protected GameObject setObj; //設置用のオブジェクト
    protected GameObject[] setObjs; //設置されてるstatue用オブジェクト

    protected GameObject[] tglobj;
    protected Toggle[] tgls = new Toggle[5]; //施設選択用
    protected Vector2[] spos = new Vector2[4]; //設置可能時の座標
    protected Boolean touchGui; //uGuiを選択しているか
    protected String FacilityName = ""; //現在選択中の施設名
    protected Boolean isSelect; //施設のアイコンを選択しているか
    protected Boolean isMoving; //施設を移動中か

    public Material showSetPositionMat; //設置可能範囲用マテリアル
    public Boolean isShow; //設置可能範囲、攻撃範囲を表示するか
    public int setType = 100; //設置できるタイプ
    protected Boolean canSelect; //施設を選択できるか
    protected Boolean isdrawAttack = false; //攻撃可能範囲を表示するか

    [SerializeField]
    protected TextMeshProUGUI nameText;
    protected Vector3 textpos; //ドラッグ中の名前
    [SerializeField]
    protected TextMeshProUGUI notsetText; //設置できないときに表示するオブジェクト
    protected int num ;
    protected int select_num;

    protected GameProgress gp; 

    public static bool generating = false;

    //タッチ判定
    protected int layerNo;
    protected int layerMask;
    protected Ray ray ;
    protected RaycastHit hit;
    protected　bool isRay = false; //raycast が当たってるか
    public int roottype; //ゴブリンを置いた時のルート位置

    public void init()
    {
      setpos = new Vector3();
      stage = GameObject.FindWithTag("Stage");
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      ss = GameObject.FindWithTag("StaticObjects").GetComponent<StageSetting>();
      gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();

      tgls = new Toggle[gs.isStatue() ? 5 : 3];

      tglobj = GameObject.FindGameObjectsWithTag("GenerateIcon");
      for(int i=0;i<tglobj.Length;i++){
        tgls[i] = tglobj[i].GetComponent<Toggle>();
      }

      for(int i=0;i<tgls.Length;i++){
        tgls[i].isOn = false;
      }

      GameObject ui = GameObject.Find("GameUI");

      foreach(Transform child in ui.transform){
        if(child.gameObject.name.Equals("objname")){
          nameText = child.gameObject.GetComponent<TextMeshProUGUI>();
        }else if(child.gameObject.name.Equals("notset")){
          notsetText = child.gameObject.GetComponent<TextMeshProUGUI>();
        }
      }

      atkPrefab = Instantiate (ResourceManager.getObject("Other/AtkPosSphere"), new Vector3(0,0.1f,0), Quaternion.identity) as GameObject ;
      atkPrefab.SetActive(false);
      setPrefab = ResourceManager.getObject("Other/setpos");
      canSelect = true;

      nameText.text = "";
      notsetText.text = "NOT SET!";
      notsetText.gameObject.SetActive(false);

     // nowStage = gp.setNowStage(ss.getStageList(0));//ここエラー
      setGp();
      textpos = new Vector3();

      layerNo = LayerMask.NameToLayer("SetPosition");
      layerMask = 1 << layerNo;

    }

    protected void setGp(){
      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
      nowStage = gp.setNowStage(null);//ここエラー
    }

    protected bool checkGp(){
        return gp.getStatus() != gp.NOW_GAME;
    }

    protected bool hasCost(int cost){
        return gp.hasCost(cost);
    }
    public bool canObjSet(){
        return gp.canObjSet();
    }

    public GameObject[] getObjs(){
        return gp.getObjs();
    }

    public void Generate(String name,Vector3 pos,bool isai,bool isstatue){
        gp.Generate(FacilityName,atkPrefab.transform.position,false,gs.isStatue());

    }

    void Update()
    {


      if(checkGp())return;

      Vector2 mousepos = Input.mousePosition;
      ray = Camera.main.ScreenPointToRay(mousepos);
      if (Physics.Raycast(ray,out hit,100.0f,layerMask)){
        setpos = hit.point;
        nameText.gameObject.transform.position = textpos;
        notsetText.gameObject.transform.position = textpos;
    　}

      textpos.Set(mousepos.x,mousepos.y+30,0);

      for(int i=0;i<tgls.Length;i++){
        if(tgls[i].name.Equals(FacilityName)){
          tgls[i].isOn = true;
          select_num = i;
        }else{
          tgls[i].isOn = false;
        }
        //召喚コストが足りなければ黒画像
        Image b = tglobj[i].GetComponent<GenerateBarManager>().getBlackImage();

        int cost = ResourceManager.getObject("Statue/" + tgls[i].name).GetComponent<FacilityManager>().getSData().cost;
        if(hasCost(cost)){
            b.enabled = false;
        }else{
            b.enabled = tglobj[i].GetComponent<GenerateBarManager>().getGenerate();
        }
      }

      int ttype = -1;
      if(Input.GetMouseButtonDown(0)){
        ttype = 0;
      }else if(Input.GetMouseButton(0)){
        ttype = 1;
      }else if(Input.GetMouseButtonUp(0)){
        ttype = 2;
      }

      touchType(ttype);
      
    }

    public void touchType(int ttype){
      switch(ttype){
        case 0:
          playEventDown();
          break;
        case 1:
          playEventDoing();
          break;
        case 2:
          playEventUp();
          break;
      }
    }

    //選択しているトグルを取得
    public void checkSelectToggles(){
      for(int i=0;i<tgls.Length;i++){
        if(tgls[i].name.Equals(EventSystem.current.currentSelectedGameObject.name)){ //タッチしているアイコンの名前とアイコンが一致した時
          if(tgls[i].GetComponent<GenerateBarManager>().getGenerate()){
            tgls[i].isOn = true;
            select_num = i;
            FacilityName = tgls[i].name;
            setType = fs.getFacilityList(i).settype;
            isShow = true;
            isSelect = true;
          }else{
            //現在選択中の施設が設置中
            canSelect = false;
          }    
        }else{
          tgls[i].isOn = false;
          //select_num = -1;
        }
      }
    }

    public void setSelectToggle(String name){
      for(int i=0;i<tgls.Length;i++){
        if(tgls[i].name.Equals(name)){
          tgls[i].isOn = true;
          FacilityName = tgls[i].name;
          select_num = i;

        }else {
          tgls[i].isOn = false;
        }
      }
    }

    public void playEventDown(){
      if(EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.tag.Equals("GenerateIcon")){ //アイコンをタッチしていた場合
        setpos.Set(-10,-10,-10);
        //checkSelectToggles();
        setSelectToggle(EventSystem.current.currentSelectedGameObject.name);
      }
      GenerateBarManager gbm = null;
      if(select_num > -1){
        gbm = tgls[select_num].GetComponent<GenerateBarManager>();
        nowStatue = gbm.getStatus(); //現在のfacilityのStatueDataを取得
        if(!gbm.checkSet()) gbm = null; //単体の設置可能数が超えていないか

      }
      //全体の設置可能数ok && Facility選択されてる && 選択したFacilityの設置可能数ok && コスト足りてる
      if(canObjSet() && (!FacilityName.Equals("")) && (gbm != null) && (hasCost(nowStatue.cost))){
        nameText.text = nowStatue.name4Preview;
        prefab = ResourceManager.getObject("Statue/" + FacilityName); //選択したFacilityのリソースを読み込む
        generating = true;
        generatePrefab = Instantiate(prefab,setpos,Quaternion.identity) as GameObject; //ドラッグ中のFacilityprefab召喚
        setObj = Instantiate(setPrefab,setpos,Quaternion.identity) as GameObject; //召喚範囲用Prefab
        setObj.transform.localScale = new Vector3(0.1f * nowStatue.setpos.x,0.1f,0.1f*nowStatue.setpos.y); 
        GameObject[] objs = getObjs();//設置されてるオブジェクト全ての設置範囲を表示
        setObjs = new GameObject[objs.Length];
        for(int i=0;i<objs.Length;i++){
            if(objs[i] == null) continue;
            if(!objs[i].gameObject.tag.Equals("Statue") && !objs[i].gameObject.tag.Equals("Goblin"))continue;
            Vector2 sp = objs[i].GetComponent<FacilityManager>().getSData().setpos;
            setObjs[i] = Instantiate(setPrefab,objs[i].transform.position,Quaternion.identity) as GameObject;
            setObjs[i].transform.localScale = new Vector3(0.1f * sp.x,0.1f,0.1f * sp.y);
        }
        canSelect = true;
        isShow = true;
        stage.GetComponent<ShowStagePosition>().showSetPosition(nowStatue.settype); //自分の設置できる箇所を表示する
        drawAttackArea();
      }else{
        canSelect = false;
        isShow = false;
        stage.GetComponent<ShowStagePosition>().hideSetPosition();
        atkPrefab.gameObject.SetActive(false);
      }

    }
    public void playEventDoing(){
      if(canSelect && nowStatue != null){
        spos = getSetPosition(nowStatue,setpos);

        if(checkPosition()){
          generatePrefab.transform.position = setPosition(generatePrefab.transform.position,setpos);
          setObj.transform.position = generatePrefab.transform.position;

          nameText.gameObject.SetActive(true);
          notsetText.gameObject.SetActive(false);
          setObj.gameObject.SetActive(true);
          generatePrefab.gameObject.SetActive(true);
          drawAttackArea();
        }else{
          nameText.gameObject.SetActive(false);
          notsetText.gameObject.SetActive(true);
          setObj.gameObject.SetActive(false);
          generatePrefab.gameObject.SetActive(false);
          atkPrefab.SetActive(false);
        }
      }
    }
    public void playEventUp(){
        generating = false;
        if(canSelect){
          if(checkPosition()){
            Generate(FacilityName,atkPrefab.transform.position,false,gs.isStatue());
            tgls[select_num].GetComponent<GenerateBarManager>().setGenerate();
            tgls[select_num].isOn = false;
             
          }
        }
        FacilityName = "";

        select_num = -1;
        nowStatue = null;
        atkPrefab.SetActive(false);

        //画面外にセット
        if(generatePrefab != null){
          atkPrefab.transform.parent = stage.transform;
          generatePrefab.SetActive(false);
          Destroy(generatePrefab);
        }

        if(nameText != null){
          nameText.gameObject.SetActive(false); 
          nameText.text = "";
          notsetText.gameObject.SetActive(false);
        }

        if(setObj != null){
          setObj.SetActive(false);
          Destroy(setObj);
        }

        if(setObjs != null){
          for(int i=0;i<setObjs.Length;i++){
            if(setObjs[i] != null){
              setObjs[i].SetActive(false);
              Destroy(setObjs[i]);
            }
          }
        }

        canSelect = true;
        stage.GetComponent<ShowStagePosition>().hideSetPosition();
    }

    //攻撃範囲を表示(Statueのみ)
    public void drawAttackArea(){
      Vector4 fp = nowStatue.attackpos;
      Vector2 p = new Vector2(generatePrefab.transform.position.x + fp.x,generatePrefab.transform.position.z + fp.y); //攻撃範囲の中心座標
      atkPrefab.transform.parent = generatePrefab.transform;
      atkPrefab.transform.localPosition = new Vector3(0,0,0);
      atkPrefab.transform.localScale = new Vector3(nowStatue.attackpos.z*2.0f,0.01f,nowStatue.attackpos.z*2.0f);
      atkPrefab.GetComponent<CapsuleCollider>().radius = nowStatue.attackpos.z;

      if(!gs.isStatue()) return;
      atkPrefab.SetActive(true);
    }

    //マウス座標がステージ外かどうか
    public Boolean isAreaOver(){
      Vector3 sp = new Vector3(0,0.01f,0);
      float ssizex = stage.GetComponent<Renderer>().bounds.size.x;
      float ssizez = stage.GetComponent<Renderer>().bounds.size.z;

      return ((setpos.x < 0) || (setpos.x > ssizex)) && ((setpos.z < 0) || (setpos.z > ssizez));
    }

    //マウス座標から、設置物の置ける範囲を取得
    public Vector2[] getSetPosition(StatueData f,Vector3 pos){
      return new Vector2[]{
        new Vector2(pos.x-f.setpos.x/2,pos.z+f.setpos.y/2), //左上
        new Vector2(pos.x+f.setpos.x/2,pos.z+f.setpos.y/2), //右上
        new Vector2(pos.x+f.setpos.x/2,pos.z-f.setpos.y/2), //右下
        new Vector2(pos.x-f.setpos.x/2,pos.z-f.setpos.y/2) //左下
      };
    }

    //点と四角形の内外判定
    public bool checkOutside(Vector2 target,Vector2[] rect){
      var count = 0;
      //rect 0 -> 左上 
      //     1 -> 右上
      //     2 -> 右下
      //     3 -> 左下 
      if(!((rect[0].x <= target.x) && (rect[0].y >= target.y))){
          return false;
      }
      if(!((rect[1].x >= target.x) && (rect[1].y >= target.y))){
          return false;
      }
      if(!((rect[2].x >= target.x) && (rect[2].y <= target.y))){
          return false;
      }
      if(!((rect[3].x <= target.x) && (rect[3].y <= target.y))){
          return false;
      }
          return true;
    }

    //設置物を可能範囲内に置けるか
    public Boolean checkPosition(){
      int incount = 0;//設置可能域にいるか
      int setincount = 0;//設置されてるStatueの中に入っているか
      roottype = -1;
      for(int i=0;i<spos.Length;i++){
        bool isin = false;
        bool isinObj = false; 
        for(int j=0;j<nowStage.enablelist.Count;j++){

          
          if(nowStage.enablelist[j][4] > setType){
            continue;
          }

          //設置可能範囲にいるか
          if(!isin){
            if(checkOutside(spos[i],nowStage.enablelistv[j])){
              isin = true;
              if(!gs.isStatue()){
                roottype = (int)nowStage.enablelist[j][5];
              }
            }
          }
          //設置されてるObjectの範囲にいないか
          if(!isinObj){
            GameObject[] objs = getObjs();
            for(int k=0;k<objs.Length;k++){
              if(objs[k] == null) continue;
              String name = gs.isStatue() ? "Statue" : "Goblin";
              if(!objs[k].gameObject.tag.Equals(name))continue;
              Vector2[] setposition = getSetPosition(objs[k].GetComponent<FacilityManager>().getSData(),objs[k].transform.position);
              if(checkOutside(spos[i],setposition)){
                isinObj = true;
                break;
              }            
            }
          }
        }
        if(isin)incount++;
        if(isinObj){
          setincount++;
          break;
        }

      }
      return (incount == spos.Length) && (setincount == 0);
    }

    public Vector3 setPosition(Vector3 nowpos,Vector3 setpos){

      //setposがステージ外だったらダメだよ
      Vector3 sp = new Vector3(0,setpos.y,0);
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
