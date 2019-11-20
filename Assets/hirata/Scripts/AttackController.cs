using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private bool myTrriger = false;
    private Collider myEnemy = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myEnemy == null && myTrriger)
        {
            gameObject.GetComponentInParent<GobMane>().OutAttackRange();
            myTrriger = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        myTrriger = true;
        gameObject.GetComponentInParent<GobMane>().InAttackRange(collider);
        myEnemy = collider;
    }

    void OnTriggerExit(Collider collider)
    {
        //オブジェクトがDestroyされて検知されなくなった場合は、colliderがなくなるため、OnTriggerExitが呼ばれない。
        //そのため、メンバ変数で管理する。
        myEnemy = null;
    }
}
