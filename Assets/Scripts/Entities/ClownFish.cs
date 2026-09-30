using UnityEngine;

namespace Entities
{
    public class ClownFish : Enemy
    {
        // Override MaxHp instead of re-declaring _hp.
        protected override int MaxHp => 20;

        protected override void Start()
        {
            // Use inherited protected fields directly — no need to re-declare them.
            _attackDistance = 0.5f;
            _damage = 5;
            _speed = 2.5f;
            _attackRate = 1.0f;

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
