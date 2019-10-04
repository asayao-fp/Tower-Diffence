using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  void Start ()
  {
    GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.normalized.x * speed,0,transform.forward.normalized.z * speed);
    objs = new Dictionary<int,GameObject>();
    gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
    obj = transform.root.gameObject;
  }


  public void Update(){
  }


 private void OnParticleCollision(GameObject other){
    Debug.Log("objct : " + other.name);
  }


  public void OnTriggerEnter(Collider other){
   if(other.gameObject.tag.Equals(Constants.GOBLIN_TAG)){
      FacilityManager fm = other.gameObject.GetComponent<FacilityManager>();
      if(fm != null){
        gp.calcDamage(fm.obj_num,attacktype);
      }

     //if(!objs.ContainsKey(other.gameObject.GetInstanceID())){
       
      //objs.Add(other.gameObject.GetInstanceID(),other.gameObject);
      //other.gameObject.GetComponent<FacilityManager>().addHP(AttackDamage);
     //}
   }
  }

  public void setType(int type){
    attacktype = type;
  }
}
