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
    private float m_FireCooldown;
    private float m_FireDelay;
    private bool m_StartShooting;

    [Header("Weapon")]
    [SerializeField] private Transform m_WeaponTranform;
    [SerializeField] private float m_WeaponDistance;
    [SerializeField] private Weapon m_CurrentWeapon;
    #endregion

    private void Awake()
    {
        // Get Componenent
        m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        // Create Compoenent
        m_PlayerInputs = new InputActionSystem();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Set weapon delay
        m_FireDelay = 60f/(float)m_CurrentWeapon.RateOfFire;
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
        m_PlayerInputs.Player.Test.performed += Testing;
        

    }



    private void Testing(InputAction.CallbackContext context)
    {

    }

    #region Input Binding

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

    private void Handle_StartShooting(InputAction.CallbackContext context)
    {
        m_StartShooting = true;
    }

    private void Handle_StopShooting(InputAction.CallbackContext context)
    {
        m_StartShooting = false;
    }


    #endregion

    // Update is called once per frame
    private void Update()
    {
        // Turm player
        m_WeaponTranform.localPosition = m_PlayerDirection * m_WeaponDistance;
        float m_DirectionAngle = Mathf.Atan2(m_PlayerDirection.x, m_PlayerDirection.y) * Mathf.Rad2Deg;
        m_Animator.SetFloat("Angle", m_DirectionAngle);

        // Attack Script
        if (Time.time >= m_FireCooldown && m_StartShooting)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, -m_DirectionAngle);
            GameObject newProjectile = Instantiate(m_CurrentWeapon.Projectile, m_WeaponTranform.position, rotation);
            m_FireCooldown = Time.time + m_FireDelay;
            m_RB.AddForce(-m_PlayerDirection * m_CurrentWeapon.Recoil, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Move
        m_WalingForce = m_InputDirection * PlayerWalkingSpeed;
        m_RB.AddForce(m_WalingForce, ForceMode2D.Force);
    }
}
