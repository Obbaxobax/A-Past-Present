using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image fade;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeIn());
    }

    IEnumerator fadeIn()
    {
        fade.gameObject.SetActive(true);
        var temp = fade.color;
        for (float i = 0; i < 256; i++)
        {
            temp.a = 1f - (i / 255f);
            fade.color = temp;
            yield return new WaitForSeconds(1 / 255);
        }
        fade.gameObject.SetActive(false);
    }
}
