using UnityEngine;
using UnityEngine.UI;

public class Buff : IUpdatable
{
    protected Unit unit;
    float duration;
    protected string imagePath = null;
    public bool playerBuff = false;
    private static GameObject buffs = GameObject.Find("Buffs");
    private GameObject buffImage;

    public Buff(Unit unit, float duration)
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

    GameObject icon;
    public void ChangeToPlayerBuff()
    {
        buffImage = Resources.Load<GameObject>("BuffImage");
        icon = GameObject.Instantiate(buffImage);
        icon.transform.SetParent(buffs.transform);
        var image = GetSprite();
        if (image == null)
            return;
        icon.GetComponent<Image>().sprite = image;
    }

    public virtual void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0)
            End();
    }

    public virtual void End()
    {
        unit.RemoveBuff(this);
        if (icon != null)
            GameObject.Destroy(icon);
    }
}