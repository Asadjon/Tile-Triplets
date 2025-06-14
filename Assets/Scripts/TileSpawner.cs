using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField] private Tile m_tilePrefab;
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField] private ActionBar m_actionBar;

        private List<Tile> GetAllTiles() =>
            GetComponentsInChildren<Tile>().ToList();

        public int TilesCount => GetAllTiles().Count;

        public bool IsEmpty => GetAllTiles().Count == 0;

        public void SpawnTiles(List<TileType> tileTypes, Action onComplate)
        {
            tileTypes = Random(tileTypes);
            StartCoroutine(SpawnTilesCoroutine(tileTypes, onComplate));
        }

        private IEnumerator SpawnTilesCoroutine(List<TileType> tileTypes, Action onComplate)
        {
            var spawnPoint2D = (Vector2)m_spawnPoint.position;
            var tilesCount = tileTypes.Count;

            for (var i = 0; i < tilesCount; i++)
            {
                var tile = Instantiate(m_tilePrefab, spawnPoint2D, Quaternion.identity, transform);
                tile.Type = tileTypes[i];
                tile.onClickTile.AddListener(OnTileClick);
                tile.OnDestroying = TileOnDestroying;

                yield return new WaitForSecondsRealtime(.05f);
            }

            onComplate?.Invoke();
        }

        private void OnTileClick(Tile tile)
        {
            if (GameManager.Instance.IsGameOver || tile.Cell) return;

            m_actionBar.AddTile(tile);
        }

        private void TileOnDestroying(Tile tile)
        {
            if (IsEmpty && !GameManager.Instance.IsGameOver)
                GameManager.Instance.Win();
        }

        private List<TileType> Random(List<TileType> tileTypes) =>
            tileTypes.OrderBy(tile => UnityEngine.Random.value).ToList();

        public void ClearAll()
        {
            // delete all tiles gameobject
            GetAllTiles().ForEach(tile => tile.Destroy());
        }
    }
}
