using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event System.Action OnClick;

    [SerializeField] private GameObject _selectedPlane;

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: Передавать какие-то аргументы
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectedPlane.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedPlane.SetActive(false);
    }
}
