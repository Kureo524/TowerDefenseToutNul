using System.Collections.Generic;
using UnityEngine;
using UI;
using Player;

namespace Buildings
{
    // [NEW] BuildingMenu is the in-game UI panel that shows available buildings.
    //         Existing OpenMenu kept; we add slot population and purchase handling.
    public class BuildingMenu : MonoBehaviour
    {
        // ---- EXISTING FIELD ----
        private List<BuildingSlot> _buildingSlots = new();

        // [NEW] Public API so PlayerController can open/close the menu.
        public bool IsOpen { get; private set; }

        // [NEW] Reference to the UI menu that actually renders the slots.
        [SerializeField] private UI.UIBuildingMenu _uiMenu;

        // ---- EXISTING METHOD (implemented) ----
        void OpenMenu()
        {
            if (_uiMenu == null) return;
            IsOpen = true;
            _uiMenu.ToggleMenu(_buildingSlots);
        }

        // [NEW] Close the menu.
        public void CloseMenu()
        {
            if (_uiMenu == null) return;
            IsOpen = false;
            _uiMenu.ToggleMenu(null);
        }

        // [NEW] Populate slots from an array of prefabs (wired in inspector or
        //         by GameManager at start).
        public void SetSlots(BuildingSlot[] slots)
        {
            _buildingSlots = new List<BuildingSlot>(slots);
            foreach (var slot in _buildingSlots)
            {
                slot.OnClick += OnSlotClicked;
            }
        }

        private void OnSlotClicked(BuildingSlot slot)
        {
            // [NEW] Hand the click to PlayerController to start drag-and-place.
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
                player.StartDrag(slot);
        }

        private void OnDestroy()
        {
            foreach (var slot in _buildingSlots)
                slot.OnClick -= OnSlotClicked;
        }
    }
}
