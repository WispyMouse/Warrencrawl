using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty
{
    public List<CombatMember> PartyMembers { get; private set; } = new List<CombatMember>();

    public void AddPartyMember(CombatMember toAdd)
    {
        PartyMembers.Add(toAdd);
    }
}
