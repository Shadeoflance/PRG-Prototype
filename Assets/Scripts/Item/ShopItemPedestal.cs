using UnityEngine;

class ShopItemPedestal : MonoBehaviour
{
    public static int price = 15;
    static Color outColor = new Color(0.5f, 0.5f, 0.3f), inColor = new Color(1, 0.9f, 0.3f);
    static GUIStyle style;
    Item item;

    static ShopItemPedestal()
    {
        style = new GUIStyle();
    }
    void Awake()
    {
        style.font = Resources.Load<Font>("font");
        item = Item.Create();
        item.transform.position = transform.position;
        item.canPickup = false;
        item.transform.SetParent(transform);
        item.GetComponent<Rigidbody2D>().isKinematic = true;
        item.GetComponent<Collider2D>().enabled = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (item == null)
            return;
        if(col.gameObject.layer == LayerMask.NameToLayer("Player") && Player.instance.controller.NeedVel().y > 0 && Player.instance.gold >= price)
        {
            Player.instance.gold -= price;
            item.PickUp();
            item = null;
        }
    }

    void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-0.24f, -0.5f)).ToV2();
        Rect rect = new Rect(0, 0, 1, 1);
        rect.x = pos.x;
        rect.y = Screen.height - pos.y - rect.height;
        ShadowAndOutline.DrawOutline(rect, price.ToString(), style, outColor, inColor, 2f);
    }
}