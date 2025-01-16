using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class Bot1Controller : Agent
{
    [Header("Car Settings")]
    [SerializeField] private float constantSpeed = 1.8f;      // The constant speed of the car
    [SerializeField] private float collisionSpeed = 0.8f;     // Speed when the car collides
    [SerializeField] private float accelerationRate = 0.4f;   // Speed recovery rate after collision
    [SerializeField] private float steeringSpeed = 200f;      // Steering sensitivity

    private float currentSpeed;
    private bool isColliding = false;

    private Rigidbody2D rb;
    
    private float rewardTimer;

    [SerializeField] private Transform[] botCheckpoints;      // List of checkpoint positions
    private int nextCheckpoint;                               // Index of the next checkpoint

    private void Start()
    {
        rewardTimer = 0f;
        nextCheckpoint = 1;
    }

    private void Update()
    {
        //Debug.Log("Collider(s) inside bot's trigger: " + colliderCount);
        //Debug.Log("Next checkpoint:" + nextCheckpoint);
    }

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = constantSpeed;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(4f, -6.2f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 42.1f);

        rewardTimer = 0f;
        nextCheckpoint = 1;

        Debug.Log("OnEpisodeBegin called.");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Direction to the next checkpoint
        Vector2 botCheckpointDirection = (botCheckpoints[nextCheckpoint].position - transform.position).normalized;
        sensor.AddObservation(botCheckpointDirection);

        sensor.AddObservation(transform.rotation.eulerAngles.z);

        //Debug.Log("Observations collected.");
    }

    Vector2 trackDirection;
    Vector2 velocityDirection;
    float dotProduct;
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the steering action from the discrete action space
        int steeringAction = actions.DiscreteActions[0];

        // Apply steering
        HandleSteering(steeringAction);

        MoveForward();
        RecoverSpeed();

        trackDirection = (botCheckpoints[nextCheckpoint].position - botCheckpoints[nextCheckpoint - 1].position).normalized;
        //Debug.Log("trackDirection: " + trackDirection);
        velocityDirection = rb.velocity.normalized;
        //Debug.Log("velocityDirection: " + velocityDirection);
        dotProduct = Vector2.Dot(velocityDirection, trackDirection);

        if (dotProduct > 0.9)
        {
            AddReward(dotProduct * 0.1f); // Reward for forward motion
            //Debug.Log("Progress made. Reward: " + dotProduct * 0.1f);
        }
        else
        {
            AddReward(Mathf.Min(-0.01f, dotProduct * 0.5f)); // Penalize off-track and backward motion
            //Debug.Log("Backward. Reward: " + Mathf.Min(-0.01f, dotProduct * 0.5f));
        }

        rewardTimer += Time.deltaTime;
        if (rewardTimer >= 1f)
        {
            if (currentSpeed == constantSpeed)
            {
                AddReward(1.0f);
                //Debug.Log("Stay on track. Reward: 1.0");
            }

            if (currentSpeed == collisionSpeed)
            {
                AddReward(-1.0f);
                //Debug.Log("Stuck too long. Reward: -1.0");
            }

            rewardTimer = 0f;
        }

        //Debug.Log("OnActionReceived called."); // Called every frame
    }

    private void HandleSteering(int steeringAction)
    {
        float horizontalInput = 0;

        // Discrete action mapping: 0 = no turn, 1 = turn right, 2 = turn left
        if (steeringAction == 1)
        {
            horizontalInput = -1; // Turn right
        }
        else if (steeringAction == 2)
        {
            horizontalInput = 1; // Turn left
        }

        // Apply rotation based on input
        if (horizontalInput != 0)
        {
            rb.rotation += horizontalInput * steeringSpeed * Time.deltaTime;
        }
    }

    private void MoveForward()
    {
        Vector2 forwardDirection = transform.right;
        rb.velocity = forwardDirection * currentSpeed;
    }

    private void RecoverSpeed()
    {
        if (!isColliding && currentSpeed < constantSpeed)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
            if (currentSpeed > constantSpeed)
            {
                currentSpeed = constantSpeed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Checkpoint"))
        {
            if (other.gameObject.CompareTag("FinishLine"))
            {
                AddReward(20f);
                Debug.Log("Finish line reached! Reward: 20");
                //EndEpisode();
                //Debug.Log("EndEpisode called.");
            }

            else
            {
                isColliding = true;
                currentSpeed = collisionSpeed;
                AddReward(-0.5f);
                //Debug.Log("Collided. Reward: -0.5");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isColliding = false;
        AddReward(0.3f);
        //Debug.Log("Escaped. Reward: 0.3");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BotTrack"))
        {
            string otherObjectName = other.gameObject.name;

            // Remove "track " from the name to get the number
            string trackNumberString = otherObjectName.Replace("track ", "").Trim();

            // Convert the number string to an integer
            if (int.TryParse(trackNumberString, out int trackNumber))
            {
                Debug.Log("Track number: " + trackNumber);
            }

            nextCheckpoint = trackNumber + 1;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 1; // Turn right
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 2; // Turn left
        }
        else
        {
            discreteActions[0] = 0; // No input
        }
    }
}
