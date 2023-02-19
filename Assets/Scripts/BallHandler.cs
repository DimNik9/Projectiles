using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BallHandler : MonoBehaviour
{
    public GameObject launchOrigin;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("I hit a " + collision.gameObject.tag);
        Messenger.Broadcast(GameEvent.OBSTACLE_HIT);
        if (collision.gameObject.tag == "Block")
        {
            Destroy(collision.gameObject);
            
        }
        
    }


}
