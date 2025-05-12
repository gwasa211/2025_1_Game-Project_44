using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    //���� Ÿ�� (0: ���, 1: ��纣��, 2: ���ڳ� .....) int�� �����.
    public int fruitType;

    //������ �̹� ���������� Ȯ���ϴ� �÷���
    public bool hasMrged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�̹� ������ ���� ����
        if (hasMrged)
            return;

        //�ٸ� ���ϰ� �浿�ߴ��� Ȯ��
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        //�浹�� ���� �����̰� ���ٸ�
        if(otherFruit !=null && !otherFruit.hasMrged && otherFruit.fruitType == fruitType)
        {
            //�������ٰ� ǥ��
            hasMrged = true;
            otherFruit.hasMrged = true;
            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;

            //���� ���� ���Ϸ� ���׷��̵�(���� ����)
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if(gameManager != null)
            {
                gameManager.MergeFuites(fruitType, mergePosition);
            }

            //���� ���� ���
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }

    }

}
