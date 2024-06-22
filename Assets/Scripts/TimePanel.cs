using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimePanel : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;
    private float year = 2030;

    // Start is called before the first frame update
    void Start()
    {
        int complete = PlayerPrefs.GetInt("completed");
        year += complete * 5;
        float yer = year + 5;
        text.text = "Current Year: " + year + "   Moving To: " + yer;
        button.onClick.AddListener(moveToLevel);
    }

    void moveToLevel()
    {
        switch (year)
        {
            case 2030:
                SceneManager.LoadScene("Level1");
                break;
            case 2035:
                SceneManager.LoadScene("Level2");
                break;
            case 2040:
                SceneManager.LoadScene("Level3");
                break;
            case 2045:
                SceneManager.LoadScene("Level4");
                break;
            case 2050:
                SceneManager.LoadScene("Level5");
                break;
            case 2055:
                SceneManager.LoadScene("Level6");
                break;
        }
    }
}
