using UnityEngine;

namespace Game.Gameplay
{
    //here we have logic for Grid and Cells for enemies to occupy and move with
    public class GridArea : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] float rowWidth = 10;
        //how many enemies there will be in a row
        [SerializeField, Range(1, 8)] int cellsCount = 4;
        [SerializeField] BoxCollider2D boxCol2D = null;

        [Zenject.Inject] GameplayController gameplay = null;

        Bounds gridBounds;
        public Bounds GridBounds => gridBounds;

        float GridSizeX => boxCol2D.size.x * transform.localScale.x;
        float CellSize => GridSizeX / cellsCount;

        //offset for positioning next Cell
        Vector3 VectorOffset => transform.right * CellSize;

        Cell[] cells;
        public int CellsCount => cellsCount;

        public void Initialize()
        {
            gameplay.OnLevelRestarting += ClearCells;
            gridBounds = new Bounds(transform.position, Vector3.zero);
            BakeCells();
        }

        void BakeCells()
        {
            cells = new Cell[CellsCount];
            Vector3 cellPosition = transform.position - transform.right * GridSizeX / 2 + VectorOffset / 2;

            //TODO: optimization and memory management
            for (int i = 0; i < cellsCount; i++)
            {
                //for enemies to have something to follow, also better bugchecking in editor
                GameObject cellTransform = new GameObject("Cell");
                cellTransform.transform.SetParent(this.transform, false);
                cellTransform.transform.position = cellPosition;

                cells[i] = new Cell(cellTransform.transform);
                cellPosition += VectorOffset;

                gridBounds.Encapsulate(cellTransform.transform.position);
            }
        }

        public void ClearCells()
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
            cells = null;
        }

        public bool TryGetFreeCell(out Cell freeCell)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].IsOccupied == false)
                {
                    freeCell = cells[i];
                    return true;
                }
            }
            freeCell = null;
            return false;
        }


        public void OnDestroy()
        {
            if (gameplay)
                gameplay.OnLevelRestarting -= ClearCells;
        }

        public class Cell
        {
            Transform worldTransform;
            Unit occupant;

            public Transform WorldTransform => worldTransform;
            public Unit Occupant => occupant;

            public bool IsOccupied
            {
                get
                {
                    if (occupant == null)
                        return false;
                    else
                        return true;
                }
            }

            public Cell(Transform worldTransform)
            {
                this.worldTransform = worldTransform;
                occupant = null;
            }

            public void OccupyCell(Unit occupant)
            {
                this.occupant = occupant;
                this.occupant.OnDeath += FreeCell;
            }

            void FreeCell(Unit deadUnit)
            {
                occupant = null;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 centerPoint = transform.position - transform.right * GridSizeX / 2 + VectorOffset / 2;
            for (int i = 0; i < cellsCount; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(centerPoint, new Vector2(CellSize, 1));
                centerPoint += VectorOffset;
            }
        }

        private void OnValidate()
        {
            transform.localScale = Vector3.one;
            if (boxCol2D)
                boxCol2D.size = new Vector2(rowWidth, 1);
        }
#endif
    }
}