public class ProgrammaticUXProvider : IGameplayUXProvider
{
    public ICoroutineRunner CoroutineRunner => new ImmediateCoroutineRunner();

    public IEncounterUXHandler GetEncounterUXHandler()
    {
        return new ProgrammaticEncounterUXHandler();
    }
}