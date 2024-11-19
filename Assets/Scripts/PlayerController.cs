using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Car Settings")]
    public float constantSpeed = 5f;       // The constant speed of the car
    public float slowDownSpeed = 2f;      // Speed when the car collides
    public float accelerationRate = 1f;  // Speed recovery rate after collision
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
        float horizontalInput = 0;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
        }

        // Rotate the car
        rb.rotation += horizontalInput * steeringSpeed * Time.deltaTime;
    }

    private void MoveForward()
    {
        // Move the car forward based on its current speed
        rb.velocity = transform.up * currentSpeed;
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
