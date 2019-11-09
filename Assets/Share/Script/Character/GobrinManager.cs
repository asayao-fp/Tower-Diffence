using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobrinManager : FacilityManager
{
    //AI用
    [SerializeField]
    private List<StageCostManager.lineList> line = new List<StageCostManager.lineList>();
    StageCostManager scm;
    Animator animator;
    [SerializeField]
    private int lineNum = 0;
    Vector3 target;
    public float speed = 2.0f;
    [SerializeField]
    private int nextPoint = 0;

    void Awake(){
      isStatue = false;
      base.initMaterial(GameObject.Find("StaticManager").GetComponent<GameSettings>().getMaterial());
      isEnd = true;
    }

    void Update()
    {
        if(base.check()){
          return;
        }

        goblinRoutine();
    }

    private void goblinRoutine(){

        Walk();

        if(hpbar != null)
        {
             hpbar.fillAmount = gstatus.hp / (float)statue.hp;
        }
    }
    public void Walk(){
        this.transform.LookAt(this.line[lineNum].List[nextPoint].transform);
        if (Vector3.Distance(transform.position, target) < 0.1)
        {
            if (this.line[lineNum].List.Count - 1 > nextPoint)
            {
                nextPoint++;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        target = this.line[lineNum].List[nextPoint].transform.position;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    public void setRoot(int num){
      //GetComponent<GobMane>().setRoot(num);
      lineNum = num;
    }   

    public override void Generate(Vector3 pos,StatueData f,bool isai){
        base.Generate(pos,f,isai);

        animator = this.gameObject.GetComponent<Animator>();
        animator.SetInteger("state", 1);
        GameObject obj = GameObject.FindWithTag("Stage");
        foreach(Transform child in obj.transform){
            if(child.gameObject.name.Equals("GoblinGenerator")){
                scm = child.GetComponent<StageCostManager>();
                line = scm.line;
                break;
            }
        }

        setEnd(false);
    }

    public override void Attack(){
      //とりあえずNOP
    }

    public override void Dead(){
      //とりあえずDestroyだけ
      Destroy(this.gameObject);
    }

    public override void checkEnemy(){
      //NOP
    }
}
