using UnityEngine;

public class AlbumManager : MonoBehaviour
{
    // Drag your 5 PhotoSlot GameObjects into this array in the Inspector
    public AlbumSlot[] slots;

    private void Start()
    {
        // Order: Teddy Bear, Swing, Book, Backpack, Chalk Drawing
        // These map to your Area0 through Area4
        string[,] filePairs = new string[,] {
            { "Area0_normal.png", "Area0_poverty.png" }, // 0: Teddy Bear
            { "Area1_normal.png", "Area1_poverty.png" }, // 1: Swing
            { "Area2_normal.png", "Area2_poverty.png" }, // 2: Fairy Tale Book
            { "Area3_normal.png", "Area3_poverty.png" }, // 3: Backpack
            { "Area4_normal.png", "Area4_poverty.png" }  // 4: Chalk Drawing
        };

        for (int i = 0; i < slots.Length; i++)
        {
            // Make sure we don't go out of bounds if slots don't match filePairs
            if (i < slots.Length && i < 5)
            {
                slots[i].LoadPhotos(filePairs[i, 0], filePairs[i, 1]);
            }
        }
    }
}