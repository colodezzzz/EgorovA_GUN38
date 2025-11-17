using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event System.Action<Vector2Int> OnClick;

    [SerializeField] private GameObject _selectedPlane;
    [SerializeField] private GameObject _turnPlane;

    private Vector2Int _position;

    public void Initialize(Vector2Int position)
    {
        _position = position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: Передавать какие-то аргументы
        OnClick?.Invoke(_position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectedPlane.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedPlane.SetActive(false);
    }

    public void ShowTurnPanel()
    {
        _turnPlane.SetActive(true);
    }

    public void HideTurnPanel()
    {
        _turnPlane.SetActive(false);
    }
}
