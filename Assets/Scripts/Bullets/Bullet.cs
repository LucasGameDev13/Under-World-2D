using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerMovement playerScript;
    private Rigidbody2D bulletRig;
    [SerializeField] private float bulletSpeed;
    private float bulletXSpeed;
    [SerializeField] private ParticleSystem bulletEffect;
    [SerializeField] private GameObject enemyEffect;

    // Start is called before the first frame update
    void Start()
    {
        bulletRig = GetComponent<Rigidbody2D>();

        playerScript = FindAnyObjectByType<PlayerMovement>();

        bulletXSpeed = playerScript.transform.localScale.x * bulletSpeed;
        
        if(bulletXSpeed > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        bulletEffect.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletRig.velocity = new Vector2(bulletXSpeed, 0f);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            GameObject enemyEffects = Instantiate(enemyEffect, collision.transform.position, transform.rotation);
            Destroy(enemyEffects, 0.7f);
            bulletEffect.Stop();
        }

        Destroy(gameObject);
        bulletEffect.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        bulletEffect.Stop();
    }
}
