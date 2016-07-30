using UnityEngine;
using System.Collections.Generic;

public class SpritePainter : MonoBehaviour 
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
        Paint(new ColorChanger(c, time, smooth));
    }

    public void Paint(Color c)
    {
        Paint(c, null, false);
    }

    public void Paint(ColorChanger cc)
    {
        if (initial == null)
            initial = sprite.color;
        changers.Add(cc);
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

}
public class ColorChanger
{
    private bool smooth;
    private float initialTime;
    public float? time;
    protected Color color;
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
    public virtual void Update()
    {
        if (time != null)
            time -= Time.deltaTime;
    }
}
public class PulsingChanger : ColorChanger
{
    Color c1, c2;
    float t = 0, period;
    public PulsingChanger(Color c1, Color c2, float? time, float period = 0.5f) : base(c1, time, false)
    {
        this.c1 = c1;
        this.c2 = c2;
        this.period = period;
    }

    bool pos = true;
    public override void Update()
    {
        base.Update();
        t += (pos ? 1 : -1) * Time.deltaTime / period;
        if (t > 1)
        {
            pos = false;
            t = 1;
        }
        if (t < 0)
        {
            pos = true;
            t = 0;
        }
        color = Color.Lerp(c1, c2, t);
    }
}
