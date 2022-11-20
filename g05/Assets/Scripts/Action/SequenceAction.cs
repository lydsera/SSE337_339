using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAction : SSAction, ISSActionCallback
{
    
    public List<SSAction> sequence;
    public int repeat = -1;
    public int cur = 0;

    public static SequenceAction GetSSAcition(int repeat, int cur, List<SSAction> sequence)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.cur = cur;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (cur < sequence.Count)
        {
            sequence[cur].Update();    
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;   
        this.cur++;
        if (this.cur >= sequence.Count)
        {
            this.cur = 0;
            if (repeat > 0) repeat--;
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
    {
    }
}
