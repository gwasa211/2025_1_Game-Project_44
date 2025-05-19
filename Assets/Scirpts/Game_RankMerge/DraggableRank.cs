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

    void StopDragging()  // 드래그 종료
    {
        isDragging = false;  // 드래그 상태 해제
        spriteRenderer.sortingOrder = 1;
        GridCell targetCell = gamemanager.FindClosestCell(transform.position);  // 가장 가까운 칸 찾기

        if (targetCell != null)
        {
            if (targetCell.currentRank == null)  // 빈 칸인 경우 - 이동
            {
                MoveAToCell(targetCell);
            }
            else if (targetCell.currentRank != this && targetCell.currentRank.ranlLevel == ranlLevel)  // 같은 랭크일 경우
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
            ReturnToOriginalPosition();  // 유효한 칸이 없으면 위치로 복귀
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

    public void ReturnToOriginalPosition() // 원래 위치로 돌아가는 함수
    {
        transform.position = originalPosiion;
    }
    // 참조 0개
    public void MergeWithCell(GridCell targetCell)
    {
        if (targetCell.currentRank == null || targetCell.currentRank.ranlLevel != ranlLevel) // 같은 레벨인지 확인
        {
            ReturnToOriginalPosition(); // 원래 위치로 돌아가기
            return;
        }

        if (currentCell != null)
        {
            currentCell.currentRank = null; // 기존 칸에서 제거
        }

        gamemanager.MergeRanks(this, targetCell.currentRank);

        // 합치기 실행 MergeRanks 함수를 통해서 실행
    }
    // 참조 0개
    public Vector3 GetMouseWorldPosition() // 마우스 월드 좌표 구하기
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
