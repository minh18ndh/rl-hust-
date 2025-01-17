using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Bot2Controller : Agent
{
    [Header("Car Settings")]
    [SerializeField] private float constantSpeed;      // The constant speed of the car
    [SerializeField] private float collisionSpeed;     // Speed when the car collides
    [SerializeField] private float accelerationRate;   // Speed recovery rate after collision
    [SerializeField] private float steeringSpeed;      // Steering sensitivity

    private float currentSpeed;
    private bool isColliding = false;

    private Rigidbody2D rb;

    private float rewardTimer;

    [SerializeField] private Transform[] botCheckpoints;      // List of checkpoint positions
    private int nextCheckpoint;                               // Index of the next checkpoint

    //private int colliderCount;

    //[SerializeField] private GameObject UIManager;
    //private TimerController tmScript;

    private float previousDistanceToCheckpoint = float.MaxValue; // Initialize with a large value

    private void Start()
    {
        rewardTimer = 0f;
        nextCheckpoint = 1;

        //tmScript = UIManager.GetComponent<TimerController>();
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
        //tmScript.timer = 0f;

        transform.position = new Vector3(4f, -6.2f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 42.1f);

        rewardTimer = 0f;
        nextCheckpoint = 1;

        previousDistanceToCheckpoint = float.MaxValue;

        //Debug.Log("OnEpisodeBegin called.");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Direction to the next checkpoint
        Vector2 botCheckpointDirection = (botCheckpoints[nextCheckpoint].position - transform.position).normalized;
        sensor.AddObservation(botCheckpointDirection);

        sensor.AddObservation(transform.rotation.eulerAngles.z);

        // Calculate distance to the middle of the track
        Vector2 p1 = botCheckpoints[nextCheckpoint - 1].position;
        Vector2 p2 = botCheckpoints[nextCheckpoint].position;
        Vector2 p = transform.position;

        Vector2 checkpointVector = p2 - p1;
        Vector2 botVector = p - p1;

        // Perpendicular distance to the middle of the track
        float distanceToMiddle = Mathf.Abs(Vector3.Cross(checkpointVector, botVector).z) / checkpointVector.magnitude;

        // Add distance to the observation space
        sensor.AddObservation(distanceToMiddle);

        //Debug.Log("Observations collected.");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the steering action from the discrete action space
        int steeringAction = actions.DiscreteActions[0];

        // Apply steering
        HandleSteering(steeringAction);

        MoveForward();
        RecoverSpeed();

        CheckTrackAlignment();

        CheckProgressAndCollision();

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

    private float trackMiddleThreshold = 0.1f;

    // Check if bot is near the middle of the track
    private void CheckTrackAlignment()
    {
        Vector2 p1 = botCheckpoints[nextCheckpoint - 1].position;
        Vector2 p2 = botCheckpoints[nextCheckpoint].position;
        Vector2 p = transform.position;

        // Vector between checkpoints
        Vector2 checkpointVector = p2 - p1;

        // Vector from previous checkpoint to the bot
        Vector2 botVector = p - p1;

        // Perpendicular distance (cross product magnitude divided by checkpoint vector length)
        float distanceToMiddle = Mathf.Abs(Vector3.Cross(checkpointVector, botVector).z) / checkpointVector.magnitude;

        // Reward or penalize the bot based on its distance to the middle
        if (distanceToMiddle < trackMiddleThreshold)
        {
            AddReward(0.1f);
            //Debug.Log("Close to the middle. Distance: " + distanceToMiddle);
        }
        else
        {
            AddReward(-0.1f);
            //Debug.Log("Far from the middle. Distance: " + distanceToMiddle);
        }
    }

    private void CheckProgressAndCollision()
    {
        // Calculate current distance to the next checkpoint
        float currentDistanceToCheckpoint = Vector2.Distance(botCheckpoints[nextCheckpoint].position, transform.position);

        // Reward or penalize based on whether the bot is getting closer
        if (currentDistanceToCheckpoint < previousDistanceToCheckpoint)
        {
            AddReward(0.1f); // Reward for progress
            //Debug.Log("Getting closer to checkpoint. Reward added.");
        }
        else
        {
            AddReward(-0.1f); // Penalty for moving away
            //Debug.Log("Moving further from checkpoint. Penalty added.");
        }

        // Update the previous distance
        previousDistanceToCheckpoint = currentDistanceToCheckpoint;

        rewardTimer += Time.deltaTime;
        if (rewardTimer >= 1.0f)
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Checkpoint"))
        {
            if (other.gameObject.CompareTag("FinishLine"))
            {
                AddReward(20f);
                Debug.Log("Finish line reached! Reward: 20");
                EndEpisode();
                Debug.Log("EndEpisode called.");
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
                //Debug.Log("Track number: " + trackNumber);
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
