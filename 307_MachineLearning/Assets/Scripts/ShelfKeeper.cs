using UnityEngine;

public class ShelfKeeper : MonoBehaviour
{
    /// <summary>
    /// Provides this class Singleton-like behaviour
    /// </summary>
    public static ShelfKeeper instance;

    /// <summary>
    /// Unity Inspector accessible Reference to the Text Mesh object needed for data
    /// </summary>
    public TextMesh dateText;

    /// <summary>
    /// Unity Inspector accessible Reference to the Text Mesh object needed for time
    /// </summary>
    public TextMesh timeText;

    /// <summary>
    /// Provides references to the spawn locations for the products prefabs
    /// </summary>
    public Transform[] spawnPoint;
}

