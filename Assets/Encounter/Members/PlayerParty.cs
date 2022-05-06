using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty
{
    public List<EncounterMember> PartyMembers { get; private set; } = new List<EncounterMember>();

    public void AddPartyMember(EncounterMember toAdd)
    {
        PartyMembers.Add(toAdd);
    }

    public static PlayerParty GetDemoParty()
    {
        PlayerParty party = new PlayerParty();
        party.AddPartyMember(new EncounterMember() { DisplayName = "Velcro" });
        party.AddPartyMember(new EncounterMember() { DisplayName = "Lace" });
        party.AddPartyMember(new EncounterMember() { DisplayName = "Zipper" });
        party.AddPartyMember(new EncounterMember() { DisplayName = "Stitch" });
        party.AddPartyMember(new EncounterMember() { DisplayName = "Button" });
        return party;
    }
}
