using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public GameObject doorOpen;
    public GameObject player;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.NextLevel();
            doorOpen.SetActive(true);
            player.GetComponent<SpriteRenderer>().enabled = false;

        }
    }
}
