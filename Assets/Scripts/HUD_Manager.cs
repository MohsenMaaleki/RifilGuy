using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Manager : MonoBehaviour
{

    [SerializeField]
    Image WeaponIcon;
    [SerializeField]
    Image NextWeaponIcon;
    [SerializeField]
    TextMeshProUGUI WeaponMagazineText;

    public static HUD_Manager instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    } 

    
    public void UpdateWeaponUI(List<WeaponsStats> playerHeldWeapons,WeaponsStats playerCurrentWeapon)
    {
        //If the player is holding more than 1 weapon
        //activate the nextWeaponIcon 
        NextWeaponIcon.enabled = (playerHeldWeapons.Count > 1);

        if (playerCurrentWeapon != null)
        {

            WeaponIcon.enabled = true;

            WeaponIcon.sprite = playerCurrentWeapon.WeaponImage;

        }
        else
        {
            WeaponIcon.enabled = false;
        }

        foreach(WeaponsStats nextWeapon in playerHeldWeapons)
        {
            if(nextWeapon != playerCurrentWeapon)
            {
                NextWeaponIcon.sprite = nextWeapon.WeaponImage;
                break;
            }
        }
    }

    //called whenever the weapon is shot/switched/dropped/reloaded
    public void UpdateMagazine(WeaponsStats playerCurrentWeapon, bool isreloading, int bulletsFired)
    {
        //if the current weapon is not null
        if(playerCurrentWeapon != null)
        {
            //check to see if the weapon is not reloading
            //we set the magazinetext to show the amount of
            //bullets fired until emptied
            if (!isreloading)
            {
                WeaponMagazineText.text = (playerCurrentWeapon.MagazineSize - bulletsFired + "/" + playerCurrentWeapon.MagazineSize);

            }
            //if the currentWeapon is reloading
            else
            {
                //set the magazine text to "RELOADING"
                WeaponMagazineText.text = ("RELOADING...");
            }
        }
        //if the current weapon is null
        else
        {
            //set the magazine text to empty!
            WeaponMagazineText.text = ("");
        }
        
    }

}
