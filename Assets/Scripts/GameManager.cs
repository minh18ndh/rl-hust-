using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI itemLeft;
    public ColorMode color;
    public Player player;
    public SpriteRenderer playGround;
    public GameObject door;
    public int itemToEat;
    public float countDownSec;

    private Collider2D doorCollider;
    private SpriteRenderer doorRenderer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        doorCollider = door.GetComponent<Collider2D>();
        doorRenderer = door.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && player.canChangeMode())
        {
            changeColorMode();
        }

        if(countDownSec > 0.0001f)
        {
          countDownSec -= Time.deltaTime;
        }

        itemLeft.text = "Item left: " + itemToEat.ToString();
        countDownText.text = Mathf.RoundToInt(countDownSec).ToString() + " secs left";
    }

    void changeColorMode()
    {
        if (color == ColorMode.Black) color = ColorMode.Orange;
        else color = ColorMode.Black;
        if (color == ColorMode.Orange)
        {
            playGround.color = new Color(0f, 0f, 0f);
            player.changeColor(ColorMode.Orange);
        }
        else
        {
            playGround.color = new Color(255f / 255f, 127f / 255f, 39f / 255f);
            player.changeColor(ColorMode.Black);
        }
    }

    public void eatItem()
    {
        itemToEat--;
        if (itemToEat == 0)
        {
            doorCollider.isTrigger = true;
            doorRenderer.color = (color == ColorMode.Black) ?
                new Color(255f / 255f, 127f / 255f, 39f / 255f) :
                new Color(0f, 0f, 0f);
            Debug.Log("Door open");
        }
    }
}

public enum ColorMode
{
    Orange,
    Black,
    White
}
