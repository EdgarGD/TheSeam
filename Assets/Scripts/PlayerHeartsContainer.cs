using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PlayerHeartsContainer : MonoBehaviour
{
    public Sprite[] Hearts;
    //public Image Image;

    private int _playerHealth;

    void Awake()
    {
         //Image = GetComponent<Image>();
    }


    void Update()
    {

        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>().Health;

        if (_playerHealth <= 0)
        {
            _playerHealth = 0;
        }
        //Image.sprite = Hearts[_playerHealth];

    }
}
