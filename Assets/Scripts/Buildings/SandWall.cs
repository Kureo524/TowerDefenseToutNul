using UnityEngine;

namespace Buildings
{
    // [NEW] SandWall is a simple HP-based defensive structure.
    //         Existing class kept, now with HP and collision.
    public class SandWall : Building
    {
        [SerializeField] private int _maxHp = 80;

        private int _currentHp;

        public override void Start()
        {
            base.Start();
            _currentHp = _maxHp;
        }

        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            _currentHp = Mathf.Max(0, _currentHp - dmg);
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                base.Die();
            }
        }

        public override void Die()
        {
            base.Die();
        }
    }
}
