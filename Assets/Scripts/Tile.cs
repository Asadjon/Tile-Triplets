using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TypeResources m_typeResources;
        [SerializeField] private SpriteRenderer m_animal;

        public UnityEvent<Tile> onClickTile;

        private SpriteRenderer mShape;
        private Rigidbody2D mRb;

        private TileType mType;
        public TileType Type
        {
            get => mType;
            set
            {
                mType = value;
                UpdateUI();
            }
        }

        private Cell mCell;
        public Cell Cell => mCell;

        public bool IsComplatedMoveToCell { get; private set; } = true;

        public Action<Tile> OnDestroying { private get; set; }

        public void SetCell(Cell cell, Action onComplate)
        {
            DOTween.Kill(transform);

            if (mCell == null)
            {
                transform.DORotateQuaternion(Quaternion.identity, .5f);
                SetCell_Internal(cell, .5f, onComplate);
            }

            else SetCell_Internal(cell, .25f, onComplate);
        }

        private void SetCell_Internal(Cell cell, float duration, Action onComplate)
        {
            if (cell == mCell) return;

            if (mCell)
            {
                mCell.Tile = null;
                mCell.gameObject.SetActive(true);
            }

            mCell = cell;

            if (!mCell) return;
            mCell.Tile = this;

            transform.DOMove(mCell.transform.position, duration)
                .OnStart(() =>
                {
                    IsComplatedMoveToCell = false;
                    DisablePhysics();
                })
                .OnPlay(() => ChangeSortingLayer("ActionBar"))
                .OnComplete(() =>
                {
                    IsComplatedMoveToCell = true;
                    mCell.gameObject.SetActive(false);
                    transform.SetPositionAndRotation(mCell.transform.position, Quaternion.identity);
                    onComplate();
                });
        }

        private void Awake()
        {
            mRb = GetComponent<Rigidbody2D>();

            Vector2 randomForce = new(UnityEngine.Random.Range(-.1f, .1f), -.5f);
            mRb.AddForce(randomForce * 50f);
        }

        public void Destroy()
        {
            if (mCell)
            {
                mCell.Tile = null;
                mCell.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }

        private void OnDestroy() => OnDestroying?.Invoke(this);

        private void UpdateUI()
        {
            (var shapePref, var animal, var color) = m_typeResources.GetResourcesOfType(mType);

            if (mShape) Destroy(mShape.gameObject);
            mShape = Instantiate(shapePref, transform);
            mShape.gameObject.GetComponent<TileClickHandler>()
                .onClick.AddListener(() => onClickTile.Invoke(this));

            mShape.color = color;
            m_animal.sprite = animal;
        }

        private void DisablePhysics()
        {
            mRb.linearVelocity = Vector2.zero;
            mRb.angularVelocity = 0f;
            mRb.bodyType = RigidbodyType2D.Kinematic;
            mRb.simulated = false;
        }

        private void ChangeSortingLayer(string layerName)
        {
            if (mShape) mShape.sortingLayerName = layerName;
            m_animal.sortingLayerName = layerName;
        }
    }
}
