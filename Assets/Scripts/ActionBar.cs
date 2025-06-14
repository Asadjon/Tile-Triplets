using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private List<Cell> m_cells;

        private List<Tile> GetTiles() =>
            m_cells.Where(cell => !cell.IsEmpty).Select(cell => cell.Tile).ToList();

        public void AddTile(Tile tile)
        {
            var emptyCell = m_cells.First(cell => cell.IsEmpty); 
            if (emptyCell)
                emptyCell.SetTile(tile, () => ComplatePut(tile));
        }

        private void ComplatePut(Tile tile)
        {
            ClearSimilarTiles(tile);

            if (CheckFill() && !GameManager.Instance.IsGameOver)
                GameManager.Instance.Defeat();
        }

        private void ClearSimilarTiles(Tile tile)
        {
            var similarTiles = GetTiles()
                .Where(t => t.Type.Equals(tile.Type));

            if (similarTiles.Count() >= 3 && similarTiles.All(tile => tile.IsComplatedMoveToCell))
            {
                similarTiles.ToList().ForEach(tile => tile.Destroy());

                Ordering();
            }
        }

        private void Ordering()
        {
            GetTiles().ForEach(tile =>
            {
                var cellIndex = GetTiles().IndexOf(tile);
                if (m_cells[cellIndex].Tile == tile) return;

                m_cells[cellIndex].SetTile(tile, () => { });
            });
        }

        private bool CheckFill() =>
            m_cells.All(cell => !cell.IsEmpty) && GetTiles().All(tile => tile.IsComplatedMoveToCell);
    }
}
