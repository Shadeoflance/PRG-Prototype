using UnityEngine;
using UnityEngine.UI;

public class Slamer
{
    public float coolDown = 5;
    private float currentCoolDown = 0;
    protected Player player;
    private Material cdMaterial;
    public Slamer(Player player)
    {
        this.player = player;
        Image i = GameObject.Find("UI/Slam").GetComponent<Image>();
        cdMaterial = GameObject.Instantiate<Material>(i.material);
        i.material = cdMaterial;
    }

    public virtual void Slam()
    {
        currentCoolDown = coolDown;
    }

    public virtual void Update()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
            if (currentCoolDown < 0)
                currentCoolDown = 0;
            cdMaterial.SetFloat("_CooldownPercentage", currentCoolDown / coolDown);
        }
    }

    protected virtual bool CanSlam()
    {
        return currentCoolDown <= 0;
    }
}