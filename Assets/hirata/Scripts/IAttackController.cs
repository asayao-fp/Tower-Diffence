using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
実装クラスは、AnimatorがアタッチされているGameObjectにアタッチすること。
アニメーションのArttackStartとAttackEndが呼ばれないため。
 */
public interface IAttackController
{
    //武器の攻撃範囲。実装クラスで[SerializeField]をつけて、Inspectorから設定する想定。
    GameObject HitBox
    {
        get;
        set;
    }

    //アニメーションイベントに設定する関数。攻撃アニメーションの開始時。
    void AttackStart();

    //アニメーションイベントに設定する関数。攻撃アニメーションの終了時。
    void AttackEnd();
}
