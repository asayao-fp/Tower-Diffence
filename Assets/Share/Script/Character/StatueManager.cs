using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatueManager : FacilityManager
{
    [SerializeField]
    protected StatueData statue; //自分のパラメータ
    protected gameStatus gstatus; //ゲーム中に変化するパラメータ

    protected float time; //攻撃間隔用
    protected float deletetime; //召喚時間用
    protected List<GameObject> enemylist; //攻撃範囲内にいる敵のリスト

    protected GameObject Gene; //召喚時のエフェクト用オブジェクト
    public GameObject Atk; //攻撃用のエフェクト
    protected Boolean isGene; //エフェクトを使用したかどうかのフラグ

    public ParticleSystem hiteffect;
    protected GameProgress gp;
    public Boolean isDebug;
    public Image hpbar;
    public String AtkName;
    public String ColName;
    public String GenName;
    public String FieldName;
    public String DeadName;
    [SerializeField]
    private GameObject[] viewModels;

    private int buttontype; //ボタンの位置

    private bool isAttacking; //攻撃中

    GameObject amm; //AttackManager check用
    GameObject obj;//敵

    GenerateBarManager gbm; //自分の召喚用ボタン

    [SerializeField]
    private bool notRotate;


    void Awake()
    {
        isStatue = true;
        Material material = GameObject.Find("StaticManager").GetComponent<GameSettings>().getMaterial();
        for (int i = 0; i < obj_materials.Length; i++)
        {
            obj_materials[i].material = material;
        }
        for (int i = 0; i < viewModels.Length; i++)
        {
            if (viewModels[i].gameObject.name.Equals("Canvas"))
            {
                viewModels[i].SetActive(false);
            }
        }
    }

    public override void init()
    {
        GameObject gameui = GameObject.Find("GameUI");
        foreach (Transform child in gameui.transform)
        {
            if (child.gameObject.name.Equals(this.gameObject.name))
            {
                gbm = child.gameObject.GetComponent<GenerateBarManager>();
            }
        }
    }

    public override void setAddStatus(AddStatus astatus){
        if(astatus == null) return;
        statue.hp += astatus.hp;
        statue.attack += astatus.attack;
        statue.atkInterval += (int)(astatus.speed * 0.15f);

        GameSettings.printLog("[StatueManager] name : " + astatus.name + " , hp : " + statue.hp + " , attack : " + statue.attack + " , speed : " + statue.atkInterval);
    }

    void Update()
    {
        if (check())
        {
            return;
        }

        if (gp.getStatus() != gp.NOW_GAME) return;

        float t = Time.deltaTime;
        deletetime += t;
        gstatus.hp -= t;

        time += t;

        checkEnemy();

        if (time >= statue.atkInterval)
        {
            time = 0;
            Attack(AtkName);
        }

        if (hpbar != null)
        {
          hpbar.fillAmount = gstatus.hp / (float)statue.hp;
        }
    }

    public override void checkEnemy()
    {
        obj = null;
        GameObject[] objs = gp.getObjs();
        for(int i=0;i<objs.Length;i++){
            if(!objs[i].tag.Equals("Statue")){
                float distance = Vector3.Distance(objs[i].transform.position,transform.position);
                if(distance <= (statue.attackpos.z * 0.2f)){
                    if(!notRotate){
                        Quaternion lockRotation = Quaternion.LookRotation(objs[i].transform.position - transform.position, Vector3.up);
                        lockRotation.z = 0;
                        lockRotation.x = 0;
                        transform.rotation = Quaternion.Lerp(transform.rotation, lockRotation, 10f);
                    }
                    obj = objs[i];
                    break;
                }
            }
        }
    }

    public override bool check()
    {
        return (amm != null) || InputManager.generating || isEnd;
    }
    public override void Attack()
    {

    }

    //攻撃の種類によって出現位置とかを変える
    private void Attack(String aName){
        if (obj == null)
        {
            return;
        }

        isAttacking = true;

        GameObject atkpre = (GameObject)ResourceManager.getObject("Attack/" + aName);
        GameObject atkobj = Instantiate(atkpre,transform.position,Quaternion.identity) as GameObject;
        atkobj.GetComponent<AttackManager>().init(statue.attack);
        Atk = atkobj;
        
        switch(aName){
            case "shoot":
            case "laser":
            case "meteo":
            case "poison":
                Atk.transform.parent = this.transform;
                Atk.transform.localPosition = atkpre.transform.position;
                Atk.transform.localScale = atkpre.transform.localScale;
                Atk.transform.localRotation = atkpre.transform.localRotation;
                break;
            case "thunder":
                Atk.transform.position = obj.transform.position;
                Atk.transform.localScale = atkpre.transform.localScale;
                Atk.transform.localRotation = atkpre.transform.localRotation;
                break;
        }
        Atk.GetComponent<AttackManager>().Attack(aName);

    }

    // 召喚した時に初期化処理をやる
    public override void Generate(Vector3 pos, Vector3 scale, StatueData s)
    {
        isEnd = true; //念の為
        setEnd(true);
        GameObject atkpre = (GameObject)Resources.Load("takuma/Prefabs/AtkCheck");
        GameObject atkcheck = Instantiate(atkpre, pos, Quaternion.identity) as GameObject;
        atkcheck.transform.position = pos;
        atkcheck.transform.parent = this.gameObject.transform;
        atkcheck.transform.localScale = atkpre.transform.localScale;
        atkcheck.GetComponent<SphereCollider>().radius = s.attackpos.z;


        hpbar.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
        hpbar.fillAmount = 1;
        setView(false);

        amm = GameObject.Find("AttackMakeManager");

        time = 0.0f;
        enemylist = new List<GameObject>();
        gp = GameObject.FindWithTag("GameManager").GetComponent<GameProgress>();

        Gene = ResourceManager.getObject("Other/" + GenName);
        GameObject geneObj = Instantiate(Gene, transform.position, Quaternion.identity) as GameObject;
        geneObj.transform.parent = this.transform;
        geneObj.transform.localPosition = Gene.transform.position;
        geneObj.transform.localScale = Gene.transform.localScale;
        geneObj.transform.localRotation = Gene.transform.localRotation;
        ParticleSystem p = geneObj.GetComponent<ParticleSystem>();

        GameObject obj = ResourceManager.getObject("Other/" + FieldName);
        GameObject fieldobj = Instantiate(obj, transform.position, Quaternion.identity) as GameObject;
        fieldobj.transform.parent = this.transform;
        fieldobj.transform.localPosition = obj.transform.position;
        fieldobj.transform.localScale = obj.transform.localScale;
        fieldobj.transform.localRotation = obj.transform.localRotation;
        ParticleSystem ps = fieldobj.GetComponent<ParticleSystem>();

        ps.Play();
        p.Play();

        deletetime = 0;
        gstatus.hp = statue.hp;

        Invoke("setEnd", 1f);
        Invoke("setView", 0.5f);

    }

    public override void EnemyOnArea(GameObject obj)
    {
        if (obj.CompareTag(Constants.GOBLIN_TAG) && !enemylist.Contains(obj))
        {
            enemylist.Add(obj);
        }
    }

    public override void setId(int id)
    {
        this.obj_num = id;
    }

    public override void addHP(int hp)
    {
        gstatus.hp += hp;

        if(gstatus.hp >= statue.hp){
            gstatus.hp = statue.hp;
        }

    }

    public override void Dead()
    {
        //消滅エフェクト実行
        setView(false);

        GameObject Gene = ResourceManager.getObject("Other/" + DeadName);
        GameObject geneObj = Instantiate(Gene, transform.position, Quaternion.identity) as GameObject;
        geneObj.transform.parent = this.transform;

        geneObj.transform.localPosition = Gene.transform.position;
        geneObj.transform.localScale = Gene.transform.localScale;
        geneObj.transform.localRotation = Gene.transform.localRotation;
        ParticleSystem p = geneObj.GetComponent<ParticleSystem>();
        p.Play();

        Destroy(this.gameObject, 2);

    }

    public void setEnd()
    {
        setEnd(false);
    }
    public void setEnd(bool isend)
    {
        isEnd = isend;
    }

    public void setView()
    {
        setView(true);
    }
    public void setView(bool isshow)
    {
        for (int i = 0; i < viewModels.Length; i++)
        {
            viewModels[i].gameObject.SetActive(isshow);
        }

    }

    public override StatueData getSData()
    {
        return statue;
    }

    public override GobrinData getGData()
    {
        return null;
    }

    public override gameStatus getStatus()
    {
        return gstatus;
    }

    public override float getHP()
    {
        return hpbar.fillAmount;
    }

    public override void attackEnd()
    {
        isAttacking = false;
    }

    public override void setNum(bool isgenerate)
    {
        gbm.setNum(isgenerate);
    }

}