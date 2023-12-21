using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastinDoor : MonoBehaviour
{
    public GameObject[] zombieList;

    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 1.5f, layerMask))
        {
            var roomNum = this.gameObject.name.Split(' ')[1];
            if(roomNum == "1"){
            if(hit.point.x > -8.25){
                setZombieChasing(0,false);
            }
            else{
                setZombieChasing(0,true);
            }
            }else if(roomNum == "2"){
            if(hit.point.x > -1.9){
                setZombieChasing(1,true);
                setZombieChasing(2,true);
            }
            else{
                setZombieChasing(1,false);
                setZombieChasing(2,false);
            }
            }else if(roomNum == "3"){
                if(hit.point.x > -8.1){
                setZombieChasing(3,false);
                setZombieChasing(4,false);
                setZombieChasing(5,false);
            }
            else{
                setZombieChasing(3,true);
                setZombieChasing(4,true);
                setZombieChasing(5,true);
            }
            } else if(roomNum == "4"){
            if(hit.point.x > -1.95){
                setZombieChasing(6,true);
                setZombieChasing(7,true);
                setZombieChasing(8,true);
                setZombieChasing(9,true);
            }
            else{
                setZombieChasing(6,false);
                setZombieChasing(7,false);
                setZombieChasing(8,false);
                setZombieChasing(9,false);
            }

            }

        }

    }

    
    public void setZombieChasing(int index,bool value){
        zombieList[index].GetComponent<ZombieMain>().isChasingLeon = value;
    }
}
