using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

enum Ability
{
    Dash, Slam
}
class CooldownIndicator : MonoBehaviour
{
    public Image image;
    public Ability ability;
    void Start()
    {
        image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -Screen.height);
        Player.instance.eventManager.SubscribeHandler(ability == Ability.Dash ? "dashStart" : "slamStart", new LambdaActionListner((ActionParams ap) => 
        {
            ap.unit.StartCoroutine(DoCooldown((float)ap["cd"]));
            return false;
        }));
    }

    IEnumerator DoCooldown(float cd)
    {
        float curCd = cd;
        while(curCd > 0)
        {
            curCd -= Time.deltaTime;
            image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -Screen.height + (1 - curCd / cd) * Screen.height);
            yield return null;
        }
        image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -Screen.height);
    }
}