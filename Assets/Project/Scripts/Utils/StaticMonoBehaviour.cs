using UnityEngine;

public class StaticMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static public T instance;
    
    /// <summary> Check if utility static class is ready </summary>
    public static bool IsReady => instance != null;
    static public void AddToScene()
    {
        System.Type TT = typeof( T );
        var container = new GameObject( TT.Name );
        container.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(container);
        instance = container.AddComponent( TT ) as T;
    }
}
