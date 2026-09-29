using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIBuildingSlot : MonoBehaviour {
        private Image _sprite;
        private GameObject _buildingPrefab;
        private Action<GameObject> _onClickEvent;
    }
}
