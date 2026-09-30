using Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public abstract class Enemy : Entity
    {
        // ---- Static registry (members folded into Enemy — no separate class) ----
        private static readonly List<Enemy> _all = new List<Enemy>();
        private static Transform _goal;

        public static Transform Goal
        {
            get => _goal;
            set => _goal = value;
        }

        public static IEnumerable<Enemy> All => new List<Enemy>(_all);

        public static void Add(Enemy e)
        {
            if (e != null && !_all.Contains(e))
                _all.Add(e);
        }

        public static void Remove(Enemy e)
        {
            _all.Remove(e);
        }

        public static event System.Action<Enemy> OnKilled;

        public static void NotifyKilled(Enemy e)
        {
            OnKilled?.Invoke(e);
        }

        // ---- Instance fields (protected so children access without re-declaring) ----
        [SerializeField] protected float _attackDistance = 0.5f;
        [SerializeField] protected int _damage = 5;
        [SerializeField] protected float _speed = 1.0f;
        [SerializeField] protected float _attackRate = 1.0f;

        public bool IsAlive => _hp > 0;

        public void Attack(Building target)
        {
            if (target == null) return;
            target.TakeDamage(_damage);
        }

        // [NEW] Children override this to set their own max HP.
        //         Called by children in Start() to initialize HP.
        protected virtual int MaxHp => 10;

        public override void Die()
        {
            NotifyKilled(this);
            Remove(this);
            base.Die();
        }

        // [NEW] Children call this in Start() to batch-set their stats
        //         (or set the protected fields directly if they prefer).
        protected void SetStats(float attackDistance, int damage, float speed, float attackRate)
        {
            _attackDistance = attackDistance;
            _damage = damage;
            _speed = speed;
            _attackRate = attackRate;
        }

        protected virtual void Update()
        {
            MoveTowardTarget();
            TryAttackIfInRange();
        }

        protected abstract Transform GetCurrentTarget();

        private void MoveTowardTarget()
        {
            Transform target = GetCurrentTarget();
            if (target == null) return;

            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            float distance = direction.magnitude;

            if (distance > 0.01f)
            {
                Vector2 moveStep = direction.normalized * _speed * Time.deltaTime;
                transform.position = (Vector2)transform.position + moveStep;
            }
        }

        private void TryAttackIfInRange()
        {
            Transform target = GetCurrentTarget();
            if (target == null) return;

            float dist = Vector2.Distance(transform.position, target.position);
            if (dist <= _attackDistance)
            {
                Building building = target.GetComponent<Building>();
                if (building != null)
                {
                    Attack(building);
                }
            }
        }
    }
}
