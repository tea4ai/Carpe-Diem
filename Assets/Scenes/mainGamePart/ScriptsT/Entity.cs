using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack Details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Collision Details")]
    [SerializeField] protected float groundCheckDistance;
    protected bool isGrounded;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    [Header("Sound Details")]
    [SerializeField] protected AudioClip[] entityClips; //hurt, dead, attack, walk, spawn(P), jump(P), land(P);
    [SerializeField] protected string[] keysForEC;      //hurt, dead, attack, walk, spawn(P), jump(P), land(P);
    public Dictionary<string, AudioClip> entityClipsDictionary;
    [Header("Visibility")]
    [SerializeField] public bool isVisible;

    //Facing details
    protected int facingDir = 1;
    protected bool facingRight = true;
    protected bool canMove = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        entityClipsDictionary = new Dictionary<string, AudioClip>();
        initializeDictionary();
    }


    protected virtual void Update()
    {
        HandleCollision();
        HandleAnimations();
    }
    protected virtual void FixedUpdate()
    {
        HandleMovement();
        HandleFlip();
    }
    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    public virtual void TakeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            canMove = false;
            PlayDamageFeedback();
          
        }

    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
        {
            StopCoroutine(damageFeedbackCoroutine);
        }

        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        canMove = false;
        anim.SetBool("hurt", true);

        rb.linearVelocity = new Vector2(-2f * facingDir, rb.linearVelocity.y);

        yield return new WaitForSeconds(damageFeedbackDuration);

        anim.SetBool("hurt", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        canMove = true;
    }

    protected virtual void Die()
    {
        /*
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        */
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;

        GetComponent<CapsuleCollider2D>().enabled = false;
        int gameOverLayer = LayerMask.NameToLayer("GameOver");
        if (gameOverLayer != -1) gameObject.layer = gameOverLayer;
        
    }

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected virtual void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }
    protected virtual void HandleAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
    protected virtual void HandleMovement()
    {
    }
    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    protected virtual void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }
    protected void initializeDictionary()
    {
        for (int i = 0; i < entityClips.Length; i++)
        {
            entityClipsDictionary.Add(keysForEC[i], entityClips[i]);
        }
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Debug.Log("Player hit the water!");
            Die();
        }
    }
    protected virtual void OnBecameVisible() => isVisible = true;
    protected virtual void OnBecameInvisible() => isVisible = false;
}