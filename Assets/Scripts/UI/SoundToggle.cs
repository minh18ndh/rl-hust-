using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Sprite sprite1; // The first sprite
    [SerializeField] private Sprite sprite2; // The second sprite
    private Image buttonImage; // Reference to the button's image component
    private bool isSprite1; // Tracks which sprite is currently active

    private void Start()
    {
        // Get the Image component on the button
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
        }

        // Set the initial sprite
        if (BackgroundMusicManager.Instance.audioSource.isPlaying)
        {
            buttonImage.sprite = sprite1;
            isSprite1 = true;
        }
        else if (!BackgroundMusicManager.Instance.audioSource.isPlaying)
        {
            buttonImage.sprite = sprite2;
            isSprite1 = false;
        }
    }

    public void ToggleSprite()
    {
        // Toggle between sprite1 and sprite2
        if (isSprite1)
        {
            BackgroundMusicManager.Instance.StopMusic();
            buttonImage.sprite = sprite2;
        }
        else
        {
            BackgroundMusicManager.Instance.PlayMusic();
            buttonImage.sprite = sprite1;
        }

        // Switch the state
        isSprite1 = !isSprite1;
    }
}
