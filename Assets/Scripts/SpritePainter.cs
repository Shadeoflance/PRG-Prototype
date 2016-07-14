using UnityEngine;
using System.Collections.Generic;

class SpritePainter : MonoBehaviour 
{
    SpriteRenderer sprite;
    Color? initial;
    Group<ColorChanger> changers = new Group<ColorChanger>();
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Paint(Color c, float? time, bool smooth)
    {
        if(initial == null)
            initial = sprite.color;
        changers.Add(new ColorChanger(c, time, smooth));
    }

    public void Paint(Color c)
    {
        Paint(c, null, false);
    }

    public void Clear()
    {
        changers.Clear();
    }

    void Update()
    {
        changers.Refresh();
        if (initial == null)
            return;
        Color c = initial.Value.Copy();
        foreach (var a in changers)
        {
            a.Update();
            c = a.Change(c);
            if (a.time != null && a.time < 0)
                changers.Remove(a);
        }
        sprite.color = c;
    }

    private class ColorChanger
    {
        private bool smooth;
        private float initialTime;
        public float? time;
        private Color color;
        public ColorChanger(Color c, float? time, bool smooth)
        {
            this.time = time;
            this.smooth = smooth;
            if (time != null)
                initialTime = time.Value;
            color = c;
        }
        public Color Change(Color c)
        {
            float smoothValue = 1;
            if (time != null && smooth)
                smoothValue = time.Value / initialTime;
            return Color.Lerp(c, color, smoothValue);
        }
        public void Update()
        {
            if(time != null)
                time -= Time.deltaTime;
        }
    }
}
