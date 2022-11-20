using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiskFactory : MonoBehaviour {
    private List<Disk> useddisklist = new List<Disk>();
    private List<Disk> freedisklist = new List<Disk>();

    public GameObject GetDisk(int type) {
        GameObject disk_prefab = null;
        if (freedisklist.Count>0) {
            for(int i = 0; i < freedisklist.Count; i++) {
                if (freedisklist[i].type == type) {
                    disk_prefab = freedisklist[i].gameObject;
                    freedisklist.Remove(freedisklist[i]);
                    break;
                }
            }     
        }

        if(disk_prefab == null) {
            if(type == 1) {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk1"),
                new Vector3(0, -10f, 0), Quaternion.Euler(-90, 0f, 0f));
            }
            else if (type == 2) {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk2"),
                new Vector3(0, -10f, 0), Quaternion.Euler(-90, 0f, 0f));
            }
            else {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk3"),
                new Vector3(0, -10f, 0), Quaternion.Euler(-90, 0f, 0f));
            }

            disk_prefab.GetComponent<Renderer>().material.color = disk_prefab.GetComponent<Disk>().color;
        }

        useddisklist.Add(disk_prefab.GetComponent<Disk>());
        disk_prefab.SetActive(true);
        return disk_prefab;
    }

    public void FreeDisk() {
        for(int i=0; i<useddisklist.Count; i++) {
            if (useddisklist[i].gameObject.transform.position.y <= -10f) {
                freedisklist.Add(useddisklist[i]);
                useddisklist.Remove(useddisklist[i]);
            }
        }          
    }

    public void Reset() {
        FreeDisk();
    }

}
