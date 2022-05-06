using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOpponents
{
    public List<EncounterMember> OpposingMembers { get; private set; } = new List<EncounterMember>();

    public void AddOpposingMember(EncounterMember toAdd)
    {
        OpposingMembers.Add(toAdd);
    }

    public static BattleOpponents GetDemoOpponents()
    {
        return new BattleOpponents()
        {
            OpposingMembers = new List<EncounterMember>()
            {
                new EncounterMember() { DisplayName = "Opponent A" },
                new EncounterMember() { DisplayName = "Opponent B" },
                new EncounterMember() { DisplayName = "Opponent C" }
            }
        };
    }
}
