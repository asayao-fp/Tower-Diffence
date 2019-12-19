using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobrinManager : FacilityManager
{
    //AI用
    [SerializeField]
    private List<StageCostManager.lineList> line = new List<StageCostManager.lineList>();
    StageCostManager scm;
    public Animator animator;

    [SerializeField]
    private int lineNum = 0;

    Vector3 target;
    private float speed = 2.0f;

    [SerializeField]
    private int nextPoint = 0;

    private GameObject weapon;

    private bool isInEnemy = false;
    private Collider myEnemy = null;

    private string ANIMATION_NAME = "state";

    private GameSettings gs;
    private GameProgress4Online gp4Online;

    protected float atkInterval; //攻撃間隔用

    public int animationType = -1;

    void Awake()
    {
        Transform[] transformArray = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in transformArray)
        {
            //Debug.Log(child.name);
            if (child.gameObject.name.Equals("weapon"))
            {
                weapon = child.gameObject;
            }
        }
        isStatue = false;
        base.initMaterial(GameObject.Find("StaticManager").GetComponent<GameSettings>().getMaterial());
        isEnd = true;

        speed = statue.speed;
        gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();
        if(gs.getOnlineType()){
            gp4Online = GameObject.FindWithTag("GameManager").GetComponent<GameProgress4Online>();
        }
    }

    void Update()
    {
        if (base.check())
        {
            return;
        }

        goblinRoutine();
    }

    private void goblinRoutine()
    {
        checkEnemy();

        //敵が攻撃範囲に内にいる場合は移動しない
        if (isInEnemy)
        {
            if(gs.getOnlineType() && !gp4Online.isParent){
                //通信対戦で子だったら無視 
                return;
            }

            float t = Time.deltaTime;
            atkInterval += t;
            if((myEnemy != null) && (atkInterval >= statue.atkInterval)){
                animator.SetInteger(ANIMATION_NAME, 2);
                atkInterval = 0;
                animationType = 2;
            }else if((myEnemy != null) && (atkInterval < statue.atkInterval)){
                animator.SetInteger(ANIMATION_NAME, 1);
                animationType = 1;
            }
            return;
        }

        Walk();

        
        if (hpbar != null)
        {
            hpbar.fillAmount = gstatus.hp / (float)statue.hp;
        }
    }
    public void Walk()
    {

        if(gs.getOnlineType() && !gp4Online.isParent){
            //通信対戦で子だったら無視 
            return;
        }
        if(this.line[lineNum].List.Count  == nextPoint){
            //ゴール到達したら歩かない
            return;
        }
        this.transform.LookAt(this.line[lineNum].List[nextPoint].transform);
        if (Vector3.Distance(transform.position, target) < 0.1)
        {
            if (this.line[lineNum].List.Count - 1 > nextPoint)
            {
                nextPoint++;
            }
            else
            {
              //  Destroy(this.gameObject);
            }
        }
        target = this.line[lineNum].List[nextPoint].transform.position;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    public void setRoot(int num)
    {
        //GetComponent<GobMane>().setRoot(num);
        lineNum = num;
    }

    public override void Generate(Vector3 pos, StatueData f, bool isai)
    {
        base.Generate(pos, f, isai);

        animator = this.gameObject.GetComponent<Animator>();
        animator.SetInteger(ANIMATION_NAME, 1);
        GameObject obj = GameObject.FindWithTag("Stage");
        foreach (Transform child in obj.transform)
        {
            if (child.gameObject.name.Equals("GoblinGenerator"))
            {
                scm = child.GetComponent<StageCostManager>();
                line = scm.line;
                break;
            }
        }

        setEnd(false);
    }

    public override void Attack()
    {
        //とりあえずNOP
    }

    public override void Dead()
    {
        //とりあえずDestroyだけ
        Destroy(this.gameObject);
    }

    public override void checkEnemy()
    {
        if (myEnemy == null && isInEnemy)
        {
            OutAttackRange();
        }

    }

    public void InAttackRange(Collider collider)
    {
        if(collider.gameObject.name.Equals("crystal")){
            isInEnemy = true;
            myEnemy = collider;

            
            //Debug.Log(collider.gameObject.name + "が攻撃範囲に入った。");
        }
    }

    public void OutAttackRange()
    {
        isInEnemy = false;
        animator.SetInteger(ANIMATION_NAME, 1);
        atkInterval = 0;
        //Debug.Log("攻撃範囲からいなくなった。");
    }

    //アニメーションで呼ばれる
    public void AttackStart()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

    //アニメーションで呼ばれる
    public void AttackEnd()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        InAttackRange(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        //オブジェクトがDestroyされて検知されなくなった場合は、colliderがなくなるため、OnTriggerExitが呼ばれない。
        //そのため、メンバ変数で管理する。
        myEnemy = null;
    }
}
