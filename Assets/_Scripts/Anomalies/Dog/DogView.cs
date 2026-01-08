using UnityEngine;

public class DogView : MonoBehaviour
{
    [SerializeField] private GameObject _defaultView;
    [SerializeField] private GameObject _selectedView;

    public void SetHighlight(bool active)
    {
        if (active)
        {
            _selectedView.SetActive(true);
        }
        else
        {
            _selectedView.SetActive(false);
        }
    }

    public void ShowAppearanceAnimation()
    {
        _defaultView.SetActive(true);
    }
    public void ShowLeavingAnimation()
    {
        _defaultView.SetActive(false);
        _selectedView.SetActive(false);
    }
}
