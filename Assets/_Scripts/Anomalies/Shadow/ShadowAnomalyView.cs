using UnityEngine;

public class ShadowAnomalyView : MonoBehaviour
{
    [SerializeField] private GameObject _shadowVisual;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    private float _scaleCoefficient;

    public void Initialize(float timeBeforeScreamer)
    {
        _scaleCoefficient = _maxScale / timeBeforeScreamer;
        _shadowVisual.transform.localScale = new Vector3(_minScale, _minScale, _minScale);
    }

    public void ChangeShadowScale(float currentTime)
    {
        float currentScale = currentTime * _scaleCoefficient + _minScale;
        _shadowVisual.transform.localScale = new Vector3(currentScale, currentScale, currentScale);;
    }
}
