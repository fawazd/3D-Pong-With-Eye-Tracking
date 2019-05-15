using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.SceneManagement;

public class Pong2Paddle : MonoBehaviour
{
    private const float PLAYERZ = -29;
    private const float COMPZ = 29;
    private const float VERTICALMID = 10;
    private const float IMPULSETIME = 20;
    private const float MAXSPEED = 9;
    private const float MINSPEED = 4;
    private Rigidbody rb;
    private bool isPlayer;
    private float width;
    private Pong2Ball ball;
    private bool collided;
    private float impulseTime;
    private float alpha;
    private Vector2 _historicPoint;
    private bool _hasHistoricPoint;
    private Color col;
    private bool playForTime;

    public void Init(bool isPlayer, Pong2Ball ball, bool playForTime)
    {
        this.ball = ball;
        this.isPlayer = isPlayer;
        this.playForTime = playForTime;
        alpha = 0.3f;
        impulseTime = 0;
        collided = false;

        //Retrieving the material component and current material color and setting the opacity to 10%
        //Used later on to display impulse effect on ball bounce
        col = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = new Color(col.r, col.g, col.b, 0.1f);

        //Setting initial positions for the player and the computer. 
        if (isPlayer)
        {
            transform.position = new Vector3(0, VERTICALMID, PLAYERZ);
        }
        else
        {
            transform.position = new Vector3(0, VERTICALMID, COMPZ);
        }

    }
    
    void Update()
    {
        //At each frame, gets the new position for the paddle, checks for collision with the walls, floor and ceiling
        //Checks and executes impulse effect on ball collision
        Vector3 newPos = getNewPos();
        PaddleBoundsLogic(newPos);
        impulseEffect();
    }
    private void trackTime()
    {

    }
    private void impulseEffect()
    {
        if (collided)
        {
            //If the ball has collided with the paddle, increases opacity of the paddle to 50%
            gameObject.GetComponent<Renderer>().material.color = new Color(col.r, col.g, col.b, 0.5f);
            //It then does a count down of 20 on each method call
            impulseTime -= 1;
            if (impulseTime < 0)
            {
                //Once the impulseTime reaches 0
                //Sets the collided to false, resets the count down and reverts the opacity back to 10%
                collided = false;
                impulseTime = IMPULSETIME;
                gameObject.GetComponent<Renderer>().material.color = new Color(col.r, col.g, col.b, 0.1f);
            }
        }
    }

    private Vector3 getNewPos()
    {

        Vector3 newPos = transform.position;

        //Gets current position and updates the x, y and z values, with different logic for player and the computer
        if (isPlayer)
        {
            //Remedies eye tracker not loading on first run
            if (double.IsNaN(TobiiAPI.GetGazePoint().Screen.x))
            {
                SceneManager.LoadScene("Pong2Scene");
            }

            //Gets a the current x and y coordinates from the Tobii API
            //The values are then sent to the smoothing function
            //The returned value is then saved  to a Vector and updated to the appropriate screen units.
            Vector2 input = Smoothify(TobiiAPI.GetGazePoint().Screen);
            newPos.x = (input.x / Screen.width) * 50 - 25;
            newPos.y = (input.y / Screen.height) * 20;
            newPos.z = -29;
        }
        else
        {
            if (playForTime)
            {
                Vector3 ballPos = ball.transform.position;
                newPos.x = ballPos.x;
                newPos.y = ballPos.y;
            }
            else
            {
                Vector3 ballPos = ball.transform.position;

                int xdir = 0;
                int ydir = 0;

                //Sets the x and y direction based on where the paddle is in proportion to the ball
                if (ballPos.x >= newPos.x) { xdir = 1; }
                else if (ballPos.x < newPos.x) { xdir = -1; }
                if (ballPos.y >= newPos.y) { ydir = 1; }
                else if (ballPos.y < newPos.y) { ydir = -1; }

                //Adds a random positive or negative value between the defined minimum and maximum speeds
                //to the x and y coordinates of the paddle's new position
                newPos.x += (Random.Range(MINSPEED, MAXSPEED) * xdir * Time.deltaTime);
                newPos.y += (Random.Range(MINSPEED, MAXSPEED) * ydir * Time.deltaTime);
            }
        }

        return newPos;
    }

    private void PaddleBoundsLogic(Vector3 newPos)
    {
        float halfPaddleWidth = transform.localScale.x / 2;

        GameObject left = GameObject.Find("LeftWall");
        GameObject right = GameObject.Find("RightWall");
        GameObject top = GameObject.Find("Ceiling");
        GameObject bottom = GameObject.Find("Floor");

        if (newPos.x - halfPaddleWidth > left.transform.position.x && newPos.x + halfPaddleWidth < right.transform.position.x)
        {
            if (newPos.y + halfPaddleWidth > top.transform.position.y)
            {
                newPos.y = top.transform.position.y - halfPaddleWidth - top.transform.localScale.y / 2;
            }
            if (newPos.y - halfPaddleWidth < bottom.transform.position.y)
            {
                newPos.y = bottom.transform.position.y + halfPaddleWidth + bottom.transform.localScale.y / 2;
            }
            transform.position = newPos;
        }

        if (newPos.y - halfPaddleWidth > bottom.transform.position.y && newPos.y + halfPaddleWidth < top.transform.position.y)
        {
            if (newPos.x + halfPaddleWidth > right.transform.position.x)
            {
                newPos.x = right.transform.position.x - halfPaddleWidth - right.transform.localScale.y / 2;
            }
            if (newPos.x - halfPaddleWidth < left.transform.position.x)
            {
                newPos.x = left.transform.position.x + halfPaddleWidth + left.transform.localScale.y / 2;
            }
            transform.position = newPos;
        }
    }

    private Vector2 Smoothify(Vector2 point)
    {
        if (!_hasHistoricPoint)
        {
            _historicPoint = point;
            _hasHistoricPoint = true;
        }

        var smoothedPoint = new Vector2(point.x * alpha + _historicPoint.x * (1.0f - alpha),
            point.y * alpha + _historicPoint.y * (1.0f - alpha));

        _historicPoint = smoothedPoint;

        return smoothedPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            impulseTime = IMPULSETIME;
            collided = true;
        }
    }

    public void ResetPaddle()
    {
        if (!isPlayer)
        {
            transform.position = new Vector3(0, 10, 29);
        }
    }
}
