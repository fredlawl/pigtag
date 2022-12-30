using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class BuildingManager : MonoBehaviour
    {
        public void OnDamaged(float amount)
        {
            /*
             * TODO: There's a bug here
             * For w/e reason, this is given the value
             * passed in via the UI instead of from the 
             * .Invoke() :thinkingface:
             */
            Debug.Log($"{gameObject.name} damaged for {amount}!");
        }

        public void OnDied()
        {
            Debug.Log($"{gameObject.name} died!");
            gameObject.SetActive(false);
        }
    }
}
