using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceCardOnSurface : MonoBehaviour
{
    public static event System.Action<GameObject> OnCardPlaced;

    ARRaycastManager raycaster;

    public GameObject[] cards;

    List<GameObject> spawned;
    
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        spawned = new List<GameObject>();

        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if( Input.touchCount < 1 ) return;

        Touch touch = Input.GetTouch(0);

        if ( touch.phase != TouchPhase.Began ) return;

        bool hit = raycaster.Raycast( touch.position, hits, TrackableType.PlaneWithinPolygon );

        if( ! hit || hits.Count < 1 ) return;

        Pose pose = hits[0].pose;

        var pos = pose.position;

        var rot = pose.rotation * Quaternion.Euler(0, 180f, 0) * Quaternion.Euler(- 90f, 0, 0);

        var card = cards[ Random.Range( 0, cards.Length ) ];

        var go = Instantiate(card, pos, rot);

        go.transform.localScale = new Vector3( 0.064f, 0.089f, 0.1f ) * 0.65f;

        spawned.Add( go );

        OnCardPlaced?.Invoke( go );
    }
}
