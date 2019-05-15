using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    private Button restart;
    public Text restartText;
    // Start is called before the first frame update
    public void RestartGame()
    {
        SceneManager.LoadScene("Pong2Scene");
        restart = GameObject.Find("restart").GetComponent<Button>();
        restart.image.enabled = false;
        restartText = GameObject.Find("restartText").GetComponent<Text>();
        restartText.enabled = false;
    }
}
