using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong2CircleIndicator : MonoBehaviour
{
    private Pong2Ball ball;
    private Vector3 ballPos;
    public Renderer rend;

    public void Init(Pong2Ball ball)
    {
        this.ball = ball;
        ballPos = ball.transform.position;
        transform.position = new Vector3(ballPos.x, ballPos.y, -49);
        
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ballPos = ball.transform.position;
        transform.position = new Vector3(ballPos.x, ballPos.y, -49);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "paddle")
        {
            rend.enabled = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        rend.enabled = false;
    }
}
