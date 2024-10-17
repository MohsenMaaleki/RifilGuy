using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class WeaponScript : MonoBehaviour
{
    //Each weapon will have its on stats and prefabs we will
    //set them up in the editor!
    public WeaponsStats stats;

    // Start is called before the first frame update
    void Start()
    {
        //Calls the function that starts the weapons ground loot animations
        AnimateWeaponOnGround();
    }
    public void AnimateWeaponOnGround()
    {
        //prepare the weapon for animation by positioning and scaling it
        transform.localScale = new Vector3(2, 2, 2);
        transform.rotation = Quaternion.Euler(0, 0, 90);

        //make the weapon jump up a bit when on the ground
        transform.DOJump(new Vector3(transform.position.x, transform.position.y + .30f, transform.position.z), .5f, 1, 3.0f).SetLoops(-1).SetEase(Ease.Linear);

        //Start to continuously rotate the weapon 
        transform.DORotate(new Vector3(0, 360, 90), 6.0f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

    }

    private void OnTriggerEnter(Collider other)
    {
        //if the gameobject that collides with this has a ShootingScript attached
        if (other.GetComponent<ShootingScript>())
        {
            // if the player is not holding max weapons (2)
            if (other.GetComponent<ShootingScript>().HeldWeapons.Count < 2)
            {
                // Ends all the "tweens" active on this weapon
                transform.DOKill();                
                // calls the add weapon function on the player that collided
                other.GetComponent<ShootingScript>().AddWeapon(this.gameObject);


            }
        }
    }

   

    //called when we want to drop a single weapon
    public void DropWeapon(Vector3 playerLocation)
    {
        //Randomly drops the weapon in a location near the player
        transform.position = new Vector3(playerLocation.x + Random.Range(0, 2)*3-1, playerLocation.y, playerLocation.z + Random.Range(0,2)*3 -1);

        //The weapon is back on the ground...make it dance
        AnimateWeaponOnGround();

    }

    

}
