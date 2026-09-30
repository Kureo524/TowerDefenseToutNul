using System.Collections;
using UnityEngine;

namespace Buildings
{
    // [NEW] TickingBuilding runs a coroutine on Start.  Derived classes override
    //         YieldTick() and start the coroutine.  Existing Tick() body kept.
    public abstract class TickingBuilding : Building
    {
        private Coroutine _tickCoroutine;

        // ---- EXISTING METHOD (kept, now called from Start) ----
        public IEnumerator Tick()
        {
            yield return new WaitForSeconds(0.1f);
        }

        public override void Start()
        {
            base.Start();
            StartTicking();
        }

        private void StartTicking()
        {
            if (_tickCoroutine != null)
                StopCoroutine(_tickCoroutine);
            _tickCoroutine = StartCoroutine(TickLoop());
        }

        private IEnumerator TickLoop()
        {
            while (true)
            {
                YieldTick();
                yield return new WaitForSeconds(0.1f);
            }
        }

        // [NEW] Override in derived classes: called every 0.1s.
        protected virtual void YieldTick()
        {
        }

        public override void Die()
        {
            if (_tickCoroutine != null)
                StopCoroutine(_tickCoroutine);
            base.Die();
        }
    }
}
