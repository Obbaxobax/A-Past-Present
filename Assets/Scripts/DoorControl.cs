using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    //Door movement
    public Transform door;
    public int doorCount;

    //Door opening requirements
    public int tripped = 0;
    public bool locked;
    public string trigger;
    public GameObject generator;
    public GameObject room;
    public bool openedAgain = false;
    public bool Horizontal;

    // Start is called before the first frame update
    void Start()
    {
        if (trigger == string.Empty && locked == false)
        {
            if (Horizontal)
            {
                door.Translate(Vector2.right * 3);
            }
            else
            {
                door.Translate(Vector2.up * 10);
            }
            for (int i = 0; i < doorCount; i++)
            {
                Color doorColor = door.GetChild(i).GetComponent<SpriteRenderer>().color;
                doorColor.a = 0f;
                door.GetChild(i).GetComponent<SpriteRenderer>().color = doorColor;
                door.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tripped == 1)
        {
            room.GetComponent<RoomControl>().entered = true;
            Debug.Log(room.GetComponent<RoomControl>().entered);
            StartCoroutine(closeDoor());
        }

        if (room.GetComponent<RoomControl>().completed == true)
        {
            openedAgain = true;
            StartCoroutine(openDoor());
        }

        if(trigger == "Generator" && generator.GetComponent<GeneratorInfo>().active == true && openedAgain == false)
        {
            Debug.Log("Generator");
            openedAgain = true;
            StartCoroutine(openDoor());
        }

        if(trigger != string.Empty && trigger != "Generator")
        {
            GameObject triggerRoom = GameObject.Find(trigger);
            if(triggerRoom.GetComponent<RoomControl>().completed == true && openedAgain == false)
            {
                openedAgain = true;
                StartCoroutine(openDoor());
            }
        }

        if(room.GetComponent<RoomControl>().entered == true && tripped < 1)
        {
            tripped = 1;
        }
    }

    IEnumerator closeDoor()
    {
        tripped = 2;
        Color doorColor = door.GetChild(0).GetComponent<SpriteRenderer>().color;
        for (int i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(0.05f);
            if (Horizontal)
            {
                door.Translate(Vector2.right * -0.3f);
            }
            else
            {
                door.Translate(Vector2.up * -1);
            }
            doorColor.a += 0.1f;
            for (int u = 0; u < doorCount; u++)
            {
                door.GetChild(u).GetComponent<SpriteRenderer>().color = doorColor;
                door.GetChild(u).GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    IEnumerator openDoor()
    {
        Color doorColor = door.GetChild(0).GetComponent<SpriteRenderer>().color;
        for (int i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(0.05f);
            if (Horizontal)
            {
                door.Translate(Vector2.right * 0.3f);
            }
            else
            {
                door.Translate(Vector2.up);
            }
            doorColor.a -= 0.1f;
            for (int u = 0; u < doorCount; u++)
            {
                door.GetChild(u).GetComponent<SpriteRenderer>().color = doorColor;
                door.GetChild(u).GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
