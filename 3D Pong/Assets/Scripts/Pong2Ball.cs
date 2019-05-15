using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pong2Ball : MonoBehaviour
{
    private const int STARTLIVES = 5;
    private const int XYVEL = 100;
    private const int ZVEL = 200;
    private const int STARTDIR = 1;
    private float xvel;
    private float yvel;
    private float zvel;
    private float xdir;
    private float ydir;
    private float zdir;
    private int playerScore;
    private int compScore;
    private bool roundReset;
    private Vector3 ballReset;
    private Text player;
    private Text comp;
    private AudioSource audioSource;
    private int lives;
    private bool playForTime;
    public AudioClip bounce;
    public AudioClip wallBounce;
    public Rigidbody rb;

    //Setting initial values for the fields and retrieving and saving UI gameobjects
    public void Init(bool playForTime)
    {
        this.playForTime = playForTime;
        if (playForTime)
        {
            lives = STARTLIVES;
        }
        xvel = yvel = XYVEL;
        zvel = ZVEL;
        xdir = ydir = zdir = STARTDIR;
        playerScore = compScore = 0;
        ballReset = new Vector3(0, 10, 0);
        roundReset = false;
        player = GameObject.Find("playerText").GetComponent<Text>();
        comp = GameObject.Find("compText").GetComponent<Text>();
        audioSource = GameObject.Find("GameSound").GetComponent<AudioSource>();
    }

    //Adds force to the ball object with the updated direction at each frame
    void FixedUpdate()
    {
        rb.AddForce(xvel * xdir * Time.deltaTime, yvel * ydir * Time.deltaTime, zvel * zdir * Time.deltaTime);
    }

    //Bouncing, scoring, sounds and reset triggered based on what the ball has collided with
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            //To mimic a bounc effect
            //When the ball collides with the walls, the x direction is inverted
            //When it collides with the ceiling or floor, the y direction is inverted
            //If it collides with either paddles, the z direction is inverted
            //The appropriate sounds are played for the different surfaces
            case "sides":
                xdir *= -1;
                audioSource.PlayOneShot(wallBounce, 1f);
                break;
            case "topNbottom":
                ydir *= -1;
                audioSource.PlayOneShot(wallBounce, 1f);
                break;
            case "paddle":
                zdir *= -1;
                audioSource.PlayOneShot(bounce, 1f);
                break;
                //If the ball collides with the invisible 'playerWall' or 'compWall' objects
                //score is added to the appropriate player and the ball is reset
            case "playerWall":
                if (playForTime)
                {
                    lives--;
                }
                else
                {
                    compScore++;
                }
                resetBall();
                break;
            case "compWall":
                playerScore++;
                resetBall();
                break;
        }
    }

    private void resetBall()
    {
        roundReset = true;
        zdir *= -1;
        transform.position = ballReset;
        updateScoreOnScreen();
    }

    private void updateScoreOnScreen()
    {
        player.text = playerScore.ToString();
        comp.text = compScore.ToString();
    }

    public bool RoundReset { get => roundReset; set => roundReset = value; }
    public int PlayerScore { get => playerScore; set => playerScore = value; }
    public int CompScore { get => compScore; set => compScore = value; }
    public int Lives { get => lives; set => lives = value; }
}
