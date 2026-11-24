using UnityEngine;
public class Enemy : Entity
{
    
    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 8f;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float detectionDistance = 1f;
    private RaycastHit2D wallHit;
    private RaycastHit2D groundAhead;

    [Header("Chaser settings")]
    [SerializeField]protected float viewRange;
    [SerializeField]protected float chasingRange;
    private bool plrDetected;
    
    //patroller settings
    private Vector3 leftBoundary;
    private Vector3 rightBoundary;
    private Vector3 currentPatrolTarget;
     
    private Transform plrPos = null;
    private bool plrInAttackRange;
    private bool isPatroller;
    
    protected override void Awake()
    {

        base.Awake();
        Player player = FindFirstObjectByType<Player>();

        if (player != null)
            plrPos = player.transform;

        isPatroller = gameObject.CompareTag("Patroller");

        if (isPatroller)
        {
            leftBoundary = transform.position + new Vector3(-10, 0, 0);
            rightBoundary = transform.position + new Vector3(10, 0, 0);
            currentPatrolTarget = leftBoundary;
        }

    }
    protected override void Update()
    {
        if (!plrPos || currentHealth <= 0 || !canMove)
        {
            if (!canMove && currentHealth > 0)
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        HandleCollision();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
        HandleAttack();
    }
    protected override void HandleAttack()
    {
        if (plrInAttackRange)
        {
            anim.SetTrigger("attack");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected override void HandleMovement()
    {
        
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        HandleJumpment();

        bool shouldChase = false;

        if (isPatroller)
        {
            // Check if plrPos still exists before getting distance
            if (plrPos != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, plrPos.position);
                if (plrDetected || distanceToPlayer <= chasingRange / 2)
                {
                    shouldChase = true;
                }
            }
        }
        else
        {
            shouldChase = true;
        }

        if (shouldChase)
        {
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
        }
        else if (isPatroller)
        {
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);

            if (Mathf.Abs(transform.position.x - currentPatrolTarget.x) < 0.5f)
            {
                currentPatrolTarget = (currentPatrolTarget == leftBoundary) ? rightBoundary : leftBoundary;
            }
        }
    }
    private void HandleJumpment()
    {
        
        wallHit = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionDistance, whatIsGround);

        Vector2 gapCheckStart = new Vector2(transform.position.x + (facingDir * detectionDistance), transform.position.y);
        groundAhead = Physics2D.Raycast(gapCheckStart, Vector2.down, 2f, whatIsGround);

        if (isGrounded)
        {
            if (wallHit.collider != null || groundAhead.collider == null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        plrInAttackRange = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);

        if (isPatroller)
        {
            plrDetected = Physics2D.OverlapBox(transform.position, new Vector2(viewRange, 3f), 0f, whatIsTarget);
        }
    }
    protected override void HandleAnimations()
    {
        if (anim != null)
        {
            float speedForAnim = canMove ? rb.linearVelocity.x : 0f;
            anim.SetFloat("xVelocity", speedForAnim);
        }
    }
    protected override void HandleFlip()
    {

        bool targetIsPlayer = false;

        if (isPatroller)
        {
            if (plrPos != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, plrPos.position);
                if (plrDetected || distanceToPlayer <= chasingRange / 2) targetIsPlayer = true;
            }
        }
        else
        {
            targetIsPlayer = true;
        }

        if (targetIsPlayer)
        {
            if (plrPos.position.x < transform.position.x && facingRight) Flip();
            else if (plrPos.position.x > transform.position.x && !facingRight) Flip();
        }
        else if (isPatroller)
        {
            if (currentPatrolTarget.x < transform.position.x && facingRight) Flip();
            else if (currentPatrolTarget.x > transform.position.x && !facingRight) Flip();
        }
    }
    protected override void Die()
    {
        base.Die();
        anim.Play("enemyDie");
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(viewRange > 0f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector2(viewRange, transform.position.y+1));
            //chasing range
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector2(chasingRange, transform.position.y));
            //raycast to wall
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector2.right * facingDir * detectionDistance);
            //raycast to gap
            Gizmos.color = Color.blue;
            Vector3 gapCheckPos = new Vector3(transform.position.x + (facingDir * detectionDistance), transform.position.y, 0);
            Gizmos.DrawRay(gapCheckPos, Vector2.down * 2f);
        }
    }
}