using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    public float CurrentHealth;

    [SerializeField] Slider Healthbar;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentHealth = MaxHealth;
        Healthbar.value = CurrentHealth;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        Healthbar.value = CurrentHealth / MaxHealth;
    }

    public void SetHealth(float amount)
    {
        CurrentHealth = amount;
        Healthbar.value = CurrentHealth / MaxHealth;
    }

    public void UpdateHealth()
    {
        Healthbar.value = CurrentHealth / MaxHealth;
    }
}
