using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileSO tileData; // Reference to the Scriptable Object

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(TileSO data)
    {
        tileData = data;
        name = $"Tile ({tileData.tileType})";
        spriteRenderer.color = tileData.tileColor;

    }
}
