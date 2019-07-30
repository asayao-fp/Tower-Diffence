using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class charamove_test : MonoBehaviour
{
  public Transform goal;

    void Start () {
       NavMeshAgent agent = GetComponent<NavMeshAgent>();
       agent.destination = goal.position;
    }


    public void Update(){
      if((goal.position.z - transform.position.z) < 0.05f){
        transform.position = new Vector3(0,0,0);
      }
    }
}
