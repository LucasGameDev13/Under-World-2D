using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemyRig;

    [SerializeField] private float enemySpeed;


    // Start is called before the first frame update
    void Start()
    {
        enemyRig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRig.velocity = new Vector2(enemySpeed, enemyRig.velocity.y);
    }

    private void FlipEnemyFacing()
    {
 
       transform.localScale = new Vector2(-(Mathf.Sign(enemyRig.velocity.x)), 1f);
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        enemySpeed = -enemySpeed;
        FlipEnemyFacing();
    }
}
