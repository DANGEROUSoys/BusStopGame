using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bonesCountText;
        [SerializeField] private TMP_Text _energyDrinkCountText;

        public void SetBonesCount(int value)
        {
            _bonesCountText.text = value.ToString();
        }

        public void SetEnergyDrinkCount(int value)
        {
            _energyDrinkCountText.text = value.ToString();
        }
    }
}