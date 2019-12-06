using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdvertManager : MonoBehaviour
{
/*    public void advertisementShow()
    {
        /*
        video:デフォルト。5秒後にスキップ可
        rewardedVideo:スキップ不可。
        */
        if (Advertisement.IsReady("video"))
        {
            ShowOptions options = new ShowOptions
            {
                resultCallback = Result
            };

            Advertisement.Show("video", options);
        }
    }


    private void Result(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("広告表示成功");
                // ここで報酬をユーザーに付与する
                break;
            case ShowResult.Skipped:
                Debug.Log("スキップされました");
                break;
            case ShowResult.Failed:
                Debug.LogError("失敗しました。");
                break;
        }
    }*/
}
