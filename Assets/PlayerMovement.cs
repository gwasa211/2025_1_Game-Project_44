using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // �̵� �ӵ� ����
    public float jumpForce = 5.0f;
    public Rigidbody rb; // Rigidbody ����
    public bool isGrounded = true;
    public int coinCount = 0;
    public int totalCoins = 5;


    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // ������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // �ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }
        if (other.CompareTag("Door") && coinCount >= totalCoins)
        {
            Debug.Log("���� Ŭ����");
        }
    }
}
