using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; } // Singleton instance
    public AudioSource audioSource;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Make persistent
            audioSource = GetComponent<AudioSource>(); // Cache the AudioSource
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public void StopMusic()
    {
        audioSource.Pause();
    }
}
