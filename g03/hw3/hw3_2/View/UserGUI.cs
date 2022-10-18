using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;

public class UserGUI : MonoBehaviour {
	private UserAction action;
	public int status = 0;
	GUIStyle style;
	GUIStyle buttonStyle;

	void Start() {
		
		action = Director.getInstance ().currentSceneController as UserAction;

		style = new GUIStyle();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;

		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
	}
	void OnGUI() {
		if (status == 1) {
			GUI.Label(new Rect(Screen.width/2-70, Screen.height/2-105, 100, 50), "游戏结束!", style);
			if (GUI.Button(new Rect(Screen.width/2-90, Screen.height/2-20, 140, 70), "重新开始", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		} else if(status == 2) {
			GUI.Label(new Rect(Screen.width/2-70, Screen.height/2-105, 100, 50), "你赢了！", style);
			if (GUI.Button(new Rect(Screen.width/2-90, Screen.height/2-20, 140, 70), "重新开始", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		}
		else
		GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-115, 180, 15), "牧师与恶魔", style);
	}
}
