using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;
public class Controller : MonoBehaviour,ISceneController,IUserAction
{
    public Side left_side;//左岸为起点起点
    public Side right_side;//终点
    public Boat boat;
    public GameObject river;
    private List<Role> role_array;
    UserGUI gui;
    public Judge judge;
    public CCActionManager action_manager;
    void Start()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();
    }
    public void LoadResources()
    {
        left_side = new Side("left");
        right_side = new Side("right");
        boat = new Boat();
        role_array = new List<Role>();
        river = Instantiate(Resources.Load("Prefabs/Water",typeof(GameObject)),new Vector3(0, -0.5F, -0.49F), Quaternion.identity) as GameObject;
        judge = new Judge();
        action_manager = gameObject.AddComponent<CCActionManager>() as CCActionManager;  
        
        for (int i = 0; i < 3; i++)
        {
            Role role = new Role("priest");
            role.SetName("priest" + i);
            role.SetPosition(left_side.GetEmptyPosition());
            role.GoSide(left_side);
            left_side.AddRole(role);
            role_array.Add(role);
        }
        for (int i = 0; i < 3; i++)
        {
            Role role = new Role("devil");
            role.SetName("devil" + i);
            role.SetPosition(left_side.GetEmptyPosition());
            role.GoSide(left_side);
            left_side.AddRole(role);
            role_array.Add(role);
        }
    }
    public void MoveBoat()
    {
         if (boat.IsEmpty() || gui.sign != 0)
            return;
        action_manager.MoveBoat(boat.GetBoat(),boat.GetTargetPos(),boat.speed);
        gui.sign = judge.Check();
    }
    public void MoveRole(Role role)
    {
        if (gui.sign != 0) 
            return;
        if (role.IsOnBoat())
        {
            Side Side;
            if (boat.GetBoatSign() == 1)
                Side = left_side;
            else
                Side = right_side;
            boat.DeleteRoleByName(role.GetName());
            Vector3 end = Side.GetEmptyPosition(); 
            Vector3 middle = new Vector3(role.GetRole().transform.position.x, end.y, end.z); 
            action_manager.MoveRole(role.GetRole(),middle,end,role.speed);
            role.GoSide(Side);
            Side.AddRole(role);
        }
        else
        {                                
            Side Side = role.GetSide();
            if (boat.GetEmptyNumber() == -1 || Side.GetSideFlag() != boat.GetBoatSign()) 
                return;   

            Side.DeleteRoleByName(role.GetName());
            Vector3 end = boat.GetEmptyPosition();
            Vector3 middle = new Vector3(end.x,role.GetRole().transform.position.y,end.z);
            action_manager.MoveRole(role.GetRole(),middle,end,role.speed);
            role.GoBoat(boat);
            boat.AddRole(role);
        }
        gui.sign = judge.Check();
        if (gui.sign == 1)
        {
        
        }
    }
    public void Restart()
    {
        left_side.Reset();
        right_side.Reset();
        boat.Reset();
        for (int i = 0; i < role_array.Count; i++)
        {
            role_array[i].Reset();
        }
        if (gui.sign == 1)
        {
        }
    }
    public int Check()
    {
        return 1;
    }
}
