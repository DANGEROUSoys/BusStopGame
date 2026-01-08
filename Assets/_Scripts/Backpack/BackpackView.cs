using UnityEngine;

public class BackpackView : MonoBehaviour
{
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
}
