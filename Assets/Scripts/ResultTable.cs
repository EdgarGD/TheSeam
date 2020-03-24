using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ResultTable : MonoBehaviour
{
    public GameObject resultPanel;
    public int _scoreCoins;
   // public Text collectedCarrot;
    //public Text traversed;
    //public Text Distace;
    public PlayerBehaviour player;
    public Transform startPosition;
    public float distance;
    public float distanceSpotter;

    void Start()
    {
        _scoreCoins = GameObject.Find("GameManager").GetComponent<GameManager>()._scoreCoins;
        resultPanel.SetActive(false);
        startPosition.transform.position = player.gameObject.transform.position;
    }

   
    void Update()
    {
        distance = Mathf.Round((player.transform.position.x - startPosition.transform.position.x) * distanceSpotter);
        //Distace.text = "" + distance;
        if (!player.IsAlive)
        {
            resultPanel.SetActive(true);
            Output();
        }
    }

    void Output()
    {
        //distance = player.transform.position.x - startPosition.transform.position.x / distanceSpotter;
        _scoreCoins = GameObject.Find("GameManager").GetComponent<GameManager>()._scoreCoins;
        //collectedCarrot.text = "Собранная морковь " + _scoreCoins;
        //traversed.text = "Пройдено " + distance + " метров";
    }
}

