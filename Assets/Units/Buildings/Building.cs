using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Building
{
    public class Building : MonoBehaviour
    {
        public enum BuildingState
        {
            Placing,
            Building,
            Built,
            Stop
        }

        private GameObject owner;
        
        public float constructionTimeInSeconds;

        // Default state for buildings already placed
        public BuildingState state = BuildingState.Built;
        public KeyCode shortcut;
        public SpriteRenderer sr;
        public Health health;

        public delegate void OnBuiltEvent(GameObject obj);
        public event OnBuiltEvent onBuilt;

        public void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            health = GetComponent<Health>();
        }

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

        public void Update()
        {
            if (state == BuildingState.Placing || state == BuildingState.Building)
            {
                sr.color = new Color()
                {
                    r = sr.color.r,
                    g = sr.color.g,
                    b = sr.color.b,
                    a = 0.5f,
                };
            }

            // Force stop state when player moves after placing starts and construction begins
            if (state == BuildingState.Stop)
            {
                Destroy(this);
            }
        }

        public GameObject Owner()
        {
            return owner;
        }

        public Building Spawn(GameObject owner)
        {
            var building = Instantiate(this, Vector3.zero, transform.rotation);
            building.owner = owner;
            building.state = BuildingState.Placing;
            building.health.SetImmunity(true);
            return building;
        }

        public IEnumerator Construction()
        {
            yield return new WaitForSeconds(constructionTimeInSeconds);
            state = BuildingState.Built;
            sr.color = new Color()
            {
                r = sr.color.r,
                g = sr.color.g,
                b = sr.color.b,
                a = 1f,
            };
            health.SetImmunity(false);
            onBuilt?.Invoke(gameObject);
        }

        public bool Construct(Vector3 pos)
        {
            if (state == BuildingState.Building)
            {
                return false;
            }

            state = BuildingState.Building;

            StartCoroutine(Construction());

            return true;
        }
    }
}
