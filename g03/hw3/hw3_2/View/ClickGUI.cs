using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;

public class ClickGUI : MonoBehaviour {
	UserAction action;
	RoleController roleController;

	public void setController(RoleController roleCtrl) {
		roleController = roleCtrl;
	}

	void Start() {
		action = Director.getInstance ().currentSceneController as UserAction;
	}

	void OnMouseDown() {
		if (gameObject.name == "boat") {
			action.moveBoat ();
		} else {
			action.roleIsClicked (roleController);
		}
	}
}
