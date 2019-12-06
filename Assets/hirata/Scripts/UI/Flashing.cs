using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
    //点滅する周期
    public float speed = 1.0f;
    public float interval = 0.2f;

    private CanvasRenderer myCanvasRenderer;
    private float tmpTime = 0;
    private bool transparentFlag = false;
    private bool intervalFlag = false;

    void Start()
    {
        myCanvasRenderer = this.GetComponent<CanvasRenderer>();
        if (myCanvasRenderer == null)
        {
            Debug.LogError("CanvasRendererがnullです");
        }
    }

    void Update()
    {
        //アタッチされているオブジェクトにレンダラーがなかったら
        if (myCanvasRenderer == null)
        {
            return;
        }

        //インターバル中
        if (intervalFlag)
        {
            tmpTime += Time.deltaTime;

            if (interval <= tmpTime)
            {
                tmpTime = 0;
                intervalFlag = false;
            }

            return;
        }

        float alpha = myCanvasRenderer.GetAlpha();
        if (transparentFlag)
        {
            alpha -= Time.deltaTime * speed;
            myCanvasRenderer.SetAlpha(alpha);

            //完全に透明になったら濃くする
            if (alpha <= 0)
            {
                transparentFlag = false;
                intervalFlag = true;
            }
        }
        else
        {
            alpha += Time.deltaTime * speed;
            myCanvasRenderer.SetAlpha(alpha);

            if (alpha >= 1)
            {
                transparentFlag = true;
                intervalFlag = true;
            }
        }
    }
}
