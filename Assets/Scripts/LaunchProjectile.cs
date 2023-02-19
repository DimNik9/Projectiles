using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField] float ThrowStrength = 20f;
    private Rigidbody projectileRb;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] Slider levelSlider;

    [SerializeField]
    [Range(5, 100)]
    private int LinePoints;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float TimeBetweenPoints = 0.1f;
    private float PointsPerDifficulty;


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
        DefineDifficulty();
        DrawProjection();
    }

    // Update is called once per frame
    void Update()
    {
 
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
                    return;

                }


        }
        
    }

    public void DefineDifficulty()
    {
        int difficultyLevel = (int) levelSlider.value;
        Debug.Log("New difficulty is " + difficultyLevel);
        switch (difficultyLevel)
        {
            case 0:
                PointsPerDifficulty = 50;
                break;
            case 1:
                PointsPerDifficulty = 15;
                break;
            case 2:
                PointsPerDifficulty = 10;
                break;
            case 3:
                PointsPerDifficulty = 6;
                break;
            case 4:
                PointsPerDifficulty = 3;
                break;

        }

        DrawProjection();
        
    }
}
