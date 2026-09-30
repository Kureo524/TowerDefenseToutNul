using UnityEngine;

namespace Entities
{
    public class SeaHorse : Enemy
    {
        // Override MaxHp instead of re-declaring _hp.
        protected override int MaxHp => 50;

        public override void Start()
        {
            // Use inherited protected fields directly — no need to re-declare them.
            _attackDistance = 0.5f;
            _damage = 10;
            _speed = 1.2f;
            _attackRate = 1.5f;

            HealDamage(MaxHp);
            Enemy.Add(this);
            base.Start();
        }

        protected override Transform GetCurrentTarget()
        {
            return Enemy.Goal;
        }

        public override void Die()
        {
            Enemy.Remove(this);
            gameObject.SetActive(false);
        }
    }
}
