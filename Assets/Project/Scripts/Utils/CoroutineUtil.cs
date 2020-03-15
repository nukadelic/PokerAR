using UnityEngine;
using System;
using System.Collections;

public class CoroutineUtil : StaticMonoBehaviour<CoroutineUtil>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    static void Init() => AddToScene();

    // - - - - - - - - - - - - - - - - - - - - - - - - - 

    #region Static Coroutine Starters

    static public void RunCoroutine(IEnumerator coroutine, Action onComplete = null)
    {
        instance.StartCoroutine(instance.CoExecute(coroutine, onComplete));
    }
    public static void DelayFrames(int frame_count, Action onComplete)
    {
        instance.StartCoroutine(instance.CoDelayFrames(frame_count, onComplete));
    }

    public static void DelaySeconds(float delay_in_seconds, Action onComplete)
    {
        instance.StartCoroutine(instance.CoDelaySeconds(delay_in_seconds, onComplete));
    }

    #endregion

    // - - - - - - - - - - - - - - - - - - - - - - - - - 

    #region Coroutine Starters

    IEnumerator CoExecute(IEnumerator enumerator, Action onComplete = null )
    {
        yield return this.StartCoroutine( enumerator );
        onComplete?.Invoke();
    }

    IEnumerator CoDelaySeconds(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        onComplete.Invoke();
    }

    IEnumerator CoDelayFrames(int count, Action onComplete)
    {
        while( (count--) > 1 ) yield return new WaitForEndOfFrame();

        onComplete.Invoke();
    }

    #endregion

    // - - - - - - - - - - - - - - - - - - - - - - - - - 
}
