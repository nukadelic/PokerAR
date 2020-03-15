using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneViz : MonoBehaviour
{
    #region Check Plane State

    public bool IsPlaneRoot => plane.subsumedBy == null;
    public bool IsPlaneTracking => plane.trackingState > 0;
    public bool IsPlaneEnabled => IsPlaneRoot && IsPlaneTracking && ARSession.state > ARSessionState.Ready;

#endregion

    LineRenderer lineRenderer;
    float lineRendererWidthMult;
    bool changed;
    int index;
    ARPlane plane;

    private void Awake() => plane = GetComponent<ARPlane>();

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRendererWidthMult = lineRenderer.widthMultiplier;

        changed = false;
        plane = GetComponent<ARPlane>();
        index = ARUtil.planes.Count;
        ARUtil.planes.Add(plane);
        plane.boundaryChanged += PlaneBoundaryChanged;
        
        LogPlane();
    }

    string IDs(TrackableId id) => $"{id.subId1}:{id.subId2}";

    void LogPlane()
    {
        var s = $"Plane :: "; 
        s += $" Align:{plane.alignment}, BounderySize:{plane.boundary.Length}";
        s += $", Classification:{plane.classification}, Pending:{plane.pending}, State: {plane.trackingState}";
        s += $", ID:{IDs(plane.trackableId)}"; 
        s += $", SubsumedByID:{ ( plane.subsumedBy == null ? "-1" : IDs(plane.subsumedBy.trackableId ) ) }";
        s += $", Vertex Changed Threshold: {plane.vertexChangedThreshold}";

        Debug.Log( s );
    }

    void OnDisable()
    {
        ARUtil.planes.Remove(plane);
        plane.boundaryChanged -= PlaneBoundaryChanged;
    }

    void Update()
    {
        lineRenderer.widthMultiplier = lineRendererWidthMult * transform.lossyScale.x;

        lineRenderer.startColor = lineRenderer.endColor = lineRenderer ? Color.red : Color.green;
    }

    void PlaneBoundaryChanged(ARPlaneBoundaryChangedEventArgs args)
    {
        //ARPlaneMeshGenerators

        var boundary = plane.boundary;

        var lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = boundary.Length;

        for (int i = 0; i < boundary.Length; ++i)
        {
            var p = boundary[ i ];

            var pos = new Vector3(p.x, 0, p.y);

            lineRenderer.SetPosition( i, new Vector3( p.x, 0, p.y ) );
        }

        LogPlane();

        //plane.vertexChangedThreshold // The largest value by which a plane's vertex may change before the mesh is regenerated. Units are in meters.
        //plane.pending // Pending means the trackable was added manually (usually via an AddTrackable-style method on its manager) but has not yet been reported as added.
        //plane.useGUILayout ???? what is this

        //Plane infinite_plane = plane.infinitePlane;
        //float distance_from_origin = infinite_plane.distance;
        //Plane flipped_copy = infinite_plane.flipped;
        //Vector3 plane_normal = infinite_plane.normal;
        //float hit_center;
        //bool ray_hit = infinite_plane.Raycast(new Ray(Vector3.zero,Vector3.up),out hit_center);
        //Vector3 dist3 = infinite_plane.ClosestPointOnPlane(Vector3.zero);
        //float dist = infinite_plane.GetDistanceToPoint(Vector3.zero);
        //Vector3 infinite_plane_center = plane.center; // any other way to get the center ? 

        ARUtil.InvokePlaneBoundaryChange(index);
    }
}
