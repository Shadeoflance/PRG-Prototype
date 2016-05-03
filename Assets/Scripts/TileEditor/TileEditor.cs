using UnityEngine;
using System.Collections.Generic;

public class TileEditor : MonoBehaviour
{
    public float width = 0.5f;
    public float height = 0.5f;
    public GameObject current;
    int currentInd = 0;
    public List<GameObject> tiles;

    public Color color = Color.white;

    public void Init()
    {
        tiles = new List<GameObject>();
        tiles.AddRange(Resources.LoadAll<GameObject>("Level/Tiles"));
        current = tiles[0];
    }

    public void SelectNext()
    {
        currentInd = (currentInd + 1) % tiles.Count;
        current = tiles[currentInd];
    }

    public void SelectPrev()
    {
        currentInd = (currentInd - 1 + tiles.Count) % tiles.Count;
        current = tiles[currentInd];
    }

    void OnDrawGizmos()
    {
        Vector3 pos = Camera.current.transform.position;

        Gizmos.color = color;

        width = width < 0.01f ? 0.01f : width;
        height = height < 0.01f ? 0.01f : height;
        for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += height)
        {
            Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / height) * height, 0.0f),
                            new Vector3(1000000.0f, Mathf.Floor(y / height) * height, 0.0f));
        }

        for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -1000000.0f, 0.0f),
                            new Vector3(Mathf.Floor(x / width) * width, 1000000.0f, 0.0f));
        }
    }
}
