using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

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

        public Board(Figure[,] board)
        {
            Size = new Vector2Int(board.GetLength(0), board.GetLength(1));
            _board = board;
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

        public void ChangeToKing(Vector2Int position)
        {
            
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
                    
                }
            }
        }

        private Figure GetFigureByString(string figure, Team team)
        {
            switch (figure)
            {
                

                default:
                    Debug.LogError("Unknown figure char!");
                    return null;
            }
        }
    }
}