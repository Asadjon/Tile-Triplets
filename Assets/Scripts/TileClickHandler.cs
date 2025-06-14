
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class TileClickHandler : MonoBehaviour
    {
        public UnityEvent onClick;

#if UNITY_EDITOR
        private void OnMouseDown()
        {
            onClick.Invoke();
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        private void Update()
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                var touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                var hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if (hit.collider && hit.collider.gameObject == gameObject)
                    onClick.Invoke();
            }
        }
#endif
    }
}
