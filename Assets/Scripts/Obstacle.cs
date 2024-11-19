using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    public ColorMode color;
    private GameManager gameManager;
    private Collider2D thisCollider;

    // Update is called once per frame
    private void Start()
    {
        gameManager = GameManager.Instance;
        thisCollider = gameObject.GetComponent<Collider2D>();
    }
    void Update()
    {
        if(gameManager.color == this.color || this.color == ColorMode.White)
        {
            thisCollider.isTrigger = false;
        }
        else
        {
            thisCollider.isTrigger = true;
        }
    }
}
