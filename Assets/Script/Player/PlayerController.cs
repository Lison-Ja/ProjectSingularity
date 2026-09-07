using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class PlayerController : MonoBehaviour
{
    #region Component

    private Rigidbody2D m_RB;
    private Animator m_Animator;

    [Header("Component")]
    [SerializeField] private Camera m_Camera;

    #endregion

    #region PlayerInput

    private InputActionSystem m_PlayerInputs;
    private Vector2 m_WalingForce;
    private Vector2 m_InputDirection;
    private Vector2 m_PlayerDirection;
    private bool UsingMouse;

    #endregion

    #region PlayerMovement 

    [Header("Player Movement")]
    [SerializeField] private float PlayerWalkingSpeed;
    [SerializeField] private float PlayerMinWalkSpeed;

    #endregion

    # region Weapon

    private int m_WeaponPointer = 0;
    
    [Header("Weapon")]
    [SerializeField] private Transform m_WeaponTranform;
    [SerializeField] private float m_WeaponDistance;
    [SerializeField] private Weapon[] m_Weapon;

    #endregion

    private void Awake()
    {
        // Get Componenent
        m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        // Create Compoenent
        m_PlayerInputs = new InputActionSystem();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        m_PlayerInputs.Enable();
        m_PlayerInputs.Player.Move.performed += Handle_Move;
        m_PlayerInputs.Player.Move.canceled += Handle_StopMove;
        m_PlayerInputs.Player.MouseAim.performed += Handle_MouseAim;
        m_PlayerInputs.Player.ControllerAim.performed += Handle_ControllerAim;
        m_PlayerInputs.Player.Shoot.performed += Handle_StartShooting;
        m_PlayerInputs.Player.Shoot.canceled += Handle_StopShooting;
        m_PlayerInputs.Player.Reload.performed += Handle_Reload;
        m_PlayerInputs.Player.AltFire.performed += Handle_AltFireMode;
        m_PlayerInputs.Player.AltFire.canceled += Handle_BaseFireMode;
        m_PlayerInputs.Player.SwapWeapon.performed += Handle_SwarpWeapon;
        m_PlayerInputs.Player.SelectPrimaryWeapon.performed += Handle_PrimarySelected;
        m_PlayerInputs.Player.SelectSpecialWeapon1.performed += Handle_Special1Selected;
        m_PlayerInputs.Player.SelectSpecialWeapon2.performed += Handle_Special2Selected;
    }

    #region Input Binding

    // Input for moving
    private void Handle_Move(InputAction.CallbackContext context)
    {
        m_InputDirection = context.ReadValue<Vector2>();
        m_Animator.SetBool("Move", true);
    }

    private void Handle_StopMove(InputAction.CallbackContext context)
    {
        m_InputDirection = Vector2.zero;
        m_Animator.SetBool("Move", false);
    }


    // Input for Aiming
    private void Handle_MouseAim(InputAction.CallbackContext context)
    {
        UsingMouse = true;
        Vector2 MousePos = m_Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        Vector2 PlayerPos = gameObject.transform.position;
        Vector2 MouseDirection = (MousePos - PlayerPos).normalized;
        m_PlayerDirection = MouseDirection;
    }
    private void Handle_ControllerAim(InputAction.CallbackContext context)
    {
        UsingMouse = false;
        Vector2 ControllerDirection = context.ReadValue<Vector2>();
        m_PlayerDirection = ControllerDirection;
    }

    // Input for Weapon
    private void Handle_StartShooting(InputAction.CallbackContext context)
    {
        m_Weapon[m_WeaponPointer].SetStartShoot(true);
    }

    private void Handle_StopShooting(InputAction.CallbackContext context)
    {
        m_Weapon[m_WeaponPointer].SetStartShoot(false);
    }

    private void Handle_Reload(InputAction.CallbackContext context)
    {
        m_Weapon[m_WeaponPointer].Reload();
    }

    private void Handle_BaseFireMode(InputAction.CallbackContext context)
    {
        m_Weapon[m_WeaponPointer].SetAltShoot(false);
    }

    private void Handle_AltFireMode(InputAction.CallbackContext context)
    {
        m_Weapon[m_WeaponPointer].SetAltShoot(true);
    }


    // This section of input change the current equipped weapon
    private void Handle_SwarpWeapon(InputAction.CallbackContext context)
    {
        int change = (int)context.ReadValue<Vector2>().y;


        m_WeaponPointer += change;

        if (m_WeaponPointer < 0)
        {
            m_WeaponPointer = 2;

        }

        else if (m_WeaponPointer > 2)
        {
            m_WeaponPointer = 0;

        }

        bool isWeapom = true;
        while (isWeapom)
        {


            if (m_Weapon[m_WeaponPointer].GetWeaponProfile() != null)
            {
                isWeapom = false;
            }

            else
            {
                m_WeaponPointer += change;

                if (m_WeaponPointer < 0)
                {
                    m_WeaponPointer = 2;
                }

                else if (m_WeaponPointer > 2)
                {
                    m_WeaponPointer = 0;
                }
            }
        }
    }

    private void Handle_PrimarySelected(InputAction.CallbackContext context)
    {
        if (m_Weapon[0].GetWeaponProfile() != null)
        {
            m_WeaponPointer = 0;
        }
    }

    private void Handle_Special1Selected(InputAction.CallbackContext context)
    {
        if (m_Weapon[1].GetWeaponProfile() != null)
        {
            m_WeaponPointer = 1;
        }
    }

    private void Handle_Special2Selected(InputAction.CallbackContext context)
    {
        if (m_Weapon[2].GetWeaponProfile() != null)
        {
            m_WeaponPointer = 2;
        }
    }

    #endregion


    private void Update()
    {
        // Turm player
        m_WeaponTranform.localPosition = m_PlayerDirection * m_WeaponDistance;
        float m_DirectionAngle = Mathf.Atan2(m_PlayerDirection.x, m_PlayerDirection.y) * Mathf.Rad2Deg;
        m_Animator.SetFloat("Angle", m_DirectionAngle);
    }

    private void FixedUpdate()
    {
        // Move
        m_WalingForce = m_InputDirection * PlayerWalkingSpeed;
        m_RB.AddForce(m_WalingForce, ForceMode2D.Force);
    }

    private void OnDisable()
    {
        m_PlayerInputs.Player.Move.performed -= Handle_Move;
        m_PlayerInputs.Player.Move.canceled -= Handle_StopMove;
        m_PlayerInputs.Player.MouseAim.performed -= Handle_MouseAim;
        m_PlayerInputs.Player.ControllerAim.performed -= Handle_ControllerAim;
        m_PlayerInputs.Player.Shoot.performed -= Handle_StartShooting;
        m_PlayerInputs.Player.Shoot.canceled -= Handle_StopShooting;
        m_PlayerInputs.Player.Reload.performed -= Handle_Reload;
        m_PlayerInputs.Player.AltFire.performed -= Handle_AltFireMode;
        m_PlayerInputs.Player.AltFire.canceled -= Handle_BaseFireMode;
        m_PlayerInputs.Player.SwapWeapon.performed -= Handle_SwarpWeapon;
        m_PlayerInputs.Player.SelectPrimaryWeapon.performed -= Handle_PrimarySelected;
        m_PlayerInputs.Player.SelectSpecialWeapon1.performed -= Handle_Special1Selected;
        m_PlayerInputs.Player.SelectSpecialWeapon2.performed -= Handle_Special2Selected;
        m_PlayerInputs.Disable();
    }

    #region Getter and Setter

    public Rigidbody2D GetRigidbody2D()
    {
        return m_RB;
    }

    public Vector2 GetPlayerDirection()
    {
        return m_PlayerDirection;
    }

    #endregion
}
