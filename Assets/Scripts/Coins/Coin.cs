using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound;
    private Animator coinAnim;
    [SerializeField] private int myScore;

    private void Start()
    {
        coinAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            coinAnim.SetTrigger("picked");
            AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
            Destroy(gameObject, 0.3f);
            FindObjectOfType<GameSession>().GameScore(myScore);
        }
    }

    
}
