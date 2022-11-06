using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PriestAndDevil
{
    //导演类控制一切，一直存在
    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
		public ISceneController CurrentScenceController { get; set; }

		public static SSDirector GetInstance()
        {
			if (_instance == null) {
				_instance = new SSDirector ();
			}
			return _instance;
		}
    }
    //导演和场景控制器的接口
    public interface ISceneController
    {
        void LoadResources();
    }
    public interface IUserAction                         
    {
        void MoveBoat();                                   
        void Restart();                                    
        void MoveRole(Role role);                     
        int Check();                                       
    }
    public class Role
    {
        GameObject role;
        int role_flag;  //0为牧师，1为恶魔
        bool on_boat;
        Side side = (SSDirector.GetInstance().CurrentScenceController as Controller).left_side;//
        Click click;
        Move move;

        public Role(string role_name)
        {
            if(role_name == "priest")
            {
                role = Object.Instantiate(Resources.Load("Prefabs/Priest",typeof(GameObject)),Vector3.zero,Quaternion.Euler(0,-90,0)) as GameObject;
                role_flag = 0;
            }
            else
            {
                role = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
                role_flag = 1;
            }
            move = role.AddComponent(typeof(Move)) as Move;
            click = role.AddComponent(typeof(Click)) as Click;
            click.SetRole(this);//
        }

        public int GetSign() { return role_flag;}
        public Side GetSide(){return side;}
        public string GetName() { return role.name; }
        public bool IsOnBoat() { return on_boat; }
        public void SetName(string name) { role.name = name; }
        public void SetPosition(Vector3 pos) { role.transform.position = pos; }

        public void PlayGameOver()
        {

        }

        public void PlayIdle()
        {

        }

        public void Move(Vector3 vec)
        {
            move.MovePosition(vec);
        }

        public void GoSide(Side land)
        {  
            role.transform.parent = null;
            side = land;
            on_boat = false;
            
        }

        public void GoBoat(Boat boat)
        {
            role.transform.parent = boat.GetBoat().transform;
            side = null;          
            on_boat = true;
        }

        public void Reset()
        {
            Side left_side = (SSDirector.GetInstance().CurrentScenceController as Controller).left_side;
            GoSide(left_side);
            SetPosition(side.GetEmptyPosition());
            side.AddRole(this);
        }
    }   
    public class Boat
    {
        GameObject boat;                                          
        Vector3[] start_empty_pos;  //开始时拿来放人的空位                            
        Vector3[] end_empty_pos;    //结束时拿来放人的空位                                    
        Move move;                                                    
        Click click;
        int boat_flag = 1;   //1表示在开始侧，-1在结束侧                                                  
        Role[] roles = new Role[2];       
        public Boat()
        {
            boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), new Vector3(5.09F, 0.25F, -0.44F), Quaternion.identity) as GameObject;
            boat.name = "boat";
            move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            start_empty_pos = new Vector3[] { new Vector3(6, 1.1F, -0.6F), new Vector3(4, 1.1F, -0.6F) };
            end_empty_pos = new Vector3[] { new Vector3(-4, 1.1F, -0.6F), new Vector3(-6, 1.1F, -0.6F) };
        }         
        public int GetBoatSign(){ return boat_flag;}    
        public GameObject GetBoat(){ return boat; }
        public bool IsEmpty()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null)
                    return false;
            }
            return true;
        }
        
        public void BoatMove()
        {
            if (boat_flag == -1)
            {
                move.MovePosition(new Vector3(5.09F, 0.25F, -0.44F));
                boat_flag = 1;
            }
            else
            {
                move.MovePosition(new Vector3(-5.09F, 0.25F, -0.44F));
                boat_flag = -1;
            }
        }
        public Role DeleteRoleByName(string role_name)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == role_name)
                {
                    Role role = roles[i];
                    roles[i] = null;
                    return role;
                }
            }
            return null;
        }
        public int GetEmptyNumber()
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
        public Vector3 GetEmptyPosition()
        {
            Vector3 pos;
            if (boat_flag == 1)
                pos = start_empty_pos[GetEmptyNumber()];
            else
                pos = end_empty_pos[GetEmptyNumber()];
            return pos;
        }
        public void AddRole(Role role)
        {
            roles[GetEmptyNumber()] = role;
        }
        public void Reset()
        {
            if (boat_flag == -1)
                BoatMove();
            roles = new Role[2];
        }
        public int[] GetRoleNumber()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                    continue;
                if (roles[i].GetSign() == 0)
                    count[0]++;
                else
                    count[1]++;
            }
            return count;
        }
        
    }
    public class Side
    {
        GameObject side;           //预制好的河岸模型
        Vector3[] empty_pos;        //存放空闲位置
        int flag;                   // 1表示起点,-1表示终点
        Role[] role_array = new Role[6];

        public Side(string side_name)
        {
            empty_pos = new Vector3[] {new Vector3(7F,1.5F,-1), new Vector3(7F,1.5F,0), new Vector3(8F,1.5F,-1),
                new Vector3(8F,1.5F,0), new Vector3(9F,1.5F,-1), new Vector3(9F,1.5F,0)};
            if (side_name == "left")
            {
                side = Object.Instantiate(Resources.Load("Prefabs/Side", typeof(GameObject)), new Vector3(7.97F, -0.1F, -0.51F), Quaternion.Euler(0,90,0)) as GameObject;
                flag = 1;
            }
            else
            {
                side = Object.Instantiate(Resources.Load("Prefabs/Side", typeof(GameObject)), new Vector3(-7.97F, -0.1F, -0.51F), Quaternion.Euler(0,90,0)) as GameObject;
                flag = -1;
            }
        }

        public int GetEmptyNumber()                     
        {
            for (int i = 0; i < role_array.Length; i++)
            {
                if (role_array[i] == null)
                    return i;
            }
            return -1;
        } 

        public Vector3 GetEmptyPosition()        
        {
            Vector3 pos = empty_pos[GetEmptyNumber()];
            pos.x = flag * pos.x;              
            return pos;
        }

        public int GetSideFlag()
        {
            return flag;
        }       

        public void AddRole(Role role)             
        {
            role_array[GetEmptyNumber()] = role;
        }

        public Role DeleteRoleByName(string role_name)      
        { 
            for (int i = 0; i < role_array.Length; i++)
            {
                if (role_array[i] != null && role_array[i].GetName() == role_name)
                {
                    Role role = role_array[i];
                    role_array[i] = null;
                    return role;
                }
            }
            return null;
        }

        public int[] GetRoleNum()
        {
            int[] count = { 0, 0 };                    //count[0]是牧师数，count[1]是魔鬼数
            for (int i = 0; i < role_array.Length; i++)
            {
                if (role_array[i] != null)
                {
                    if (role_array[i].GetSign() == 0)
                        count[0]++;
                    else
                        count[1]++;
                }
            }
            return count;
        }

        public void Reset()
        {
            role_array = new Role[6];
        }
    }
    public class Move : MonoBehaviour
    {
        float move_speed = 100;//移动速度
        int move_flag = 0;//0是不动，1表示进行第一阶段移动，2表示进行第二阶段移动
        Vector3 end_pos;
        Vector3 middle_pos;

        void Update()
        {
            if (move_flag == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, middle_pos, move_speed * Time.deltaTime);
                if (transform.position == middle_pos)
                    move_flag = 2;
            }
            else if (move_flag == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, end_pos, move_speed * Time.deltaTime);
                if (transform.position == end_pos)
                    move_flag = 0;           
            }
        }
        public void MovePosition(Vector3 position)
        {
            end_pos = position;
            if (position.y == transform.position.y)         //船只会水平移动
            {  
                move_flag = 2;
            }
            else if (position.y < transform.position.y)      //角色从陆地到船
            {
                middle_pos = new Vector3(position.x, transform.position.y, position.z);
            }
            else                                          //角色从船到陆地
            {
                middle_pos = new Vector3(transform.position.x, position.y, position.z);
            }
            move_flag = 1;
        }
    }
    public class Click : MonoBehaviour
    {
        IUserAction action;
        Role role = null;
        Boat boat = null;
        public void SetRole(Role role)
        {
            this.role = role;
        }
        public void SetBoat(Boat boat)
        {
            this.boat = boat;
        }
        void Start()
        {
            action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        }
        void OnMouseDown()
        {
            if (boat == null && role == null) return;
            if (boat != null)
                action.MoveBoat();
            else if(role != null)
                action.MoveRole(role);
        }
    }

}