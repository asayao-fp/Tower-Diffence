using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GenerateBarManager : MonoBehaviour
{

    public GameObject generatebar;
    private Boolean isGenerate; //設置しているか
    private Boolean canGenerate; //設置可能か
    public float generateTime; //設置してから次に召喚するまでの時間
    private float time;
    float fill;
    // Start is called before the first frame update
    void Start()
    {
      time = 0.0f;
      generatebar.GetComponent<Image>().fillAmount = 0;
      canGenerate = true;
      fill = generatebar.GetComponent<Image>().fillAmount;
      fill = 0;
    }

    // Update is called once per frame
    void Update()
    {
      if(!isGenerate)return;

      time += Time.deltaTime;
      fill = time / generateTime;


      if(fill >= 1){
        fill = 0;
        time = 0.0f;
        isGenerate = false;
        canGenerate = true;
      }

      generatebar.GetComponent<Image>().fillAmount = fill;
    }

    public Boolean getGenerate(){
      return canGenerate;
    }

    public void setGenerate(){
      isGenerate = true;
      canGenerate = false;
    }
}
