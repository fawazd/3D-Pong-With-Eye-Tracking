using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pong2GameManager : MonoBehaviour
{
    public Pong2Ball ball;
    public Pong2Paddle paddle;
    public Indicator indicator;
    public Room room;
    public Pong2Paddle playerPaddle;
    public Pong2Paddle computerPaddle;
    public Indicator movingIndicator;
    public Pong2CircleIndicator circleIndicator;
    public Text winningText;
    public Text restartText;
    public Text timerText;
    public Button restart;
    private float time;
    private float startTime;

    void Start()
    {
        ball = Instantiate(ball) as Pong2Ball;
        room = Instantiate(room) as Room;

        playerPaddle = Instantiate(paddle) as Pong2Paddle;
        computerPaddle = Instantiate(paddle) as Pong2Paddle;
        movingIndicator = Instantiate(indicator) as Indicator;

        playerPaddle.Init(true, ball, false);
        computerPaddle.Init(false, ball, false);
        movingIndicator.Init(true, ball, false);
        ball.Init(true);

        restartText.enabled = false;
        restart.image.enabled = false;
        startTime = Time.time;
    }
    void Update()
    {
        //Resets the computer paddle when the ball is reset
        if (ball.RoundReset)
        {
            computerPaddle.ResetPaddle();
            ball.RoundReset = false;
        }

        //if (ball.Lives == 0)
        //{
        //    ball.rb.velocity = Vector3.zero;
        //    ball.rb.angularVelocity = Vector3.zero;
        //    ball.enabled = false;
        //    playerPaddle.enabled = false;
        //    computerPaddle.enabled = false;

        //    if (ball.PlayerScore == 5)
        //    {
        //        winningText.text = "You win!";
        //    }
        //    else if (ball.CompScore == 5)
        //    {
        //        winningText.text = "Computer wins";
        //    }

        //    restart.image.enabled = true;
        //    restartText.enabled = true;
        //}

        time = 0;
        if (ball.Lives != 0)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text =  minutes + ":" + seconds;
        }
        else
        {
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;
            ball.enabled = false;
            playerPaddle.enabled = false;
            computerPaddle.enabled = false;
        }
    }
}
