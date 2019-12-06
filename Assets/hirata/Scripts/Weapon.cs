using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("武器の攻撃力")]
    private int power;

    public int Power
    {
        set
        {
            this.power = value;
        }

        get
        {
            return this.power;
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

    void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject.tag == Constants.CRYSTAL_TAG))
        {
            //もし敵に当たった場合、2回当たらないように当たり判定を消す。
            //Debug.Log("attata");
            collider.gameObject.GetComponent<CrystalManager>().AddHP(power);
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }
}
