using System.Collections.Generic;
using Buildings;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // [NEW] UIBuildingMenu is the on-screen menu that renders building slots.
    //         Existing ToggleMenu kept; we add slot instantiation and purchase
    //         wiring.
    public class UIBuildingMenu : MonoBehaviour
    {
        // ---- EXISTING FIELD ----
        private LayoutGroup _layoutGroup;

        // [NEW] Reference to the slot prefab (inspector).
        [SerializeField] private UIBuildingSlot _slotPrefab;
        [SerializeField] private Transform _contentParent;

        // [NEW] Currently open slots.
        private List<UIBuildingSlot> _openSlots = new();

        // ---- EXISTING METHOD (implemented) ----
        public void ToggleMenu(List<BuildingSlot> buildingSlots)
        {
            if (buildingSlots == null || _slotPrefab == null)
                return;

            gameObject.SetActive(buildingSlots.Count > 0);

            if (buildingSlots.Count == 0)
            {
                ClearMenu();
                return;
            }

            // [NEW] Clear previous and populate from the BuildingSlot list.
            ClearMenu();

            foreach (var slot in buildingSlots)
            {
                UIBuildingSlot uiSlot = Instantiate(_slotPrefab, _contentParent);
                uiSlot.SetBuildingPrefab(slot.buildingPrefab);
                uiSlot.SetPrice(slot.Price);
                uiSlot.OnClick += () => OnSlotClicked(slot);
                _openSlots.Add(uiSlot);
            }
        }

        private void OnSlotClicked(BuildingSlot slot)
        {
            // [NEW] Hand the click to the PlayerController so it can start a drag.
            PlayerController player = FindObjectOfType<Player.PlayerController>();
            if (player != null)
                player.StartDrag(slot);
        }

        private void ClearMenu()
        {
            foreach (var uiSlot in _openSlots)
            {
                if (uiSlot != null)
                    Destroy(uiSlot.gameObject);
            }
            _openSlots.Clear();
        }

        private void OnDestroy()
        {
            ClearMenu();
        }
    }
}
