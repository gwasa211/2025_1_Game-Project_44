using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;//ü���� ���� �Ѵ�.(int)
    public float Timer = 1.0f;//Ÿ�̸� ������ �����Ѵ�.(float)
    public int AttackPoint = 50;
    // ���� �������� ������Ʈ �Ǳ� �� �ѹ� ����
    void Start()
    {
        Health = 100;                              // �� ��ũ��Ʈ�� ���� �� �� 100�� �� �÷��ش�.
    }

    // ���� �����߿� �� �÷��� ���� ȣ��ȴ�.
    void Update()
    {
        Timer -= Time.deltaTime; // �ð��� �� �÷��̸��� ���� ��Ų��. (deltaTime ������ ������ �ð��� �ǹ��մϴ�)
                                 //(Timer = timer - time.deltaTime)
            if(Timer <= 0)  //����  Timer�� ��ġ�� 0���Ϸ� ���� ��� (1�� ���� ���۵Ǵ� �ൿ�� ���鶧)
        {
            Timer = 1;  //�ٽ� 1�ʷ� Ÿ�̸Ӹ� �ʱ�ȭ�����ش�.
            Health += 10;  //1�ʸ��� ü���� 10 �÷��ش�.
        }

            if(Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;    // ü�� ����Ʈ�� ��� ����Ʈ ��ŭ ���� �����ش�. (Health
        }

            if(Health <= 0)  //ü���� 0 ���� �� ���
        {
            Destroy(gameObject); //�� ������Ʈ�� �ı� ��Ų��
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
