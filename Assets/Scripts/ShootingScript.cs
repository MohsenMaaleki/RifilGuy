using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ShootingScript : MonoBehaviour
{

    //Firing Stuff
    //a variable to test the firerate against
    float timer = 0;

    //Weapon Stuff
    //List of Weaponstats to hold data for the weapons the player is holding
    public List<WeaponsStats> HeldWeapons = null;

    //Where the player model is holding the weapon
    WeaponsStats CurrentWeapon = new WeaponsStats();

    //The location of the hand that holds the weapon
    public Transform PlayerHand;

    //Maintain count of the number of bullets we have fired
    int BulletsFired = 0;

    //Used to determine if the current gun is reloading
    bool isReloading = false;


    private void Start()
    {
        CurrentWeapon = null;
        
    }

    //a function called from the Playermovement script that starts the firing process
    public void FireBullets()
    {
        // starts a coroutine 
        StartCoroutine(SpawnAtEndOfFrame());
    }




    //this coroutine waits until the end of the frame and fires a bullet 
    IEnumerator SpawnAtEndOfFrame()
    {
        //Wait for the end of frame
        yield return new WaitForEndOfFrame();
        //-Check to make sure the currentWeapon is set before attempting to shoot
        //also check to make sure the gun is *not* reloading
        if (CurrentWeapon != null && !isReloading)
        {
            //if the time between fires has surpassed the fire rate, the gun can fire again.
            if (CurrentWeapon.firerate + timer <= Time.time)
            {

                //-Shake the camera everytime the gun is shot
                CameraMovement.instance.CameraShake();


                timer = Time.time;

                //create a bullet at the muzzle point, with the players last rotation.
                GameObject go = Instantiate(/*ADDED*/CurrentWeapon.projectile, /*ADDED*/CurrentWeapon.MuzzlePoint.position, transform.rotation);

                //the "WhoFiredMe" variable in the bullet script is set to this players Gameobject.
                go.GetComponent<BulletScript>().WhoFiredMe = this.gameObject;
                //Set the damage of the bullet to the currentWeapons damage
                go.GetComponent<BulletScript>().damage = CurrentWeapon.Damage;

                //-Add 1 to the bullets fired
                BulletsFired += 1;

                //Plays the sound we assigned to the current weapons shot
                GetComponent<AudioSource>().PlayOneShot(CurrentWeapon.WeaponShotSound);

                //Update the magazine stat using the HUD_Manager
                HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false, BulletsFired);

                //If our bullets fired is more or equal to the mag size
                //of this weapon then we set isReloading to true and start the reload
                if (BulletsFired >= CurrentWeapon.MagazineSize)
                {
                    isReloading = true;
                    StartCoroutine(ReloadWeapon());
                }
            }
        }

    }


    IEnumerator ReloadWeapon()
    {
        //Update the HUD_Manager to show the weapon is reloading
        HUD_Manager.instance.UpdateMagazine(CurrentWeapon, true/*true=reloading*/, BulletsFired);

        //Reloading time is over
        yield return new WaitForSeconds(1);
        isReloading = false;

        //reset the bullets fired
        BulletsFired = 0;

        //Update the HUD_Manager to show the weapon is no longer reloading
        HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false/*false=is not reloading*/, BulletsFired);
    }

    public void AddWeapon(GameObject WeaponBeingPickedUp)
    {
        
        // if the player is not holding atleast two weapons
        if (HeldWeapons.Count < 2)
        {
            //turns the Weapon gameobject off. We only need to turn a 
            //weapon on if the current weapon is empty.
            WeaponBeingPickedUp.SetActive(false);

            //Adds the weapon data from the WeaponBeingPickedUp
            HeldWeapons.Add(WeaponBeingPickedUp.GetComponent<WeaponScript>().stats);

            //decrease the weapon size back to normal before picking up
            //since the weapon prefabs we are using are much bigger than player
            WeaponBeingPickedUp.transform.localScale = new Vector3(1, 1, 1);
            //Set the WeaponBeingPickedUp as a child of the players hand
            WeaponBeingPickedUp.transform.SetParent(PlayerHand);
            //Move the pickedUpWeapon to correct position in the players hand
            WeaponBeingPickedUp.transform.localPosition = new Vector3(0, 0, 0);
            //Rotate the Weapon to the correct rotation in the players hand
            WeaponBeingPickedUp.transform.localRotation = Quaternion.Euler(0, 90, 90);

            //Plays the sound we assigned to the current when picked up
            GetComponent<AudioSource>().PlayOneShot(WeaponBeingPickedUp.GetComponent<WeaponScript>().stats.WeaponPickupSound);
        }
       
        //if the current weapon has not been set yet
        if (CurrentWeapon == null)
        {
            //Set the first weapon in the list as the weapon
            CurrentWeapon = WeaponBeingPickedUp.GetComponent<WeaponScript>().stats;
            //Turn on the weapon in the players hand
            WeaponBeingPickedUp.SetActive(true);
        }
        
        //update the magazine and WeaponsUI
        HUD_Manager.instance.UpdateWeaponUI(HeldWeapons, CurrentWeapon);
        HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false, BulletsFired);

    }


    //If the gamer touches the SwitchWeapons button
    //this function is called
    public void SwitchWeapons()
    {
        if (HeldWeapons.Count > 1)
        {
            //Turn off the currentWeapon first
            CurrentWeapon.WeaponPrefab.SetActive(false);

            foreach (WeaponsStats weapon in HeldWeapons)
            {
                if (CurrentWeapon != weapon)
                {
                    CurrentWeapon = weapon;
                    //Turn on the weapon in the players hand
                    weapon.WeaponPrefab.SetActive(true);
                    //We break the foreach loop here
                    break;
                }

            }
            //reset the bullets fired (this could lead to switch weapon cheese lol)
            BulletsFired = 0;

            //Update the UI using the HUD_Manager
            HUD_Manager.instance.UpdateWeaponUI(HeldWeapons, CurrentWeapon);
            HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false, BulletsFired);
        }
        else if (HeldWeapons.Count == 1)
        {
            //the only index in held weapons will be the new current weapon
            CurrentWeapon = HeldWeapons[0];
            //Turn on the weapon in the players hand
            CurrentWeapon.WeaponPrefab.SetActive(true);

            //update the magazine and WeaponsUI
            HUD_Manager.instance.UpdateWeaponUI(HeldWeapons, CurrentWeapon);
            HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false, BulletsFired);
        }

    }

   

    //Called when the player wants to drop the weapon
    //by swiping up on the weapon icon
    public void DropWeapon(ScrollRect scrollRect)
    {
        //If the player scrolls the ScrollRect up to the limit (130) for me
        //Then we will toss the weapon
        if (scrollRect.content.position.y >= 130)
        {
            //We turn the scrollrect off, return its position to default
            // and then turn it back on
            scrollRect.enabled = false;

            scrollRect.content.position = new Vector3(
            scrollRect.content.position.x, 0,scrollRect.content.position.z);

            scrollRect.enabled = true;

            //iterate through each weapon in held weapons
            foreach (WeaponsStats weapon in HeldWeapons)
            {
                //if the weapon we are checking now is the the CurrentWeapon
                if (CurrentWeapon.WeaponPrefab == weapon.WeaponPrefab)
                {
                   
                    //Unparent them from the player
                    weapon.WeaponPrefab.transform.SetParent(null);

                    //Set them to active to make them visible again
                    weapon.WeaponPrefab.SetActive(true);

                    // orient the discarded weapon
                    weapon.WeaponPrefab.transform.rotation = Quaternion.Euler(0, 0, 90);

                    //Called the dropweapon function on the weapon to "jump" it away
                    weapon.WeaponPrefab.GetComponent<WeaponScript>().DropWeapon(transform.position);

                    //set the currentWeapon back to null and remove the discarded
                    //weapon from the list of HeldWeapons
                    CurrentWeapon = null;
                    HeldWeapons.Remove(weapon);

                    break;
                }
            }
            //reset the bullets fired
            BulletsFired = 0;

            //We call the switch weapons function after dropping a 
            //weapon to check for a switch to a second weapon that is held
            SwitchWeapons();

            //update the magazine and WeaponsUI
            HUD_Manager.instance.UpdateWeaponUI(HeldWeapons, CurrentWeapon);
            HUD_Manager.instance.UpdateMagazine(CurrentWeapon, false, BulletsFired);
        }
        //As long as the ScrollRect is not above the limit you choose
        //Return without doing anything
        else
        {
            return;
        }

    }

   

    



}











