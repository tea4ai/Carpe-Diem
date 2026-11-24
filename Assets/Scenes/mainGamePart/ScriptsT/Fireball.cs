using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    //[SerializeField] private float maxSpeed;
    //[SerializeField] private float lifetime = 3f;
    private Camera cam;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

    }
    public void Init(Vector2 dir)
    {
        Vector2 direction = dir.normalized;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        rb.linearVelocity = direction * speed;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player")))
            return;
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage();
        }
        Destroy(gameObject);


    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
        //if (Mouse.current.leftButton.isPressed)
        //{
        //    // Calculate mouse direction
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        //    Vector2 direction = (mousePos - transform.position).normalized;

        //    // Move player in direction of mouse
        //    transform.up = direction;
        //    rb.AddForce(direction * speed);
        //    if (rb.linearVelocity.magnitude > maxSpeed)
        //    {
        //       rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        //    }
        //}
