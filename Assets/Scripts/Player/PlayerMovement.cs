using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    private Rigidbody2D playerRig;
    private Animator playerAnim;
    

    [Header("Player Moviments Settings")]
    private Vector2 moveInput;
    [SerializeField] private float playerSpeed;
    private float playerCurrentSpeed;
    [SerializeField] private float playerJumpSpeed;
    [SerializeField] private float playerClimbSpeed;
    [SerializeField] private Vector2 playerDieDirection;
    private float currentPlayerGravity;
    private CapsuleCollider2D playerBodyCollider;
    private BoxCollider2D playerFeetCollider;
    private bool isAlive = true;
    private bool isClimbing;

    [Header("Player Attack")]
    [SerializeField] private bool isAttacking;

    [Header("Player Sounds")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dieSound;

    public bool _isAttacking
    {
        get { return isAttacking; }
        private set { isAttacking = value; }
    }

    private void Awake()
    {
        isAttacking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        currentPlayerGravity = playerRig.gravityScale;
        playerCurrentSpeed = playerSpeed;


    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {  return; }

           OnRun();
           FlipSprite();
           OnClimbLadder();
           Die();
           PlayerAttacks();
        
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
       
        moveInput = value.Get<Vector2>();
        
    }


    private void OnRun()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * playerCurrentSpeed, playerRig.velocity.y);
        playerRig.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRig.velocity.x) > Mathf.Epsilon;

        playerAnim.SetBool("isWalking", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRig.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRig.velocity.x), 1f);
        }
    }

    private void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            playerRig.velocity += new Vector2(0f, playerJumpSpeed);
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void OnClimbLadder()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            playerRig.gravityScale = currentPlayerGravity;
            isClimbing = false;
            playerAnim.SetBool("isClimbing", false);
            return ;
        }

        if (!isAttacking)
        {
            Vector2 climbVelocity = new Vector2(playerRig.velocity.x, moveInput.y * playerClimbSpeed);
            playerRig.velocity = climbVelocity;
            playerRig.gravityScale = 0f;


            bool playerHasVerticalSpeed = Mathf.Abs(playerRig.velocity.y) > Mathf.Epsilon;
            isClimbing = playerHasVerticalSpeed;
            playerAnim.SetBool("isClimbing", isClimbing);
            
        }
        
    }

    private void PlayerAttacks()
    {

        bool FireButton1 = Input.GetKeyDown(KeyCode.K);
        bool FireButtonStop1 = Input.GetKeyUp(KeyCode.K);
        bool FireButton2 = Input.GetMouseButtonDown(0);
        bool FireButtonStop2 = Input.GetMouseButtonUp(0);

        if (!isClimbing)
        {

            if (FireButton1 || FireButton2)
            {
                isAttacking = true;
            }
            else if (FireButtonStop1 || FireButtonStop2)
            {
                isAttacking = false;
            }

            playerAnim.SetBool("isAttacking", isAttacking);
        }
    }

    public void PlayerBulletInstantiate()
    {
        PlayerAttack playerBullet = GetComponentInChildren<PlayerAttack>();
        playerBullet.PlayerShoot();
    }

    void Die()
    {
        bool playerCollisionEnemies = playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"));
        bool playerCollisionWater = playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water"));

        if (playerCollisionEnemies)
        {
            audioSource.PlayOneShot(dieSound);
            isAlive = false;
            playerAnim.SetTrigger("die");
            playerBodyCollider.enabled = false;
            playerFeetCollider.enabled = false;
            float Horizontal = Random.Range(-2, 2);
            float Vertical = Random.Range(2, 4);
            playerDieDirection = new Vector2(Horizontal, Vertical);
            playerRig.velocity = playerDieDirection * playerCurrentSpeed;
            string playerDeath = "PlayerDeath";
            SpriteRenderer playerPos = GetComponent<SpriteRenderer>();
            playerPos.sortingLayerName = playerDeath;
            Invoke("ResetScene", 1f);
        }
        else if(playerCollisionWater)
        {
            isAlive = false;
            Invoke("ResetScene", 1f);    
        }   
    }

    private void ResetScene()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
