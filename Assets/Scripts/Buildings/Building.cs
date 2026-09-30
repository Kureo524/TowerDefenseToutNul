using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    // [NEW] Building base now holds HP and implements the drag-and-drop interface
    //         used by PlayerController and the UI slots.  Existing fields kept.
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Building : Entities.Entity
    {
        // ---- EXISTING FIELDS ----
        public int price;

        // [NEW] Buildings have HP so they can be destroyed by enemies.
        [SerializeField] private int _maxHp = 100;
        private int _currentHp;
        
        // Field Added to change transparency
        SpriteRenderer _sprite;

        // ---- EXISTING METHODS (implemented) ----

        public void Drop(Vector2 location)
        {
            transform.position = location;
        }

        public void StartDrag()
        {
            // [NEW] Called when the player picks up a building from the UI slot.
        }

        public void Drag(Vector2 location)
        {
            transform.position = location;
        }

        public bool CanBePlaced()
        {
            // [NEW] Override in derived classes; default = always placeable.
            return true;
        }

        public void SetTransparent(bool transparent)
        {
            if (_sprite != null)
                _sprite.color = transparent ? Color.gray : Color.white;
        }

        // ---- NEW HP MANAGEMENT ----

        public override void Start()
        {
            base.Start();
            _currentHp = _maxHp;
            _sprite = GetComponent<SpriteRenderer>();
        }

        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            _currentHp = Mathf.Max(0, _currentHp - dmg);
            if (_currentHp <= 0)
                Die();
        }

        public override void HealDamage(int amount)
        {
            base.HealDamage(amount);
            _currentHp = Mathf.Min(_maxHp, _currentHp + amount);
        }

        public override void Die()
        {
            // [NEW] Just deactivate — registry handled by caller (PlayerController).
            gameObject.SetActive(false);
        }

        // [NEW] Public read access.
        public bool IsAlive => _currentHp > 0;
    }
}
