using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOpponents
{
    public List<CombatMember> OpposingMembers { get; private set; } = new List<CombatMember>();

    public void AddOpposingMember(CombatMember toAdd)
    {
        OpposingMembers.Add(toAdd);
    }
}
