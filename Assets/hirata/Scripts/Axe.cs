using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IAttackController
{
    [Tooltip("武器のヒットボックス")]
    [SerializeField]
    private GameObject hitBox;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
