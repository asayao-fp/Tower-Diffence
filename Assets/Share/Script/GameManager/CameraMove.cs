using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    GameObject camera;
    void Start(){
        camera = GameObject.Find("Camera");
    }

    public void mapMove(){
        camera.GetComponent<CameraController>().CameraMove();
    }
}
