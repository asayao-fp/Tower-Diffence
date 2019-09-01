using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Effekseer;

public class StatueManager : FacilityManager
{
    [SerializeField]
    private StatueData statue; //自分のパラメータ
    private gameStatus gstatus; //ゲーム中に変化するパラメータ
    
    protected float time; //攻撃間隔用
    protected float deletetime; //召喚時間用
    protected List<GameObject> enemylist; //攻撃範囲内にいる敵のリスト

    protected GameObject Gene; //召喚時のエフェクト用オブジェクト
    public GameObject Atk; //攻撃用のエフェクト
    protected Boolean isGene; //エフェクトを使用したかどうかのフラグ
    protected GameProgress gp;    
    protected int obj_num; //オブジェクトのユニークid
    public Boolean isDebug;
    public Image hpbar;
    public String AtkName;

    GameObject amm; //AttackManager check用
    GameObject obj;//敵

    void Awake(){
      Material material= GameObject.Find("StaticManager").GetComponent<GameSettings>().getMaterial();
      for(int i=0;i<obj_materials.Length;i++){
        obj_materials[i].material = material;
      }
    }

    void Update()
    {
      if(check()){
        return;
      }
        
      if(gp.getStatus() != gp.NOW_GAME)return;

      float t = Time.deltaTime;
      deletetime += t;
      time += t;

      checkEnemy();

      if(time >= statue.atkInterval){
        time = 0;
        Attack();
      }

      if(hpbar != null){
        hpbar.fillAmount = (1 - (deletetime / (float)statue.time)) + (1 - (gstatus.hp / (float)statue.hp));
      }
    }

    public override void checkEnemy(){
      obj = null;
      for(int i=0;i<enemylist.Count;i++){          
        if(enemylist[i] == null){
          continue;
        }
        obj = enemylist[i];
        float distance = Vector3.Distance(obj.transform.position,transform.position);
  
        //攻撃範囲内にいなければその敵の方向には向かない
        if(distance <= statue.attackpos.z/2){
          Quaternion lockRotation = Quaternion.LookRotation(obj.transform.position - transform.position, Vector3.up);    
          lockRotation.z = 0;
          lockRotation.x = 0;
          transform.rotation = Quaternion.Lerp(transform.rotation, lockRotation, 10f);
          break;
        }else{
          obj = null;
          continue;
        }
      }
    }

    public override bool check(){
      return (amm != null) || InputManager.generating || isEnd;
    }
    public override void Attack(){
      if(obj == null){
        return;
      }
      Atk.GetComponent<AttackManager>().Attack();
    }

     // 召喚した時に初期化処理をやる
     public override　void Generate(Vector3 pos,Vector3 scale,StatueData s){
      isEnd = true; //念の為
      GameObject atkpre = (GameObject)Resources.Load("takuma/Prefabs/AtkCheck");
      GameObject atkcheck = Instantiate(atkpre,pos,Quaternion.identity) as GameObject;
      atkcheck.transform.position = pos;
      atkcheck.transform.localScale = scale;
      atkcheck.transform.parent = this.gameObject.transform;

      hpbar.transform.position = new Vector3(this.transform.position.x ,this.transform.position.y + 0.3f,this.transform.position.z);
      hpbar.fillAmount = 1;

      amm = GameObject.Find("AttackMakeManager");

      time = 0.0f;
      Gene = (GameObject)Resources.Load("takuma/Prefabs/Generate");
      enemylist = new List<GameObject>();
      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();


      foreach(Transform child in transform){
        if(child.gameObject.name.StartsWith(AtkName)){
          Atk = child.gameObject;
        }
      }
    
      Gene.transform.position = transform.position;
      EffekseerEmitter ee = Gene.GetComponent<EffekseerEmitter>();
      EffekseerEffectAsset ea = ee.effectAsset;
      ee.Play(ea);

      gstatus.hp = statue.hp;
      isEnd = false;
    }
    
    public override　void EnemyOnArea(GameObject obj){
      if(obj.CompareTag(Constants.GOBLIN_TAG) && !enemylist.Contains(obj)){
        enemylist.Add(obj);
      }
    }

    public override　void setId(int id){
      this.obj_num = id;
    }

    public override　void addHP(int hp){
        gstatus.hp += hp;
    }

    public override void Dead(){
      //消滅エフェクト実行
      
      Destroy(this.gameObject);
    }

    public override StatueData getSData(){
      return statue;
    }

    public override GobrinData getGData(){
      return null;
    }

    public override gameStatus getStatus(){
      return gstatus;
    }

    public override float getHP(){
      return hpbar.fillAmount;
    }
}
