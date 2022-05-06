using System.Collections;
using System.Collections.Generic;

// Taken from Alexzander DeJardin's provided coroutine running code
public class ImmediateCoroutineRunner : ICoroutineRunner
{
    //Coroutines are weird, so we have this function to just go through and crush the entire thing in one go, thus completely negating the point of using them
    //BUT, this allows us to properly execute coroutine functions outside of coroutines
    public void PlayCoroutine(IEnumerator coroutine)
    {
        List<IEnumerator> coroutineStack = new List<IEnumerator>();
        coroutineStack.Add(coroutine);
        IEnumerator thisStep = coroutine;
        while (thisStep != null)
        {
            //If we successfully got to the next step
            bool moreRemaining = thisStep.MoveNext();

            //If we don't have any more remaining, then we pop this step from the stack and maybe grab the one below it
            if (!moreRemaining)
            {
                coroutineStack.RemoveAt(coroutineStack.Count - 1);
                if (coroutineStack.Count > 0)
                {
                    thisStep = coroutineStack[coroutineStack.Count - 1];
                    //Debug.LogError("    Finished a branch coroutine, going up 1 layer");
                }
                else
                {
                    thisStep = null;
                    //Debug.LogError("    Ran out of coroutine");
                }
            }
            else
            {
                //If this step has a sub-coroutine, we have to add that to the stack
                if (thisStep.Current != null)
                {
                    coroutineStack.Add(thisStep.Current as IEnumerator);
                    //Debug.LogError("    Beginning a branch coroutine, going down 1 layer: " + (thisStep.Current as IEnumerator));
                    thisStep = coroutineStack[coroutineStack.Count - 1];
                }
            }
        }
    }
}