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

        int l = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,1);
        level.text = "level : " + l;
        yield return new WaitForSeconds (1.0f); 

        int esum = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);
        expsum.text = "expsum : " + esum;
        yield return new WaitForSeconds (1.0f); 

        int e = rd.getExp();
        exp.text = "exp : " + e;

        yield return new WaitForSeconds (1.0f); 


        exp.text = "exp : " + 0;
        esum = e + esum;
        expsum.text = "expsum : " + esum;
        PlayerPrefs.SetInt(UserData.USERDATA_EXP,esum);

        yield return new WaitForSeconds(1.0f);


        int afterl = GameObject.FindWithTag("StaticObjects").GetComponent<expData>().getLevel(esum);
        if(l != afterl){
            level.text = "levelUp! " + l + " -> " + afterl;
            PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,afterl);
        }
        FindObjectOfType<UserAuth>().saveData();

    }

    public void firstScene(){
      	SceneManager.LoadScene ("StageSelectScene");

    }
}
