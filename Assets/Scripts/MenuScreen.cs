using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public Button resume;
    public Button toTitle;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        resume.onClick.AddListener(resumes);
        toTitle.onClick.AddListener(title);
    }

    void resumes()
    {
        panel.SetActive(false);
    }

    void title()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                PlayerPrefs.SetInt("completed", 0);
                break;
            case "Level2":
                PlayerPrefs.SetInt("completed", 1);
                break;
            case "Level3":
                PlayerPrefs.SetInt("completed", 2);
                break;
            case "Level4":
                PlayerPrefs.SetInt("completed", 3);
                break;
            case "Level5":
                PlayerPrefs.SetInt("completed", 4);
                break;
            case "Level6":
                PlayerPrefs.SetInt("completed", 5);
                break;
        }
        Debug.Log("Excuse Me!");
        SceneManager.LoadScene("Title Screen");
    }
}
