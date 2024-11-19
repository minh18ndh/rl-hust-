using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Car Settings")]
    public float constantSpeed = 1f;       // The constant speed of the car
    public float slowDownSpeed = 0.5f;      // Speed when the car collides
    public float accelerationRate = 0.5f;  // Speed recovery rate after collision
    public float steeringSpeed = 200f;   // Steering sensitivity

    private float currentSpeed;
    private bool isColliding = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = constantSpeed;
    }

    private void Update()
    {
        HandleSteering();
    }

    private void FixedUpdate()
    {
        MoveForward();
        RecoverSpeed();
    }

    private void HandleSteering()
    {
        // Only rotate if A or D is pressed
        float horizontalInput = 0;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
        }

        // Rotate the car if there's input
        if (horizontalInput != 0)
        {
            rb.rotation += horizontalInput * steeringSpeed * Time.deltaTime;
        }
    }

    private void MoveForward()
    {
        // Move the car forward along its "forward" direction (tail to head)
        // The car's forward direction is along its local X-axis
        Vector2 forwardDirection = transform.right;  // Tail to head along the local X-axis
        rb.velocity = forwardDirection * currentSpeed;
    }

    private void RecoverSpeed()
    {
        if (!isColliding && currentSpeed < constantSpeed)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
            if (currentSpeed > constantSpeed)
            {
                currentSpeed = constantSpeed; // Clamp to max speed
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        currentSpeed = slowDownSpeed; // Reduce speed on collision
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false; // Resume acceleration after exiting collision
    }
}
