using UnityEngine;

public class SpriteScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public Vector2 initial_velocity;
    public Vector2 added_velocity;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = initial_velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb2d.AddForce(initial_velocity + added_velocity);
    }
}