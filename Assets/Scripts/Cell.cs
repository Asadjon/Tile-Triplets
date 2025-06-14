using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class Cell : MonoBehaviour
    {
        public Tile Tile { get; set; }
        public bool IsEmpty => !Tile;

        public void SetTile(Tile tile, Action onComplate)
        {
            Tile = tile;
            tile?.SetCell(this, onComplate);
        }
    }
}
