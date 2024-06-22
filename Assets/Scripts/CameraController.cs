using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject cameras;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        cameras = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position - new Vector3(0, 0, 10);
        cameras.transform.position = pos;
    }
}
