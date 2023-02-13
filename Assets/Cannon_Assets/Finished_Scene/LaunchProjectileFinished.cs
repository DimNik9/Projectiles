using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectileFinished : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 700f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject ball = Instantiate(projectile,transform.position, transform.rotation);

            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity,0));
            Debug.Log("Destroy");
            StartCoroutine(DestroyCannonball(ball));
            
        }
    }

    private IEnumerator DestroyCannonball(GameObject sphere)
    {
        yield return new WaitForSeconds(2);
        //Debug.Log("Destroy");
        Destroy(sphere);
    }
}
