using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;
public class AttackVisualize : MonoBehaviour
{
    [SerializeField,Tooltip("可視Colliderの色")]
    private ColliderVisualizer.VisualizerColorType _visualizerColor;

    [SerializeField,Tooltip("メッセージ")]
    private string mes;
    [SerializeField,Tooltip("フォントサイズ")]
    private int fSize = 36;
  //  [SerializeField,Tooltip("オブジェクト")]
    private GameObject obj;
    private Collider collider;
    private GameSettings gs;
    private bool generating;
    void Start()
    {
        GameObject stobj = GameObject.FindWithTag("StaticObjects");
        gs = stobj.GetComponent<GameSettings>();
 
        obj = this.gameObject;
        collider = obj.GetComponent<Collider>();
        generating = false;
    }

    void Update()
    {
        if(!gs.showVisualizer){
            return;
        }
        checkVisualizer();
    }

    private void checkVisualizer(){

        if(!collider.enabled && generating){
            if(obj.GetComponent<ColliderVisualizer>() != null){
                Destroy(obj.GetComponent<ColliderVisualizer>());
            }
        }else if(collider.enabled && !generating){
            if(obj.GetComponent<ColliderVisualizer>() == null){
                generating = true;
                obj.AddComponent<ColliderVisualizer>().Initialize(_visualizerColor, mes, fSize);
            }
        }
    }
}
