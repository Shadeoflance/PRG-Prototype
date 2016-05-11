using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public string amount;
    GUIStyle style = new GUIStyle();
    static int initialSize = 50;
    float currentSize;
    public Color inColor, outColor;
    public static void Create(Vector2 position, string amount, Color inColor, Color outColor)
    {
        GameObject newInstance = new GameObject("Damage Text");
        var dmgText = newInstance.AddComponent<DamageText>();
        dmgText.amount = amount;
        dmgText.inColor = inColor;
        dmgText.outColor = outColor;
        newInstance.transform.position = position;
        var rb = newInstance.AddComponent<Rigidbody2D>();
        var forceVec = Vector2.up.Rotate(Random.Range(-Mathf.PI / 3, Mathf.PI / 3));
        rb.AddForce(forceVec * 300);
    }
    void Start()
    {
        style.fontSize = initialSize;
        style.font = Resources.Load<Font>("font");
        currentSize = initialSize;
    }

    void Update()
    {
        inColor = new Color(inColor.r, inColor.g, inColor.b, inColor.a - Time.deltaTime);
        outColor = new Color(outColor.r, outColor.g, outColor.b, inColor.a);
        if (inColor.a < 0.1f)
            Destroy(gameObject);
        style.normal.textColor = inColor;
        currentSize -= initialSize * Time.deltaTime;
        style.fontSize = (int)currentSize;
    }

    void OnGUI()
    {
        Vector2 pos = Utils.ToV2(Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f));
        Rect rect = new Rect(0, 0, 1, 1);
        rect.x = pos.x;
        rect.y = Screen.height - pos.y - rect.height;
        ShadowAndOutline.DrawOutline(rect, amount, style, outColor, inColor, 5f);
    }
}