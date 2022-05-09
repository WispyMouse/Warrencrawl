public interface IGameplayUXProvider
{
    public ICoroutineRunner CoroutineRunner { get; }

    public IEncounterUXHandler GetEncounterUXHandler();
}