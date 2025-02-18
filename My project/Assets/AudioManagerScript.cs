using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioClip[] playlist1; // Playlist 1
    public AudioClip[] playlist2; // Playlist 2
    public AudioClip[] playlist3; // Playlist 3

    public GameObject speakerOn;
    public GameObject speakerOff;

    private AudioSource audioSource; // Reference to AudioSource
    private bool isPlaying = false; // To track if the playlist is playing
    private int currentTrackIndex = 0; // To track the current track index
    private AudioClip[] currentPlaylist; // The currently active playlist
    private float timePaused = 0f; // To track time when paused

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentPlaylist = playlist1; // Default to playlist1 at the start
    }

    void Update()
    {
        // Check if the current track has finished playing
        if (isPlaying && !audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
        {
            PlayNextTrack(); // Play the next track if the current one finished
        }
    }

    // Method to toggle the playlist (play/stop)
    public void TogglePlaylist()
    {
        if (isPlaying)
        {
            PausePlaylist(); // Pause the playlist if it's currently playing
        }
        else
        {
            ResumePlaylist(); // Resume the playlist if it's paused
        }
    }

    // Method to start the playlist (first track or random)
    public void PlayPlaylist()
    {
        if (currentPlaylist.Length > 0)
        {
            audioSource.clip = currentPlaylist[currentTrackIndex];
            audioSource.Play();
            isPlaying = true; // Set to playing
        }
    }

    // Method to pause the playlist
    public void PausePlaylist()
    {
        if (audioSource.isPlaying)
        {
            timePaused = audioSource.time; // Store the current playback time
            audioSource.Pause();
            isPlaying = false; // Set to paused
        }
    }

    // Method to resume the playlist from where it left off
    public void ResumePlaylist()
    {
        if (currentPlaylist.Length > 0)
        {
            audioSource.clip = currentPlaylist[currentTrackIndex];
            audioSource.time = timePaused; // Resume from the last paused time
            audioSource.Play();
            isPlaying = true; // Set to playing
        }
    }

    // Method to play the next track in the playlist
    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % currentPlaylist.Length; // Move to the next track
        audioSource.clip = currentPlaylist[currentTrackIndex];
        audioSource.Play();
        isPlaying = true; // Ensure it's playing
    }

    // Method to change to a specific track in the current playlist
    public void ChangeTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < currentPlaylist.Length)
        {
            currentTrackIndex = trackIndex; // Update the current track index
            audioSource.clip = currentPlaylist[currentTrackIndex];
            audioSource.Play();
            isPlaying = true; // Ensure it's playing
        }
    }

    // Method to switch to playlist 1
    public void SwitchToPlaylist1()
    {
        currentPlaylist = playlist1;
        currentTrackIndex = 0; // Reset track to first one
        PlayPlaylist(); // Start the new playlist
        speakerOff.SetActive(false);
        speakerOn.SetActive(true);

    }

    // Method to switch to playlist 2
    public void SwitchToPlaylist2()
    {
        currentPlaylist = playlist2;
        currentTrackIndex = 0; // Reset track to first one
        PlayPlaylist(); // Start the new playlist
        speakerOff.SetActive(false);
        speakerOn.SetActive(true);

    }

    // Method to switch to playlist 3
    public void SwitchToPlaylist3()
    {
        currentPlaylist = playlist3;
        currentTrackIndex = 0; // Reset track to first one
        PlayPlaylist(); // Start the new playlist
        speakerOff.SetActive(false);
        speakerOn.SetActive(true);

    }
}
