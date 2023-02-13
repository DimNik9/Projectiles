using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{

    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField]
    [Range(3, 30)]
    private int _lineSegmentCount = 100;

    private List<Vector3> _linePoints = new List<Vector3>();

    public static DrawTrajectory instance;

    private void Awake()
    {
        instance = this; 
    }

    public void UpdateTrajectory(Vector3 force, Rigidbody rb, Vector3 startingPoint)
    {
        Vector3 velocity = (force / rb.mass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = FlightDuration / _lineSegmentCount;

        _linePoints.Clear();

        for (int i=0; i < _lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed);

            _linePoints.Add(-MovementVector + startingPoint);
        }
        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
