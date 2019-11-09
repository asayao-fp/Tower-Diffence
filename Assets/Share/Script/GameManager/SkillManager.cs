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


    void Start()
    {
      gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
    }
    public void TapSkill(){
        if(gp.addSkillCost(1)){
            backimage.enabled = false;;
        }else{
            backimage.enabled = true;
        }
    }

    public void StartSkill(){
        if(gp.doSkill()){
            backimage.enabled = true;
        }
    }
}
