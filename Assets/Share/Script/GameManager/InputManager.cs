using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InputManager : MonoBehaviour
{
    private GameObject stage;　//ステージ
    private FacilitySetting fs; //施設セッティング
    private StageSetting ss; //ステージセッティング
    private Stage nowStage; //現在のステージ情報
    private StatueData nowStatue; //現在の施設情報

    private Vector3 setpos; //マウス座標
    private GameObject generatePrefab; //ドラッグ中に使用する施設用オブジェクト
    GameObject prefab; //生成用のプレハブ
    private GameObject atkPrefab; //攻撃範囲用のプレハブ
    private GameObject setPrefab; //設置範囲用のプレハブ
    private GameObject setObj; //設置用のオブジェクト
    private GameObject[] setObjs; //設置されてるstatue用オブジェクト

    public GameObject[] tglobj;
    public Toggle[] tgls = new Toggle[4]; //施設選択用
    private Vector2[] spos = new Vector2[4]; //設置可能時の座標

    Boolean touchGui; //uGuiを選択しているか
    String FacilityName = ""; //現在選択中の施設名
    Boolean isSelect; //施設のアイコンを選択しているか
    Boolean isMoving; //施設を移動中か

    public Material showSetPositionMat; //設置可能範囲用マテリアル
    public Boolean isShow; //設置可能範囲、攻撃範囲を表示するか
    public int setType = 100; //設置できるタイプ
    private Boolean canSelect; //施設を選択できるか
    private Boolean isdrawAttack = false; //攻撃可能範囲を表示するか

    [SerializeField]
    private TextMeshProUGUI nameText;
    private Vector3 textpos; //ドラッグ中の名前
    [SerializeField]
    private TextMeshProUGUI notsetText; //設置できないときに表示するオブジェクト


    private int select_num;

    GameProgress gp; 

    public static bool generating = false;

    void Start()
    {
      setpos = new Vector3();
      stage = GameObject.FindWithTag("Stage");
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      ss = GameObject.FindWithTag("StaticObjects").GetComponent<StageSetting>();
      for(int i=0;i<tgls.Length;i++){
        tgls[i].isOn = false;
      }
      //showSetPositionMat = new Material (Shader.Find ("Unlit/TestShader"));
      //atkPrefab = Instantiate ((GameObject)Resources.Load ("takuma/Prefabs/AtkPosSphere"), new Vector3(0,0,0), Quaternion.identity) as GameObject ;
      atkPrefab = Instantiate (ResourceManager.getObject("takuma/Master/Character/AtkPosSphere"), new Vector3(0,0.1f,0), Quaternion.identity) as GameObject ;
      atkPrefab.SetActive(false);
      setPrefab = ResourceManager.getObject("Other/setpos");
      canSelect = true;
      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
      nameText.text = "";
      notsetText.text = "NOT SET!";
      notsetText.gameObject.SetActive(false);

      nowStage = gp.setNowStage(ss.getStageList(0));
      textpos = new Vector3();

    }

    void Update()
    {

      if(gp.getStatus() != gp.NOW_GAME)return;

      for(int i=0;i<tgls.Length;i++){
        
        if(tgls[i].name.Equals(FacilityName)){
          tgls[i].isOn = true;
          select_num = i;
        
        }else{
          tgls[i].isOn = false;
        }
        //召喚コストが足りなければ黒画像
        Image b = tglobj[i].GetComponent<GenerateBarManager>().getBlackImage();

        if(gp.hasCost(fs.getFacility(tgls[i].name).cost)){
            b.enabled = false;
        }else{
            b.enabled = tglobj[i].GetComponent<GenerateBarManager>().getGenerate();
        }
      }

       Vector2 mousepos = Input.mousePosition;
       textpos.Set(mousepos.x,mousepos.y+30,0);
       GameObject current = EventSystem.current.currentSelectedGameObject;
       Ray ray = Camera.main.ScreenPointToRay(mousepos);
       RaycastHit hit;
       if (Input.GetMouseButtonDown(0)) {
           if(current){ //アイコンをタッチしていた場合
             setpos.Set(-10,-10,-10);
             for(int i=0;i<tgls.Length;i++){
               if(tgls[i].name.Equals(current.name)){ //タッチしているアイコンの名前とアイコンが一致した時
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
           }else{
            //アイコン以外をタッチしていた時
            if (Physics.Raycast(ray,out hit,10.0f))
            {
              setpos = hit.point;
              nameText.gameObject.transform.position = textpos;
              notsetText.gameObject.transform.position = textpos;
              int count = 0;
              for(int i=0;i<tgls.Length;i++){
                //一つでもアイコンが選択されていた時
                if(tgls[i].isOn){
                  FacilityName = tgls[i].name;
                  count++;
                }
              }
              if(count == 0){
                isSelect = false;
              }else{
                isShow = true;
              }
            }else{
              canSelect = false;
            }
         }


         if(gp.canObjSet() &&  canSelect && (!FacilityName.Equals(""))){
           //設置可能範囲内なら置ける
           GenerateBarManager gbm = tgls[select_num].GetComponent<GenerateBarManager>();
           nowStatue = gbm.getStatus();

           if(!gbm.checkSet()){
            //設置可能数を超えたら設置できない
            canSelect = false;
            isShow = false;
            stage.GetComponent<ShowStagePosition>().hideSetPosition();


           }else if(gp.hasCost(nowStatue.cost)){
            //コストが足りなかったら召喚できない

            prefab = ResourceManager.getObject("Statue/" + FacilityName);

            spos = getSetPosition(nowStatue,setpos);

            //召喚中！！
            generating = true;
            generatePrefab = Instantiate (prefab, setpos, Quaternion.identity) as GameObject;

            StatueData sd = generatePrefab.GetComponent<FacilityManager>().getSData();
            nameText.text = sd.name4Preview;

            //選択したオブジェクトの設置範囲を表示
            setObj = Instantiate(setPrefab,setpos,Quaternion.identity) as GameObject;
            Vector2 Spos = sd.setpos;
            setObj.transform.localScale = new Vector3(0.1f * Spos.x,0.1f,0.1f*Spos.y);

            //設置されてるオブジェクト全ての設置範囲を表示
            GameObject[] objs = gp.getObjs();
            setObjs = new GameObject[objs.Length];
            for(int i=0;i<objs.Length;i++){
                if(!objs[i].gameObject.tag.Equals("Statue"))continue;
                Vector2 sp = objs[i].GetComponent<FacilityManager>().getSData().setpos;
                setObjs[i] = Instantiate(setPrefab,objs[i].transform.position,Quaternion.identity) as GameObject;
                setObjs[i].transform.localScale = new Vector3(0.1f * sp.x,0.1f,0.1f * sp.y);
            }

            stage.GetComponent<ShowStagePosition>().showSetPosition(setType);
            if(checkPosition()){
              generatePrefab.gameObject.SetActive(true);
              nameText.gameObject.SetActive(true);
              notsetText.gameObject.SetActive(false);
            }else{
              generatePrefab.gameObject.SetActive(false);
              nameText.gameObject.SetActive(false);
              notsetText.gameObject.SetActive(true);
            }
           }else{
            canSelect = false;
            isShow = false;
            stage.GetComponent<ShowStagePosition>().hideSetPosition();

           }
         }else{
           stage.GetComponent<ShowStagePosition>().hideSetPosition();
         }
      }
      else if (Input.GetMouseButtonUp(0)) {
        generating = false;
        if (Physics.Raycast(ray,out hit,10.0f))
        {
          setpos = hit.point;
          nameText.gameObject.transform.position = textpos;
          notsetText.gameObject.transform.position = textpos;
        }
        //マウスを離した時に設置可能位置に置けなかった場合か、toggleを洗濯していたら設置可能位置は表示したまま
         if(isSelect && canSelect){
           if(checkPosition() && (generatePrefab != null)){
           //|| isMoving){

             //gobrinとstatueで分ける
             gp.Generate(FacilityName,atkPrefab.transform.position);


             isShow = false;
             FacilityName = "";
             select_num = -1;
             nowStatue = null;
             for(int i=0;i<tgls.Length;i++){
               if(tgls[i].isOn){
                 tgls[i].GetComponent<GenerateBarManager>().setGenerate();
               }
               tgls[i].isOn = false;
             }
           }
         }else{
           if(EventSystem.current.currentSelectedGameObject !=null){
            for(int i=0;i<tgls.Length;i++){
              if(tgls[i].name.Equals(FacilityName))continue;
              if(tgls[i].name.Equals(EventSystem.current.currentSelectedGameObject.name)){
                tgls[i].isOn = true;
                isShow = true;
              }
            }
          }
        }


        GameObject[] geneobjs = GameObject.FindGameObjectsWithTag("Statue");
        for(int i=0;i<geneobjs.Length;i++){
          if(!geneobjs[i].activeSelf){
            Destroy(geneobjs[i]);
          }
        }
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

        isMoving = false;
        canSelect = true;
        stage.GetComponent<ShowStagePosition>().hideSetPosition();
      }
      else if (Input.GetMouseButton(0)) {
       // Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
        //if(Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask))
        if (Physics.Raycast(ray,out hit,100.0f))
        {
          setpos = hit.point;
          nameText.gameObject.transform.position = textpos;
          notsetText.gameObject.transform.position = textpos;
          setpos.y = 0.1f;
        }
        if(isSelect && (nowStatue != null) && canSelect){
          spos = getSetPosition(nowStatue,setpos);
          stage.GetComponent<ShowStagePosition>().showSetPosition(setType);
          //ドラッグ中に設置可能範囲内に置いたら常に表示しておく
          if(checkPosition()){
            isdrawAttack = true;
            isMoving = true;
            generatePrefab.transform.position = setPosition(generatePrefab.transform.position,setpos);
            generatePrefab.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            notsetText.gameObject.SetActive(false);

            setObj.gameObject.SetActive(true);
            setObj.transform.position = generatePrefab.transform.position;
          }else{
            generatePrefab.gameObject.SetActive(false);
            setObj.gameObject.SetActive(false);
            isdrawAttack = false;
            nameText.gameObject.SetActive(false);
            notsetText.gameObject.SetActive(true);
          }
        }else{
          stage.GetComponent<ShowStagePosition>().hideSetPosition();
        }
      }

      if(isdrawAttack && isMoving && (nowStatue != null) && canSelect){
        drawAttackArea();
      }else if(atkPrefab != null){
        atkPrefab.SetActive(false);
      }

      if(isShow && canSelect && isSelect){
        stage.GetComponent<ShowStagePosition>().showSetPosition(setType);
      }else{
        stage.GetComponent<ShowStagePosition>().hideSetPosition();
      }
    }

    public void drawAttackArea(){
      Vector4 fp = nowStatue.attackpos;
      Vector2 p = new Vector2(generatePrefab.transform.position.x + fp.x,generatePrefab.transform.position.z + fp.y); //攻撃範囲の中心座標
      atkPrefab.transform.parent = generatePrefab.transform;
      //atkPrefab.transform.localPosition = new Vector3(p.x,0.05f,p.y);
      atkPrefab.transform.localPosition = new Vector3(0,0.05f,0);
      atkPrefab.transform.localScale = new Vector3(nowStatue.attackpos.z*2.0f,0.01f,nowStatue.attackpos.z*2.0f);
      atkPrefab.GetComponent<CapsuleCollider>().radius = nowStatue.attackpos.z;
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
            }
          }

          //設置されてるObjectの範囲にいないか
          if(!isinObj){
            GameObject[] objs = gp.getObjs();
            for(int k=0;k<objs.Length;k++){
              if(!objs[k].gameObject.tag.Equals("Statue"))continue;
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
