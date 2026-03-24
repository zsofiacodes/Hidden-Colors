using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
    [Range(0, 1)] public float volume; // Individual volume for this specific track
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public MusicTrack GetTrackFromName(string trackName)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
                return track;
            }
        }
        return default;
    }
}