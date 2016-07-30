using UnityEngine;

class BossRoom : SubRoom
{
    public override Color RoomColor
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
        GameObject[] e = Resources.LoadAll<GameObject>("Enemies/Bosses");
        GameObject boss = Instantiate(e[Random.Range(0, e.Length)]);
        boss.transform.SetParent(enemies);
        boss.transform.localPosition = new Vector3(0, -Level.roomSize.y / 4);
        boss.GetComponent<Unit>().eventManager.SubscribeHandler("die", new GameOverHandler());
    }
    class GameOverHandler : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("end");
            return true;
        }
    }
}