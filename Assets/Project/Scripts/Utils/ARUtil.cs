using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System;

public partial class ARUtil : StaticMonoBehaviour<ARUtil>
{
    [RuntimeInitializeOnLoadMethod] static void Init() => AddToScene();

    static public List<ARPlane> planes = new List<ARPlane>();

    static public event Action<int> OnPlaneBoundaryChanged;

    internal static void InvokePlaneBoundaryChange( int index ) => OnPlaneBoundaryChanged?.Invoke( index );

}
