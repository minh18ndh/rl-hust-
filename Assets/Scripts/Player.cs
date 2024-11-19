using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Boolean isTriggered = false;

    void Start()
    {
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void changeColor(ColorMode color)
    {
        spriteRenderer.color = (color == ColorMode.Orange) ?
            new Color(255f / 255f, 127f / 255f, 39f / 255f) :
            new Color(0f, 0f / 255f, 0f / 255f);
    }

    public Boolean canChangeMode()
    {
        return !isTriggered;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.GetComponent<Obstacle>() != null)
        {
            isTriggered = true;
        }
        else
        {
            // Check if `gameManager` is not null
            if (gameManager != null && obj != null)
            {
                // Safely check if `Item` component exists before accessing its properties
                Item item = obj.GetComponent<Item>();
                if (item != null && item.color == gameManager.color)
                {
                    gameManager.eatItem();
                    Destroy(obj);
                }
                else if (obj.name == "door")
                {
                    if (SceneManager.GetActiveScene().buildIndex <= 5)
                    { 
                        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    else
                    {
                        SceneManager.LoadSceneAsync(1);
                    }
                }
            }
            else
            {
                Debug.LogWarning("GameManager or collision object is null");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }

}
