using UnityEngine;

public class ProgrammaticEncounterUXHandler : IEncounterUXHandler
{
    public void SetCommandForCurrentActingPartyMember(string command)
    {
        Debug.Log($"Set command: {command}");
    }

    public void SetCurrentActingPartyMember(EncounterMember member)
    {
        Debug.Log($"Set current acting party member: {member.DisplayName}");
    }

    public void ShowTargetingForCurrentCommand()
    {
        Debug.Log("Show targeting for current command");
    }

    public void SubmitTargetForCurrentCommand(EncounterMember target)
    {
        Debug.Log($"Submit target for current command: {target.DisplayName}");
    }
}