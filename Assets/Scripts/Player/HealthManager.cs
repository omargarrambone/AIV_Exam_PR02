using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    [SerializeField] private float setMaxHealth = 100;
    public float CurrentHealth;
    public UnityEvent OnDeath;
    private Animator anim;
    public bool IsImmune;
    public bool IsDead { get { return CurrentHealth <= 0; } }
    public BarScript HealthBar;
    void Start()
    {
        MaxHealth = setMaxHealth;

        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);

        anim = GetComponent<Animator>();
    }

    public void AddHealth(float health)
    {
        CurrentHealth += health;

        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

        HealthBar.SetHealth(CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        if (IsImmune) return;

        CurrentHealth -= damage;

        // if (IsDead)
        // {
        //GetComponent<CapsuleCollider>().enabled = false;
        //anim.SetTrigger("Death");
        //}

        HealthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetHealth(CurrentHealth);
        anim.Play("Idle");
    }
}
