using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSound
{
    CreatureSoundPlayer sp;

    public CreatureSound(CreatureSoundPlayer sp)
    {
        this.sp = sp;
    }

    public void DoHitSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.Hit, isPlayer);
    }

    public void DoJumpSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.Jump, isPlayer);
    }

    public void DoDashSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.Dash, isPlayer);
    }

    public void DofireSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.Fire, isPlayer);
    }

    public void DoDeadSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.Dead, isPlayer);
    }

    public void DoItemGetSound(bool isPlayer)
    {
        sp.PlaySound(CreatureEffectSoundType.ItemGet, isPlayer);
    }
}
