using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CameraController : MonoBehaviour
{

    private float positionStep = 5.0f;

    //マウス感度
    private float mouseSensitive = 2.0f;

    private Boolean cameraMoveActive = true;

    private Transform _camTransform;

    private Vector3 startMousePos;
    private Vector3 presentCamPos;

    // Start is called before the first frame update
    public float rightMaxPos;
    public float leftMaxPos;

    InputManager inputmanager;
    void Start()
    {
      _camTransform = this.gameObject.transform;
      inputmanager = GameObject.FindWithTag("GameManager").GetComponent<InputManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if(!inputmanager.isShow){
          CameraSlideMouseControll();
        }
    }

    private void CameraSlideMouseControll(){
      if(Input.GetMouseButtonDown(0)){
        startMousePos = Input.mousePosition;
        presentCamPos = _camTransform.position;
      }

      if(Input.GetMouseButton(0)){
          //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
         float x = (startMousePos.x - Input.mousePosition.x) / Screen.width;
         float y = (startMousePos.y - Input.mousePosition.y) / Screen.height;

         x = x * positionStep;
        // y = y * positionStep;

         Vector3 velocity = _camTransform.rotation * new Vector3(x, 0, 0);
         velocity = velocity + presentCamPos;

         if(velocity.x >= rightMaxPos){
          velocity.x = rightMaxPos;
         }else if( velocity.x <= leftMaxPos){
            velocity.x = leftMaxPos;
         }

         _camTransform.position = velocity;

    

      }
    }

    public void CameraMove(){
      Vector3 cpos = _camTransform.position;

      if(cpos.x == leftMaxPos){
        cpos.x = leftMaxPos;
      }else if(cpos.x == rightMaxPos){
        cpos.x = rightMaxPos;
      }else{
        cpos.x = leftMaxPos;
      }

      _camTransform.position = cpos;

    }
}
