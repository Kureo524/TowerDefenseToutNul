using UnityEngine;
using UnityEngine.UI;

namespace Entities {
    public abstract class Entity : MonoBehaviour {
        private int _hp;
        private string _name;
        private Image _sprite;
        private Image _uiImage;
        private Collider2D _collider;
        private Rigidbody2D _rb;

        public virtual void TakeDamage(int dmg) {
            
        }

        public virtual void HealDamage(int amount) {
            
        }

        public virtual void Die() {
            
        }
    }
}
