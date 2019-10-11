using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;


public class AttackObjManager : MonoBehaviour
{
  public float speed = 10;
  private float time = 0;
  public float removetime ;
  public Dictionary<int,GameObject> objs;
  public int AttackDamage; //ダメージ量
  GameProgress gp;

  private int count = 0; //判定の回数

  private int attacktype; //攻撃の種類
  private GameObject obj;//親のオブジェクト

  private int attack; //攻撃力

  void Start ()
  {
    GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.normalized.x * speed,0,transform.forward.normalized.z * speed);
    objs = new Dictionary<int,GameObject>();
    gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
    obj = transform.root.gameObject;


    //var color      = ColliderVisualizer.VisualizerColorType.Red;
    //var message    = "ピカチュウ";
    //var fontSize   = 36;

    if(gp.getDebug()){
     // this.gameObject.AddComponent<ColliderVisualizer>().Initialize(color, message, fontSize);
    }

  }


  public void Update(){
  }

  public void OnTriggerEnter(Collider other){
   if(other.gameObject.tag.Equals(Constants.GOBLIN_TAG)){
    if(!objs.ContainsKey(other.gameObject.GetInstanceID())){
      FacilityManager fm = other.gameObject.GetComponent<FacilityManager>();
      if(fm != null){
        gp.calcDamage(fm.obj_num,attacktype,attack);
      }       
      objs.Add(other.gameObject.GetInstanceID(),other.gameObject);
    }
   }
  }

  public void setType(int type,int attack){
    attacktype = type;
    this.attack = attack;
  }
}
