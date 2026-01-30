using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour, IDisposable
    {
        [SerializeField] private short _maxBonesCount;
        [SerializeField] private short _maxEnergyDrinksCount;
        [SerializeField] private InventoryView _inventoryView;
        private short _bonesCount;
        private short _energyDrinksCount;
        private EventBus _eventBus;

        public void Initialize()
        {
            _eventBus = ProjectContext.Instance.EventBus;

            _bonesCount = 0;
            _energyDrinksCount = 0;

            _eventBus.Subscribe<GetBonesCount, short>(GetBonesCount);
            _eventBus.Subscribe<GetEnergyDrinksCount, short>(GetEnergyDrinksCount);
            _eventBus.Subscribe<GetMaxBonesCount, short>(GetMaxBonesCount);
            _eventBus.Subscribe<GetMaxEnergyDrinksCount, short>(GetMaxEnergyDrinksCount);
            _eventBus.Subscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
            _eventBus.Subscribe<DogWasFed>(DecreaseBonesCount);
        }

        public short GetBonesCount() => _bonesCount;
        public short GetEnergyDrinksCount() => _energyDrinksCount;
        public short GetMaxBonesCount() => _maxBonesCount;
        public short GetMaxEnergyDrinksCount() => _maxEnergyDrinksCount;

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
            _eventBus.UnSubscribe<GetBonesCount, short>(GetBonesCount);
            _eventBus.UnSubscribe<GetEnergyDrinksCount, short>(GetEnergyDrinksCount);
            _eventBus.UnSubscribe<GetMaxBonesCount, short>(GetMaxBonesCount);
            _eventBus.UnSubscribe<GetMaxEnergyDrinksCount, short>(GetMaxEnergyDrinksCount);
            _eventBus.UnSubscribe<BoneMiniGameWasComplited>(IncreaseBonesCount);
            _eventBus.UnSubscribe<DogWasFed>(DecreaseBonesCount);
            _eventBus = null;
        }
    }
}