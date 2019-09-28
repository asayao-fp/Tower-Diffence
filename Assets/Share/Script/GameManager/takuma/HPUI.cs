using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{

    [SerializeField]
    private Vector3 pos;
    void Start(){
        GameObject amm = GameObject.Find("AttackMakeManager");
        if(amm != null){
            this.gameObject.SetActive(false);
        }else{
            this.gameObject.SetActive(true);
        }
    }
    void LateUpdate() {
        //　カメラと同じ向きに設定
       transform.rotation = Camera.main.transform.rotation;
       transform.localPosition = pos;        
    }
}
//0.75 20.92 6.98 statue1 -2.5 22.4 -0.3
//-1.4 15.17 8.56 statue2
//0.5 15.76 9.67 statue3
//-1.05 13.09 5.33 statue4
// 1.1 35.6 19.9 statue5
