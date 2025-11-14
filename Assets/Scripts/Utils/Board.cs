using System;
using UnityEngine;

namespace Utils
{
    public class Board
    {
        public event Action<Vector2Int, Vector2Int> OnMoveFigure;
        public event Action<Vector2Int> OnDeleteFigure;
        public event Action<Vector2Int, FigureType> OnChangePawn;

        public readonly Vector2Int Size;

        private Figure[,] _board;

        public Board(Vector2Int size)
        {
            Size = size;
            _board = new Figure[Size.x, Size.y];

            for (int y = 0; y < _board.GetLength(1); y++)
            {
                _board[y, 1] = new Figure(FigureType.Pawn, Team.White);
            }

            for (int y = 0; y < _board.GetLength(1); y++)
            {
                _board[y, Size.x - 1] = new Figure(FigureType.Pawn, Team.Black);
            }
        }

        public void MoveFigure(Vector2Int oldPosition, Vector2Int newPosition)
        {
            Figure figure = _board[oldPosition.x, oldPosition.y];
            _board[oldPosition.x, oldPosition.y] = _board[newPosition.x, newPosition.y];
            _board[newPosition.x, newPosition.y] = figure;

            OnMoveFigure?.Invoke(oldPosition, newPosition);
        }

        public void DeleteFigure(Vector2Int position)
        {
            _board[position.x, position.y] = null;
            OnDeleteFigure?.Invoke(position);
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
                OnChangePawn?.Invoke(position, newType);
            }
        }

        public Figure GetFigureByPosition(Vector2Int position)
        {
            return _board[position.x, position.y];
        }
    }
}