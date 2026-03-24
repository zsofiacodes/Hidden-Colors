using UnityEngine;

public class CinematicAudioBridge : MonoBehaviour
{
    public void PlayLaugh()
    {
        SoundManager.Instance.PlaySound2D("KidLaugh");
    }

    public void PlayFall()
    {
        SoundManager.Instance.PlaySound2D("KidFall");
    }

    public void StopAllMusic()
    {
        // Call this at the end of the timeline (e.g., 7:00)
        MusicManager.Instance.StopMusic(1.5f);
    }
}