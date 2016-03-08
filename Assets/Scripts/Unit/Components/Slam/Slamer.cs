public abstract class Slamer
{
    protected Player player;
    public Slamer(Player player)
    {
        this.player = player;
    }

    public abstract void Slam();
}