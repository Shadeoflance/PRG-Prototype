public class DefaultDasher : Dasher
{
    public float speed, distance;
    public DefaultDasher(Player p, float speed = 50, float distance = 5) : base(p) 
    {
        this.speed = speed;
        this.distance = distance;
    }

    public override void Dash()
    {
        player.currentState.Transit(new DashState(player, speed, distance));
    }
}