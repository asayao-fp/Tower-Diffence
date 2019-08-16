using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

public class StageSetting : MonoBehaviour
{
    private List<Stage> stagelist = new List<Stage>();
    public Boolean isText;
    // Start is called before the first frame update
    void Awake()
    {
      DontDestroyOnLoad(this);
      setStage();
    }

    public Stage getStageList(int num){
      return stagelist[num];
    }

    public void setStage(){
      StreamReader sr = new StreamReader(System.IO.Path.GetFullPath("./Assets/takuma/Stage.txt"));
      System.Text.Encoding.GetEncoding("shift_jis");
      int num = 0;
      Boolean add = false;
      Stage s = null;
      while(sr.Peek() != -1) {
          String line = sr.ReadLine();
          add = false;
          // Console.WriteLine(line);
          if (line.StartsWith("stageid"))
          {
              num = int.Parse(line.Substring(8));
              s = new Stage();
              s.stageid = num;
          }else if(line.StartsWith("stagename")){
              s.stagename = line.Substring(10);
          }else if (line.StartsWith("stageendid"))
          {
              //stageidと同じじゃなければ追加しない
              if(int.Parse(line.Substring(11)) == num) {
                  add = true;
              }
          }else if (line.Equals("setpositionstart"))
          {
              String field = sr.ReadLine();
              int l = field.IndexOf(",");
              int fieldx = int.Parse(field.Substring(9, l - 9)); //stageのxの数*100
              int fieldz = int.Parse(field.Substring(l + 1, field.Length - (l + 1))); //stageのzの数*10000

              s.enablemap = new float[fieldx][];
              s.enablelist = new List<float[]>();

              for(int i = 0; i < s.enablemap.Length; i++)
              {
                  s.enablemap[i] = new float[fieldz]; //0.01指定できる
                  for(int j = 0; j < s.enablemap[i].Length; j++)
                  {
                      s.enablemap[i][j] = 0; // 初期化
                  }
              }

              int stagenum = int.Parse(sr.ReadLine().Substring(4));

              for(int i = 0; i < stagenum; i++)
              {
                  String posstr = sr.ReadLine();
                  String xrange = posstr.Substring(3, posstr.IndexOf(",") - 3);
                  String zrange = posstr.Substring(posstr.IndexOf(",") + 1, posstr.IndexOf(":") - (posstr.IndexOf(",") + 1));
                  int value = int.Parse(posstr.Substring(posstr.LastIndexOf("=")+1)); //座標の値
                  float sx = float.Parse(xrange.Substring(0, xrange.IndexOf("~"))); //xの開始座標
                  float ex = float.Parse(xrange.Substring(xrange.IndexOf("~") + 1));//xの終了座標
                  float sz = float.Parse(zrange.Substring(0, zrange.IndexOf("~")));//zの開始座標
                  float ez = float.Parse(zrange.Substring(zrange.IndexOf("~") + 1)); //zの終了座標
                  float[] ssee = new float[]{sx,ex,sz,ez,value};
                  s.enablelist.Add(ssee);
                  for(int j= (int)(sx * 100); j < (int)(ex * 100); j++)
                  {
                      for (int k = (int)(sz * 100); k < (int)(ez * 100); k++)
                      {

                          s.enablemap[j][k] = value;
                      }
                  }
              }
          }

          if (add)
          {
              if(isText){
                //テキスト出力
                StreamWriter maptxt = new StreamWriter("./Assets/takuma/" + s.stagename + ".txt",false);
                for(int i = s.enablemap.Length - 1; i > 0; i--)
                {
                    for(int j = 0; j < s.enablemap[i].Length ; j++)
                    {
                          maptxt.Write(s.enablemap[i][j]);
                    }
                    maptxt.WriteLine("");
                }
                maptxt.Flush();
                maptxt.Close();
              }
              stagelist.Add(s);
          }
      }

      sr.Close();
      Console.ReadKey();
    }


}


public class Stage{
  public int stageid;
  public String stagename = "";
  public float[][] enablemap; //施設を設置できる位置を格納
  public List<float[]> enablelist; //施設を設置できる位置のリスト
}
