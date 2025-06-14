using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager mInstance;
        public static GameManager Instance => mInstance ? mInstance : (mInstance = FindAnyObjectByType<GameManager>());

        [SerializeField] private TileSpawner m_tileSpawner;
        [SerializeField, Range(21, 81)] private int m_spawnedTilesCount;
        [SerializeField] private UnityEvent<bool> onChangeGameOver;
        [SerializeField] private UnityEvent onWon;
        [SerializeField] private UnityEvent onDefeated;

        public int SpawnedTilesCount { get => m_spawnedTilesCount; set => m_spawnedTilesCount = value; }
        public bool IsGameOver { get; private set; }

        private void Awake()
        {
            if (mInstance) Destroy(gameObject);

            mInstance = this;
            DontDestroyOnLoad(this);
        }

        private void Start() =>
            RestartGame();

        public void RestartGame()
        {
            Time.timeScale = 1;
            m_tileSpawner.ClearAll();

            onChangeGameOver.Invoke(false);
            IsGameOver = true;
            var tileTypes = GenerateTilePool(m_spawnedTilesCount);
            m_tileSpawner.SpawnTiles(tileTypes, () => IsGameOver = false);
        }

        private List<TileType> GenerateTileTypes()
        {
            List<TileType> tileTypes = new();
            foreach (var shape in Enum.GetValues(typeof(Shape)))
                foreach (var color in Enum.GetValues(typeof(FrameColor)))
                    foreach (var animal in Enum.GetValues(typeof(Animal)))
                        tileTypes.Add(new((Shape)shape, (Animal)animal, (FrameColor)color));

            return tileTypes;
        }

        private List<TileType> GenerateTilePool(int count)
        {
            List<TileType> allTileTypes = new();
            var tileTypes = GenerateTileTypes();
            var random = new System.Random();
            count -= count % 3;

            while (count > 0)
            {
                if (tileTypes.Count == 0) break;
                var index = random.Next(tileTypes.Count - 1);
                var tileType = tileTypes[index];
                tileTypes.RemoveAt(index);
                count -= 3;

                for (var i = 0; i < 3; i++) allTileTypes.Add(tileType);
            }

            return allTileTypes;
        }

        public void Reshuffle()
        {
            IsGameOver = true;
            var tileTypes = GenerateTilePool(m_tileSpawner.TilesCount);
            m_tileSpawner.ClearAll();
            m_tileSpawner.SpawnTiles(tileTypes, () => IsGameOver = false);
        }

        public void Win()
        {
            print("You won!");
            GameOver();
            onWon.Invoke();
        }

        public void Defeat()
        {
            print("You defeated");
            GameOver();
            onDefeated.Invoke();
        }

        private void GameOver()
        {
            IsGameOver = true;
            Time.timeScale = 0;
            onChangeGameOver.Invoke(IsGameOver);
        }
    }
}
