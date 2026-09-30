using System;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    // [NEW] BuildingSlot represents one building option in the menu.
    //         Existing prefab field kept; adds a Button for clicks.
    public class BuildingSlot : MonoBehaviour
    {
        // ---- Existing field ----
        public GameObject buildingPrefab;

        // [NEW] Event raised when the player clicks this slot.
        public event Action<BuildingSlot> OnClick;

        // [NEW] UI Button (wired in Inspector).
        [SerializeField] private Button _button;

        // [NEW] Price exposed for the UI to display.
        public int Price => buildingPrefab != null
            ? buildingPrefab.GetComponent<Building>()?.price ?? 0
            : 0;

        private void Awake()
        {
            if (_button != null)
                _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            OnClick?.Invoke(this);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
