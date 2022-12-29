using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health = 100f;

    [SerializeField]
    private bool isImmune = false;

    public UnityEvent onDied;
    public UnityEvent onImmunitySet;
    public UnityEvent<float> onDamaged;

    /**
     * This takes a callback because there can be
     * a race that if a caller destroys the game object
     * in onDied() event, then any cleanup after damage
     * applied wont work because of a bad refference.
     */
    public TResult TakeDamage<TResult>(float amount, Func<TResult> callback)
    {
        ConsumeDamage(amount);

        TResult result = callback();

        if (IsDead())
        {
            onDied?.Invoke();
        }

        return result;
    }

    public void TakeDamage(float amount)
    {
        ConsumeDamage(amount);

        if (IsDead())
        {
            onDied?.Invoke();
        }
    }

    private void ConsumeDamage(float amount)
    {
        if (isImmune)
        {
            onDamaged?.Invoke(0);
            return;
        }

        health -= amount;
        onDamaged?.Invoke(amount);
    }

    public void SetImmunity(bool immune)
    {
        isImmune = immune;
        onImmunitySet?.Invoke();
    }

    public void ToggleImmunity()
    {
        SetImmunity(!isImmune);
    }

    public void Kill()
    {
        /*
         * TODO: Handle overflow problem
         * Astute programmers may notice that
         * we're using a float type. What if
         * health is set to MAX_FLOAT and we
         * add one to it? It overflows!
         */
        ConsumeDamage(health + 1);
        onDied?.Invoke();
    }

    public bool IsImmune() => isImmune;
    public bool IsDead() => health <= 0 && !isImmune;
}
