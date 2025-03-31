using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;//체력을 선언 한다.(int)
    public float Timer = 1.0f;//타이머 변수를 선언한다.(float)
    public int AttackPoint = 50;
    // 최초 프레임이 업데이트 되기 전 한번 실행
    void Start()
    {
        Health = 100;                              // 이 스크립트가 실행 될 때 100을 더 올려준다.
    }

    // 게임 진행중에 매 플래임 마다 호출된다.
    void Update()
    {
        Timer -= Time.deltaTime; // 시간을 매 플레이마다 감소 시킨다. (deltaTime 프레임 간격의 시간을 의미합니다)
                                 //(Timer = timer - time.deltaTime)
            if(Timer <= 0)  //만약  Timer의 수치가 0이하로 낼갈 경우 (1초 마다 동작되는 행동을 만들때)
        {
            Timer = 1;  //다시 1초로 타이머를 초기화시켜준다.
            Health += 10;  //1초마다 체력을 10 올려준다.
        }

            if(Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;    // 체력 포인트를 곡격 포인트 만큼 막소 시켜준다. (Health
        }

            if(Health <= 0)  //체역이 0 이하 일 경우
        {
            Destroy(gameObject); //이 오브젝트를 파괴 시킨다
        }
    }

    public void CharacterHit( int Damage)
    { 
        Health -= Damage;
    }

    void ChackDeath()
    {
        if ( Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1;
            Health += 10;

        }
    }

}
