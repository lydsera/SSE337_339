using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;
public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    public int sign = 0;
    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
    void OnGUI()
    {
        GUIStyle text_style;
        GUIStyle button_style;
        text_style = new GUIStyle()
        {
            fontSize = 30
        };
        button_style = new GUIStyle("button")
        {
            fontSize = 15
        };
        if (sign == 1)
        {
            GUI.Label(new Rect(Screen.width / 2-90, Screen.height / 2-120, 100, 50), "游戏失败!", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 100, 50), "重新开始", button_style))
            {
                action.Restart();
                sign = 0;
            }
        }
        else if (sign == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 120, 100, 50), "游戏胜利!", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 100, 50), "重新开始", button_style))
            {
                action.Restart();
                sign = 0;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
