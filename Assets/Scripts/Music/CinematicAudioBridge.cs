using UnityEngine;

public class CinematicAudioBridge : MonoBehaviour
{
    // Called by the Animation Event at the start of the cinematic
    public void PlayLaugh()
    {
        SoundManager.Instance.PlaySound2D("KidLaugh");
    }

    // Called by the Animation Event when the kid hits the ground
    public void PlayFall()
    {
        // Now it just plays the sound, keeping the camera still and focused
        SoundManager.Instance.PlaySound2D("KidFall");
    }

    // Called by the Animation Event at the very end of the cinematic
    public void StopAllMusic()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic(1.5f);
        }
    }
}