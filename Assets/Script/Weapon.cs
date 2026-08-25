using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public string Name = "Weapon";
    public RuntimeAnimatorController Animation;
    public GameObject Projectile;
    public int RateOfFire = 1;
    public float Recoil;
    public int MagazineSize = 1;
    public float ReloadSpeed = 1;
}
