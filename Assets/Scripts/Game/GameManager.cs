using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class GameManager : MonoBehaviour
{
    public event Action<Team> OnTeamChange;

    [Header("Main Settings")]
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private Transform _cellsContainer;
    [SerializeField] private VisualManager _visualManager;

    [Header("Additional Settings")]
    [SerializeField] private Board _board;

    private FigureMovement _figureMovement;
    private bool IsFigureChose = false;
    private Vector2Int _choseFigurePosition;

    private List<Cell> _availableCells = new List<Cell>();
    private List<Vector2Int> _availableTurns = new List<Vector2Int>();

    private Team _currenTeamTurn = Team.White;

    private void Awake()
    {
        Vector2Int size = new Vector2Int(_boardSize.y, _boardSize.x);

        _board = new Board(size);
        _figureMovement = new FigureMovement(_board);
    }

    private void Start()
    {
        for (int i = 0; i < _cellsContainer.childCount; i++)
        {
            if (_cellsContainer.GetChild(i).TryGetComponent(out Cell cell))
            {
                Vector2Int position = new Vector2Int(i / _board.Size.x, i % _board.Size.y);
                cell.Initialize(position);
                cell.OnClick += Cell_OnClick;
            }
        }

        _visualManager.Initialize(_board);
    }

    private void Cell_OnClick(Vector2Int position)
    {
        if (IsFigureChose)
        {
            Turn(position);
            IsFigureChose = false;
        }
        else
        {
            Figure figure = _board.GetFigureByPosition(position);

            if (figure != null && figure.Team == _currenTeamTurn)
            {
                _choseFigurePosition = position;
                IsFigureChose = true;
            }
        }
    }

    private void Turn(Vector2Int position)
    {
        // TODO:
        // Проверка - обычный ход или можно съесть
        // Если обычный ход, то ход переходит к сопернику
        // Если можно съесть, то ход не заканчивается, а вновь даётся выбор
        if (_board.GetFigureByPosition(position) == null)
        {
            _board.MoveFigure(_choseFigurePosition, position);
        }
    }
}
