using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriestAndDevil;
public class SSAction : ScriptableObject //动作基类          
{

    public bool enable = true;                    
    public bool destroy = false;                  

    public GameObject gameobject{ get; set;}           
    public Transform transform{ get; set;}                   
    public ISSActionCallback callback{get; set;}             

    protected SSAction() {}                        

    public virtual void Start()                    
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class CCMoveToAction : SSAction   //动作实现                     
{
    public Vector3 target;        
    public float speed;          

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }

    public override void Start()
    {}
}

public class CCSequenceAction : SSAction, ISSActionCallback //动作组合实现复杂动作
{
    public List<SSAction> sequence;   
    public int repeat = -1;         
    public int start = 0;              

    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();     
        }
    }
    //接口定义
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;          
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) 
                repeat--;
            if (repeat == 0)
            {
                this.destroy = true;               
                this.callback.SSActionEvent(this); 
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;              
            action.Start();
        }
    }

    void OnDestroy()
    {}
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,int intParam = 0, string strParam = null, Object objectParam = null);
}

//动作管理器基类
public class SSActionManager : MonoBehaviour, ISSActionCallback                     
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    
    private List<SSAction> waitingAdd = new List<SSAction>();                       
    private List<int> waitingDelete = new List<int>();                                           

    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;                                     
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            Destroy(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
    protected void Start()
    {

    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null){}
}
//动作管理器实现
public class CCActionManager:SSActionManager
{
    private CCSequenceAction moverole_act;
    private CCMoveToAction moveboat_act;
    public Controller controller;

    protected new void Start()
    {
        controller = SSDirector.GetInstance().CurrentScenceController as Controller;
        controller.action_manager = this;
    }
    public void MoveBoat(GameObject boat,Vector3 target,float speed)
    {
        moveboat_act = CCMoveToAction.GetSSAction(target,speed);
        this.RunAction(boat,moveboat_act,this);
    }
    public void MoveRole(GameObject role,Vector3 middle,Vector3 end,float speed)
    {
        SSAction move_to_middle = CCMoveToAction.GetSSAction(middle,speed);
        SSAction move_to_end = CCMoveToAction.GetSSAction(end,speed);
        List<SSAction> act_list = new List<SSAction> {move_to_middle,move_to_end};
        moverole_act = CCSequenceAction.GetSSAction(1,0,act_list);
        this.RunAction(role,moverole_act,this);
    }
}