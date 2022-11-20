using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreRecorder : MonoBehaviour {
    private float score;
    void Start () {
        score = 0;
    }
    public void Record(GameObject disk) {
        score += disk.GetComponent<Disk>().score;
    }
    public float GetScore() {
        return score;
    }
    public void Reset() {
        score = 0;
    }
}
