using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;

public class ShowCollider : MonoBehaviour
{
   private void Awake()
    {
        var visualizer = gameObject.AddComponent<ColliderVisualizer>();
        var color      = ColliderVisualizer.VisualizerColorType.Red;
        var message    = "";
        var fontSize   = 36;

        visualizer.Initialize( color, message, fontSize );
    }
}
