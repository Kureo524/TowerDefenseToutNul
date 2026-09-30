using Entities;
using UnityEngine;

namespace Buildings
{
    public class SandTower : TickingBuilding
    {
        // [NEW] Configurable via Inspector
        [SerializeField] private float _detectionRadius = 3f;
        [SerializeField] private float _attackCooldown = 0.5f;
        [SerializeField] private int _damagePerShot = 10;
        [SerializeField] private Transform _muzzleTransform;

        private float _nextAttackTime;
        private Enemy _currentTarget;

        // [NEW] Shoot damage and rotate towards target
        void Shoot(Enemy enemy)
        {
            if (enemy == null || !enemy.IsAlive) return;

            enemy.TakeDamage(_damagePerShot);
            if (_muzzleTransform != null)
                RotateToward(enemy.transform.position);
        }

        // ---- TickingBuilding calls this every 0.1s ----
        protected override void YieldTick()
        {
            // [NEW] Find nearest enemy in detection radius
            if (Time.time < _nextAttackTime) return;

            Enemy closest = FindNearestEnemyInRange();
            if (closest != null)
            {
                _currentTarget = closest;
                Shoot(closest);
                _nextAttackTime = Time.time + _attackCooldown;
            }
            else
            {
                _currentTarget = null;
            }
        }

        private Enemy FindNearestEnemyInRange()
        {
            float bestDist = _detectionRadius;
            Enemy best = null;

            // [NEW] Iterate over static enemy list from Enemy class
            foreach (Enemy e in Enemy.All)
            {
                if (e == null || !e.IsAlive) continue;
                float d = Vector2.Distance(transform.position, e.transform.position);
                if (d <= bestDist)
                {
                    bestDist = d;
                    best = e;
                }
            }
            return best;
        }

        // [NEW] Rotate tower to face target
        private void RotateToward(Vector2 target)
        {
            Vector2 direction = target - (Vector2)transform.position;
            if (direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public override void Start()
        {
            base.Start();
            // [NEW] Create detection zone collider if not assigned
            if (_enemyDetectionZone == null)
            {
                GameObject zoneObj = new GameObject("DetectionZone");
                zoneObj.transform.SetParent(transform);
                zoneObj.transform.localPosition = Vector3.zero;
                _enemyDetectionZone = zoneObj.AddComponent<CircleCollider2D>();
                _enemyDetectionZone.radius = _detectionRadius;
                _enemyDetectionZone.isTrigger = true;
            }
        }

        // [NEW] OnTrigger kept from original skeleton
        private void OnTriggerEnter2D(Collider2D other)
        {
        }
    }
}
