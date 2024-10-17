using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;
    //player health
    public int health = 50;

  
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetSize(health / 100f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //a function that is called when two colliders collide
    void OnCollisionEnter(Collision collision)
    {
        //if the bullet hits anything but the player that fired it.
        if (collision.gameObject.tag == "Enemy")
        {
            //decrement the health of the player
            health -= 10;
            //update the health bar
            healthBar.SetSize(health / 100f);


            
            //if the player's health reaches 0
            if (health <= 0)
            {

                
                FindObjectOfType<GameManager>().EndGame();
                
                //destroy the player
                //Destroy(gameObject);
            }
        }
    }

}
