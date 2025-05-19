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

    // ���� 1��
    void InitializeGrid() // �׸��� �ʱ�ȭ
    {
        grid = new GridCell[gridWith, gridHeight]; // 2���� �迭 ����

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

                grid[x, y] = cell; // �迭�� ����
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
        if (cell == null || !cell.isEmpty()) return null;  // ����ִ� ĭ�� �ƴϸ� ���� ����

        level = Mathf.Clamp(level, 1, maxRankLevel);       // ���� ���� Ȯ��

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f); // ����� ��ġ ����

        // �巡�� ������ ����� ������Ʈ �߰�
        GameObject rankObj = Instantiate(rankPreabs, rankPosition, Quaternion.identity, gridCentainer);
        rankObj.name = "Rank_Lvl" + level;

        DraggableRank rank = rankObj.AddComponent<DraggableRank>();
        rank.SetRanekLevel(level);

        cell.SetRank(rank);

        return rank;
    }

    private GridCell FindEmptyCell() // ����ִ� ĭ ã��
    {
        List<GridCell> emptyCells = new List<GridCell>(); // �� ĭ���� ������ ����Ʈ

        // ��� ĭ�� �˻�
        for (int x = 0; x < gridWith; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].isEmpty()) // ĭ�̶�� ����Ʈ�� �߰�
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if (emptyCells.Count == 0) // ��ĭ�� ������ null �� ��ȯ
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)]; // �����ϰ� �� ĭ �ϳ� ����
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

    public void RemoveRank(DraggableRank rank)  // ����� ����
    {
        if (rank == null) return;

        if (rank.currentCell != null)           // ĭ���� ����
        {
            rank.currentCell.currentRank = null;
        }

        Destroy(rank.gameObject);               // ���� ������Ʈ ����
    }




    public void MergeRanks(DraggableRank draggedRank, DraggableRank targetRank)
    {
        if (draggedRank == null || targetRank == null || draggedRank.ranlLevel != targetRank.ranlLevel) // ���� ������ �ƴϸ� ��ġ�� ����
        {
            if (draggedRank != null)
                draggedRank.ReturnToOriginalPosition();
            return;
        }

        int newLevel = targetRank.ranlLevel + 1;  // �� ���� ���
        if (newLevel > maxRankLevel)              // �ִ� ���� �ʰ� �� ó��
        {
            RemoveRank(draggedRank);              // �巡���� ����常 ����
            return;
        }

        targetRank.SetRanekLevel(newLevel);        // Ÿ�� ����� ���� ���׷��̵�
        RemoveRank(draggedRank);                  // �巡���� ����� ����

        if (Random.Range(0, 100) < 60)            // 60% Ȯ���� ����� ��ġ�� ���� �� �������� �� ����� ����
        {
            SpawnNewRank();
        }
    }

}
