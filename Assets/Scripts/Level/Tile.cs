﻿using UnityEngine;

class Tile : MonoBehaviour
{
    public int x, y;
    SpritePainter painter;

    void Awake()
    {
        painter = GetComponent<SpritePainter>();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            painter.Paint(new Color(0.6f, 1f, 0.6f), 0.5f, true);
        }
    }
}