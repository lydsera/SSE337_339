using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem[] fireworks;
    int cnt=0;
    int l;
    
    void Start()
    {
        GameObject[] tmp;
        tmp = GameObject.FindGameObjectsWithTag("Fireworks");
        l=tmp.Length;
        fireworks = new ParticleSystem[l];
        
        
        for(int i = 0; i < l; ++i)
        {
            fireworks[i] = tmp[i].GetComponent<ParticleSystem>();
            fireworks[i].Stop();
        }
        

       
    }
    
    
    private void OnGUI() {
        if (GUI.Button(new Rect(Screen.width/2-150, Screen.height-100, 80, 50), "Start")) fireworks[cnt].Play();
        if (GUI.Button(new Rect(Screen.width/2-50, Screen.height-100, 80, 50), "Pause")) fireworks[cnt].Stop();
        if (GUI.Button(new Rect(Screen.width/2+50, Screen.height-100, 80, 50), "Change")){
            fireworks[cnt].Stop();
            cnt++;
            cnt%=l;
            fireworks[cnt].Play();
        }
    }
}

