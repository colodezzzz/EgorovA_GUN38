using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviour
{
    public event Action<Team> OnGameEnd;
    public event Action<Team> OnTeamChange;

    [Header("Main Settings")]
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private Transform _cellsContainer;
    [SerializeField] private VisualManager _visualManager;
    [SerializeField] private UIManager _uiManager;

    [Header("Additional Settings")]
    [SerializeField] private Board _board;

    private FigureMovement _figureMovement;
    private bool IsFigureChose = false;
    private Vector2Int _choseFigurePosition;

    private List<Vector2Int> _availableTurns = new List<Vector2Int>();

    private Team _currenTeamTurn = Team.White;
    private bool IsGameEnd;

    private void Awake()
    {
        Vector2Int size = new Vector2Int(_boardSize.y, _boardSize.x);

        _board = new Board(size);
        _board.OnDeleteFigure += Board_OnDeleteFigure;

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
        _uiManager.Initialize(this);

        OnTeamChange?.Invoke(_currenTeamTurn);
    }

    private void Cell_OnClick(Vector2Int position)
    {
        if (IsGameEnd)
        {
            return;
        }

        HideAvailableTurns();

        if (IsFigureChose)
        {
            if (TryTurn(position))
            {
                _currenTeamTurn = (Team)(((int)_currenTeamTurn + 1) % Enum.GetValues(typeof(Team)).Length);
                OnTeamChange?.Invoke(_currenTeamTurn);
            }
            
            IsFigureChose = false;
            _availableTurns.Clear();
        }
        else
        {
            Figure figure = _board.GetFigureByPosition(position);

            if (figure != null && figure.Team == _currenTeamTurn)
            {
                _availableTurns = _figureMovement.GetTurns(figure, position);
                ShowAvailableTurns();

                if (_availableTurns.Count > 0)
                {
                    _choseFigurePosition = position;
                    IsFigureChose = true;
                }
            }
        }
    }

    private void Board_OnDeleteFigure(Vector2Int position)
    {
        if (_board.WhiteCheckersCount == 0)
        {
            OnGameEnd?.Invoke(Team.White);
            IsGameEnd = true;
            return;
        }
        else if (_board.BlackCheckersCount == 0)
        {
            OnGameEnd?.Invoke(Team.Black);
            IsGameEnd = true;
            return;
        }
        else
        {
            Debug.LogError("Game is end but anyone has 0 checkers!");
        }
    }

    private bool TryTurn(Vector2Int position)
    {
        if (_availableTurns.Contains(position))
        {
            _board.MoveFigure(_choseFigurePosition, position);
            return true;
        }

        return false;
    }

    private void ShowAvailableTurns()
    {
        foreach (Vector2Int pos in _availableTurns)
        {
            if (_cellsContainer.GetChild(pos.x * _board.Size.y + pos.y).TryGetComponent(out Cell cell))
            {
                cell.ShowTurnPanel();
            }
        }
    }

    private void HideAvailableTurns()
    {
        foreach (Vector2Int pos in _availableTurns)
        {
            if (_cellsContainer.GetChild(pos.x * _board.Size.y + pos.y).TryGetComponent(out Cell cell))
            {
                cell.HideTurnPanel();
            }
        }
    }

    private void UnsubscribeFromCells()
    {
        for (int i = 0; i < _cellsContainer.childCount; i++)
        {
            if (_cellsContainer.GetChild(i).TryGetComponent(out Cell cell))
            {
                cell.OnClick -= Cell_OnClick;
            }
        }
    }
}
