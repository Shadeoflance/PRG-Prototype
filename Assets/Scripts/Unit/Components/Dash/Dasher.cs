using UnityEngine;
using UnityEngine.UI;

public class Dasher : IUpdatable
{
    public float coolDown = 5;
    private float currentCoolDown = 0;
    protected Player player;
    private Material cdMaterial;
    public Dasher(Player player)
    {
        this.player = player;
        Image i = GameObject.Find("UI/Dash").GetComponent<Image>();
        cdMaterial = GameObject.Instantiate<Material>(i.material);
        i.material = cdMaterial;
    }

    public virtual void Dash()
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

    protected virtual bool CanDash()
    {
        return currentCoolDown <= 0;
    }
}