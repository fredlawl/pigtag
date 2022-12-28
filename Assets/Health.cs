using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health = 100f;

    [SerializeField]
    private bool isImmune = false;

    public UnityEvent onNoHealth;
    public UnityEvent onImmunitySet;
    public UnityEvent<float> onDamaged;

    public void TakeDamage(float amount)
    {
        if (isImmune)
        {
            onDamaged.Invoke(0);
            return;
        }

        health -= amount;
        onDamaged.Invoke(amount);

        if (health <= 0)
        {
            onNoHealth.Invoke();
        }
    }

    public void SetImmunity(bool immune)
    {
        isImmune = immune;
        onImmunitySet.Invoke();
    }

    public void ToggleImmunity()
    {
        SetImmunity(!isImmune);
    }

    public bool IsImmune()
    {
        return isImmune;
    }
}
