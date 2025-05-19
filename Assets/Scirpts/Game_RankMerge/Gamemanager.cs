using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public int gridWith = 7;
    public int gridHeight = 7;
    public float cellSize = 1.4f;
    public GameObject cellPrefabs;
    public Transform gridCentainer;

    public GameObject rankPreabs;
    public Sprite[] rankSprites;
    public int maxRankLevel = 7;

    public GridCell[,] grid;

    // 참조 1개
    void InitializeGrid() // 그리드 초기화
    {
        grid = new GridCell[gridWith, gridHeight]; // 2차원 배열 생성

        for (int x = 0; x < gridWith; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(
                    x * cellSize - (gridWith * cellSize / 2) + cellSize / 2,
                    y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2,
                    1f
                );

                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridCentainer);
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.Initialilze(x, y);

                grid[x, y] = cell; // 배열에 저장
            }
        }
    }







    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();

        for (int i = 0; i < 4; i++)
        {
            SpawnNewRank();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public DraggableRank CreateRankInCell(GridCell cell, int level)
    {
        if (cell == null || !cell.isEmpty()) return null;  // 비어있는 칸이 아니면 생성 실패

        level = Mathf.Clamp(level, 1, maxRankLevel);       // 레벨 범위 확인

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f); // 계급장 위치 설정

        // 드래그 가능한 계급장 컴포넌트 추가
        GameObject rankObj = Instantiate(rankPreabs, rankPosition, Quaternion.identity, gridCentainer);
        rankObj.name = "Rank_Lvl" + level;

        DraggableRank rank = rankObj.AddComponent<DraggableRank>();
        rank.SetRanekLevel(level);

        cell.SetRank(rank);

        return rank;
    }

    private GridCell FindEmptyCell() // 비어있는 칸 찾기
    {
        List<GridCell> emptyCells = new List<GridCell>(); // 빈 칸들을 저장할 리스트

        // 모든 칸을 검사
        for (int x = 0; x < gridWith; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].isEmpty()) // 칸이라면 리스트에 추가
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if (emptyCells.Count == 0) // 빈칸이 없으면 null 값 반환
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)]; // 랜덤하게 빈 칸 하나 선택
    }

    public bool SpawnNewRank()
    {
        GridCell emptyCell = FindEmptyCell();
        if (emptyCell == null) return false;

        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankInCell(emptyCell, rankLevel);
        return true;
    }

    GridCell closestCell = null;
    float closestDistance = float.MaxValue;
    public GridCell FindClosestCell(Vector3 position)
    {
        for (int x = 0; x < gridWith; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }

        if (closestDistance > cellSize * 2)
        {
            return null;
        }
        return closestCell;
    }

    public void RemoveRank(DraggableRank rank)  // 계급장 제거
    {
        if (rank == null) return;

        if (rank.currentCell != null)           // 칸에서 제거
        {
            rank.currentCell.currentRank = null;
        }

        Destroy(rank.gameObject);               // 게임 오브젝트 제거
    }




    public void MergeRanks(DraggableRank draggedRank, DraggableRank targetRank)
    {
        if (draggedRank == null || targetRank == null || draggedRank.ranlLevel != targetRank.ranlLevel) // 같은 레벨이 아니면 합치기 실패
        {
            if (draggedRank != null)
                draggedRank.ReturnToOriginalPosition();
            return;
        }

        int newLevel = targetRank.ranlLevel + 1;  // 새 레벨 계산
        if (newLevel > maxRankLevel)              // 최대 레벨 초과 시 처리
        {
            RemoveRank(draggedRank);              // 드래그한 계급장만 제거
            return;
        }

        targetRank.SetRanekLevel(newLevel);        // 타겟 계급장 레벨 업그레이드
        RemoveRank(draggedRank);                  // 드래그한 계급장 제거

        if (Random.Range(0, 100) < 60)            // 60% 확률로 계급장 합치기 성공 시 랜덤으로 새 계급장 생성
        {
            SpawnNewRank();
        }
    }

}
