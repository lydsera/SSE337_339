using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : SSActionManager {
    public DiskFlyAction fly;  
    public FirstController myscenecontroller;           

    protected void Start() {
        myscenecontroller = (FirstController)SSDirector.GetInstance().CurrentScenceController;
        myscenecontroller.actionmanager = this;     
    }
    public void DiskFly(GameObject disk, float angle, float power) {
        int lor = 1;
        if (disk.transform.position.x > 0) lor = -1;
        fly = DiskFlyAction.GetSSAction(lor, angle, power);
        this.RunAction(disk, fly, this);
    }
}
