
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1.UIシステムを使うときに必要なライブラリ
using UnityEngine.UI;
// 2.Scene関係の処理を行うときに必要なライブラリ
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStageSelect1()
    {
        SceneManager.LoadScene("stage1-1");
    }

    public void OnStageSelect2()
    {
        SceneManager.LoadScene("stage2");
    }

    public void OnStageSelect3()
    {
        SceneManager.LoadScene("stage3");
    }
}