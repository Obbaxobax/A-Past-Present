using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;
    public bool enemy;
    public ParticleSystem bump;
    public bool fire;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            //Instantiate(bump, gameObject.transform.position, );
            Destroy(gameObject);
            
        }
    }

}
