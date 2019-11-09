using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGoblin : MonoBehaviour
{
    bool isStatueExist = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //オブジェクトが衝突したとき
    void OnTriggerEnter(Collider other)
    {
        if (isStatueExist)
            return;

        //親オブジェクト取得
        GameObject parentObject = transform.parent.gameObject;

        //スタチュー生成位置から、生成要求
        parentObject.GetComponent<StatueAI>().GenerateStatue(100, this.transform.position, this.gameObject);
    }

    public void setStatueExist(bool value)
    {
        this.isStatueExist = value;
    }
}
