using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Button exitButton;
    private GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Options Menu");
        exitButton.onClick.AddListener(closeMenu);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.GetComponent<Slider>().value);
    }

    void closeMenu()
    {
        menu.SetActive(false);
    }
}
