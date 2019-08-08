using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour, IAttackController
{
    [Tooltip("武器のヒットボックス")]
    [SerializeField]
    private GameObject hitBox;

    public Transform goal;
    Animator animator;
    UnityEngine.AI.NavMeshAgent agent;

    string animationStateName = "state";

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goal.position);

        animator = this.gameObject.GetComponent<Animator>();
        //歩くアニメーションを設定
        animator.SetInteger(animationStateName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, goal.position);
        if (dist <= 1)
        {
            Debug.Log("目的地に到着");
            animator.SetInteger(animationStateName, 0);
            Invoke("destoyGoblin", 3f);
        }
    }

    void destoyGoblin()
    {
        Destroy(this.gameObject);
    }

    public void InAttackRange(Collider collider)
    {
        animator.SetInteger(animationStateName, 2);
        agent.isStopped = true;
        Debug.Log(collider.gameObject.name + "が攻撃範囲に入った。");
    }

    public void OutAttackRange()
    {
        Debug.Log("攻撃範囲からいなくなった。");
        animator.SetInteger(animationStateName, 1);
        agent.isStopped = false;
    }

    public GameObject HitBox
    {
        set
        {
            this.hitBox = value;
        }

        get
        {
            return this.hitBox;
        }
    }

    public void AttackStart()
    {
        HitBox.GetComponent<BoxCollider>().enabled = true;
    }

    public void AttackEnd()
    {
        HitBox.GetComponent<BoxCollider>().enabled = false;
    }
}
