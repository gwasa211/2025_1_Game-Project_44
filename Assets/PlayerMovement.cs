using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5.0f; // �̵� �ӵ� ����
    public float jumpForce = 7.0f;
    public float turnSpeed = 10f;

    [Header("���� ���� ����")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;


    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGround = true;

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderMoveSpeed = 7.0f;
    public float glidermFallsSpeed = 1.0f;
    public float gliferMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;


    public Rigidbody rb; // Rigidbody ����
    public bool isGrounded = true;
    public int coinCount = 0;
    public int totalCoins = 5;
    



    // Start is called before the first frame update
    void Start()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
        // Rigidbody ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody>();
        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

       
        if(movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.G) && ! isGrounded && gliderTimeLeft > 0)
        {
            if(!isGrounded)
            {
                EnableGlider();
                //�۶��̴� Ȱ��ȭ �Լ�
            }

            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <= 0)
            {
                DisableGlider();   //�۶��̴� ��Ȱ��ȭ �Լ�
            }
           
        }
        else if (isGliding)
        {
            DisableGlider();
        }

        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);

        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        if (isGrounded)
        {
            if (isGrounded)
            {
                DisableGlider();
            }
            gliderTimeLeft = gliferMaxTime;
        }
            
        // �ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGround = false;
            coyoteTimeCounter = 0;
        }
        updateGroundedState();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = false;
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
    void updateGroundedState()
    {
        if (realGround)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
    void EnableGlider()
    {
        isGliding = true;

        if(gameObject !=null)
        {
            gameObject.SetActive(true);
        }
        rb.velocity = new Vector3(rb.velocity.x, glidermFallsSpeed, rb.velocity.z);
    }
    void DisableGlider()
    {
        isGliding = false;
        if (gliderObject !=null)
        {
            gliderObject.SetActive(false);
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    void ApplyGliderMovement(float horiaontal , float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horiaontal * gliderMoveSpeed, glidermFallsSpeed, vertical * gliderMoveSpeed);

        rb.velocity = gliderVelocity;
    }
   
}
