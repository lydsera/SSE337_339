using UnityEngine;
using System.Collections;

public class TTT : MonoBehaviour {
	//用二位数组存棋盘，0代表空，1代表O，2代表O
	private int[,] chess = new int[3,3] {{0,0,0},{0,0,0},{0,0,0}};
	//0是O的回合，1是X的回合
	private int turn = 0;
    private int res=0;
	void Start () {
		
	}
    
	//OnGUI会自动刷新
	void OnGUI() {
        //x,y,w,h
		if (GUI.Button(new Rect(310,300,100,50),"重新开始"))  reset();
		
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
                
				if (chess [i,j] == 1) {
					
					GUI.Button (new Rect (70 * i+250, 70 * j, 70, 70), "O");
					Debug.Log(i+" "+j+":"+chess[i,j]);
					res=check();
					Debug.Log(res);
				} else if (chess [i,j] == 2) {
					GUI.Button (new Rect (70 * i+250, 70 * j, 70, 70), "X");
					Debug.Log(i+" "+j+":"+chess[i,j]);
					res=check();
					Debug.Log(res);
				} else {
					if (GUI.Button (new Rect (70 * i+250, 70 * j, 70, 70), "")) {
                        
						if (res == 0) {
							if (turn == 0) {
								chess [i, j] = 1;
								turn = 1;
							} else {
								chess [i, j] = 2;
								turn = 0;
							}
						}
                    
					}
				}
                
			}
		}
        res = check();
		if (res == 1) {
			GUI.Label (new Rect (340, 230, 100, 50), "O赢");
		} else if (res == 2) {
			GUI.Label (new Rect (340, 230, 100, 50), "X赢");
		} else if(res==3){
            GUI.Label (new Rect (340, 230, 100, 50), "平局");
        } else{
            if(turn==0) GUI.Label (new Rect (335, 230, 100, 50), "O的回合");
            else GUI.Label (new Rect (335, 230, 100, 50), "X的回合");
        }
	}
	void reset() {
		//重开置空
        turn=0;
        res=0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				chess [i,j] = 0;
			}
		}
		turn = 0;
	}
	int check() {
		//检查行
		for (int i = 0; i < 3; i++) {
			if (chess[i,0]!=0&&chess [i,0] == chess [i,1] && chess [i,1] == chess [i,2]) {
				return chess [i,0];
			}
		}
		//检查列
		for (int j = 0; j < 3; j++) {
			if (chess[0,j]!=0&&chess [0,j] == chess [1,j] && chess [1,j] == chess [2,j]) {
				return chess [0,j];
			}
		}
		//检查对角线
		if ((chess[0,0]!=0&&chess [0, 0] == chess [1, 1] && chess [1, 1] == chess [2, 2]) ||
		    (chess[0,2]!=0&&chess [0, 2] == chess [1, 1] && chess [1, 1] == chess [2, 0])) {
			return chess [1, 1];
		}
        //检查满
        int count = 0;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (chess [i, j] != 0)
                    count++;
            }
        }
        if(count==9)return 3;
		return 0;
	}
}