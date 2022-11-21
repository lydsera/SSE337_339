using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT : MonoBehaviour
{
    private int [,]chess = {{0,0,0},{0,0,0},{0,0,0}};
    private int round=1;
    // Start is called before the first frame update
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI() 
    {
        GUI.Label(new Rect(Screen.width / 2 -15, 60 , 100, 100), "井字棋");
        
        if(GUI.Button(new Rect(Screen.width/2-30,Screen.height/2+110,70,30),"重新开始"))
        {
            Restart();
        }
        int c = check();
        // Debug.Log(c);
        if(c==1)
        {
            GUI.Label(new Rect(Screen.width / 2 -5, 80 , 100, 100), "O赢");
        }
        else if(c==2)
        {
            GUI.Label(new Rect(Screen.width / 2 -5, 80 , 100, 100), "X赢");
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2 -20, 80 , 100, 100), (round==1?"O":"X")+"的回合");
        }
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if(chess[i,j]==0)
                {
                    if(GUI.Button(new Rect(Screen.width/2+(i-1)*50-20,Screen.height/2+(j-1)*50,50,50),"")&&c==0)
                    {
                        chess[i,j]=round;
                        round=round==1?2:1;
                    }
                }
                else if(chess[i,j]==1) GUI.Button(new Rect(Screen.width/2+(i-1)*50-20,Screen.height/2+(j-1)*50,50,50),"O");
                else if(chess[i,j]==2) GUI.Button(new Rect(Screen.width/2+(i-1)*50-20,Screen.height/2+(j-1)*50,50,50),"X");
            }
        }
    }
    int check()
    {
        
        //行
        for(int i=0;i<3;i++)
        {
            //chess[i,0]!=0很容易忽略，可能有两列，一列空，一列全O或全X，但就是赢不了
            if((chess[i,0]==chess[i,1])&&(chess[i,0]==chess[i,2])&&chess[i,0]!=0) {
                // Debug.Log(chess[i,0]);
                return chess[i,0];
                
            }
        }
        //列
        for(int i=0;i<3;i++)
        {
            if((chess[0,i]==chess[1,i])&&(chess[0,i]==chess[2,i])&&chess[0,i]!=0) return chess[0,i];
        }
        //对角线
        if((chess[0,0]==chess[1,1])&&(chess[0,0]==chess[2,2])) return chess[0,0];
        if((chess[0,2]==chess[1,1])&&(chess[0,2]==chess[2,0])) return chess[0,2];
        return 0;
    }
    void Restart()
    {
        round=1;
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                chess[i,j]=0;
            }
        }
    }
}
