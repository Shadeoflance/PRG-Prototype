using UnityEngine;

public static class DoubleJumpClouds
{
    static GameObject doubleJump;
    public static void Create(Vector2 position)
    {
        if (doubleJump == null)
        {
            doubleJump = Resources.Load<GameObject>("DoubleJump");
        }
        var newInstance = GameObject.Instantiate<GameObject>(doubleJump);
        newInstance.transform.position = position;
        var ps = newInstance.GetComponent<ParticleSystem>();
        ps.Play();
        GameObject.Destroy(newInstance, ps.duration);
    }
}