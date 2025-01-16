using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    private Image buttonImage;
    private bool isSprite1;

    private void Start()
    {
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

        isSprite1 = !isSprite1;
    }
}
