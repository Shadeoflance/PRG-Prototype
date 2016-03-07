public abstract class Dasher
{
    protected Player player;
    public Dasher(Player player)
    {
        this.player = player;
    }

    public abstract void Dash();
}