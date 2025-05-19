using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, y;
    public DraggableRank currentRank;
    public SpriteRenderer cellRenderers;

    private void Awake()
    {
        cellRenderers = GetComponent<SpriteRenderer>();
    }



    //��ǥ�ʱ�ȭ
    public void Initialilze(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "_" + y;
    }

    
   public bool isEmpty()
    {
        return currentRank == null;

    }
    public bool ContainsPosition(Vector3 position)
    {
        Bounds bounds = cellRenderers.bounds;
        return bounds.Contains(position);
    }

    public void SetRank(DraggableRank rank )
    {
        currentRank = rank;

        if (rank !=null)
        {
            rank.currentCell = this;
        }

        rank.originalPosiion = new Vector3(transform.position.x, transform.position.y, 0);
        rank.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
