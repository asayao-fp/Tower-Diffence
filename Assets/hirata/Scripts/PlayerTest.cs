using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{

    public float Speed = 1.0f; // 移動速度 m/s

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float moveLength = Speed * Time.smoothDeltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += moveLength;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= moveLength;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= moveLength;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += moveLength;
        }
        transform.position = pos;
    }
}
