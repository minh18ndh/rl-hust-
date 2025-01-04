using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float constantSpeed = 1f;        // The constant speed of the car
    [SerializeField] private float collisionSpeed = 0.2f;     // Speed when the car collides
    [SerializeField] private float accelerationRate = 0.1f;   // Speed recovery rate after collision
    [SerializeField] private float steeringSpeed = 200f;      // Steering sensitivity

    private float currentSpeed;
    private bool isColliding = false;

    private Rigidbody2D rb;

    [SerializeField] private GameObject FinishLine;
    private GameManager gmScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = constantSpeed;

        gmScript = FinishLine.GetComponent<GameManager>();
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
        // Because the car races in rear mode, press A to turn right and D to turn left
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
        // Move the car forward along its head-to-tail vector
        // The car's forward direction is along its local +X-axis
        Vector2 forwardDirection = transform.right;  // Head to tail along the local +X-axis
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        isColliding = true;
        currentSpeed = collisionSpeed; // Reduce speed on collision
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isColliding = false; // Resume acceleration after exiting collision
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            //Debug.Log("Collided with: " + other.name);
            gmScript.CheckpointReached();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint") && gmScript != null && gmScript.isCheckpointPassed)
        {
            Destroy(other.gameObject);
        }
    }
}
