using System.Collections.Generic;

public interface IEncounterUXHandler
{
    public void SetCurrentActingPartyMember(EncounterMember member);
    public void SetCommandForCurrentActingPartyMember(string command);
    public void ShowTargetingForCurrentCommand();
    public void SubmitTargetForCurrentCommand(EncounterMember target);
}