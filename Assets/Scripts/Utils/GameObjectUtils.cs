using UnityEngine;

public static class GameObjectUtils
{
    public static bool IsPlayer(this GameObject g)
    {
        return g.layer == LayerMask.NameToLayer("Player");
    }
}