using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;

public class FirstController : MonoBehaviour, SceneController, UserAction 
{

	readonly Vector3 water_pos = new Vector3(0,0.5F,0);


	UserGUI userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private RoleController[] roles;

	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		roles = new RoleController[6];
		loadResources ();
	}
	//加载场景
	public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();

		for (int i = 0; i < 3; i++) {
			RoleController cha = new RoleController ("priest");
			cha.setName("priest" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			roles [i] = cha;
		}

		for (int i = 0; i < 3; i++) {
			RoleController cha = new RoleController ("devil");
			cha.setName("devil" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			roles [i+3] = cha;
		}
	}




	public void moveBoat() {
		if (boat.isEmpty ())
			return;
		boat.Move ();
		userGUI.status = check ();
	}

	public void roleIsClicked(RoleController roleCtrl) {
		if (roleCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { 
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (roleCtrl.getName());
			roleCtrl.moveToPosition (whichCoast.getEmptyPosition ());
			roleCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (roleCtrl);

		} else {									//人在岸上
			CoastController whichCoast = roleCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		//船满了
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	//船不在角色那边
				return;

			whichCoast.getOffCoast(roleCtrl.getName());
			roleCtrl.moveToPosition (boat.getEmptyPosition());
			roleCtrl.getOnBoat (boat);
			boat.GetOnBoat (roleCtrl);
		}
		userGUI.status = check ();
	}

	int check() {	//1输2赢0未结束
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getRoleNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getRoleNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		//赢
			return 2;

		int[] boatCount = boat.getRoleNum ();
		if (boat.get_to_or_from () == -1) {	
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) //输
		{
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;			//未完成
	}

	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < roles.Length; i++) {
			roles [i].reset ();
		}
	}
}
