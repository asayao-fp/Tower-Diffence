using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateController : MonoBehaviour
{
    [SerializeField]
    private GameObject goblin;

    [SerializeField]
    private int lineNum = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            goblin.GetComponent<GobMane>().setLine(lineNum);
            Instantiate(goblin);
        }
    }
}
