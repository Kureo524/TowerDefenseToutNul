using System;
using UnityEngine;

namespace Buildings
{
    // [NEW] Produces bubbles every tick.  Existing fields kept.
    public class Chest : TickingBuilding
    {
        // ---- Existing fields ----
        private int _bubblePerSecond;
        private Action<int> _onBubbleGained;

        // [NEW] Inspector-configurable.
        [SerializeField] private int _bubblesPerTick = 1;
        [SerializeField] private int _maxHp = 60;

        private int _currentHp;

        // [NEW] Cached PlayerData reference (set once at Start).
        private Player.PlayerData _playerData;

        // ---- Existing ProduceBubbles (implemented) ----
        private void ProduceBubbles()
        {
            if (_playerData != null)
                _playerData.ReceiveBubble(_bubblesPerTick);
        }

        // ---- TickingBuilding integration ----
        protected override void YieldTick()
        {
            base.YieldTick();
            ProduceBubbles();
        }

        public override void Die()
        {
            if (_onBubbleGained != null)
                _onBubbleGained(0);
            base.Die();
        }

        // [NEW] Cache PlayerData once at Start.
        public override void Start()
        {
            base.Start();
            _currentHp = _maxHp;
            _bubblePerSecond = _bubblesPerTick;
            _playerData = FindObjectOfType<Player.PlayerData>();
        }

        // [NEW] Allow external systems to subscribe to bubble events.
        public void SetBubbleCallback(Action<int> callback)
        {
            _onBubbleGained = callback;
        }
    }
}
