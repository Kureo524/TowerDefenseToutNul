using Buildings;
using UnityEngine;

namespace Entities {
    public abstract class Enemy : MonoBehaviour {
        private float _attackDistance;
        private int _damage;
        private float _speed;
        private float _attackRate;
        
        public void Attack(Building target) {
            
        }
    }
}
