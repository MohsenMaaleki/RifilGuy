using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    KillCounter killCountScript;
   
    // Start is called before the first frame update
    void Start()
    {
         killCountScript = GameObject.Find("KCO").GetComponent<KillCounter>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            killCountScript.Addkill();
            
            Destroy(gameObject);
            Debug.Log("Player hit");
        }
    }
}
