using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillStatus : MonoBehaviour
{
    public TextMeshProUGUI hptext;
    private int hpnum;
    public TextMeshProUGUI attacktext;
    private int attacknum;
    public TextMeshProUGUI speedtext;
    private int speednum;
    private int level;
    private SkillNumManager snm;

    public TextMeshProUGUI nametext;

    void Start()
    {
        hpnum = 0;
        attacknum = 0;
        speednum = 0;

        snm = GameObject.Find("init").GetComponent<SkillNumManager>();
        updateLayout();
    }


    public void updateLayout(){
        hptext.text = "hp+" + hpnum;
        attacktext.text = "attack+" + attacknum;
        speedtext.text = "speed+" + speednum;
    }
    public void upSkill(int num){
        if(snm.checkStatus(1)){
            snm.addStatus();
            if(num == 1){
                hpnum ++;
            }else if(num == 2){
                attacknum++;
            }else if(num == 3){
                speednum ++;
            }
        }else{
        }
 
        updateLayout();

    }

    public void downSkill(int num){
        if(snm.checkStatus(2)){
            bool minus = false;
            if(num == 1 && 0 < hpnum){
                hpnum --;
                minus = true;
            }else if(num == 2 && 0 < attacknum){
                attacknum--;
                minus = true;
            }else if(num == 3 && 0 < speednum){
                speednum --;
                minus = true;
            }else {
            }
            if(minus){
                snm.minusStatus();
            }
        }else{
        }
       


        updateLayout();
    }

    public void setName(string name){
        nametext.text = name;
    }

    public AddStatus GetStatus(){
        AddStatus a = new AddStatus();
        a.name = nametext.text;
        a.hp = hpnum;
        a.attack = attacknum;
        a.speed = speednum;

        GameSettings.printLog("GetStatus : " + a.name + " " + a.hp + " " + a.attack + " " + a.speed);
        return a;
    }
}
