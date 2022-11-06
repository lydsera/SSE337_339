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
        boat.BoatMove();
        gui.sign = Check();
        if (gui.sign == 1)
        {
            // for (int i = 0; i < 3; i++)
            // {
            //     role_array[i].PlayGameOver();
            //     role_array[i + 3].PlayGameOver();
            // }
        }
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
            role.Move(Side.GetEmptyPosition());
            role.GoSide(Side);
            Side.AddRole(role);
        }
        else
        {                                
            Side Side = role.GetSide();
            if (boat.GetEmptyNumber() == -1 || Side.GetSideFlag() != boat.GetBoatSign()) 
                return;   

            Side.DeleteRoleByName(role.GetName());
            role.Move(boat.GetEmptyPosition());
            role.GoBoat(boat);
            boat.AddRole(role);
        }
        gui.sign = Check();
        if (gui.sign == 1)
        {
            // for (int i = 0; i < 3; i++)
            // {
            //     role_array[i].PlayGameOver();
            //     role_array[i + 3].PlayGameOver();
            // }
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
            for (int i = 0; i < 3; i++)
            {
                // role_array[i + 3].PlayIdle();
                // role_array[i].PlayIdle();
            }
        }
    }
    public int Check()
    {
        int start_priest = (left_side.GetRoleNum())[0];
        int start_devil = (left_side.GetRoleNum())[1];
        int end_priest = (right_side.GetRoleNum())[0];
        int end_devil = (right_side.GetRoleNum())[1];

        if (end_priest + end_devil == 6)     //获胜
            return 2;

        int[] boat_role_num = boat.GetRoleNumber();
        if (boat.GetBoatSign() == 1)         //在开始岸和船上的角色
        {
            start_priest += boat_role_num[0];
            start_devil += boat_role_num[1];
        }
        else                                  //在结束岸和船上的角色
        {
            end_priest += boat_role_num[0];
            end_devil += boat_role_num[1];
        }
        if (start_priest > 0 && start_priest < start_devil) //失败
        {      
            return 1;
        }
        if (end_priest > 0 && end_priest < end_devil)        //失败
        {
            return 1;
        }
        return 0;                                             //未完成
    }
}
