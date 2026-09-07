using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_ProjectileSpeed;
    [SerializeField] private float m_LifeSpan;

    private Rigidbody2D m_RB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float Angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 Direction = new Vector2(-Mathf.Sin(Angle), Mathf.Cos(Angle));
        m_RB.linearVelocity = Direction * m_ProjectileSpeed;
        Destroy(gameObject, m_LifeSpan);
    }
    
}
