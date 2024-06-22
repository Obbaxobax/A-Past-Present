using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorInfo : MonoBehaviour
{
    public bool active = false;
    public Sprite activeSprite;

    void Update()
    {
        if(active == true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
    }
}
