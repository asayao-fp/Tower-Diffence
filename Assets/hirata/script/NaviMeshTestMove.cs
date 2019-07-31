using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviMeshTestMove : MonoBehaviour
{
    public Transform goal;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goal.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
