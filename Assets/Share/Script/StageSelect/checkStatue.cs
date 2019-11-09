using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkStatue : MonoBehaviour
{

    GameSettings gs;
    public TextMeshProUGUI tp;
    public GameObject status4statue;
    public GameObject status4gobrin;

    public SkillNumManager snm;

    void Start()
    {
      gs = GameObject.FindWithTag("StaticObjects").GetComponent<GameSettings>();
      updateLayout(true);
    }

    public void updateLayout(bool isfirst){
      tp.text = gs.isStatue() ? "Statue" : "Gobrin";

      status4statue.SetActive(isfirst ? false : gs.isStatue());
      status4gobrin.SetActive(isfirst ? false : !gs.isStatue());

      snm.changeType();
        
    }
    public void setStatue(){
        gs.setStatue(!gs.isStatue());
        updateLayout(true);
    }
}
