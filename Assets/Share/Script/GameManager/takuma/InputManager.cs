using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;

public class InputManager : MonoBehaviour
{
    private GameObject stage;　//ステージ
    private FacilitySetting fs; //施設セッティング
    private StageSetting ss; //ステージセッティング
    private Stage nowStage; //現在のステージ情報
    private Facility nowFacility; //現在の施設情報

    private Vector3 setpos; //マウス座標
    private GameObject generatePrefab; //ドラッグ中に使用する施設用オブジェクト
    GameObject prefab; //生成用のプレハブ
    private GameObject atkPrefab; //攻撃範囲用のプレハブ

    public GameObject[] tglobj;
    public Toggle[] tgls = new Toggle[4]; //施設選択用
    private Vector2[] spos = new Vector2[4]; //設置可能時の座標

    Boolean touchGui; //uGuiを選択しているか
    String FacilityName = ""; //現在選択中の施設名
    Boolean isSelect; //施設のアイコンを選択しているか
    Boolean isMoving; //施設を移動中か

    static Material showSetPositionMat; //設置可能範囲用マテリアル
    public Boolean isShow; //設置可能範囲、攻撃範囲を表示するか
    public int setType = 100; //設置できるタイプ
    private Boolean canSelect; //施設を選択できるか
    private Boolean isdrawAttack = false; //攻撃可能範囲を表示するか

    GameProgress gp; 
    GenerateCostManager gcm;


    void Start()
    {
      setpos = new Vector3();
      stage = GameObject.FindWithTag("Stage");
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      ss = GameObject.FindWithTag("StaticObjects").GetComponent<StageSetting>();
      nowStage = ss.getStageList(0);
      for(int i=0;i<tgls.Length;i++){
        tgls[i].isOn = false;
      }
      showSetPositionMat = new Material (Shader.Find ("Unlit/TestShader"));
      atkPrefab = Instantiate ((GameObject)Resources.Load ("takuma/Prefabs/AtkPosSphere"), new Vector3(0,0,0), Quaternion.identity) as GameObject ;
      atkPrefab.SetActive(false);
      canSelect = true;
      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
      gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
    }

    void Update()
    {

      if(gp.getStatus() != gp.NOW_GAME)return;

      for(int i=0;i<tgls.Length;i++){
        
        if(tgls[i].name.Equals(FacilityName)){
          tgls[i].isOn = true;
        
        }else{
          tgls[i].isOn = false;
        }
        //召喚コストが足りなければ黒画像
        if(gcm.getCost() >= fs.getFacility(tgls[i].name).cost){
            tglobj[i].GetComponent<GenerateBarManager>().blackImage.enabled = false;
        }else{
            tglobj[i].GetComponent<GenerateBarManager>().blackImage.enabled = tglobj[i].GetComponent<GenerateBarManager>().getGenerate();
        }
      }

       GameObject current = EventSystem.current.currentSelectedGameObject;
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       RaycastHit hit;
       if (Input.GetMouseButtonDown(0)) {
           if(current){ //アイコンをタッチしていた場合
             setpos.Set(-10,-10,-10);
             for(int i=0;i<tgls.Length;i++){
               if(tgls[i].name.Equals(current.name)){ //タッチしているアイコンの名前とアイコンが一致した時
                 if(tgls[i].GetComponent<GenerateBarManager>().getGenerate()){
                   tgls[i].isOn = true;
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
               }
             }
           }else{
            //アイコン以外をタッチしていた時
            if (Physics.Raycast(ray,out hit,10.0f))
            {
              setpos = hit.point;
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


         if(isSelect &&  canSelect && (!FacilityName.Equals(""))){
           //設置可能範囲内なら置ける
           nowFacility = fs.getFacility(FacilityName);
           
           //コストが足りなかったら召喚できない
           if(gcm.getCost() >= nowFacility.cost){
            spos = getSetPosition(nowFacility,setpos);
            prefab = (GameObject)Resources.Load ("takuma/Prefabs/" + FacilityName);
            generatePrefab = Instantiate (prefab, setpos, Quaternion.identity) as GameObject;
            stage.GetComponent<ShowStagePosition>().showSetPosition(setType);
            if(checkPosition()){
              generatePrefab.gameObject.SetActive(true);
            }else{
              generatePrefab.gameObject.SetActive(false);
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

        if (Physics.Raycast(ray,out hit,10.0f))
        {
          setpos = hit.point;
        }
        //マウスを離した時に設置可能位置に置けなかった場合か、toggleを洗濯していたら設置可能位置は表示したまま
         if(isSelect && canSelect){
           if(checkPosition() && (generatePrefab != null)){
           //|| isMoving){


             GameObject generate = Instantiate (prefab, generatePrefab.transform.position, Quaternion.identity) as GameObject;
             generate.GetComponent<FacilityManager>().Generate(atkPrefab.transform.position,atkPrefab.transform.localScale,nowFacility);
             
             //召喚
             gcm.generateCost(nowFacility.cost);

             isShow = false;
             FacilityName = "";
             nowFacility = null;
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
          generatePrefab.SetActive(false);
          Destroy(generatePrefab);
        }
        isMoving = false;
        canSelect = true;
        stage.GetComponent<ShowStagePosition>().hideSetPosition();
      }
      else if (Input.GetMouseButton(0)) {
        if (Physics.Raycast(ray,out hit,100.0f))
        {
          setpos = hit.point;
          setpos.y = 0.1f;
        }
        if(isSelect && (nowFacility != null) && canSelect){
          spos = getSetPosition(nowFacility,setpos);
          stage.GetComponent<ShowStagePosition>().showSetPosition(setType);
          //ドラッグ中に設置可能範囲内に置いたら常に表示しておく
          if(checkPosition()){
            isdrawAttack = true;
          //|| isMoving){
            isMoving = true;
            generatePrefab.transform.position = setPosition(generatePrefab.transform.position,setpos);
            generatePrefab.gameObject.SetActive(true);


          }else{
            generatePrefab.gameObject.SetActive(false);
            isdrawAttack = false;
          }
        }else{
          stage.GetComponent<ShowStagePosition>().hideSetPosition();
        }
      }

      if(isdrawAttack && isMoving && (nowFacility != null) && canSelect){
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
      Vector4 fp = nowFacility.attackpos;
      Vector2 p = new Vector2(generatePrefab.transform.position.x + fp.x,generatePrefab.transform.position.z + fp.y); //攻撃範囲の中心座標
      atkPrefab.transform.position = new Vector3(p.x,0.05f,p.y);
      atkPrefab.transform.localScale = new Vector3(fp.z,fp.z,fp.z);
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
        for(int j=0;j<nowStage.enablelist.Count;j++){

          if(nowStage.enablelist[j][4] > setType){
            continue;
          }

          float[] area = nowStage.enablelist[j];
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

    //設置可能範囲、攻撃範囲描画
    /*
    void OnRenderObject () {
      if(!isShow)return;

      if(!showSetPositionMat){
        Debug.Log("show set material is not Active");
        return;
      }

      showSetPositionMat.SetPass (0);

      GL.PushMatrix ();
      for(int i=0;i<nowStage.enablelist.Count;i++){
        if(nowStage.enablelist[i][4] > setType){
          continue;
        }
        GL.Begin (GL.QUADS);
        GL.Vertex3(nowStage.enablelist[i][0],0,nowStage.enablelist[i][2]); //左下
        GL.Vertex3(nowStage.enablelist[i][0],0,nowStage.enablelist[i][3]); //左上
        GL.Vertex3(nowStage.enablelist[i][1],0,nowStage.enablelist[i][3]); //右上
        GL.Vertex3(nowStage.enablelist[i][1],0,nowStage.enablelist[i][2]); //右下
        GL.End ();
      }

      GL.PopMatrix ();
    }*/
}
