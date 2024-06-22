using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    public bool entered;
    public bool completed = false;
    public GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(entered == true)
        {
            //entered = false;
            for(int i = 0; i < room.transform.childCount; i++)
            {
                room.transform.GetChild(i).GetComponent<EnemyController>().active = true;
            }
        }

        if(room.transform.childCount < 1)
        {
            completed = true;
        }
    }
}
