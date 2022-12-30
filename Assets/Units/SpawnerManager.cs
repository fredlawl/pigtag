using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnable;

    [SerializeField]
    private bool spawnInstantly = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject Spawn()
    {
        return Instantiate(spawnable, transform.position, transform.rotation);
    }
}
