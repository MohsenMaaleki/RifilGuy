using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraMovement : MonoBehaviour
{
    //reference to the player
    public Transform player;

    [SerializeField]
    //Variable used to set the distance between the player and the camera
    Vector3 offset; 

    //A static variable, used so that other scripts can easily
    //access the functions contained within this camera script
    public static CameraMovement instance;
    void Awake()
    {
        //if an instance of the camera script does not yet exist
        if (instance == null)
        {
            //set this one as the static instance
            instance = this;
        }        

    }

    // Update is called once per frame
    void Update()
    {
        // follow the player at a distant of the variable offset
        transform.position = player.position + offset;
    }

    //Called whenever we shoot a bullet or explode a grenade/rocket
    public void CameraShake()
    {
        transform.DOComplete();
        //Use a tween to shake the camera a little bit
        transform.transform.GetChild(0).DOShakePosition(.04f, .08f, 2, 90);

    }
}
