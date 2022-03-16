using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCommand
{
    public CombatMember ActingMember { get; set; }
    public CombatMember Target { get; set; }

    public BattleCommand(CombatMember actor, CombatMember target)
    {
        this.ActingMember = actor;
        this.Target = target;
    }
}
