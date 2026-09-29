using UnityEngine;

namespace Player {
    public class PlayerData : MonoBehaviour {
        public int bubbles;
        
        public void ReceiveBubble(int amount) {
            bubbles += amount;
        }
        
        public void RemoveBubbles(int amount) {
            bubbles -= amount;
        }

    }
}
