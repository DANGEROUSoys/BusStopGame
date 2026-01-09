using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour, IDisposable
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

            _eventBus.Subscribe<DogWasInteracted, short>(GetBonesCount);
            _eventBus.Subscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
            _eventBus.Subscribe<DogWasFed>(DecreaseBonesCount);
        }

        private short GetBonesCount()
        {
            return _bonesCount;
        }

        private void IncreaseBonesCount()
        {
            if (_bonesCount < _maxBonesCount)
            {
                _bonesCount++;
                _inventoryView.SetBonesCount(_bonesCount);
            }
            else
                Debug.Log("Колличество костей уже максимальное!");
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
                Debug.Log("Колличество костей: 0!");
            }
        }

        public void Dispose()
        {
            _eventBus.UnSubscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
            _eventBus.UnSubscribe<DogWasFed>(DecreaseBonesCount);
        }
    }
}