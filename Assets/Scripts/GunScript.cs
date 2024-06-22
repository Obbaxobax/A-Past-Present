using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject hand;

    //Stats
    public float damage;
    public float firerate;
    public float totalAmmo;
    public float bulletSpeed;
    public GameObject bulletSprite;
    public float bulletSpread;
    public float bulletsFired;

    //Gun control
    private bool canShoot = true;
    private float ammoInClip;
    public TextMeshProUGUI ammo;

    // Start is called before the first frame update
    void Start()
    {
        ammoInClip = totalAmmo;
        ammo.text = ammoInClip + "/" + totalAmmo;
        ammo.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        ammo.text = ammoInClip + "/" + totalAmmo;
        rotateToMouse();

        if (Input.GetMouseButtonDown(0))
        {
                shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(reload());
        }
    }

    void rotateToMouse()
    {
        Vector3 diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - hand.transform.position).normalized;
        float roat = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
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

    void shoot()
    {
        if (canShoot && ammoInClip > 0)
        {
            for (int i = 0; i < bulletsFired + 1; i++)
            {
                GameObject bullet = Instantiate(bulletSprite, gun.transform.GetChild(0).transform.position, bulletSprite.transform.rotation);
                Vector2 direction = gun.transform.right;
                float ran = Random.Range(-bulletSpread / 10, bulletSpread / 10);
                direction.x -= ran;
                direction.y += ran;
                direction = direction.normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
                bullet.GetComponent<BulletScript>().damage = damage;
            }
            ammoInClip--;
            StartCoroutine(shootCooldown());
        }
    }

    IEnumerator shootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(firerate / 1000);
        if(ammo.color != Color.red)
        {
            canShoot = true;
        }
    }

    IEnumerator reload()
    {
        canShoot = false;
        ammo.color = Color.red;
        yield return new WaitForSeconds(firerate / 200);
        ammo.color = Color.white;
        ammoInClip = totalAmmo;
        canShoot = true;
    }
}
