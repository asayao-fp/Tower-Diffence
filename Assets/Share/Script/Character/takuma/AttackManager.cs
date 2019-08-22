using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Effekseer;

public class AttackManager : MonoBehaviour
{
    public GameObject atkObj; //エフェクト
    public GameObject atkCollider; //当たり判定
    public Boolean isDebug;
    float time;


    void Start()
    {
        time = 0.0f;
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time > 5.0f){
            time = 0;
           
        }
    }

    public void Attack(){
         EffekseerEmitter ee = atkObj.GetComponent<EffekseerEmitter>();
            EffekseerEffectAsset ea = ee.effectAsset;
            ee.Play(ea);
            atkCollider.GetComponent<Animation>().Play();
    }

    void OnDrawGizmos()
    {
        if(isDebug){    
           // Gizmos.color = Color.green;
           // Gizmos.DrawSphere( transform.position, 0.1f );
           // Gizmos.DrawWireSphere( transform.position, Radius );
        }
    }
}
