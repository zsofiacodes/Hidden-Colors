using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AlbumSlot : MonoBehaviour
{
    public Image normalSlot;   // Drag your 'SliderMask/CameraImage' here
    public Image povertySlot;  // Drag your 'PovertyImage' here

    public void LoadPhotos(string normalName, string povertyName)
    {
        LoadSinglePhoto(normalName, normalSlot);
        LoadSinglePhoto(povertyName, povertySlot);
    }

    private void LoadSinglePhoto(string fileName, Image targetImage)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            targetImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            targetImage.color = Color.white;
        }
    }
}