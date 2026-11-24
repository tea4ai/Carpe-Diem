using UnityEngine;

public class Player : Entity
{
    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 7;
    private float xInput;
    private bool canJump = true;
    [Header("Attack Details")]
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown;
    private float attackTimer;
    [Header("HP slider")]
    [SerializeField] HealthBar health;

    protected override void Awake()
    {
        health.setMaxHealth(maxHealth);
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
        HandleInput();
        HandleAttack();

    }
    protected override void HandleAttack()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && attackTimer <= 0)
        {
            anim.SetTrigger("Attack");
            TryToAttack();
        }
    }
    protected override void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
           rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }
    private void TryToAttack()
    {
        
        if (fireballPrefab == null)
            return;
        if (attackTimer > 0)
            return;
        attackTimer = attackCooldown;
        Vector2 dir;
        if (facingDir == -1)
        {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.right;
        }
        Vector3 spawnPos = firePoint.position;
        Fireball fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        fireball.Init(dir);
    }

    protected override void Die()
    {
        health.setHealth(0);
        // rb.simulated = false;
        anim.SetTrigger("dead");
        base.Die();
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        if (GameOver.instance != null)
        {
            GameOver.instance.EnableGameOver();
        }
    }
    public override void TakeDamage()
    {
        base.TakeDamage();
        health.setHealth(currentHealth);
    }
    public void ResetPlayer()
    {
        this.enabled = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        currentHealth = maxHealth;
        health.setHealth(currentHealth);

        anim.Rebind();
        anim.Update(0f);

        EnableMovement(true);
        Debug.Log("Player physics reset and teleported.");
    }
}
