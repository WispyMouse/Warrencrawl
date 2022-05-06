using System.Collections;

public interface ICoroutineRunner
{
    public void PlayCoroutine(IEnumerator toRun);
}