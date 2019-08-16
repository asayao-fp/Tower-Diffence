using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    private List<GameObject> sg_objs;
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
 

    void Awake(){
      game_status = 0;
      start = 5;
    }

    void Start()
    {
      fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
      sg_objs = new List<GameObject>();
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

    }

    //現在のステータスを取得
    public int getStatus(){
      return game_status;
    }

    //死んだ
    public void Dead(GameObject obj){

    }

    //召喚された
    public void Generate(GameObject obj){
      if(gcm == null){
        gcm = GameObject.FindWithTag("GenerateCost").GetComponent<GenerateCostManager>();
      }

      gcm.generateCost(obj.GetComponent<FacilityManager>().fInfo.cost);

    }

    //攻撃受けた
    public void AddHP(int serial_id,int hp){

    }

}
