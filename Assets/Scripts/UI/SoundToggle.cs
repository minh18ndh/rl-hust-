using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Sprite sprite1; // The first sprite
    [SerializeField] private Sprite sprite2; // The second sprite
    private Image buttonImage; // Reference to the button's image component
    private bool isSprite1 = true; // Tracks which sprite is currently active

    private void Start()
    {
        // Get the Image component on the button
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
        }

        // Set the initial sprite
        buttonImage.sprite = sprite1;
    }

    public void ToggleSprite()
    {
        // Toggle between sprite1 and sprite2
        if (isSprite1)
        {
            buttonImage.sprite = sprite2;
        }
        else
        {
            buttonImage.sprite = sprite1;
        }

        // Switch the state
        isSprite1 = !isSprite1;
    }
}
