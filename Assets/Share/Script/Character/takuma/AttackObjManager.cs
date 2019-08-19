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

  void Start ()
  {
    GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.normalized.x * speed,0,transform.forward.normalized.z * speed);
    objs = new Dictionary<int,GameObject>();
  }


  public void Update(){
  }

  public void OnTriggerEnter(Collider other){
   if(other.gameObject.tag.Equals(Constants.GOBLIN_TAG)){
     if(!objs.ContainsKey(other.gameObject.GetInstanceID())){
      objs.Add(other.gameObject.GetInstanceID(),other.gameObject);
      other.gameObject.GetComponent<FacilityManager>().addHP(AttackDamage);
     }
   }
  }


}
