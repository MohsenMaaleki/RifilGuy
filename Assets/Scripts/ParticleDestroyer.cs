using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //destroys the particle gameobject at the end of its duration.
        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
    }


}
