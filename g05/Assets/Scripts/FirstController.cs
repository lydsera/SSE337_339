using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public UserGUI gui;
    public DiskFactory diskfactory;
    public FlyActionManager actionmanager;
    public ScoreRecorder scorerecorder;
    private int round = 0;//记录是第几回合                                                  
    private int num = 0;//发射飞碟数
    private bool status=true;//status标记游戏是否结束，true则不结束
    public bool ifroundend=false;//如果回合结束则为ture，游戏暂停几秒,OnGUI中调用Getflag得到true，展示下一回合目标,然后开始下一回合
    public bool flag=false;//flag用于标记记录curtime

    public float []target=new float[3]{10,25,40};//每回合的目标分数                                             
    private bool ifrun = false;//是否运行，点击游戏开始会调用ReStart使之变true
    private float curtime=0;//进入回合前记录当前时间，用于等待
    private int interval = 0;//每次发飞碟的间隔时间
    void Start () 
    {
        SSDirector director = SSDirector.GetInstance();     
        director.CurrentScenceController = this;
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        diskfactory = Singleton<DiskFactory>.Instance;
        actionmanager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        scorerecorder = Singleton<ScoreRecorder>.Instance;
    }
	void Update () 
    {
        if(ifrun) 
        {
            if(round==0)
            {
                num=0;
                ifroundend=true;
                if(flag==false)
                {
                    curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                    flag=true;
                }
                else
                {
                    float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                    if(tmp<(curtime+3)) return;//第一回合前等待3秒
                    ifroundend=false;
                    flag=false;
                    round+=1;
                }  
           }
            interval++;
            if (Input.GetButtonDown("Fire1")) 
            {
                Vector3 pos = Input.mousePosition;
                Hit(pos);
            }
            if(round==1)
            {
                    if (interval >= 180) 
                    {
                        if(ifroundend)
                        {
                            float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                            if(tmp<(curtime+3)) return;//第二回合前等待3秒
                            else
                            {
                                num=0;
                                round=2;
                                ifroundend=false;
                            }
                        }
                        
                        if (num == 20) 
                        {
                            if(flag==false)
                            {
                                curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                flag=true;
                            }
                            else
                            {
                                float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                if(tmp<(curtime+3)) return;//等待三秒结算
                                flag=false;
                                curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                checkstatus();
                                if(status==true)
                                {
                                    ifroundend=true;
                                }
                            }  
                        }
                        else{
                                interval = 0;
                                SendDisk(1);
                                num += 1;
                        }
                    }
                }
            if(round==2)
            {
                    if (interval >= 120) 
                    {
                        if(ifroundend)
                        {
                            float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                            if(tmp<(curtime+3)) return;//第三回合前等待3秒
                            else
                            {
                                num=0;
                                round=3;
                                ifroundend=false;
                                flag=false;
                            }
                        }
                        
                        if (num == 18) 
                        {
                            if(flag==false)
                            {
                                curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                flag=true;
                            }
                            else
                            {
                                float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                if(tmp<(curtime+3)) return;//等待三秒结算
                                flag=false;
                                curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                checkstatus();
                                if(status==true)
                                {
                                    ifroundend=true;
                                }
                            }
                        }
                        else
                        {
                            interval = 0;
                            if (num % 2 == 0) SendDisk(1);
                            else SendDisk(2);
                            num += 1;
                        }
                    }
                }
            if(round==3)
            {
                    if (interval >= 80) 
                    {
                        
                        if (num == 10) 
                        {
                            if(flag==false)
                            {
                                curtime=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                flag=true;
                            } 
                            else
                            {
                                float tmp=System.DateTime.Now.Hour*3600+System.DateTime.Now.Minute*60+System.DateTime.Now.Second;
                                if(tmp<(curtime+3)) return;//等待三秒结算
                                flag=false;
                                ifrun=false;
                                num+=1;
                                checkstatus();
                            }  
                        }
                        else{
                            interval = 0;
                            if (num % 3 == 0) SendDisk(1);
                            else if(num % 3 == 1)SendDisk(2);
                            else SendDisk(3);
                            num += 1;
                        }
                    }

                }
            diskfactory.FreeDisk();
        }
        
    }
    

    public void LoadResources() {
        diskfactory.GetDisk(round);
        diskfactory.FreeDisk();
    }
    
    private void SendDisk(int type) {
        GameObject disk = diskfactory.GetDisk(type);

        float ran_y = 0;
        float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;

        float power = 0;
        float angle = 0;
        if (type == 1) {
            ran_y = Random.Range(1f, 5f);
            power = Random.Range(4f, 6f);
            angle = Random.Range(25f,30f);
        }
        else if (type == 2) {
            ran_y = Random.Range(2f, 3f);
            power = Random.Range(5f, 7f);
            angle = Random.Range(15f, 17f);
        }
        else {
            ran_y = Random.Range(5f, 6f);
            power = Random.Range(6f, 8f);
            angle = Random.Range(10f, 12f);
        }
        disk.transform.position = new Vector3(ran_x*16f, ran_y, 0);
        actionmanager.DiskFly(disk, angle, power*0.2f);
    }
    //检测游戏是否结束
    public void checkstatus(){
        status=GetScore()>=target[round-1] ? true:false;
    }
    //射击
    public void Hit(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<Disk>() != null) {
                scorerecorder.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -10, 0);
            }
        }
    }

    public float GetScore() {
        return scorerecorder.GetScore();
    }
    public bool GetStatus() {
        return status;
    }
    public float GetTarget(){
        return target[round];
    }
    public int GetRound() {
        return round;
    }
    public bool Getflag() {
        return ifroundend;
    }
    public int GetTrial() {
        return num;
    }

    //重新开始
    public void ReStart() {
        ifrun = true;
        scorerecorder.Reset();
        diskfactory.Reset();
        round = 0;
        num = 1;
        status=true;
    }
    //游戏结束
    public void GameOver() {
        ifrun = false;
    }
}

