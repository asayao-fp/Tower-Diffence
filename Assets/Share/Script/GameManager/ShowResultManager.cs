using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ShowResultManager : MonoBehaviour
{
    private TextMeshProUGUI result;
    private TextMeshProUGUI level;
    private TextMeshProUGUI exp;
    private TextMeshProUGUI expsum;
    private ResultData rd;
    void Start()
    {
        foreach(Transform child in this.transform){
            if(child.gameObject.name.Equals("result")){
                result = child.gameObject.GetComponent<TextMeshProUGUI>();
            }
            else if(child.gameObject.name.Equals("level")){
                level = child.gameObject.GetComponent<TextMeshProUGUI>();                
            }
            else if(child.gameObject.name.Equals("exp")){
                exp = child.gameObject.GetComponent<TextMeshProUGUI>();                
            }
            else if(child.gameObject.name.Equals("expsum")){
                expsum = child.gameObject.GetComponent<TextMeshProUGUI>();
            }
        }

        rd = GameObject.FindWithTag("StaticObjects").GetComponent<ResultData>();

        StartCoroutine("showResult");
    }

    void Update()
    {
        
    }

    public IEnumerator showResult(){
        result.text = "result : " + (rd.getResultType() ? "Win!!" : "Lose...");
        yield return new WaitForSeconds (1.0f); 

        level.text = "level : " + PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,0);
        yield return new WaitForSeconds (1.0f); 

        int esum = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);
        expsum.text = "expsum : " + esum;
        yield return new WaitForSeconds (1.0f); 

        int e = rd.getExp();
        exp.text = "exp : " + e;
        PlayerPrefs.SetInt(UserData.USERDATA_EXP,e + esum);
        yield return new WaitForSeconds (1.0f); 
    }

    public void firstScene(){
      	SceneManager.LoadScene ("StageSelectScene");

    }
}
