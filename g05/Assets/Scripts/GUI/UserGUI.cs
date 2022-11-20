using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;   
    GUIStyle button_style = new GUIStyle();
    GUIStyle start_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    GUIStyle text_style2 = new GUIStyle();
    GUIStyle over_style = new GUIStyle();
    GUIStyle round_style = new GUIStyle();
    GUIStyle target_style = new GUIStyle();
    GUIStyle red_style = new GUIStyle();
    GUIStyle yellow_style = new GUIStyle();
    GUIStyle green_style = new GUIStyle();
    private bool game_start = false;

    void Start () {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
	
	void OnGUI () {
        start_style.fontSize = 35;
        button_style.fontSize = 20;
        text_style.fontSize = 18;
        text_style2.fontSize = 16;
        red_style.normal.textColor = Color.red;
        red_style.fontSize = 18;
        yellow_style.normal.textColor = Color.yellow;
        yellow_style.fontSize = 18;
        green_style.normal.textColor = Color.green;
        green_style.fontSize = 18;
        over_style.fontSize = 50;
        round_style.fontSize = 50;
        target_style.fontSize = 20;
        if (game_start) 
        {
            GUI.Label(new Rect(5, 5, 200, 50), "red disk=1", red_style);
            GUI.Label(new Rect(105, 5, 50, 50), "yellow disk=2", yellow_style);
            GUI.Label(new Rect(225, 5, 50, 50), "green disk=3", green_style);
            GUI.Label(new Rect(Screen.width-300, 5, 50, 50), "Score:"+ action.GetScore().ToString(), text_style);
            GUI.Label(new Rect(Screen.width-200, 5, 50, 50), "Round:" + action.GetRound().ToString(), text_style);
            GUI.Label(new Rect(Screen.width-100, 5, 50, 50), "Num:" + action.GetTrial().ToString(), text_style);
            if(action.Getflag())
            {
                GUI.Label(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 60 , 100, 100), "Round"+(action.GetRound()+1).ToString(), round_style);
                GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 +20 , 100, 100), "Target Score:"+(action.GetTarget()).ToString(), target_style);
            }
            if (action.GetStatus()==false) {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 80, 100, 100), "You Lose!", over_style);
                GUI.Label(new Rect(Screen.width / 2 - 45, Screen.height / 2, 50, 50), "Your Score:" + action.GetScore().ToString(), text_style2);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2+40, 100, 50), "Restart")) {
                    action.ReStart();
                    return;
                }
                action.GameOver();
            }
            else if(action.GetStatus()==true&&action.Getflag())
            {
                GUI.Label(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 60 , 100, 100), "Round"+(action.GetRound()+1).ToString(), round_style);
                GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 +20 , 100, 100), "Target Score:"+(action.GetTarget()).ToString(), target_style);
            }
            if (action.GetRound() == 3 && action.GetTrial() == 11&&action.GetStatus()) {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 80, 100, 100), "You Win!", over_style);
                GUI.Label(new Rect(Screen.width / 2 - 45, Screen.height / 2, 50, 50), "Your Score:" + action.GetScore().ToString(), text_style2);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2+40, 100, 50), "Restart")) {
                    action.ReStart();
                    return;
                }
                action.GameOver();
            }
        }
        else {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 100, 100, 100), "Hit UFO",start_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2-20, 100, 50), "Start")) {
                game_start = true;
                action.ReStart();
            }
        }
    }
   
}
