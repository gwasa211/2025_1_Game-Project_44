using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    public static bool canPlay = true;
    public static bool anyBallMoving = false;

    

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        CheckAllBall();
        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�. ");

        }
    }

    void CheckAllBall()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;

        foreach (SimpleBallController ball in allBalls)
        {
            if(ball.IsMoving())
            {
                anyBallMoving = true;
                break;
            }
        }
    }


    public static void OnBallHit()
    {
        canPlay = false;
        anyBallMoving = true;
        Debug.Log("�� ����! ���� ���� �� ���� ��ٸ�����");
    }
}
