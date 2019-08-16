using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjManager : MonoBehaviour
{
  public float speed = 10;
  private float time = 0;
  public float removetime ;
  void Start ()
  {
    GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.normalized.x * speed,0,transform.forward.normalized.z * speed);
  }


  public void Update(){
  }

  public void OnTriggerEnter(Collider other){
    Debug.Log("trieeeeee : " + other.gameObject.tag);
  }


}
