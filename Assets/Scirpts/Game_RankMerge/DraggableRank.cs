using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Build.Content;
using UnityEngine;

public class DraggableRank : MonoBehaviour
{
    public int ranlLevel = 1;
    public float dragSeed = 10f;
    public float snapBackSed = 20f;

    public bool isDragging = false;
    public Vector3 originalPosiion;
    public GridCell currentCell;

    public Camera mainCamera;
    public Vector3 dragOffset;
    public SpriteRenderer spriteRenderer;
    public Gamemanager gamemanager;

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gamemanager = FindObjectOfType<Gamemanager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosiion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, dragSeed * Time.deltaTime);
        }
        else if(transform.position != originalPosiion && currentCell != null)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosiion, snapBackSed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }

    void StartDragging()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPosition();
        spriteRenderer.sortingOrder = 10;

    }

    void StopDragging()  // �巡�� ����
    {
        isDragging = false;  // �巡�� ���� ����
        spriteRenderer.sortingOrder = 1;
        GridCell targetCell = gamemanager.FindClosestCell(transform.position);  // ���� ����� ĭ ã��

        if (targetCell != null)
        {
            if (targetCell.currentRank == null)  // �� ĭ�� ��� - �̵�
            {
                MoveAToCell(targetCell);
            }
            else if (targetCell.currentRank != this && targetCell.currentRank.ranlLevel == ranlLevel)  // ���� ��ũ�� ���
            {
                MergeWithCell(targetCell);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();  // ��ȿ�� ĭ�� ������ ��ġ�� ����
        }
    }


    public void MoveAToCell(GridCell targetCell)
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null;
        }

        currentCell = targetCell;
        targetCell.currentRank = this;

        originalPosiion = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
        transform.position = originalPosiion;
    }

    public void ReturnToOriginalPosition() // ���� ��ġ�� ���ư��� �Լ�
    {
        transform.position = originalPosiion;
    }
    // ���� 0��
    public void MergeWithCell(GridCell targetCell)
    {
        if (targetCell.currentRank == null || targetCell.currentRank.ranlLevel != ranlLevel) // ���� �������� Ȯ��
        {
            ReturnToOriginalPosition(); // ���� ��ġ�� ���ư���
            return;
        }

        if (currentCell != null)
        {
            currentCell.currentRank = null; // ���� ĭ���� ����
        }

        gamemanager.MergeRanks(this, targetCell.currentRank);

        // ��ġ�� ���� MergeRanks �Լ��� ���ؼ� ����
    }
    // ���� 0��
    public Vector3 GetMouseWorldPosition() // ���콺 ���� ��ǥ ���ϱ�
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
    public void SetRanekLevel (int level)
    {
        ranlLevel = level;

        if(gamemanager != null && gamemanager . rankSprites.Length > level -1)
        {
            spriteRenderer.sprite = gamemanager.rankSprites[level - 1];
        }
    }

}
