using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        //歩くアニメーションを設定
        animator.SetInteger("state", 1);
    }

    // Update is called once per frame
    void Update()
    {


    }
}