using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawPosition : MonoBehaviour
{

    StagePosition sp;
    static Material mat;
    List<Vector3> vertexlist ;
    public Stage stage;
    public Boolean isShow;
    public int setType = 100;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<StagePosition>();
        stage = sp.getStageList(0);

         mat = new Material (Shader.Find ("Unlit/TestShader"));
         vertexlist = new List<Vector3>();
    }

    void OnRenderObject () {
      if(!isShow)return;

      mat.SetPass (0);

      GL.PushMatrix ();
      for(int i=0;i<stage.enablelist.Count;i++){
        //Debug.Log(" " + setType + " " + (int)stage.enablelist[i][4] + " " + (setType & (int)stage.enablelist[i][4]));
        //if(((int)stage.enablelist[i][4] & setType) == 0){
        if(stage.enablelist[i][4] > setType){
          continue;
        }
        GL.Begin (GL.QUADS);
        GL.Vertex(new Vector3(stage.enablelist[i][0],0,stage.enablelist[i][2]));
        GL.Vertex(new Vector3(stage.enablelist[i][0],0,stage.enablelist[i][3]));
        GL.Vertex(new Vector3(stage.enablelist[i][1],0,stage.enablelist[i][3]));
        GL.Vertex(new Vector3(stage.enablelist[i][1],0,stage.enablelist[i][2]));
        GL.End ();
      }
      GL.PopMatrix ();
    }
}
