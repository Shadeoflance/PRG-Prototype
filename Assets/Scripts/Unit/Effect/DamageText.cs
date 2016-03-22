using UnityEngine;

public class DamageText : MonoBehaviour
{
    public string amount;
    GUIStyle style = new GUIStyle();
    static int initialSize = 50;
    float currentSize;
    public static void Create(Vector2 position, string amount)
    {
        GameObject newInstance = new GameObject("Damage Text");
        var dmgText = newInstance.AddComponent<DamageText>();
        dmgText.amount = amount;
        newInstance.transform.position = position;
        var rb = newInstance.AddComponent<Rigidbody2D>();
        var forceVec = Vector2.up.Rotate(Random.Range(-Mathf.PI / 3, Mathf.PI / 3));
        rb.AddForce(forceVec * 300);
    }
    void Start()
    {
        style.normal.textColor = new Color(1f, 0.5f, 0f);
        style.fontSize = initialSize;
        currentSize = (float)initialSize;
    }

    void Update()
    {
        Color c = style.normal.textColor;
        c = new Color(c.r, c.g, c.b, c.a - Time.deltaTime);
        if (c.a < 0)
            Destroy(gameObject);
        style.normal.textColor = c;
        currentSize -= initialSize * Time.deltaTime;
        style.fontSize = (int)currentSize;
    }

    void OnGUI()
    {
        Vector2 pos = VectorUtils.ToV2(Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f));
        Rect rect = new Rect(0, 0, 1, 1);
        rect.x = pos.x;
        rect.y = Screen.height - pos.y - rect.height;
        GUI.Label(rect, amount, style);
    }
}