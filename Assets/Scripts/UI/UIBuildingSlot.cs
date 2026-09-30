using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // [NEW] UIBuildingSlot is the UI widget for one building option in the menu.
    //         Existing fields kept; we add button wiring and price display.
    public class UIBuildingSlot : MonoBehaviour
    {
        // ---- EXISTING FIELDS ----
        private Image _sprite;
        private GameObject _buildingPrefab;
        private Action<GameObject> _onClickEvent;

        // [NEW] UI elements.
        [SerializeField] private Button _button;
        [SerializeField] private Text _priceText;
        [SerializeField] private Image _iconImage;

        // [NEW] Click event for the menu.
        public event Action OnClick;

        // ---- EXISTING BEHAVIOUR (wired to the button) ----

        private void Awake()
        {
            if (_button != null)
                _button.onClick.AddListener(OnButtonClicked);
        }

        // [NEW] Call from UIBuildingMenu to configure this slot.
        public void SetBuildingPrefab(GameObject prefab)
        {
            _buildingPrefab = prefab;
            if (_iconImage != null && prefab != null)
            {
                Sprite sprite = prefab.GetComponent<SpriteRenderer>()?.sprite
                    ?? prefab.GetComponent<Image>()?.sprite;
                if (sprite != null)
                    _iconImage.sprite = sprite;
            }
        }

        public void SetPrice(int price)
        {
            if (_priceText != null)
                _priceText.text = price.ToString();
        }

        private void OnButtonClicked()
        {
            OnClick?.Invoke();
            _onClickEvent?.Invoke(_buildingPrefab);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
