using UnityEngine;

class TutorialInit
{
    static Room respawnRoom;
    public static void Init()
    {
        respawnRoom = Level.instance.current;
        Player.instance.eventManager.SubscribeInterceptor("bombButtonDown", new LambdaActionListner((ActionParams ap) =>
        {
            if (Player.instance.orbs <= 0 && Level.instance.current.name.StartsWith("T3"))
                OrbPickup.Drop(3, Level.instance.current.transform.position);
            return false;
        }));
        Player.instance.eventManager.SubscribeHandler("roomChange", new LambdaActionListner((ActionParams ap) =>
        {
            string name = (ap["room"] as Room).name;
            respawnRoom = ap["prevRoom"] as Room;
            if (name.StartsWith("T5") || name.StartsWith("T4") || name.StartsWith("T6"))
                Player.instance.orbs = 0;
            if (name.StartsWith("T6"))
            {
                Player.instance.attack = new Attack(Player.instance);
                Player.instance.slamer = null;
                Player.instance.gun.gameObject.SetActive(false);
                Player.instance.eventManager.SubscribeHandler("kill", new LambdaActionListner((ActionParams a) =>
                {
                    ResetPlayer();
                    return true;
                }));
            }
            if(name.StartsWith("T7"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            return false;
        }));
        Player.instance.eventManager.SubscribeHandler("die", new LambdaActionListner((ActionParams ap) =>
        {
            ap.unit.stats.hp = 1;
            ap.unit.transform.position = respawnRoom.transform.position;
            Player.instance.gameObject.SetActive(true);
            Level.instance.ForceChangeRoom(respawnRoom);
            ResetPlayer();
            return false;
        }));
    }

    private static void ResetPlayer()
    {
        Player.instance.attack = new BulletWeapon(Player.instance);
        Player.instance.gun.gameObject.SetActive(true);
        Player.instance.slamer = new DefaultSlamer(Player.instance);
    }
}