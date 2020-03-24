using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using UnityEngine.Video;

public class LogoVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void FixedUpdate() {
         if (!videoPlayer.isPlaying)
            {
                SceneManager.LoadScene("StartMenu",LoadSceneMode.Single);
            }    
    }
}
