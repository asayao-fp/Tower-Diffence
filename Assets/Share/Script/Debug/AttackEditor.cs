using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;
using Effekseer.Editor;
using HC.Debug;
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
        private bool systemInitialized;
   		private double lastTime;
        public float posx,posy,posz,rotx,roty,rotz;
        public float efx,efy,efz,efrotx,efroty,efrotz;
		private bool loop;

        void OnEnable()
        {
            atkeditor = (AttackEditor)target;
            

        }

        void OnDisable()
        {
            TermSystem();

        }

        void TermSystem(){
			if (EffekseerEditor.instance.inEditor) {
				EditorApplication.update -= Update;
				atkeditor.StopImmediate();
				EffekseerEditor.instance.TermSystem();
			}
			systemInitialized = false;
        }

        void InitSystem(){

            if(atkeditor != null){
            //   atkeditor.effectAsset = atkeditor.AtkObj.GetComponent<EffekseerEmitter>().effectAsset;
            }
            if (EffekseerEditor.instance.inEditor) {
				EffekseerEditor.instance.InitSystem();
				EffekseerSystem.Instance.LoadEffect(atkeditor.effectAsset);
				lastTime = EditorApplication.timeSinceStartup;
				EditorApplication.update += Update;
			}
			systemInitialized = true;            
        }

        void Update()
        {
            if(!EnableUpdate)
                return;

	        double currentTime = EditorApplication.timeSinceStartup;
			float deltaTime = (float)(currentTime - lastTime);
			float deltaFrames = Utility.TimeToFrames(deltaTime);
			lastTime = currentTime;
			
			if (atkeditor.exists) {
				RepaintEffect();
			} else if (loop) {
				atkeditor.Play();
			}

			foreach (var handle in atkeditor.handles) {
				handle.UpdateHandle(deltaFrames);
			}
			atkeditor.Update();


            Animation animation = atkeditor.AtkCollider.GetComponent<Animation>();
          //  Debug.Log("animation : " + animation["MagicCollider"].);
             atkeditor.AtkCollider.GetComponent<ColliderVisualizer>().LateUpdate();

        }

        void LateUpdate(){
        }


		void RepaintEffect()
		{
			SceneView.RepaintAll();
			var assembly = typeof(EditorWindow).Assembly;
			var type = assembly.GetType("UnityEditor.GameView");
			var gameview = EditorWindow.GetWindow(type);
			gameview.Repaint();
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
                if (systemInitialized == false) {
					InitSystem();
				}
                atkeditor.Play();
            }

            if (GUILayout.Button("Stop")){

            }
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Collider");
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            posx = EditorGUILayout.FloatField("pos X", posx);
            rotx = EditorGUILayout.FloatField("rot X",rotx);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            posy = EditorGUILayout.FloatField("pos Y", posy);
            rotx = EditorGUILayout.FloatField("rot Y",rotx);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            posz = EditorGUILayout.FloatField("pos Z", posz);
            rotx = EditorGUILayout.FloatField("rot Z",rotx);

			GUILayout.EndHorizontal();
        
            GUILayout.BeginHorizontal(); //Collider 
			if (GUILayout.Button("Collider Scale")) {
                atkeditor.SetColliderScale(posx,posy,posz);
            }
			GUILayout.EndHorizontal();

            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Effekseer");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            efx = EditorGUILayout.FloatField("pos X", efx);
            efrotx = EditorGUILayout.FloatField("rot X",rotx);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            efy = EditorGUILayout.FloatField("pos Y", efy);
            efrotx = EditorGUILayout.FloatField("rot Y",rotx);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            efz = EditorGUILayout.FloatField("pos Z", efz);
            efrotx = EditorGUILayout.FloatField("rot Z",rotx);

			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); //Effekseer 
			if (GUILayout.Button("Effekseer Scale")) {
                atkeditor.SetEffekseerScale(efx,efy,efz);
            }
			GUILayout.EndHorizontal();

            GUILayout.EndArea();

            Handles.EndGUI();
        }

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}

  
    }




    public class AttackEditor : EffekseerEmitter{
        public GameObject AtkCollider;
        public GameObject AtkObj;

        public void Play(){
            AtkCollider.GetComponent<ShowCollider>().showCollider();
            Animation animation = AtkCollider.GetComponent<Animation>();
            animation.Play ();
            
            Play(effectAsset);
        }

        public void SetColliderScale(float x,float y,float z){
            Debug.Log("Collider " + x + " " + y + " " + z);
            Vector3 pos = new Vector3(x,y,z);
            AtkCollider.transform.localPosition = pos;
        }

        public void SetEffekseerScale(float x,float y,float z){
            Debug.Log("Effekseer " + x + " " + y + " " + z);
            Vector3 pos = new Vector3(x,y,z);
            AtkObj.transform.localPosition = pos;
        }

    }
}

#endif