using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenControl : MonoBehaviour
{
    public Button startOver;

    // Start is called before the first frame update
    void Start()
    {
        startOver.onClick.AddListener(backToMenu);
    }

    void backToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
