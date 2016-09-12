using UnityEngine;
using UnityEngine.UI;
using System;

public class Buff : IUpdatable
{
    protected Unit unit;
    float? duration;
    protected string imagePath = null;
    public bool playerBuff = false;
    private static GameObject buffs = GameObject.Find("Buffs");
    private GameObject buffImage;

    public Buff(Unit unit, float? duration)
    {
        this.unit = unit;
        this.duration = duration;
    }

    private Sprite sprite = null;
    private Sprite GetSprite()
    {
        if (imagePath == null)
            return null;
        if (sprite == null)
        {
            sprite = GameObject.Instantiate<Sprite>(Resources.Load<Sprite>(imagePath));
        }
        return GameObject.Instantiate<Sprite>(sprite);
    }

    Image icon;
    public void ChangeToPlayerBuff()
    {
        buffImage = Resources.Load<GameObject>("BuffImage");
        icon = GameObject.Instantiate(buffImage).GetComponent<Image>();
        icon.transform.SetParent(buffs.transform);
        var image = GetSprite();
        if (image == null)
            return;
        icon.sprite = image;
    }

    public virtual void Update()
    {
        if (duration == null)
            return;
        duration -= Time.deltaTime;
        if (duration < 0)
            End();
        if (icon != null)
        {
            float t = 1;
            if (duration < 1)
            {
                t = (float)Math.Sin(duration.Value * Math.PI * 12) / 3 + 0.66f;
            }
            else if (duration < 5)
            {
                t = (float)Math.Sin(duration.Value * Math.PI * 4) / 3 + 0.66f;
            }
            icon.color = icon.color.WithAlpha(t);
        }
    }

    public virtual void End()
    {
        unit.RemoveBuff(this);
        if (icon != null)
            GameObject.Destroy(icon.gameObject);
    }
}