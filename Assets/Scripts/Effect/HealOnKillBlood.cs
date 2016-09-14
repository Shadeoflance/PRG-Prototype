using UnityEngine;

class HealOnKillBlood
{
    static Prefab prefab = new Prefab("Effects/HealOnKill");

    public static void Create(Vector2 position)
    {
        var newInstance = prefab.Instantiate();
        newInstance.transform.position = position;
        var ps = newInstance.GetComponent<ParticleSystem>();
        ps.Play();
        Object.Destroy(newInstance, ps.duration);
    }
}