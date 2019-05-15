using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Pong2Ball ball;
    private bool isMoving;
    // Start is called before the first frame update
    public void Init(bool isMoving, Pong2Ball ball, bool player)
    {
        this.isMoving = isMoving;
        this.ball = ball;

        if (!isMoving)
        {
            if (player)
            {
                transform.position = new Vector3(0, 0, -49);
            }
            else
            {
                transform.position = new Vector3(0, 0, 49);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            var position = transform.position;
            position.z = ball.transform.position.z;
            transform.position = position;
        }
    }
}
