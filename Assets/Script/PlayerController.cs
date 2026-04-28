using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_RB;
    private InputActionSystem m_PlayerInputs;

    private Vector2 m_DirectionInput;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_PlayerInputs = new InputActionSystem();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        m_PlayerInputs.Enable();
        m_PlayerInputs.Player.Move.performed += Handle_Move;
        m_PlayerInputs.Player.Move.canceled += Handle_StopMove;

    }

    #region Input Binding

    private void Handle_Move(InputAction.CallbackContext context)
    {
        m_DirectionInput = context.ReadValue<Vector2>();
    }

    private void Handle_StopMove(InputAction.CallbackContext context)
    {
        m_DirectionInput = Vector2.zero;
    }

    #endregion

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_RB.AddForce(m_DirectionInput, ForceMode2D.Force);
    }
}
