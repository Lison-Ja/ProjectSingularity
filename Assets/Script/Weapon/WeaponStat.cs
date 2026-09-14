using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProfile", menuName = "Scriptable Objects/WeaponProfile")]
public class WeaponStat : ScriptableObject
{
    [Header("Main Property")]
    public string Name = "Weapon";
    public RuntimeAnimatorController Animation; 
    public bool SpecialWeapon = true;

    [Header("Capacity")]
    public int MagazineSize = 1;
    public int ReserveCapasity = 1;

    [Header("Reload")]
    public float ReloadCooldown = 1;
    public ReloadType ReloadType = ReloadType.Magazine;
    [HideInInspector] public int AmountAddedPerCycle = 1;
    [HideInInspector] public float DelayReload;

    //Base Fire
    [HideInInspector] public GameObject BaseProjectile;
    [HideInInspector] public int BaseRateOfFire = 1;
    [HideInInspector] public float BaseRecoil;
    [HideInInspector] public float BaseBulletSpread;
    [HideInInspector] public int BaseAmmoCost = 1;
    [HideInInspector] public FireMode BaseFireMode = FireMode.Base;
    //Burst
    [HideInInspector] public int BaseBurstFireRate = 1;
    [HideInInspector] public int BaseNumberInBurst = 1;

    // Alt Fire
    [HideInInspector] public GameObject AltProjectile;
    [HideInInspector] public int AltRateOfFire = 1;
    [HideInInspector] public float AltRecoil;
    [HideInInspector] public float AltBulletSpread;
    [HideInInspector] public int AltAmmoCost = 1;
    [HideInInspector] public FireMode AltFireMode = FireMode.Base;
    //Burst
    [HideInInspector] public int AltBurstFireRate = 1;
    [HideInInspector] public int AltNumberInBurst = 1;
}

public enum FireMode
{
    Base, Burst
}

public enum ReloadType
{
    Magazine, OneByOne
}

[CustomEditor(typeof(WeaponStat))]
public class WeaponStatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponStat _WeaponStat = (WeaponStat)target;

        if (_WeaponStat.ReloadType == ReloadType.OneByOne)
        {
            _WeaponStat.AmountAddedPerCycle = EditorGUILayout.IntField("Amount Added Per Cycle", _WeaponStat.AmountAddedPerCycle);
            _WeaponStat.DelayReload = EditorGUILayout.FloatField("DelayReload", _WeaponStat.DelayReload);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base Fire", EditorStyles.boldLabel);

        _WeaponStat.BaseProjectile = (GameObject) EditorGUILayout.ObjectField("Base Projectile", _WeaponStat.BaseProjectile, typeof(GameObject), true);
        _WeaponStat.BaseRateOfFire = EditorGUILayout.IntField("Base Rate of Fire", _WeaponStat.BaseRateOfFire);
        _WeaponStat.BaseRecoil = EditorGUILayout.FloatField("Base Recoil", _WeaponStat.BaseRecoil);
        _WeaponStat.BaseBulletSpread = EditorGUILayout.FloatField("Base Bullet Spread", _WeaponStat.BaseBulletSpread);
        _WeaponStat.BaseAmmoCost = EditorGUILayout.IntField("Base Ammo Cost", _WeaponStat.BaseAmmoCost);
        _WeaponStat.BaseFireMode = (FireMode) EditorGUILayout.EnumPopup("Base Fire Mode", _WeaponStat.BaseFireMode);

        if (_WeaponStat.BaseFireMode == FireMode.Burst)
        {
            _WeaponStat.BaseBurstFireRate = EditorGUILayout.IntField("Base Burst Fire Rate", _WeaponStat.BaseBurstFireRate);
            _WeaponStat.BaseNumberInBurst = EditorGUILayout.IntField("Base Number In Burst", _WeaponStat.BaseNumberInBurst);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Alt Fire", EditorStyles.boldLabel);

        _WeaponStat.AltProjectile = (GameObject)EditorGUILayout.ObjectField("Base Projectile", _WeaponStat.AltProjectile, typeof(GameObject), true);
        _WeaponStat.AltRateOfFire = EditorGUILayout.IntField("Base Rate of Fire", _WeaponStat.AltRateOfFire);
        _WeaponStat.AltRecoil = EditorGUILayout.FloatField("Base Recoil", _WeaponStat.AltRecoil);
        _WeaponStat.AltBulletSpread = EditorGUILayout.FloatField("Alt Bullet Spread", _WeaponStat.AltBulletSpread);
        _WeaponStat.AltAmmoCost = EditorGUILayout.IntField("Base Ammo Cost", _WeaponStat.AltAmmoCost);
        _WeaponStat.AltFireMode = (FireMode)EditorGUILayout.EnumPopup("Base Fire Mode", _WeaponStat.AltFireMode);

        if (_WeaponStat.AltFireMode == FireMode.Burst)
        {
            _WeaponStat.AltBurstFireRate = EditorGUILayout.IntField("Alt Burst Fire Rate", _WeaponStat.AltBurstFireRate);
            _WeaponStat.AltNumberInBurst = EditorGUILayout.IntField("Alt Number In Burst", _WeaponStat.AltNumberInBurst);
        }

    }
}