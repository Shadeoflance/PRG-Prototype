using UnityEngine;
using System.Collections;

public static class Utils 
{
	public static Vector2 ToV2(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.y);
	}
	public static Vector3 ToV3(this Vector2 vec)
	{
		return new Vector3(vec.x, vec.y, 0);
    }
	public static Vector2 TrimY(this Vector2 vec)
	{
		return new Vector2(vec.x, 0);
	}
	public static Vector2 TrimX(this Vector2 vec)
	{
		return new Vector2(0, vec.y);
	}
    public static Vector2 Rotate(this Vector2 v, float value)
    {
        float sin = Mathf.Sin(value);
        float cos = Mathf.Cos(value);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
    public static Color WithAlpha(this Color c, float value)
    {
        return new Color(c.r, c.g, c.b, value);
    }
    public static Color Copy(this Color c)
    {
        return new Color(c.r, c.g, c.b, c.a);
    }
}
