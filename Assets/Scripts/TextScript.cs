using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject panel;
    public GameObject oskar;
    public GameObject player;
    private float messageNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Tim... You have to find the crystal. Its somewhere in this lab. Use [WASD] to navigate.";
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = oskar.transform.position - player.transform.position;
        if (Mathf.Abs(pos.x) < 3 && Mathf.Abs(pos.y) < 3)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(panel.activeSelf == true && messageNum < 7)
                {
                    switch (messageNum)
                    {
                        case 1:
                            messageNum++;
                            text.text = "Beware, there are enemies. You can shoot them with [left click] and avoi their bullets by teleporting using [right click].";
                            break;
                        case 2:
                            messageNum++;
                            text.text = "If you find another weapon, you can equip it with the number keys.";
                            break;
                        case 3:
                            messageNum++;
                            text.text = "You see those clocks on the top left? Thats your health. You will only heal at the end of a level.";
                            break;
                        case 4:
                            messageNum++;
                            text.text = "Some doors may be closed because of the power. You may need to find a generator.";
                            break;
                        case 5:
                            messageNum++;
                            text.text = "You should get going before our enemies get the crystal first. Once you find it return to the time machine.";
                            break;
                        case 6:
                            panel.SetActive(false);
                            break;
                    }
                }
                else if(panel.activeSelf == false)
                {
                    panel.SetActive(true);
                    Debug.Log("TExt");
                }
            }
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
