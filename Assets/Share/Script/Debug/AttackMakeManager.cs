using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Effekseer;
using UnityEditor;
using PlayModeSaver;

namespace takuma_fp
{
    public class AttackMakeManager : MonoBehaviour
    {

        public  GameObject effectObj; //エフェクト用オブジェクト
        public GameObject colliderObj; //当たり判定用オブジェクト
        public GameObject saveObj; //保存する時に使用
        public GameObject parentobj;
        public InputField[] effectInput;
        public InputField[] colliderInput;
        public Text[] effectPosText;
        public Text[] colliderPosText;
        public GameObject StatueList; //スタチュー一覧
        private Vector3 initPos;
        private bool isgenerate;

        public GameObject camera;
        private Vector3 startMousePos;
        private Vector3 presentCamPos;
        private float positionStep = 5.0f;
        private float mouseSensitive = 2.0f;
        private Boolean cameraMoveActive = true;
        private Vector3 lastMousePosition;
        private Vector3 newAngle = new Vector3(0, 0, 0);


        

        // マウスホイールの回転値を格納する変数
        private float scroll;
        // カメラ移動の速度
        public float speed = 1f;
        
        private GameObject visualObj; //表示用のオブジェクト
        void Start()
        {
            initPos = new Vector3(0,0,0);
            isgenerate = true;
            foreach(Transform child in StatueList.transform){
                if(child.gameObject.name.Equals("facility_1")){
                    child.GetComponent<Toggle>().isOn = true;
                }else{
                    child.GetComponent<Toggle>().isOn = false;
                }
            }
            isgenerate = false;
            GenerateStatue(null);
            setObj(effectObj,colliderObj,saveObj);
            updatePosition();
        }

        void Update()
        {
            if((colliderObj != null) && (visualObj != null)){
                visualObj.GetComponent<MeshRenderer>().enabled = colliderObj.GetComponent<Collider>().enabled;
            }

            CameraSlideMouseControll();           
        }

        private void CameraSlideMouseControll(){
            //ズーム
            scroll = Input.GetAxis("Mouse ScrollWheel");
            camera.transform.position += camera.transform.forward * scroll * speed;

            if(Input.GetMouseButtonDown(0)){
                startMousePos = Input.mousePosition;
                presentCamPos = camera.transform.position;
            }else if(Input.GetMouseButtonDown(1)){
                // マウスクリック開始(マウスダウン)時にカメラの角度を保持(Z軸には回転させないため).
                newAngle = camera.transform.localEulerAngles;
                lastMousePosition = Input.mousePosition;
            }else if(Input.GetMouseButton(0)){
                //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
                float x = (startMousePos.x - Input.mousePosition.x) / Screen.width;
                float y = (startMousePos.y - Input.mousePosition.y) / Screen.height;

                x = x * positionStep;
                y = y * positionStep;

                Vector3 velocity = camera.transform.rotation * new Vector3(x, y, 0);
                velocity = velocity + presentCamPos;

                /* if(velocity.x >= Constants.CAMERA_MAX_X){
                velocity.x = Constants.CAMERA_MAX_X;
                }else if( velocity.x <= Constants.CAMERA_MIN_X){
                    velocity.x = Constants.CAMERA_MIN_X;
                }*/

                camera.transform.position = velocity;
            }else if (Input.GetMouseButton(1)){
                // マウスの移動量分カメラを回転させる.
                newAngle.y += (Input.mousePosition.x - lastMousePosition.x) * 0.1f;
                newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * 0.1f;
                camera.gameObject.transform.localEulerAngles = newAngle;

                lastMousePosition = Input.mousePosition;
            }
        }

        public void CameraMove(){
            Vector3 cpos = camera.transform.position;
            camera.transform.position = cpos;
        }

        public String getStatue(){
             foreach(Transform child in StatueList.transform){
                 if(child.GetComponent<Toggle>().isOn){
                    return child.gameObject.name;
                 }
            }
            return "";
        }

        public void GenerateStatue(Toggle toggle){
            if(isgenerate)return;
            if(toggle == null){

            }else{
                if(!toggle.isOn)return;
            }
            if(saveObj != null){
                Destroy(saveObj);
            }
            if(effectObj != null){
                Destroy(effectObj);
            }
            if(colliderObj != null){
                Destroy(colliderObj);
            }
            if(parentobj != null){
                Destroy(parentobj);
            }

            GameObject obj = (GameObject)Resources.Load("takuma/Prefabs/" + getStatue());
            parentobj = Instantiate(obj,initPos,obj.transform.rotation) as GameObject;
            foreach(Transform child in parentobj.transform){
                if(child.gameObject.GetComponent<AttackManager>() != null){
                    saveObj = child.gameObject;
                    break;
                }
            }
            foreach(Transform child in saveObj.transform){
                if(child.gameObject.name.Equals("AtkCollider")){
                    colliderObj = child.gameObject;
                    visualObj = colliderObj.transform.GetChild(0).gameObject;
                }else{
                    effectObj = child.gameObject;
                }
            }

            updatePosition();
        }

        public void PlayAttack(){
            colliderObj.GetComponent<Animation>().Play();
            EffekseerEmitter ee = effectObj.GetComponent<EffekseerEmitter>();
            EffekseerEffectAsset ea = ee.effectAsset;
            ee.Play(ea);
        }

        public void SavePosition(){
            //saveObj.GetComponent<SavePlayModeChanges>().setEnable(true);
            UnityEditor.PrefabUtility.CreatePrefab ("Assets/Resources/takuma/Prefabs/" + getStatue() + ".prefab", parentobj);

        }

        public void SetPosition(int type){
            float px,py,pz,rx,ry,rz,sx,sy,sz;
            if(type == 1){
                //Collider
                px = float.Parse(colliderInput[0].text);
                py = float.Parse(colliderInput[1].text);
                pz = float.Parse(colliderInput[2].text);
                rx = float.Parse(colliderInput[3].text); 
                ry = float.Parse(colliderInput[4].text); 
                rz = float.Parse(colliderInput[5].text); 
                sx = float.Parse(colliderInput[6].text);
                sy = float.Parse(colliderInput[7].text);
                sz = float.Parse(colliderInput[8].text);

                colliderObj.transform.position = new Vector3(px,py,pz);
                colliderObj.transform.localEulerAngles = new Vector3(rx,ry,rz);
                colliderObj.transform.localScale = new Vector3(sx,sy,sz);
            }else if(type == 2){
                //Effekseer
                px = float.Parse(effectInput[0].text);
                py = float.Parse(effectInput[1].text);
                pz = float.Parse(effectInput[2].text);
                rx = float.Parse(effectInput[3].text);
                ry = float.Parse(effectInput[4].text);
                rz = float.Parse(effectInput[5].text);
                sx = float.Parse(effectInput[6].text);
                sy = float.Parse(effectInput[7].text);
                sz = float.Parse(effectInput[8].text);

                effectObj.transform.position = new Vector3(px,py,pz);
                effectObj.transform.localEulerAngles = new Vector3(rx,ry,rz);
                effectObj.transform.localScale = new Vector3(sx,sy,sz);
            }
        }
        
        public void setObj(GameObject atkobj,GameObject colliderobj,GameObject saveobj){
            effectObj = atkobj;
            colliderObj = colliderobj;
            saveObj = saveobj;
        }

        public void updatePosition(){

            // Collider
            Vector3 cp = colliderObj.transform.position;
            Vector3 cr = colliderObj.transform.localEulerAngles;
            Vector3 cs = colliderObj.transform.localScale;
            colliderInput[0].text = "" + cp.x;
            colliderInput[1].text = "" + cp.y;
            colliderInput[2].text = "" + cp.z;
            colliderInput[3].text = "" + cr.x;
            colliderInput[4].text = "" + cr.y;
            colliderInput[5].text = "" + cr.z;
            colliderInput[6].text = "" + cs.x;
            colliderInput[7].text = "" + cs.y;
            colliderInput[8].text = "" + cs.z;
            

            colliderPosText[0].text = "" + cp.x;
            colliderPosText[1].text = "" + cp.y;
            colliderPosText[2].text = "" + cp.z;
            colliderPosText[3].text = "" + cr.x;
            colliderPosText[4].text = "" + cr.y;
            colliderPosText[5].text = "" + cr.z;
            colliderPosText[6].text = "" + cs.x;
            colliderPosText[7].text = "" + cs.y;
            colliderPosText[8].text = "" + cs.z;


            // Effekseer
            Vector3 ep =effectObj.transform.position;
            Vector3 er = effectObj.transform.localEulerAngles;
            Vector3 es = effectObj.transform.localScale;
            effectInput[0].text = "" + ep.x;
            effectInput[1].text = "" + ep.y;
            effectInput[2].text = "" + ep.z;
            effectInput[3].text = "" + er.x;
            effectInput[4].text = "" + er.y;
            effectInput[5].text = "" + er.z;
            effectInput[6].text = "" + es.x;
            effectInput[7].text = "" + es.y;
            effectInput[8].text = "" + es.z;

            effectPosText[0].text = "" + ep.x;
            effectPosText[1].text = "" + ep.y;
            effectPosText[2].text = "" + ep.z;
            effectPosText[3].text = "" + er.x;
            effectPosText[4].text = "" + er.y;
            effectPosText[5].text = "" + er.z;
            effectPosText[6].text = "" + es.x;
            effectPosText[7].text = "" + es.y;
            effectPosText[8].text = "" + es.z;

        }
    }
}
