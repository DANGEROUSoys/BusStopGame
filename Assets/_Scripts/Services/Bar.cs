using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetProgress(float progress)
    {
        _slider.value = progress;
    }
}
