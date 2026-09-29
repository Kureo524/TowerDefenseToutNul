using System.Collections;
using UnityEngine;

namespace Buildings {
    public abstract class TickingBuilding : Building {
        public IEnumerator Tick() {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
