using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject SpawnInactive()
    {
        GameObject obj = SpawnActive();
        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnActive()
    {
        return Instantiate(spawnable, transform.position, transform.rotation);
    }
}
