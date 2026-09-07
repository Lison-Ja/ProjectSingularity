using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProfile", menuName = "Scriptable Objects/WeaponProfile")]
public class WeaponStat : ScriptableObject
{
    [Header("Main Property")]
    public string Name = "Weapon";
    public RuntimeAnimatorController Animation;
    public float ReloadSpeed = 1;
    public bool SpecialWeapon = true;

    [Header("Capacity")]
    public int MagazineSize = 1;
    public int ReserveCapasity = 1;

    [Header("Base Fire")]
    public GameObject BaseProjectile;
    public int BaseRateOfFire = 1;
    public float BaseRecoil;
    public int BaseAmmoCost = 1;


    [Header("Alt Fire")]
    public GameObject AltProjectile;
    public int AltRateOfFire = 1;
    public float AltRecoil;
    public int AltAmmoCost = 1;
}
