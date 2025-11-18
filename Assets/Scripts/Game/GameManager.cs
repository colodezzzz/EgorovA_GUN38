using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        ShowFigureInfo(position);

        if (IsFigureChose)
        {
            if (_choseFigurePosition != position && _availableTurns.Contains(position))
            {
                // Ход совершен
                if (_board.GetFigureByPosition(position) != null)
                {
                    _board.DeleteFigure(position);
                }

                _board.MoveFigure(_choseFigurePosition, position);
                _currenTeamTurn = (Team)(((int)_currenTeamTurn + 1) % Enum.GetValues(typeof(Team)).Length);

                OnTeamChange?.Invoke(_currenTeamTurn);
            }

            foreach (Cell cell in _availableCells)
            {
                cell.HideTurnPanel();
            }

            _availableCells.Clear();
            _availableTurns.Clear();

            IsFigureChose = false;
        }
        else
        {
            Figure figure = _board.GetFigureByPosition(position);

            if (figure != null)
            {
                // Ход, только если у команды текущий ход
                if (figure.Team == _currenTeamTurn)
                {
                    _availableTurns = _figureMovement.GetTurns(figure.Type, figure.Team, position);

                    if (_availableTurns.Count > 0)
                    {
                        for (int i = 0; i < _cellsContainer.childCount; i++)
                        {
                            if (_cellsContainer.GetChild(i).TryGetComponent(out Cell cell) && _availableTurns.Contains(cell.Position))
                            {
                                cell.ShowTurnPanel();
                                _availableCells.Add(cell);
                            }
                        }

                        _choseFigurePosition = position;
                        IsFigureChose = true;
                    }
                    else
                    {
                        IsFigureChose = false;
                    }
                }
            }
        }
    }

    public void ShowFigureInfo(Vector2Int position)
    {
        Figure figure = _board.GetFigureByPosition(position);

        if (figure != null)
        {
            Debug.Log($"Type: {figure.Type} | Team: {figure.Team} | Position: {position}");
        }
        else
        {
            Debug.Log($"Type: {null} | Team: {null} | Position: {position}");
        }
    }

    public GameState GetGameState(Team currentTeamTurn)
    {
        Vector2Int kingPosition = new Vector2Int(-1, -1);

        for (int x = 0; x < _boardSize.x; x++)
        {
            for (int y = 0; y < _boardSize.y; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Figure fig = _board.GetFigureByPosition(pos);

                if (fig.Type == FigureType.King && fig.Team == currentTeamTurn)
                {
                    kingPosition = pos;
                    break;
                }
            }

            if (kingPosition.x != -1)
            {
                break;
            }
        }

        for (int x = 0; x < _boardSize.x; x++)
        {
            for (int y = 0; y < _boardSize.y; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Figure fig = _board.GetFigureByPosition(pos);

                if (fig.Team != currentTeamTurn && _figureMovement.GetTurns(fig.Type, fig.Team, pos).Contains(kingPosition))
                {
                    return GameState.Check;
                }
            }
        }

        return GameState.None;
    }

    public GameState GetGameState(Figure[,] figures, Team currentTeamTurn)
    {
        Vector2Int kingPosition = currentTeamTurn == Team.White ? _board.WhiteKingPosition : _board.BlackKingPosition;
        Board board = new Board(figures);
        FigureMovement figMove = new FigureMovement(board);

        for (int x = 0; x < board.Size.x; x++)
        {
            for (int y = 0; y < board.Size.y; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Figure fig = board.GetFigureByPosition(pos);

                if (fig != null
                    && fig.Team != currentTeamTurn
                    && figMove.GetTurns(fig.Type, fig.Team, pos).Contains(kingPosition))
                {
                    return GameState.Check;
                }
            }
        }

        return GameState.None;
    }
}

public enum GameState
{
    None,
    Check,
    Mate
}