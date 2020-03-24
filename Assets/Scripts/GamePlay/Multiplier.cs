using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Multiplier : MonoBehaviour
{
    public int MultiplyFactor;
    public bool Multiplication;
    //public Text UIText;

    private PlayerBehaviour Player;


    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Multiplication)
        {
            MultiplicationOn();
            UIChange();
        }
        else
        {
            //UIText.gameObject.SetActive(false);
        }
    }

    public void MultiplicationOn()
    {
        foreach (var PickUps in Player.GameObjectsinView)
        {
            PickUps.GetComponent<PickUps>().Value = MultiplyFactor;
        }
    }

    public void UIChange()
    {
        //UIText.gameObject.SetActive(true);
        //UIText.text = MultiplyFactor + "x";
    }
}
