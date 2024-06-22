using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Positioning
    [Header("Movement")]
    public Transform player;
    private Transform enemyTransform;
    private Vector3[] points;
    GameObject closest;
    public LayerMask wall;

    //Weapon
    [Header("Weapon")]
    public float damage;
    public float firerate;
    public GameObject bullet;
    public float bulletSpeed;
    public GameObject spawn;
    public float bulletsFired;
    public float maxAmmo;
    public float bulletSpread;
    private float ammoInClip;

    //Stats
    [Header("Stats")]
    public float speed;
    public float maxHealth;
    private float health;
    public bool hasUpgrade;
    public string upgrade;
    public bool hasCrystal;
    public GameObject crystalPrefab;

    //Other
    public bool active;
    private bool isShooting;

    //Sprites
    [Header("Sprites")]
    public GameObject hand;
    public GameObject gun;
    public Sprite front;
    public Sprite back;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        enemyTransform = gameObject.GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        isShooting = false;
        ammoInClip = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            if (isShooting == false)
            {
                isShooting = true;
                StartCoroutine(shoot());
            }
            //Get player direction
            Vector2 distance = player.position - enemyTransform.position;
            Vector2 direction = distance.normalized;

            //Movement Shenanigans
            RaycastHit2D ray = Physics2D.Raycast(enemyTransform.position, direction, Mathf.Infinity, wall);
            Vector2 pos = new Vector2(1000, 1000);
            if (ray)
            {
                if (ray.transform.CompareTag("Counter"))
                {
                    for(int i = 0; i < ray.transform.childCount - 1; i++)
                    {
                        Transform child = ray.transform.GetChild(i);
                        Vector2 dis = player.transform.position - child.position;
                        if(Mathf.Abs(dis.x) < Mathf.Abs(pos.x) && Mathf.Abs(dis.y) < Mathf.Abs(pos.y))
                        {
                            pos = dis;
                            closest = child.gameObject;
                        }
                    }
                    RaycastHit2D ray2 = Physics2D.Raycast(pos, new Vector2(player.position.x, player.position.y) - pos, Mathf.Infinity, wall);
                    if (ray2)
                    {
                        switch (closest.name)
                        {
                            case "GameObject":
                                Vector2 dir = ray.transform.Find("GameObject (1)").transform.position - enemyTransform.position;
                                move(dir.normalized);
                                break;
                            case "GameObject (1)":
                                Vector2 dir2 = ray.transform.Find("GameObject").transform.position - enemyTransform.position;
                                move(dir2.normalized);
                                break;
                            case "GameObject (2)":
                                Vector2 dir3 = ray.transform.Find("GameObject (1)").transform.position - enemyTransform.position;
                                move(dir3.normalized);
                                break;
                            case "GameObject (3)":
                                Vector2 dir4 = ray.transform.Find("GameObject").transform.position - enemyTransform.position;
                                move(dir4.normalized);
                                break;
                        }
                    }
                    else
                    {
                        Vector2 dir = new Vector2(enemyTransform.position.x, enemyTransform.position.y) - pos;
                        move(dir.normalized);
                    }
                }
                else
                {
                    move(direction.normalized);
                }
            }
            else
            {
                move(direction.normalized);
            }

            //Die
            if (health < 1)
            {
                if(gameObject.transform.name == "Boss" && hasUpgrade)
                {
                    PlayerPrefs.SetFloat("has" + upgrade, 1);
                    StartCoroutine(upgradeText());
                }
                else
                {
                    if (hasCrystal)
                    {
                        crystalPrefab.transform.position = transform.position;
                    }
                    Destroy(gameObject);
                }
            }

            //Set hand position and orientation
            handMovement();
        }
    }

    void move(Vector2 direction)
    {
        enemyTransform.Translate(direction * speed * Time.deltaTime);
    }

    void handMovement()
    {
        Vector2 playerDirection = (player.position - enemyTransform.position).normalized;

        if(playerDirection.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }else if (playerDirection.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (playerDirection.y > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = back;
            hand.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Idk";
            gun.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Idk";
        }
        else if (playerDirection.y < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = front;
            hand.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            gun.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        }

        hand.transform.position = new Vector2(playerDirection.x * 0.75f + enemyTransform.position.x, playerDirection.y * 0.3f + enemyTransform.position.y);

        float roat = Mathf.Atan2(playerDirection.x, playerDirection.y) * Mathf.Rad2Deg;
        float y = 0f;
        if (roat < 0)
        {
            y = 180f;
            hand.transform.rotation = Quaternion.Euler(0f, y, roat + 90);
        }
        else
        {
            hand.transform.rotation = Quaternion.Euler(0f, y, -roat + 90);
        }
    }

    void strafe()
    {
        int random = Random.Range(-1, 1);
        if (random > 0)
        {
            enemyTransform.Translate(player.up * Random.Range(-1, 1) * Time.deltaTime * speed);
        }
        else if (random < 0)
        {
            enemyTransform.Translate(player.right * Random.Range(-1, 1) * Time.deltaTime * speed);
        }

    }

    IEnumerator shoot()
    {
        while (true)
        {
            if (ammoInClip > 0)
            {
                yield return new WaitForSeconds(firerate / 1000);
                for (int i = 0; i < bulletsFired + 1; i++)
                {
                    GameObject bullets = Instantiate(bullet, spawn.transform.position, bullet.transform.rotation);

                    //Set Bullet values
                    if(bullets.GetComponent<BulletScript>().fire == false)
                    {
                        bullets.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    bullets.GetComponent<BulletScript>().enemy = true;
                    bullets.GetComponent<BulletScript>().damage = damage;

                    //Move Bullet
                    Vector2 direction = (player.position - enemyTransform.position).normalized;
                    float ran = Random.Range(-bulletSpread / 10, bulletSpread / 10);
                    direction.x -= ran;
                    direction.y += ran;
                    direction = direction.normalized;
                    bullets.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
                }
                ammoInClip--;
            }
            else
            {
                yield return new WaitForSeconds(firerate / 500);
                ammoInClip = maxAmmo;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (collision.GetComponent<BulletScript>().enemy == false)
            {
                health -= collision.GetComponent<BulletScript>().damage;
                Destroy(collision.gameObject);
            }
        }
    }

    private Vector2 checkPoints(float dist, GameObject test, RaycastHit2D ray)
    {
        for (int i = 0; i < dist / 2; i--)
        {
            test.transform.Translate(Vector2.down * 2);

            RaycastHit2D test2 = Physics2D.Raycast(test.transform.position, Vector2.right);
            if (test2.point.x > ray.point.x)
            {
                return test2.point;
            }
        }
        return Vector2.zero;
    }

    private void pathFind(Vector2 direction)
    {
        RaycastHit2D ray = Physics2D.Raycast(enemyTransform.position, direction);

        if (ray)
        {
            if (ray.collider.transform.CompareTag("Wall"))
            {
                if (ray.collider.bounds.size.y > ray.collider.bounds.size.x)
                {
                    //Make object for pathing test
                    GameObject test = new GameObject();
                    test.transform.position = new Vector2(ray.point.x - 1, ray.point.y);

                    //Get max height
                    Vector2 upperBounds = test.transform.position;
                    Vector2 lowerBounds = test.transform.position;
                    RaycastHit2D bounds = Physics2D.Raycast(test.transform.position, Vector2.up);

                    if (bounds && bounds.transform.CompareTag("Wall"))
                    {
                        upperBounds = bounds.point;
                    }

                    bounds = Physics2D.Raycast(test.transform.position, Vector2.down);
                    if (bounds && bounds.transform.CompareTag("Wall"))
                    {
                        lowerBounds = bounds.point;
                    }

                    float totalDist = upperBounds.y - lowerBounds.y;
                    test.transform.position = upperBounds;

                    //Check every 2 units if can get closer to player
                    Vector2 points2 = checkPoints(totalDist, test, ray);
                    if (points2 != Vector2.zero)
                    {
                        points.SetValue(points2, points.Length + 1);
                        test.transform.position = points2;

                        Vector2 toPlayer = new Vector2(player.position.x - points2.x, player.position.y - points2.y);
                        pathFind(toPlayer);
                    }
                }else if(ray.collider.bounds.size.y < ray.collider.bounds.size.x)
                {
                    //Make object for pathing test
                    GameObject test = new GameObject();
                    test.transform.position = new Vector2(ray.point.x, ray.point.y - 1);

                    //Get max height
                    Vector2 upperBounds = test.transform.position;
                    Vector2 lowerBounds = test.transform.position;
                    RaycastHit2D bounds = Physics2D.Raycast(test.transform.position, Vector2.right);

                    if (bounds && bounds.transform.CompareTag("Wall"))
                    {
                        upperBounds = bounds.point;
                    }

                    bounds = Physics2D.Raycast(test.transform.position, Vector2.left);
                    if (bounds && bounds.transform.CompareTag("Wall"))
                    {
                        lowerBounds = bounds.point;
                    }

                    float totalDist = upperBounds.x - lowerBounds.x;
                    test.transform.position = upperBounds;

                    //Check every 2 units if can get closer to player
                    Vector2 points2 = checkPoints(totalDist, test, ray);
                    if (points2 != Vector2.zero)
                    {
                        points.SetValue(points2, points.Length + 1);
                        test.transform.position = points2;

                        Vector2 toPlayer = new Vector2(player.position.x - points2.x, player.position.y - points2.y);
                        pathFind(toPlayer);
                    }
                }
            }
        }
        else
        {
            
        }
    }

    void moveTowardsPoints()
    {

    }

    IEnumerator upgradeText()
    {
        text.SetActive(true);
        gameObject.transform.position = new Vector2(10000, 10000);
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
        Destroy(gameObject);
    }
}
