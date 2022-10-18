using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    //用于和各行星对应
    public Transform Sun;  
    public Transform Earth;  
    public Transform Mars;  
    public Transform Mercury;  
    public Transform Venus;  
    public Transform Jupiter;  
    public Transform Saturn;  
    public Transform Uranus;  
    public Transform Neptune; 

    // Start is called before the first frame update
    void Start()
    {
        //初始化位置
        Sun.position = new Vector3(0, 0, 0);
        Earth.position = new Vector3(-6, 0, 0);
        Mars.position = new Vector3(9, 0, 0);
        Mercury.position = new Vector3(3, 0, 0);
        Venus.position = new Vector3(5, 0, 0);
        Jupiter.position = new Vector3(-11, 0, 0);
        Saturn.position = new Vector3(14, 0, 0);
        Uranus.position = new Vector3(-20, 0, 0);
        Neptune.position = new Vector3(22, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //public void RotateAround(Vector3 point, Vector3 axis, float angle);//公转
        //Rotate函数表示自转
        
        Earth.RotateAround(Sun.position, Vector3.up, 14 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 34 * Time.deltaTime);
        
	    Mars.RotateAround(Sun.position, new Vector3(0, 14, 5), 12 * Time.deltaTime);
        Mars.Rotate(new Vector3(0, 13, 6) * 45 * Time.deltaTime);
        
        Mercury.RotateAround(Sun.position, new Vector3(0, 4, 1), 16 * Time.deltaTime);
        Mercury.Rotate(new Vector3(0, 5, 1) * 5 * Time.deltaTime);
        
	    Venus.RotateAround(Sun.position, new Vector3(0, 2, 1), 17 * Time.deltaTime);
        Venus.Rotate(new Vector3(0, 2, 1) * Time.deltaTime);
        
	    Jupiter.RotateAround(Sun.position, new Vector3(0, 9, 4), 11 * Time.deltaTime);
        Jupiter.Rotate(new Vector3(0, 10, 3) * 32 * Time.deltaTime);
	    
	    Saturn.RotateAround(Sun.position, new Vector3(0, 2, 1), 10 * Time.deltaTime);
        Saturn.Rotate(new Vector3(0, 3, 1) * 21 * Time.deltaTime);
        
        Uranus.RotateAround(Sun.position, new Vector3(0, 10, 1), 8 * Time.deltaTime);
        Uranus.Rotate(new Vector3(0, 10, 1) * 23 * Time.deltaTime);
        
        Neptune.RotateAround(Sun.position, new Vector3(0, 8, 2), 6 * Time.deltaTime);
        Neptune.Rotate(new Vector3(0, 9, 1) * 30 * Time.deltaTime);

    }
}
