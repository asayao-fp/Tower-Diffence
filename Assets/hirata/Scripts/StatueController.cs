using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueController : MonoBehaviour
{
    public int HP = 20;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name + "にがぶつかってきた。");
        if ((collider.gameObject.tag == Constants.GOBLIN_ATTACK_TAG) && (collider.gameObject.GetComponent<Weapon>() != null))
        {
            int enemyPower = collider.gameObject.GetComponent<Weapon>().Power;
            this.HP -= enemyPower;
            Debug.Log(HP);
        }
    }

}
