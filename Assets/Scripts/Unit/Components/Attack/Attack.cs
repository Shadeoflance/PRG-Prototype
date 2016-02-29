public abstract class Attack
{
    protected Unit unit;
    public Attack(Unit unit)
    {
        this.unit = unit;
    }
    public abstract void DoAttack();
}