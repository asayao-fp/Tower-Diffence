using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUsedSound : MonoBehaviour
{

    float time = 0;
    void Start()
    {
      DontDestroyOnLoad(this);
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time > 10.0f){
            time = 0;
            checkSound();
        }
    }

    private void checkSound(){
        GameObject[] soundobjs = GameObject.FindGameObjectsWithTag("sound");
        for(int i=0;i<soundobjs.Length;i++){
            if(soundobjs[i].GetComponent<CriAtomSource>().status == CriAtomSource.Status.PlayEnd){
                Destroy(soundobjs[i]);
            }
        }
    }
}
