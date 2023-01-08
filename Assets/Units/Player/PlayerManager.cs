using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public UnityEvent onDied;
        
        public void Awake()
        {
        }

        public void OnDamaged(float amount)
        {
            Debug.Log($"{gameObject.name} damaged for {amount}!");
        }

        public void OnDied()
        {
            Debug.Log($"{gameObject.name} died!");
            gameObject.SetActive(false);
            onDied?.Invoke();
        }
    }
}
