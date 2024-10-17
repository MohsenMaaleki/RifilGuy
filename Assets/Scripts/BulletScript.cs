using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //allows you to control the speed of the bullet after it is fired
    [Range(1, 25)] 
    public float bulletSpeed;

    // the amount of time before the bullet destroys itself
    float DestroyTimer = .5F; 

    //Reference to the player who fired the bullet
    public GameObject WhoFiredMe;
    //The particle that is created and plays when the bullet hits something
    public GameObject Hitparticle;
    //The particle that is created and plays when the bullet fires
    public GameObject muzzleFlashParticle;

    

    //Added!!!!!!!!
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        //Ignore any collisions with the player shot this bullet
        Physics.IgnoreCollision(GetComponent<Collider>(), WhoFiredMe.GetComponent<Collider>());

        //creates a muzzleflash particle when the bullet is created.
        GameObject go = Instantiate(muzzleFlashParticle, transform.position, transform.rotation);
        // adds the particle destroyer script to the muzzle particle
        go.AddComponent<ParticleDestroyer>();


       

    }

    // Update is called once per frame
    void Update()
    {
        //moves the bullet in the direction it was fired, at the speed of the Bulletspeed variable
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); 

        //decrements the destroyTimer variable until it reaches 0
        DestroyTimer -= Time.deltaTime;
        //once the DestroyTimer reaches 0
        if (DestroyTimer <= 0)
        {

            //set the timer back to its original value
            DestroyTimer = .5F; 
            //destroy the bullet
            Destroy(gameObject);
        }

    }
    // a function that is called when two colliders collide
    private void OnCollisionEnter(Collision collision)
    {
        //if the bullet hits anything but the player that fired it.
        if (collision.gameObject != WhoFiredMe)
        {
            //creates a hit particle when the bullet hits something
            GameObject go = Instantiate(Hitparticle, transform.position, transform.rotation);
            // adds the particle destroyer script to the hit particle
            go.AddComponent<ParticleDestroyer>();

              




            //Added!!!!!!!!
    //        if (collision.gameObject.GetComponent<DamageableThing>())
     //           collision.gameObject.GetComponent<DamageableThing>().GotHit(damage, WhoFiredMe);

            Destroy(gameObject);//Destroy the bullet


        }
    }

}
