using UnityEngine;
using Utils;

[CreateAssetMenu(fileName= "FiguresData", menuName= "SO/FiguresData")]
public class FiguresData : ScriptableObject
{
    [Header("White Figures Prefabs")]
    [SerializeField] private Transform _whitePawnPrefab;
    [SerializeField] private Transform _whiteRookPrefab;
    [SerializeField] private Transform _whiteKnightPrefab;
    [SerializeField] private Transform _whiteBishopPrefab;
    [SerializeField] private Transform _whiteQueenPrefab;
    [SerializeField] private Transform _whiteKingPrefab;

    [Header("Black Figures Prefabs")]
    [SerializeField] private Transform _blackPawnPrefab;
    [SerializeField] private Transform _blackRookPrefab;
    [SerializeField] private Transform _blackKnightPrefab;
    [SerializeField] private Transform _blackBishopPrefab;
    [SerializeField] private Transform _blackQueenPrefab;
    [SerializeField] private Transform _blackKingPrefab;

    public Transform GetFigure(Figure figure)
    {
        if (figure.Team == Team.White)
        {
            switch (figure.Type)
            {
                case FigureType.None:
                    Debug.LogError("Figure type is none!");
                    break;

                case FigureType.Pawn:
                    return _whitePawnPrefab;

                case FigureType.Rook:
                    return _whiteRookPrefab;

                case FigureType.Knight:
                    return _whiteKnightPrefab;

                case FigureType.Bishop:
                    return _whiteBishopPrefab;

                case FigureType.Queen:
                    return _whiteQueenPrefab;

                case FigureType.King:
                    return _whiteKingPrefab;

                default:
                    Debug.LogError("Wrong figure type!");
                    break;
            }
        }
        else if (figure.Team == Team.Black)
        {
            switch (figure.Type)
            {
                case FigureType.None:
                    Debug.LogError("Figure type is none!");
                    break;

                case FigureType.Pawn:
                    return _blackPawnPrefab;

                case FigureType.Rook:
                    return _blackRookPrefab;

                case FigureType.Knight:
                    return _blackKnightPrefab;

                case FigureType.Bishop:
                    return _blackBishopPrefab;

                case FigureType.Queen:
                    return _blackQueenPrefab;

                case FigureType.King:
                    return _blackKingPrefab;

                default:
                    Debug.LogError("Wrong figure type!");
                    break;
            }
        }
        else
        {
            Debug.LogError("Wrong figure team!");
        }

        return null;
    }
}