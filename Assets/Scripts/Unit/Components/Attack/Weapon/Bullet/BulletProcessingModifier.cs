public interface BulletProcessingModifier
{
    void Modify(Bullet bullet);
    BulletProcessingModifier Instantiate();
}