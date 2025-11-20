using UnityEngine;
using Utils;

[CreateAssetMenu(fileName= "FiguresData", menuName= "SO/FiguresData")]
public class FiguresData : ScriptableObject
{
    [Header("Figures Prefabs")]
    [SerializeField] private Transform _whiteCheckerPrefab;
    [SerializeField] private Transform _blackCheckerPrefab;

    public Transform GetFigure(Figure figure)
    {
        

        return null;
    }
}