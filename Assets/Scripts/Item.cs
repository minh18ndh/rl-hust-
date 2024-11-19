using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
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
    }
}
