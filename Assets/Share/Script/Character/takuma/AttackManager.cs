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
    public Boolean notRotate; //親オブジェクトと一緒に回転させないか

    [SerializeField]
    private int attacktype; //攻撃の種類

    void Start()
    {
        time = 0.0f;
        atkCollider.AddComponent<AttackObjManager>();
        atkCollider.GetComponent<AttackObjManager>().setType(attacktype);
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time > 5.0f){
            time = 0;
           
        }

        if(notRotate){
          gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    public void Attack(String name){
            ParticleSystem p = atkObj.GetComponent<ParticleSystem>();
            p.Play();
            atkCollider.GetComponent<Animator>().Play(name,-1,0);
    }

    void OnDrawGizmos()
    {
        if(isDebug){    
          //  Gizmos.color = Color.green;
          // Gizmos.DrawSphere( transform.position, 0.1f );
          // Gizmos.DrawWireSphere( transform.position, Radius );
        }
    }
}
