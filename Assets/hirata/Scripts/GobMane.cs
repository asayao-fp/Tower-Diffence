using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobMane : MonoBehaviour
{
    [SerializeField]
    private int lineNum = 0;
    Vector3 target;
    public float speed = 2.0f;
    [SerializeField]
    private int nextPoint = 0;

    Animator animator;

    [System.SerializableAttribute]
    public class lineList
    {
        public List<GameObject> List = new List<GameObject>();

        public lineList(List<GameObject> list)
        {
            List = list;
        }
    }

    [SerializeField]
    public List<StageCostManager.lineList> line = new List<StageCostManager.lineList>();


    StageCostManager scm;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame


    void Update()
    {
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

    public void setLine(int lineNum)
    {
        this.lineNum = lineNum;
    }

    public void setRoot(int root){
        lineNum = root;

    }
}
