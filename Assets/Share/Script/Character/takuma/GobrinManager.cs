using System.Collections;
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
          gstatus.hp = statue.hp;
        }
    }

    void Update()
    {
        if(gp.getStatus() != gp.NOW_GAME)return;


        if(hpbar != null){
            hpbar.fillAmount = gstatus.hp / (float)statue.hp;
        }
    }

    public override void Generate(Vector3 pos,Vector3 scale,StatueData f){
        if(gp == null){
          gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        }
    }

    public override void Dead(){
      Destroy(this.gameObject);
    }


    public override StatueData getSData(){
      return null;
    }

    public override GobrinData getGData(){
      return null;
    }

    public override void checkEnemy(){
      //NOP
    }
}
