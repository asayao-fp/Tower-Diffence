using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace tk_project
{

    /// <summary xml:lang="ja">
    /// 攻撃を再生できる
    /// 当たり判定を可視化して見れる
    /// 当たり判定の位置を編集できる
    /// </summary>
    [CustomEditor(typeof(AttackEditor))]
    public class AttackEditorManager : UnityEditor.Editor
    {

        private AttackEditor atkeditor;
        public static bool EnableUpdate = true;
        public float posx,posy,posz;
        public float efx,efy,efz;
        void OnEnable()
        {
            atkeditor = (AttackEditor)target;
        }

        void OnDisable()
        {

        }

        void Update()
        {
            if(!EnableUpdate)
                return;
        }
      
        void OnSceneGUI()
        {

            if(atkeditor == null)
                return;

   			if (!atkeditor.isActiveAndEnabled)
                return;

            SceneView sceneView = SceneView.currentDrawingSceneView;
            Handles.BeginGUI();

            float screenWidth  = sceneView.camera.pixelWidth;
			float screenHeight = sceneView.camera.pixelHeight;
			
			float width = 220;
			float height = 190;
			var boxRect  = new Rect(10, screenHeight - height - 45, width + 20, height + 40);
			var areaRect = new Rect(20, screenHeight - height - 20, width, height);

			GUI.Box(boxRect, "AttackEditor");
			GUILayout.BeginArea(areaRect);
           
            GUILayout.BeginHorizontal(); //横に表示？？
            
			if (GUILayout.Button("Play")) {
                atkeditor.Play();
            }

            if (GUILayout.Button("Stop")){

            }
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
            posx = EditorGUILayout.FloatField("Collider X", posx);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            posy = EditorGUILayout.FloatField("Collider Y", posy);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            posz = EditorGUILayout.FloatField("Collider Z", posz);
			GUILayout.EndHorizontal();
        
            GUILayout.BeginHorizontal(); //Collider 
			if (GUILayout.Button("Collider Scale")) {
                atkeditor.SetColliderScale(posx,posy,posz);
            }
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            efx = EditorGUILayout.FloatField("Effekseer X", efx);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            efy = EditorGUILayout.FloatField("Effekseer Y", efy);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            efz = EditorGUILayout.FloatField("Effekseer Z", efz);
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); //Effekseer 
			if (GUILayout.Button("Effekseer Scale")) {
                atkeditor.SetEffekseerScale(efx,efy,efz);
            }
			GUILayout.EndHorizontal();

            GUILayout.EndArea();

            Handles.EndGUI();
        }
  
    }


    public class AttackEditor : MonoBehaviour{
        public GameObject AtkCollider;
        public GameObject AtkObj;
        public void Play(){
            Debug.Log("play");
        }

        public void SetColliderScale(float x,float y,float z){
            Debug.Log("Collider " + x + " " + y + " " + z);
            Vector3 pos = new Vector3(x,y,z);
            AtkCollider.transform.position = pos;
        }

        public void SetEffekseerScale(float x,float y,float z){
            Debug.Log("Effekseer " + x + " " + y + " " + z);
            Vector3 pos = new Vector3(x,y,z);
            AtkObj.transform.position = pos;
        }

    }
}

#endif