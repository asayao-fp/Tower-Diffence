using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShowResultManager : MonoBehaviour
{
    private TextMeshProUGUI result;
    private TextMeshProUGUI level;
    private TextMeshProUGUI exp;
    private TextMeshProUGUI expsum;
    private ResultData rd;
    private UserAuth instance;
    public Image expBar;
    private expData ed;

    public Image[] items;
    public TextMeshProUGUI[] itemlabels;
    public GameObject imagePanel;
    void Start()
    {

        instance = GetComponent<UserAuth>();

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

        result.text = "";
        level.text = "";
        expsum.text = "";
        exp.text = "";
        expBar.fillAmount = 0;

        rd = GameObject.FindWithTag("StaticObjects").GetComponent<ResultData>();
        ed = GameObject.FindWithTag("StaticObjects").GetComponent<expData>();
        StartCoroutine("showResult");
    }

    void Update()
    {
        
    }

    public IEnumerator showResult(){

        result.text = "result : " + (rd.getResultType() ? "Win!!" : "Lose...");
        yield return new WaitForSeconds (1.0f); 


        int esum = PlayerPrefs.GetInt(UserData.USERDATA_EXP,0);

        expsum.text = "expsum : " + esum;
        yield return new WaitForSeconds (1.0f); 

        //int l = PlayerPrefs.GetInt(UserData.USERDATA_LEVEL,1);
        int l = ed.getLevel(esum);
        level.text = "level : " + l;
        yield return new WaitForSeconds (1.0f); 


        int e = rd.getExp();
        exp.text = "exp : " + e;

        yield return new WaitForSeconds (1.0f); 


        esum = e + esum; 
        PlayerPrefs.SetInt(UserData.USERDATA_EXP,esum);

        yield return new WaitForSeconds(1.0f);

        int tmpl = ed.getLevel(esum);
        int nokori = ed.getSumExp(tmpl+1) - esum;
        float nokoriper = (float)nokori/(float)ed.getSumExp(tmpl+1);
        expBar.fillAmount = nokoriper;

        if(l != tmpl){
            level.text = "levelUp! " + l + " -> " + tmpl;
        }else{
            level.text = "";
        }

        PlayerPrefs.SetInt(UserData.USERDATA_LEVEL,tmpl);

        FindObjectOfType<UserAuth>().saveData();

        yield return new WaitForSeconds(3.0f);

        UserBaggage ub = FindObjectOfType<UserBaggage>();

        this.gameObject.AddComponent<DropItemManager>();
        DropItemManager di = GetComponent<DropItemManager>();
        
        //ドロップの仕組みは考察必要
        int[] count = new int[]{0,0,0,0,0,0,0,0};
        for(int i=0;i<10;i++){
            int num = di.getDropItem1();
            GameSettings.printLog("[ShowResultManager]DropItem : " + num);
            ub.addItem(num,1);
            count[num]++;
        }


        for(int i=0;i<count.Length;i++){
            GameSettings.printLog("[ShowResultManager]DropItem " + i + " num -> " + count[i]);
        }

        Vector3 firstPos4image = new Vector3(-160,60,0);
        Vector3 firstPos4label = new Vector3(-30,50,0);
        int itemcount = 0;
        for(int i=0;i<itemlabels.Length;i++){
            if(count[i] == 0){
                itemlabels[i].gameObject.SetActive(false);
                items[i].gameObject.SetActive(false);
                continue;
            }
            items[itemcount].transform.localPosition = firstPos4image;
            itemlabels[itemcount].transform.localPosition = firstPos4label;
            itemlabels[itemcount].text = "X" + count[itemcount];
            itemcount++;
            if(itemcount > 3){
                firstPos4image.x = -160;
                firstPos4image.y -= 60;
                firstPos4image.x = -30;
                firstPos4label.y -= 60;
            }else{
                firstPos4image.x += 120;
                firstPos4label.x += 120;
            }

        }

        imagePanel.gameObject.SetActive(true);
    }

    public void firstScene(){
      	SceneManager.LoadScene ("StageSelectScene");

    }
}
