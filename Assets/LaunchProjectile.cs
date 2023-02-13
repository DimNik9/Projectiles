using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField] float ThrowStrength = 20f;
    private Rigidbody projectileRb;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField]
    [Range(5, 100)]
    private int LinePoints;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float TimeBetweenPoints = 0.1f;
    private float PointsPerDifficulty;
    private int difficultyLevel = 3;


    private LayerMask GrenadeCollisionMask;
    private Transform InitialParent;
    private Vector3 InitialLocalPosition;
    private Quaternion InitialRotation;


    private void Awake()
    {
        InitialParent = projectile.transform.parent;
        InitialRotation = projectile.transform.localRotation;
        InitialLocalPosition = projectile.transform.localPosition;
        //projectile.freezeRotation = true;

        int grenadeLayer = projectile.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                GrenadeCollisionMask |= 1 << i; // magic
            }
        }
    }

    void OnEnable()
    {
        //Debug.Log("Broadcast listener");
        Messenger.AddListener(GameEvent.OBSTACLE_HIT, DrawProjection);
    }
    /*void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.OBSTACLE_HIT, DrawProjection);
    } */
    // Start is called before the first frame update
    void Start()
    {
        projectileRb = projectile.GetComponent<Rigidbody>();
        DrawProjection();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 force = transform.TransformPoint(new Vector3(0, launchVelocity, 0));
        //DrawTrajectory.instance.UpdateTrajectory(force, projectileRb, gameObject.transform.position);

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, ThrowStrength, 0), ForceMode.Impulse);
            StartCoroutine(DestroyCannonball(ball));
        }
        
    }
    private IEnumerator DestroyCannonball(GameObject sphere)
    {
        yield return new WaitForSeconds(3);
        Destroy(sphere);
    }


    public void DrawProjection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints  / TimeBetweenPoints) + 1;
        DefineDifficulty();
        
        Vector3 startPosition = gameObject.transform.position;
        Vector3 startVelocity = ThrowStrength * gameObject.transform.up / projectileRb.mass;
        
        float diff = PointsPerDifficulty * TimeBetweenPoints;
        lineRenderer.positionCount = Mathf.CeilToInt(PointsPerDifficulty) + 1;


        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < diff; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);


            lineRenderer.SetPosition(i, point);
            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);


         if (Physics.Raycast(lastPosition,
                    (point - lastPosition).normalized,
                    out RaycastHit hit,
                    (point - lastPosition).magnitude,
                    GrenadeCollisionMask
                   ))
                {

                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    Debug.Log(i);
                    return;

                }


        }
        
    }

    public void DefineDifficulty()
    {
        if (difficultyLevel == 0)
        {
            PointsPerDifficulty = 50;
        }else if (difficultyLevel == 1)
        {
            PointsPerDifficulty = 30;
        }else if (difficultyLevel == 2)
        {
            PointsPerDifficulty = 10;
        }else if (difficultyLevel == 3)
        {
            PointsPerDifficulty = 7;
        }
        
    }
}
