using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Transforms for positioning
    [Header("Transforms")]
    public Transform player;
    public Transform hand;
    private Transform gun;

    //Movement
    [Header("Movement")]
    private float horizontal;
    private float vertical;
    private bool dodgeUsable;
    private float speed = 100000f;

    //Health
    [Header("Health")]
    public float health;
    public float maxHealth;
    public GameObject healthBar;

    //Sprites
    [Header("Sprites")]
    public Sprite front;
    public Sprite back;
    public ParticleSystem teleportParticles;

    //hand
    private GameObject pos1;
    private GameObject pos2;
    private string equippedGun = "Pistol";

    //collision
    private bool horizontalCollision;
    private bool verticalCollision;

    //Interactables
    [Header("Interactables")]
    public GameObject generator;
    public GameObject interactText;
    public bool hasCrystal;
    public GameObject timeMachine;
    public GameObject crystal;
    public GameObject oskar;

    [Header("Cutscene Stuff")]
    public Image fade;
    public GameObject panel;

    private void Start()
    {
        health = maxHealth;
        gun = hand.Find(equippedGun).transform;
        pos1 = GameObject.Find("Pos1");
        pos2 = GameObject.Find("Pos2");
        dodgeUsable = true;
        interactText.SetActive(false);
        for(int i = 0; i < hand.childCount; i++)
        {
            hand.GetChild(i).transform.gameObject.SetActive(false);
        }
        gun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //player.Translate(Vector2.up * speed * vertical * Time.deltaTime);
        //player.Translate(Vector2.right * speed * horizontal * Time.deltaTime);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * vertical * speed * Time.deltaTime, ForceMode2D.Force);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);

        //sprites
        setSprite();

        //dodge
        if (Input.GetMouseButtonDown(1)) { dodge(); }

        //Check health
        if(health < 1)
        {
            SceneManager.LoadScene("Gameover");
        }

        //Interact
        Vector2 genDis = new Vector2(Mathf.Abs(player.position.x - generator.transform.position.x), Mathf.Abs(player.position.y - generator.transform.position.y));
        Vector2 crystDis = new Vector2(Mathf.Abs(player.position.x - crystal.transform.position.x), Mathf.Abs(player.position.y - crystal.transform.position.y));
        Vector2 oskDis = new Vector2(Mathf.Abs(player.position.x - oskar.transform.position.x), Mathf.Abs(player.position.y - oskar.transform.position.y));
        Vector2 timeDis = new Vector2(Mathf.Abs(player.position.x - timeMachine.transform.position.x), Mathf.Abs(player.position.y - timeMachine.transform.position.y));

        if(genDis.x < 6 && genDis.y < 6 && generator.GetComponent<GeneratorInfo>().active == false)
        {
            interactText.SetActive(true);
            listenForInteract();
        }else if(crystDis.x < 3 && crystDis.y < 3)
        {
            interactText.SetActive(true);
            pickUpCrystal();
        }else if(oskDis.x < 3 && oskDis.y < 3)
        {
            interactText.SetActive(true);
        }else if(timeDis.x < 3 && timeDis.y < 3 && hasCrystal)
        {
            interactText.SetActive(true);
            StartCoroutine(endGame());
        }
        else
        {
            interactText.SetActive(false);
        }

        swapWeapons();
    }

    void setSprite()
    {
        //Get Directions and Positions
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 mouseDirection = new Vector2(mousePos.x - player.position.x, mousePos.y - player.position.y).normalized;

        if (mouseDirection.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (mouseDirection.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (mouseDirection.y > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = back;
            hand.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Idk";
            gun.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Idk";
        }
        else if (mouseDirection.y < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = front;
            hand.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            gun.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        }

        hand.transform.position = new Vector2(mouseDirection.x * 0.75f + player.transform.position.x, mouseDirection.y * 0.3f + player.transform.position.y);
    }

    void dodge()
    {
        if (dodgeUsable == true)
        {
            //Calculate direction
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 mouseDirection = new Vector2(mousePos.x - player.position.x, mousePos.y - player.position.y).normalized;

            LayerMask layerMask = LayerMask.GetMask("Wall");

            //Verify if wall is in way
            RaycastHit2D hit = Physics2D.Raycast(player.position, mouseDirection, 5, layerMask);

            if (hit && !hit.transform.CompareTag("Counter"))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Door"))
                {
                    Vector2 dist = new Vector2(player.position.x, player.position.y) - hit.point;
                    Vector2 norm = dist.normalized;
                    if(norm.x < 0)
                    {
                        norm.x = -1;
                    }
                    else
                    {
                        norm.x = 1;
                    }

                    if(norm.y < 0)
                    {
                        norm.y = -1;
                    }
                    else
                    {
                        norm.y = 1;
                    }
                    norm.x *= player.GetComponent<CapsuleCollider2D>().bounds.size.x / 2;
                    norm.y *= player.GetComponent<CapsuleCollider2D>().bounds.size.y / 2;
                    Vector2 yourMother = hit.point - -norm;

                    //Create Particles
                    ParticleSystem part = Instantiate(teleportParticles, player.position, teleportParticles.transform.rotation);
                    ParticleSystem part3 = Instantiate(teleportParticles, player.position, teleportParticles.transform.rotation);

                    //Move Particles
                    part.gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
                    part.Play();
                    part3.gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
                    part3.transform.position = yourMother;
                    part3.Play();

                    //Move
                    player.transform.position = yourMother;

                    //Start cooldown
                    StartCoroutine(removeParticle(part, part3));
                    StartCoroutine(dodgeCooldown());
                }
            }
            else
            {
                //Create Particles
                ParticleSystem part = Instantiate(teleportParticles, player.position, teleportParticles.transform.rotation);
                ParticleSystem part3 = Instantiate(teleportParticles, player.position, teleportParticles.transform.rotation);

                //Move Particles
                part.gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
                part.Play();
                part3.gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
                part3.transform.Translate(mouseDirection * 5);
                part3.Play();

                //Move
                player.transform.Translate(mouseDirection * 5);

                //Start cooldown
                StartCoroutine(removeParticle(part, part3));
                StartCoroutine(dodgeCooldown());
            }
        }
    }

    IEnumerator dodgeCooldown()
    {
        dodgeUsable = false;
        yield return new WaitForSeconds(1);
        dodgeUsable = true;
    }

    IEnumerator removeParticle(ParticleSystem part2, ParticleSystem part4)
    {
        yield return new WaitForSeconds(1f);
        Destroy(part2.gameObject);
        Destroy(part4.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        switch (collision.tag)
        {
            case "Bullet":
                if (collision.GetComponent<BulletScript>().enemy == true)
                {
                    Destroy(collision.gameObject);
                    health -= collision.GetComponent<BulletScript>().damage;
                    int temp = (int)health;
                    Transform health2 = healthBar.transform.GetChild(temp);
                    health2.gameObject.SetActive(false);
                }
                break;
            case "Door":
                if (collision.GetComponent<DoorControl>().tripped == 0)
                {
                    collision.GetComponent<DoorControl>().tripped = 1;
                }
                break;
            case "Pit":
                StartCoroutine(pitFall(collision.gameObject));
                break;
        }
    }

    void listenForInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            generator.GetComponent<GeneratorInfo>().active = true;
        }
    }

    void pickUpCrystal()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            hasCrystal = true;
            crystal.SetActive(false);
            crystal.transform.position += new Vector3(10000, 10000, 100000);
        }
    }

    IEnumerator endGame()
    {
        if (Input.GetKeyDown(KeyCode.E) && hasCrystal)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1":
                    fade.gameObject.SetActive(true);
                    var temp = fade.color;
                    for (float i = 0; i < 256; i++)
                    {
                        temp.a = i / 255f;
                        fade.color = temp;
                        yield return new WaitForSeconds(1 / 255);
                    }
                    PlayerPrefs.SetInt("completed", 1);
                    SceneManager.LoadScene("TimeMachine");
                    fade.gameObject.SetActive(false);
                    break;
                case "Level2":
                    fade.gameObject.SetActive(true);
                    var temp2 = fade.color;
                    for (float i = 0; i < 256; i++)
                    {
                        temp2.a = i / 255f;
                        fade.color = temp2;
                        yield return new WaitForSeconds(1 / 255);
                    }
                    PlayerPrefs.SetInt("completed", 2);
                    SceneManager.LoadScene("TimeMachine");
                    fade.gameObject.SetActive(false);
                    break;
                case "Level3":
                    fade.gameObject.SetActive(true);
                    var temp3 = fade.color;
                    for (float i = 0; i < 256; i++)
                    {
                        temp3.a = i / 255f;
                        fade.color = temp3;
                        yield return new WaitForSeconds(1 / 255);
                    }
                    PlayerPrefs.SetInt("completed", 3);
                    SceneManager.LoadScene("TimeMachine");
                    fade.gameObject.SetActive(false);
                    break;
                case "Level4":
                    fade.gameObject.SetActive(true);
                    var temp4 = fade.color;
                    for (float i = 0; i < 256; i++)
                    {
                        temp4.a = i / 255f;
                        fade.color = temp4;
                        yield return new WaitForSeconds(1 / 255);
                    }
                    PlayerPrefs.SetInt("completed", 4);
                    SceneManager.LoadScene("TimeMachine");
                    fade.gameObject.SetActive(false);
                    break;
                case "Level5":
                    fade.gameObject.SetActive(true);
                    var temp5 = fade.color;
                    for (float i = 0; i < 256; i++)
                    {
                        temp5.a = i / 255f;
                        fade.color = temp5;
                        yield return new WaitForSeconds(1 / 255);
                    }
                    PlayerPrefs.SetInt("completed", 5);
                    SceneManager.LoadScene("TimeMachine");
                    fade.gameObject.SetActive(false);
                    break;
            }
        }
    }

    void swapWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PlayerPrefs.GetFloat("hasShotgun") == 1)
            {
                equippedGun = "Shotgun";
                gun = hand.Find(equippedGun);
                for (int i = 0; i < hand.childCount; i++)
                {
                    hand.GetChild(i).transform.gameObject.SetActive(false);
                }
                gun.gameObject.SetActive(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            equippedGun = "Pistol";
            gun = hand.Find(equippedGun);
            for (int i = 0; i < hand.childCount; i++)
            {
                hand.GetChild(i).transform.gameObject.SetActive(false);
            }
            gun.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panel.activeSelf == true)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
            }
        }
    }

    IEnumerator pitFall(GameObject pit)
    {
        health--;
        int temp2 = (int)health;
        Transform health2 = healthBar.transform.GetChild(temp2);
        health2.gameObject.SetActive(false);
        fade.gameObject.SetActive(true);
        var temp = fade.color;
        for(float i = 0; i < 11; i++)
        {
            temp.a = i / 10f;
            fade.color = temp;
            yield return new WaitForSeconds(1 / 10);
        }
        player.position = pit.transform.GetComponent<PitFallSettings>().spawn.transform.position;
        for (float i = 0; i < 11; i++)
        {
            temp.a = 1f - (i / 10f);
            fade.color = temp;
            yield return new WaitForSeconds(1 / 10);
        }
        fade.gameObject.SetActive(false);
    }
}
