using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        // ---- EXISTING FIELDS (kept as-is, now backed by serializable properties) ----
        [SerializeField] protected int _hp = 10;
        [SerializeField] protected string _name = "Entity";
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rb;
        
        public bool IsAlive => _hp > 0;

        // ---- EXISTING METHODS (implemented) ----

        public virtual void TakeDamage(int dmg)
        {
            if (dmg <= 0) return;
            _hp -= dmg;
            if (_hp <= 0)
            {
                _hp = 0;
                Die();
            }
        }

        public virtual void HealDamage(int amount)
        {
            if (amount <= 0) return;
            _hp += amount;
        }

        public virtual void Die()
        {
            gameObject.SetActive(false);
        }

        public virtual void Start()
        {
        }
    }
}
