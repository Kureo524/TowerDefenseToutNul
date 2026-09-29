using UnityEngine;

namespace Buildings {
    public abstract class Building : Entities.Entity {
        public int price;

        public void Drop(Vector2 location) {
            
        }

        public void StartDrag() {
            
        }

        public void Drag(Vector2 location) {
            
        }
        
        public bool CanBePlaced() {
            return true;
        }

        public void SetTransparent(bool transparent) {
            
        }

    }
}
