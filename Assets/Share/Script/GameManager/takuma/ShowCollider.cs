using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;

public class ShowCollider : MonoBehaviour
{
  
   public bool isShow;

   private void Awake()
    {
       showCollider();
    }

    public void showCollider(){

      //  if(!isShow) return;

        var visualizer = gameObject.AddComponent<ColliderVisualizer>();
        var color      = ColliderVisualizer.VisualizerColorType.Blue;
        var message    = "";
        var fontSize   = 36;

        visualizer.Initialize( color, message, fontSize );
    }

    public void setShow(bool isshow){
        isShow = isshow;
    }
}
