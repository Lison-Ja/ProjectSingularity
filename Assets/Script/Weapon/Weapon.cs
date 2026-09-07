using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponStat m_WeaponStat;
    [SerializeField] private PlayerController m_Player;

    private float m_FireCooldown;
    private float m_FireDelay;

    private int m_CurrentMag;
    private int m_CurrentReserve;
    private int m_AmmoCost;
    private GameObject m_Projectile;
    private float m_Recoil;

    private bool m_StartShooting;
    private bool m_CanShoot;
    private bool m_Reloading;
    private bool m_AltShoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_WeaponStat == null)
        {
            return;
        }

        SwitchFire();

        m_CurrentMag = m_WeaponStat.MagazineSize;
        m_CurrentReserve = m_WeaponStat.MagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_WeaponStat == null)
        {
            return;
        }

        if (Time.time >= m_FireCooldown && !m_Reloading)
        {

            if (m_CurrentMag > 0)
            {
                m_CanShoot = true;
            }

            else
            {

                Reload();
            }

        }

        // Attack Script
        if (m_CanShoot && m_StartShooting)
        {
            BaseShoot();
        }
    }

    private void BaseShoot()
    {
        Vector2 PlayerDirection = m_Player.GetPlayerDirection();
        Quaternion rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(PlayerDirection.x, PlayerDirection.y) * Mathf.Rad2Deg);
        GameObject newProjectile = Instantiate(m_Projectile, transform.position, rotation);
        m_Player.GetRigidbody2D().AddForce(-PlayerDirection * m_Recoil, ForceMode2D.Impulse);
        m_CanShoot = false;
        m_FireCooldown = Time.time + m_FireDelay;
        m_CurrentMag-= m_AmmoCost;
    }

    


    public void Reload()
    {
        if (m_WeaponStat.SpecialWeapon)
        {
            if (m_CurrentReserve > 0)
            {
                DoReload();
            }
        }
        else
        {
            DoReload();
        }
    }

    public void DoReload()
    {
        StartCoroutine(BaseReload());
    }


    public WeaponStat GetWeaponProfile()
    {
        return m_WeaponStat;
    }

    public void SetStartShoot(bool Result)
    {
        m_StartShooting = Result;
    }

    public void SetAltShoot(bool Result)
    {
        m_AltShoot = Result;
        SwitchFire();
    }

    public void SwitchFire()
    {
        if (m_AltShoot)
        {
            m_FireDelay = 60f / (float)m_WeaponStat.AltRateOfFire;
            m_Projectile = m_WeaponStat.AltProjectile;
            m_Recoil = m_WeaponStat.AltRecoil;
            m_AmmoCost = m_WeaponStat.AltAmmoCost;
        }

        else
        {
            m_FireDelay = 60f / (float)m_WeaponStat.BaseRateOfFire;
            m_Projectile = m_WeaponStat.BaseProjectile;
            m_Recoil = m_WeaponStat.BaseRecoil;
            m_AmmoCost = m_WeaponStat.BaseAmmoCost;
        }
    }

    public void RegainAmmo()
    {
        m_CurrentReserve += m_WeaponStat.MagazineSize;
        m_CurrentReserve = Mathf.Clamp(m_CurrentReserve, 0, m_WeaponStat.ReserveCapasity);
    }

    private IEnumerator BaseReload()
    {
        m_CanShoot = false;
        m_Reloading = true;
        yield return new WaitForSeconds(m_WeaponStat.ReloadSpeed);
        
        if (m_WeaponStat.SpecialWeapon)
        {
            int ChangeInAmmo = m_WeaponStat.MagazineSize - m_CurrentMag;
            if (m_CurrentReserve >= ChangeInAmmo)
            {
                m_CurrentReserve -= ChangeInAmmo;
                m_CurrentMag = m_WeaponStat.MagazineSize;
                m_CanShoot = true;
                m_Reloading = false;
            }

            else
            {
                m_CurrentMag += m_CurrentReserve;
                m_CurrentReserve = 0;
                m_CanShoot = true;
                m_Reloading = false;
            }

        }

        else
        {
            m_CurrentMag = m_WeaponStat.MagazineSize;
            m_CanShoot = true;
            m_Reloading = false;
        }
        
    }
    
};
