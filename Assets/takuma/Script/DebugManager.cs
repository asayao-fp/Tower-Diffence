using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Facility");
        for(int i=0;i<obj.Length;i++){
          Destroy(obj[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
