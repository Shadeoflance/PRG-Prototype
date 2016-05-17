using UnityEngine;

public class StunnedBuff : Buff
{
    private static ParticleSystem stun;
    public StunnedBuff(Unit u, float duration) : base(u, duration) 
    {
        if (stun == null)
            stun = Resources.Load<ParticleSystem>("Effects/Stun");
        var newInstance = GameObject.Instantiate<ParticleSystem>(stun);
        newInstance.transform.position = u.transform.position + new Vector3(0f, u.size.y + 0.4f, 0f);
        newInstance.transform.SetParent(u.gameObject.transform);
        GameObject.Destroy(newInstance, duration);
        if (u is Player)
            u.currentState.Transit(new PlayerStunnedState(u));
        else u.currentState.Transit(new StunnedState(u));
        imagePath = "Buffs/stunned";
    }
    public override void End()
    {
        base.End();
        if (unit.currentState is StunnedState)
            (unit.currentState as StunnedState).End();
        else if (unit.currentState is PlayerStunnedState)
            (unit.currentState as PlayerStunnedState).End();
    }

}