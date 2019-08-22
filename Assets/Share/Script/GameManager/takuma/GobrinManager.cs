﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobrinManager : StatueManager
{

    void Start()
    {
        time = 0.0f;
        enemylist = new List<GameObject>();

        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        if(hpbar != null){
            hpbar.transform.position = new Vector3(this.transform.position.x ,this.transform.position.y + 0.3f,this.transform.position.z);
            hpbar.fillAmount = 1;
        }

        if(isDebug){
          fs = GameObject.FindWithTag("StaticObjects").GetComponent<FacilitySetting>();
          fInfo = fs.getFacility("gobrin_1");
        }
    }

    void Update()
    {
        if(gp.getStatus() != gp.NOW_GAME)return;
        if(!isGenerate)return;

        fInfo = gp.getFM(obj_num,isDebug);

        if(hpbar != null){
            hpbar.fillAmount = (float)fInfo.hp / (float)fInfo.maxhp;
        }
    }

    /**
     * 召喚時に呼び出す
     * isGenerateをtrueにすることでUpdate関数が実行される
     */
    public override void Generate(Vector3 pos,Vector3 scale,Facility f){
        fInfo = f;

        if(gp == null){
          gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        }
        gp.Generate(this.gameObject);

        isGenerate = true;
    }

    /**
     * 死んだ時に呼ばれる
     * アニメーションとか？
     */
    public override void Dead(){
      Destroy(this.gameObject);
    }
}
