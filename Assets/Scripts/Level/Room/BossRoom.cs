using UnityEngine;

class BossRoom : SubRoom
{
    public override Color InitialColor
    {
        get
        {
            return Color.red;
        }
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void InitEnemies()
    {
        foreach (Transform a in enemies)
        {
            enemiesAlive.Add(a.GetComponent<Enemy>());
        }
    }
}