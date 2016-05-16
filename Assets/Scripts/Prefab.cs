using UnityEngine;

class Prefab
{
    GameObject prefab;
    string path;
    public Prefab(string path)
    {
        this.path = path;
    }

    public GameObject Instantiate()
    {
        if (prefab == null)
            prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab);
    }
}