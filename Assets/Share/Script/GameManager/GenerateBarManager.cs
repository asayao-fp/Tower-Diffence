using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GenerateBarManager : MonoBehaviour
{
    private StatueData sData;
    private GobrinData gData;
    public StatueManager sm;
    public GobrinManager gm;


    private Image blackImage; //召喚不可能時に表示するテクスチャ
    private GameObject generatebar;
    private TextMeshProUGUI setnum; //１体ごとの設置可能数
    private Boolean isGenerate; //設置しているか
    private Boolean canGenerate; //設置可能か
    public float generateTime; //設置してから次に召喚するまでの時間
    private float time;
    float fill;
    int setnownum; //現在の設置数
    GameProgress gp;

    void Start()
    {
        time = 0.0f;
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();

        sData = sm.getSData();

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Equals("Generate"))
            {
                generatebar = child.gameObject;
            }
            else if (child.gameObject.name.Equals("setnum"))
            {
                setnum = child.gameObject.GetComponent<TextMeshProUGUI>();
            }
            else if (child.gameObject.name.Equals("notset"))
            {
                blackImage = child.gameObject.GetComponent<Image>();
            }

        }
        generatebar.GetComponent<Image>().fillAmount = 0;
        canGenerate = true;
        fill = generatebar.GetComponent<Image>().fillAmount;
        fill = 0;

        setnownum = 0;
        setnum.text = sData.maxsetNum + "/" + setnownum;
        generateTime = sData.generateInterval;
    }

    void Update()
    {
        if (gp.getStatus() != gp.NOW_GAME) return;
        if (!isGenerate) return;

        time += Time.deltaTime;
        fill = time / generateTime;


        if (fill >= 1)
        {
            fill = 0;
            time = 0.0f;
            isGenerate = false;
            canGenerate = true;
        }
        else
        {
            canGenerate = false;
        }


        generatebar.GetComponent<Image>().fillAmount = fill;
    }

    public Boolean getGenerate()
    {
        return canGenerate;
    }

    public void setGenerate()
    {
        isGenerate = true;
        canGenerate = false;
    }

    public StatueData getStatus()
    {
        return sData;
    }

    public Image getBlackImage()
    {
        return blackImage;
    }

    //今設置できるか
    public bool checkSet()
    {
        return sData.maxsetNum > setnownum;
    }
    //最大設置可能数と現在設置されてる数を更新
    public void setNum(bool isgenerate)
    {
        setnownum += isgenerate ? 1 : -1;
        setnum.text = sData.maxsetNum + "/" + setnownum;
    }
}
