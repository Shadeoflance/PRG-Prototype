using UnityEngine;
using System.Collections.Generic;

public class SpritePainter : MonoBehaviour 
{
    SpriteRenderer sprite;
    Color initial;
    List<ColorChanger> changers = new List<ColorChanger>();
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        initial = sprite.color;
    }

    public void Paint(Color c, float time, bool smooth)
    {
        changers.Add(new ColorChanger(c, time, smooth));
    }

    void Update()
    {
        Color c = initial.Copy();
        foreach (var a in changers)
        {
            a.Update();
            c = a.Change(c);
        }
        sprite.color = c;
    }

    private class ColorChanger
    {
        private bool smooth;
        private float time, initialTime;
        private Color color;
        public ColorChanger(Color c, float time, bool smooth)
        {
            this.time = time;
            this.smooth = smooth;
            initialTime = time;
            color = c;
        }
        public Color Change(Color c)
        {
            return Color.Lerp(c, color, smooth ? time / initialTime : 1f);
        }
        public void Update()
        {
            time -= Time.deltaTime;
        }
    }
}
