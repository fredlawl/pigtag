using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;

    private void Awake()
    {
        Debug.Log("Awake called");
    }

    private void Start()
    {
        /*
         * TODO: We'll spawn a player/enemy in at start
         * of game, so this lookup wont be necessary
         * but we'll leave it for now.
         */
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");

        Debug.Log("Start called");
    }

    public void OnCountdownTimerFinished()
    {
        Debug.Log("Game over!");
        enemy.GetComponent<Health>()?.Kill();
    }
}
