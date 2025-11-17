using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Board
    {
        public event Action<Vector2Int, Vector2Int> OnMoveFigure;
        public event Action<Vector2Int> OnDeleteFigure;
        public event Action<Vector2Int, Figure> OnChangePawn;

        public readonly Vector2Int Size;

        [TextArea(8, 8)] public string BoardString;

        private Figure[,] _board;

        public Board(Vector2Int size)
        {
            Size = size;
            _board = new Figure[Size.x, Size.y];

            string[] boardString =
            {
                "rkbqKbkr",
                "pppppppp",
                "nnnnnnnn",
                "nnnnnnnn",
                "nnnnnnnn",
                "nnnnnnnn",
                "pppppppp",
                "rkbKqbkr",
            };

            Fill(boardString);

            ShowBoardConsole();
        }

        public void ShowBoardConsole()
        {
            BoardString = "";

            for (int x = 0; x < _board.GetLength(0); x++)
            {
                for (int y = 0; y < _board.GetLength(1); y++)
                {
                    if (_board[x, y] != null)
                    {
                        BoardString += _board[x, y].Sign;
                    }
                    else
                    {
                        BoardString += "-";
                    }
                }

                BoardString += "\n";
            }
        }

        public void MoveFigure(Vector2Int oldPosition, Vector2Int newPosition)
        {
            _board[newPosition.x, newPosition.y] = _board[oldPosition.x, oldPosition.y];
            _board[oldPosition.x, oldPosition.y] = null;

            OnMoveFigure?.Invoke(oldPosition, newPosition);
            ShowBoardConsole();
        }

        public void DeleteFigure(Vector2Int position)
        {
            _board[position.x, position.y] = null;
            OnDeleteFigure?.Invoke(position);
            ShowBoardConsole();
        }

        public void ChangePawn(Vector2Int position, FigureType newType)
        {
            if (_board[position.x, position.y].Type != FigureType.Pawn)
            {
                Debug.LogError("Figure is not PAWN!");
                return;
            }

            if (position.x == 0 || position.x == Size.x-1)
            {
                DeleteFigure(position);
                _board[position.x, position.y].Type = newType;
                OnChangePawn?.Invoke(position, _board[position.x, position.y]);
            }

            ShowBoardConsole();
        }

        public Figure GetFigureByPosition(Vector2Int position)
        {
            return _board[position.x, position.y];
        }

        private void Fill(string[] figures)
        {
            for (int x = 0; x < figures.Length; x++)
            {
                for (int y = 0; y < figures[x].Length; y++)
                {
                    Team team = x > 1 ? Team.Black : Team.White;
                    _board[x, y] = GetFigureByString(figures[x][y].ToString(), team);
                }
            }
        }

        private Figure GetFigureByString(string figure, Team team)
        {
            switch (figure)
            {
                case "n":
                    return null;

                case "p":
                    return new Figure(FigureType.Pawn, team, figure);

                case "r":
                    return new Figure(FigureType.Rook, team, figure);

                case "k":
                    return new Figure(FigureType.Knight, team, figure);

                case "b":
                    return new Figure(FigureType.Bishop, team, figure);

                case "q":
                    return new Figure(FigureType.Queen, team, figure);

                case "K":
                    return new Figure(FigureType.King, team, figure);

                default:
                    Debug.LogError("Unknown figure char!");
                    return null;
            }
        }
    }
}