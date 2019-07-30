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
    time += Time.deltaTime;

    if(time > removetime){
      Destroy(this.gameObject);
    }
  }
}
