using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFlyAction : SSAction {
    public float gravity = -1;//重力
    private Vector3 start_vector;//初速度
    private Vector3 gravity_vector = Vector3.zero;//y方向速度
    private float time;

    private DiskFlyAction() { }
    public static DiskFlyAction GetSSAction(int lor, float angle, float power) {
        //初速度
        DiskFlyAction action = CreateInstance<DiskFlyAction>();
        if (lor == -1) {
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else {
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update() {
        time += Time.fixedDeltaTime;
        gravity_vector.y = gravity * time * 0.1f;
        transform.position += (start_vector + gravity_vector) * Time.fixedDeltaTime;
        if (this.transform.position.y < -10) {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }

    public override void Start() { }
}
