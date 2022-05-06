public class EncounterCommand
{
    public EncounterMember ActingMember { get; private set; }
    public string Command { get; private set; }
    public EncounterMember Target { get; private set; }

    public EncounterCommand (EncounterMember actingMember, string command)
    {
        ActingMember = actingMember;
        Command = command;
    }

    public void SetTarget(EncounterMember target)
    {
        Target = target;
    }
}