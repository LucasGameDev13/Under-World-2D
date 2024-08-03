using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerScript;
    [SerializeField] private GameObject bulletProf;
    private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponentInParent<PlayerMovement>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void PlayerShoot()
    {
        if(playerScript._isAttacking)
        {
            GameObject bulletInstance = Instantiate(bulletProf, transform.position, transform.rotation);
            audioSource.PlayOneShot(shootSound);
            Destroy(bulletInstance, 3f);
        }
    }
}
