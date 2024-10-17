using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponsStats
{
    
    public string Name;
    public int Damage;
    public int ScreenShakeForce;
    public float firerate;
    public int MagazineSize;
    public GameObject projectile;
    public Transform MuzzlePoint;
    public GameObject WeaponPrefab;
    public Sprite WeaponImage;
    public AudioClip WeaponShotSound;
    public AudioClip WeaponPickupSound;


}
