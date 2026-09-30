using Buildings;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    // [NEW] Main entry point: player input, building placement, game state.
    //       No separate GameManager — everything lives here.
    public class PlayerController : MonoBehaviour
    {
        // ---- Existing fields kept from skeleton ----
        private Image _cursor;
        private BuildingMenu _buildingMenu;
        private Collider2D _collider;

        // [NEW] References set at Start.
        private PlayerData _playerData;
        private Camera _mainCamera;

        // [NEW] Drag state.
        private BuildingSlot _draggedSlot;
        private GameObject _ghostBuilding;
        private bool _isDragging;

        // [NEW] Inspector-configurable ghost prefab (same as the building prefab).
        [SerializeField] private GameObject _ghostPrefab;

        // [NEW] Game state (no GameManager — held here).
        [SerializeField] private Transform _goalTransform;
        [SerializeField] private int _startingLives = 20;
        [SerializeField] private int _scorePerKill = 10;

        private int _lives;
        private int _score;
        private bool _isGameOver;

        // ---- Lifecycle ----

        private void Start()
        {
            _playerData = FindObjectOfType<PlayerData>();
            _mainCamera = Camera.main;
            _buildingMenu = GetComponent<BuildingMenu>();

            _lives = _startingLives;
            _score = 0;

            // [NEW] Subscribe to enemy-kill event for scoring.
            Enemy.OnKilled += OnEnemyKilled;

            // [NEW] Wire up UI slots.
            if (_buildingMenu != null)
                _buildingMenu.SetSlots(FindObjectsOfType<BuildingSlot>());
        }

        private void Update()
        {
            HandleDragUpdate();
            CheckGameOver();
        }

        private void OnDestroy()
        {
            Enemy.OnKilled -= OnEnemyKilled;
        }

        // ---- Drag logic ----

        private void HandleDragUpdate()
        {
            if (!_isDragging) return;

            // [NEW] Move ghost building to mouse world position.
            Vector3 mouseWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            if (_ghostBuilding != null)
                _ghostBuilding.transform.position = mouseWorld;

            // [NEW] On left-click release, try to place.
            if (Input.GetMouseButtonUp(0))
                TryPlaceBuilding(mouseWorld);
        }

        // ---- Existing skeleton methods (implemented) ----

        void OnClick()
        {
            if (_isDragging)
                CancelDrag();
        }

        void OnDrag()
        {
            // [NEW] Dragging handled in HandleDragUpdate via Update.
        }

        void OnClickInstantiateBuilding()
        {
            // [NEW] Called by UI when a slot is clicked — handled via BuildingSlot events.
        }

        // ---- Public: called by UI slot when clicked ----

        public void StartDrag(BuildingSlot slot)
        {
            if (_isDragging) return;
            if (_playerData == null) return;
            if (slot.buildingPrefab == null) return;

            int price = slot.Price;
            if (price < 0) price = 0;
            if (_playerData.bubbles < price)
            {
                Debug.Log($"Not enough bubbles to build {slot.buildingPrefab.name}");
                return;
            }

            _draggedSlot = slot;
            _isDragging = true;

            // [NEW] Create ghost following cursor.
            if (_ghostPrefab != null)
                _ghostBuilding = Instantiate(_ghostPrefab);
            else if (slot.buildingPrefab != null)
                _ghostBuilding = Instantiate(slot.buildingPrefab);

            if (_ghostBuilding != null)
            {
                _ghostBuilding.SetActive(true);
                Building buildingComp = _ghostBuilding.GetComponent<Building>();
                if (buildingComp != null)
                    buildingComp.SetTransparent(true);
            }

            // [NEW] Reserve cost so player can't spend twice.
            _playerData.RemoveBubbles(price);
        }

        private void TryPlaceBuilding(Vector2 worldPos)
        {
            if (!_isDragging || _draggedSlot == null) return;
            if (_ghostBuilding == null) return;

            Building buildingComp = _ghostBuilding.GetComponent<Building>();
            Vector2 gridPos = SnapToGrid(worldPos);

            bool canPlace = buildingComp != null && buildingComp.CanBePlaced();
            if (!canPlace)
            {
                CancelDrag();
                return;
            }

            // [NEW] Finalize: snap to grid, restore color, destroy ghost.
            _ghostBuilding.transform.position = gridPos;
            if (buildingComp != null)
                buildingComp.SetTransparent(false);

            _isDragging = false;
            _draggedSlot = null;
            Destroy(_ghostBuilding);
            _ghostBuilding = null;
        }

        private void CancelDrag()
        {
            if (!_isDragging) return;

            // [NEW] Refund bubbles on cancel.
            if (_draggedSlot != null && _playerData != null)
                _playerData.ReceiveBubble(_draggedSlot.Price);

            if (_ghostBuilding != null)
            {
                Destroy(_ghostBuilding);
                _ghostBuilding = null;
            }

            _isDragging = false;
            _draggedSlot = null;
        }

        // ---- Game state (no GameManager) ----

        private void OnEnemyKilled(Enemy enemy)
        {
            _score += _scorePerKill;
        }

        private void CheckGameOver()
        {
            if (_isGameOver) return;
            if (_goalTransform == null) return;

            // [NEW] If any alive enemy reaches the goal, lose a life.
            foreach (Enemy e in Enemy.All)
            {
                if (e == null || !e.IsAlive) continue;
                float dist = Vector2.Distance(e.transform.position, _goalTransform.position);
                if (dist < 0.5f)
                {
                    _lives--;
                    Debug.Log($"Enemy reached base! Lives remaining: {_lives}");
                    if (_lives <= 0)
                    {
                        _isGameOver = true;
                        Debug.Log($"Game Over! Final score: {_score}");
                    }
                    break;
                }
            }
        }

        // ---- Helper ----

        private Vector2 SnapToGrid(Vector2 pos)
        {
            float gridSize = 1f;
            return new Vector2(
                Mathf.Round(pos.x / gridSize) * gridSize,
                Mathf.Round(pos.y / gridSize) * gridSize
            );
        }
    }
}
