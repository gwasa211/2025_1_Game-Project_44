using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    //과일 타입 (0: 사과, 1: 블루베리, 2: 코코넛 .....) int로 만든다.
    public int fruitType;

    //과일이 이미 합쳐졌는지 확인하는 플레그
    public bool hasMrged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //이미 랍쳐진 과일 무시
        if (hasMrged)
            return;

        //다른 과일과 충동했는지 확인
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        //충돌한 것이 과일이고 같다면
        if(otherFruit !=null && !otherFruit.hasMrged && otherFruit.fruitType == fruitType)
        {
            //합쳐졌다고 표시
            hasMrged = true;
            otherFruit.hasMrged = true;
            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;

            //다음 던계 과일로 업그레이드(추후 구현)
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if(gameManager != null)
            {
                gameManager.MergeFuites(fruitType, mergePosition);
            }

            //기존 과일 재거
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }

    }

}
