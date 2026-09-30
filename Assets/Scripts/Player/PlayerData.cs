using UnityEngine;

namespace Player
{
    // [NEW] PlayerData is the single source of truth for the player's resources.
    //         Existing fields/methods kept; we add validation and events.
    public class PlayerData : MonoBehaviour
    {
        // ---- EXISTING FIELD ----
        public int bubbles = 100;

        // [NEW] Starting amount (set in inspector or via GameManager).
        [SerializeField] private int _startingBubbles = 100;

        // [NEW] Events for UI to listen to.
        public System.Action<int> OnBubblesChanged;

        private void Start()
        {
            bubbles = _startingBubbles;
            OnBubblesChanged?.Invoke(bubbles);
        }

        // ---- EXISTING METHODS (implemented with events) ----

        public void ReceiveBubble(int amount)
        {
            if (amount <= 0) return;
            bubbles += amount;
            OnBubblesChanged?.Invoke(bubbles);
        }

        public void RemoveBubbles(int amount)
        {
            if (amount <= 0) return;
            bubbles = Mathf.Max(0, bubbles - amount);
            OnBubblesChanged?.Invoke(bubbles);
        }

        // [NEW] Safe purchase: deduct only if affordable, return false otherwise.
        public bool TryPurchase(int cost)
        {
            if (cost < 0) cost = 0;
            if (bubbles < cost) return false;
            RemoveBubbles(cost);
            return true;
        }

        // [NEW] Reset to initial state (for game over / restart).
        public void Reset()
        {
            bubbles = _startingBubbles;
            OnBubblesChanged?.Invoke(bubbles);
        }
    }
}
