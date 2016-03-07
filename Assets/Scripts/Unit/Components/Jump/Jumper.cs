public abstract class Jumper
{
    protected Unit unit;
    public Jumper(Unit unit)
    {
        this.unit = unit;
    }

    public abstract void Jump();

    protected abstract bool CanJump();
}