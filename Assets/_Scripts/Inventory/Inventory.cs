using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private short _maxBonesCount;
        [SerializeField] private float _maxEnergyDrinksCount;
        [SerializeField] private InventoryView _inventoryView;
        private short _bonesCount;
        private short _energyDrinksCount;
        private EventBus _eventBus;

        public short BonesCount => _bonesCount;
        public short EnergyDrinksCount => _energyDrinksCount;

        public void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;

            _bonesCount = 0;
            _energyDrinksCount = 0;

            _eventBus.Subscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
            _eventBus.Subscribe<DogWasFed>(DecreaseBonesCount);
        }

        private void OnDisable()
        {
            if (_eventBus != null)
            {
                _eventBus.UnSubscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
                _eventBus.UnSubscribe<DogWasFed>(DecreaseBonesCount);
            }
        }

        private void IncreaseBonesCount()
        {
            if (_bonesCount < _maxBonesCount)
            {
                _bonesCount++;
                _inventoryView.SetBonesCount(_bonesCount);
            }
            else
                Debug.Log("Количество костей уже максимальное! Это странно.");
        }
        private void DecreaseBonesCount()
        {
            if (_bonesCount > 0)
            {
                _bonesCount--;
                _inventoryView.SetBonesCount(_bonesCount);
            }
            else
            {
                _bonesCount = 0;
                Debug.Log("Количество костей равно нулю!");
            }
        }
    }
}