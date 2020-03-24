using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlatformEffector2D))]
public class Transition : MonoBehaviour
{
    PlayerBehaviour player;
    GameObject playerGameObject;
    KeyboardInput playerKeyboard;
    Collider2D platform;
    void Awake()
    {
        platform = GetComponent<Collider2D>();
        platform.usedByEffector = true;
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.GetComponent<PlayerBehaviour>();
        playerKeyboard = playerGameObject.GetComponent<KeyboardInput>();
    }

    private void Update() {

        if (playerGameObject.transform.position.y < transform.localPosition.y)
        {
            player.IsOnSky = false;
        }
        else if (playerGameObject.transform.position.y >= transform.position.y)
        {
            player.IsOnSky = true;

        }
              
    }

    public void OnCollisionEnter2D(Collision2D other){
        player.CurrentPlatform = GetComponent<BoxCollider2D>();
    }
  

   
}
