using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;
using UnityEngine.UI;

public class CrystalManager : MonoBehaviour
{
    protected GameProgress gp;
    [SerializeField]
    private int hp = 50;
    [SerializeField]
    private Image hpbar;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private String deadName;
    [SerializeField]
    private GameObject[] viewModels;

    [SerializeField]
    private int nowHP = 0;

    private float time;

    private GameSettings gs;

    void Start()
    {
        GameObject stobj = GameObject.FindWithTag("StaticObjects");
        gs = stobj.GetComponent<GameSettings>();

        nowHP = hp;
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();
        time = 0.0f;

        GameObject obj = GameObject.Find("GameUI");
        foreach (Transform child in obj.transform)
        {
            if (child.gameObject.name.Equals("progress"))
            {
                hpbar = child.GetComponent<ObjectReference>().objects[0].GetComponent<Image>();
            }
        }

    }

    void Update()
    {
        if (gp.getStatus() != gp.NOW_GAME) return;

        //time += Time.deltaTime;
        if(gs.getOnlineType() && !((GameProgress4Online)gp).isParent)return;

        hpbar.fillAmount = ((float)nowHP / (float)hp);

    }

    public void Dead()
    {
        canvas.SetActive(false);

        for (int i = 0; i < viewModels.Length; i++)
        {
            viewModels[i].gameObject.SetActive(false);
        }
        GameObject deadobj = ResourceManager.getObject("Other/" + deadName);
        GameObject obj = Instantiate(deadobj, transform.position, Quaternion.identity) as GameObject;

        obj.transform.parent = this.transform;

        obj.transform.localPosition = deadobj.transform.position;
        obj.transform.localScale = deadobj.transform.localScale;
        obj.transform.localRotation = deadobj.transform.localRotation;
        ParticleSystem p = obj.GetComponent<ParticleSystem>();
        p.Play();

        Destroy(this.gameObject, 2);

    }

    public void AddHP(int hp)
    {
        if(gs.getOnlineType() && !((GameProgress4Online)gp).isParent){
          return;
        }
        this.nowHP -= hp;
    }

    public void setHP(float hp){
        hpbar.fillAmount = hp;
    }
    public float getHP()
    {
        //return 1;
        return hpbar.fillAmount;
    }
}