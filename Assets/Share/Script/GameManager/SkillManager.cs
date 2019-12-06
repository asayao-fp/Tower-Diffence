using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    [SerializeField]
    private Image backimage;

    private int tapCount;
    [SerializeField]
    private int skillNum = 50; //スキルを発動するために必要な回数
    private int nowSkillNum;

    [SerializeField]
    private int skillMaxNum = 3; //１回のゲームで使用できるスキルの回数

    [SerializeField]
    GameProgress gp;

    GameProgress4Online gp4online;

    GameSettings gs;

    void Start()
    {

      GameObject stobj = GameObject.FindWithTag("StaticObjects");

      gs = stobj.GetComponent<GameSettings>();

      if(gs.getOnlineType()){
         gp4online = GameObject.FindWithTag("GameManager").GetComponent<GameProgress4Online>();
      }else{
          gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
      }
    }
    public void TapSkill(){

        if(gs.getOnlineType()){
            if(gp4online.addSkillCost(1)){
                backimage.enabled = false;
            }else{
                backimage.enabled = true;
            }
        }
        else{
            if(gp.addSkillCost(1)){
                backimage.enabled = false;;
            }else{
                backimage.enabled = true;
            }
        }
    }

    public void StartSkill(){
        if(gs.getOnlineType()){
            if(gp4online.doSkill()){
                backimage.enabled =true;
            }
        }else{
            if(gp.doSkill()){
                backimage.enabled = true;
            }
        }
    }
}
