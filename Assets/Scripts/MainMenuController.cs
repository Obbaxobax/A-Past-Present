using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public Button start;
    public Button cont;
    public Button options;
    public GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(startGame);
        cont.onClick.AddListener(continueGame);
        options.onClick.AddListener(openOptions);
        if(PlayerPrefs.GetInt("completed") == 0)
        {
            cont.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alpha = 0.5f;
            Debug.Log("No Save");
        }
        else
        {
            Debug.Log(PlayerPrefs.GetInt("completed"));
            cont.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alpha = 1f;
        }
    }

    void startGame()
    {
        SceneManager.LoadScene("Level1");
        PlayerPrefs.DeleteAll();
    }

    void continueGame()
    {
        switch (PlayerPrefs.GetInt("completed"))
        {
            case 1:
                SceneManager.LoadScene("Level2");
                break;
            case 2:
                SceneManager.LoadScene("Level3");
                break;
            case 3:
                SceneManager.LoadScene("Level4");
                break;
            case 4:
                SceneManager.LoadScene("Level5");
                break;
            case 5:
                SceneManager.LoadScene("Level6");
                break;
        }
    }

    void openOptions()
    {
        optionsMenu.SetActive(true);
    }
}
