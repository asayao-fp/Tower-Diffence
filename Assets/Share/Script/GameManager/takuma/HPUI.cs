using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    void Start(){
        GameObject amm = GameObject.Find("AttackMakeManager");
        if(amm != null){
            this.gameObject.SetActive(false);
        }
    }
    void LateUpdate() {
        //　カメラと同じ向きに設定
        transform.rotation = Camera.main.transform.rotation;
    }
}
