using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatueManager : FacilityManager
{
    protected float atkInterval; //攻撃間隔用
    protected List<GameObject> enemylist; //攻撃範囲内にいる敵のリスト
    protected GameObject targetEnemy; //ターゲットしてる敵オブジェクト

    void Awake()
    {
        isStatue = true;
        base.initMaterial(GameObject.Find("StaticManager").GetComponent<GameSettings>().getMaterial());
        isEnd = true;

    }

    void Update()
    {
        if (base.check())
        {
            return;
        }

        statueRoutine();
    }

    private void statueRoutine(){
        checkEnemy();

        float t = Time.deltaTime;
        gstatus.hp -= t;

        atkInterval += t;

        if (atkInterval >= statue.atkInterval)
        {
            atkInterval = 0;
            Attack();
        }

        if (hpbar != null)
        {
          hpbar.fillAmount = gstatus.hp / (float)statue.hp;
        }
    }

    // 召喚した時に初期化処理をやる
    public override void Generate(Vector3 pos,StatueData s,bool isai){
        base.Generate(pos,s,isai);

        atkInterval = 0.0f;
        enemylist = new List<GameObject>();

        base.setView(false);

        GameObject atkcheck = Instantiate(atkCheckEffect, pos, Quaternion.identity) as GameObject;
        atkcheck.transform.position = pos;
        atkcheck.transform.parent = this.gameObject.transform;
        atkcheck.transform.localScale = atkCheckEffect.transform.localScale;
        atkcheck.GetComponent<SphereCollider>().radius = s.attackpos.z;

        GameObject geneObj = Instantiate(generateEfect, transform.position, Quaternion.identity) as GameObject;
        geneObj.transform.parent = this.transform;
        geneObj.transform.localPosition = generateEfect.transform.position;
        geneObj.transform.localScale = generateEfect.transform.localScale;
        geneObj.transform.localRotation = generateEfect.transform.localRotation;
        ParticleSystem p = geneObj.GetComponent<ParticleSystem>();

        GameObject fieldobj = Instantiate(fieldEffect, transform.position, Quaternion.identity) as GameObject;
        fieldobj.transform.parent = this.transform;
        fieldobj.transform.localPosition = fieldEffect.transform.position;
        fieldobj.transform.localScale = fieldEffect.transform.localScale;
        fieldobj.transform.localRotation = fieldEffect.transform.localRotation;
        ParticleSystem ps = fieldobj.GetComponent<ParticleSystem>();

        ps.Play();
        p.Play();

        SoundManager.SoundPlay("facility_generate",this.gameObject.name);

        Invoke("setEnd", 1f);
        Invoke("setView", 0.5f);
    }
    
    //攻撃の種類によって出現位置とかを変える
    public override void Attack(){
        if (targetEnemy == null)
        {
            return;
        }
        isAttacking = true;
        GameObject atkobj = Instantiate(atkEffect,transform.position,Quaternion.identity) as GameObject;
        atkobj.GetComponent<AttackManager>().init(statue.attack);

        switch(atkName){
            case "shoot":
            case "laser":
            case "meteo":
            case "poison":
                atkobj.transform.parent = this.transform;
                atkobj.transform.localPosition = atkEffect.transform.position;
                atkobj.transform.localScale = atkEffect.transform.localScale;
                atkobj.transform.localRotation = atkEffect.transform.localRotation;
                break;
            case "thunder":
                atkobj.transform.position = targetEnemy.transform.position;
                atkobj.transform.localScale = atkEffect.transform.localScale;
                atkobj.transform.localRotation = atkEffect.transform.localRotation;
                break;
        }
        atkobj.GetComponent<AttackManager>().Attack(atkName);
    }


    public override void checkEnemy()
    {
        targetEnemy = null;
        GameObject[] objs = gp.getObjs();
        for(int i=0;i<objs.Length;i++){
            if(objs[i] == null) continue;
            if(!objs[i].tag.Equals("Statue")){
                float distance = Vector3.Distance(objs[i].transform.position,transform.position);
                if(distance <= (statue.attackpos.z * 0.2f)){
                    Quaternion lockRotation = Quaternion.LookRotation(objs[i].transform.position - transform.position, Vector3.up);
                    lockRotation.z = 0;
                    lockRotation.x = 0;
                    transform.rotation = Quaternion.Lerp(transform.rotation, lockRotation, 10f);
                    targetEnemy = objs[i];
                    break;
                }
            }
        }
    }

}