using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Board
    {
        public event Action<Vector2Int, Vector2Int> OnMoveFigure;
        public event Action<Vector2Int> OnDeleteFigure;
        public event Action<Vector2Int, Figure> OnChangeToKing;

        public readonly Vector2Int Size;

        public Vector2Int WhiteKingPosition { get; private set; }
        public Vector2Int BlackKingPosition { get; private set; }

        [TextArea(8, 8)] public string BoardString;

        private Figure[,] _board;

        public Board(Vector2Int size)
        {
            Size = size;
            _board = new Figure[Size.x, Size.y];

            string[] boardString =
            {
                "nbnbnbnb",
                "bnbnbnbn",
                "nbnbnbnb",
                "nnnnnnnn",
                "nnnnnnnn",
                "wnwnwnwn",
                "nwnwnwnw",
                "wnwnwnwn",
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
            if (IsPositionCorrect(oldPosition) == false)
            {
                Debug.LogError($"Old position is not correct!");
                return;
            }

            if (IsPositionCorrect(newPosition) == false)
            {
                Debug.LogError("New position is not correct!");
                return;
            }

            _board[newPosition.x, newPosition.y] = _board[oldPosition.x, oldPosition.y];
            _board[oldPosition.x, oldPosition.y] = null;

            OnMoveFigure?.Invoke(oldPosition, newPosition);
            ShowBoardConsole();
        }

        public void DeleteFigure(Vector2Int position)
        {
            if (IsPositionCorrect(position) == false)
            {
                Debug.LogError("Position is not correct!");
                return;
            }

            _board[position.x, position.y] = null;
            OnDeleteFigure?.Invoke(position);
            ShowBoardConsole();
        }

        public void ChangeToKing(Vector2Int position)
        {
            if (IsPositionCorrect(position) == false)
            {
                Debug.LogError("Position is not correct!");
                return;
            }

            // Превратить шашку в дамку
        }

        public Figure GetFigureByPosition(Vector2Int position)
        {
            return _board[position.x, position.y];
        }

        public bool IsPositionCorrect(Vector2Int position)
        {
            if (position.x < 0
                || position.x >= Size.x
                || position.y < 0
                || position.y >= Size.y)
            {
                return false;
            }

            return true;
        }

        private void Fill(string[] figures)
        {
            for (int x = 0; x < figures.Length; x++)
            {
                for (int y = 0; y < figures[x].Length; y++)
                {
                    _board[x, y] = GetFigureByString(figures[x][y].ToString());
                }
            }
        }

        private Figure GetFigureByString(string figureChar)
        {
            switch (figureChar)
            {
                case "w":
                    return new Figure(FigureType.Checker, Team.White, figureChar);

                case "b":
                    return new Figure(FigureType.Checker, Team.Black, figureChar);

                default:
                    return null;
            }
        }
    }
}