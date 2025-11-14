using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public enum FigureType
    {
        None,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public enum Team
    {
        White,
        Black
    }

    public class FigureMovement
    {
        private Vector2Int _boardSize;
        private Board _board;

        public FigureMovement(Board board)
        {
            _board = board;
            _boardSize = _board.Size;
        }

        public List<Vector2Int> GetTurns(FigureType figureType, Team team, int index)
        {
            List<Vector2Int> turns;
            Vector2Int figurePosition = GetPosition(index);

            switch (figureType)
            {
                case FigureType.Pawn:
                    turns = GetPawnTurns(figurePosition, team);
                    break;

                case FigureType.Rook:
                    turns = GetRookTurns(figurePosition);
                    break;

                case FigureType.Knight:
                    turns = GetKnightTurns(figurePosition);
                    break;

                case FigureType.Bishop:
                    turns = GetBishopTurns(figurePosition);
                    break;

                case FigureType.Queen:
                    turns = GetQueenTurns(figurePosition);
                    break;

                case FigureType.King:
                    turns = GetKingTurns(figurePosition);
                    break;

                default:
                    turns = new List<Vector2Int>();
                    Debug.LogError("Unknown figure type!");
                    break;
            }

            return turns;
        }

        private List<Vector2Int> GetPawnTurns(Vector2Int position, Team team)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            if (team == Team.Black)
            {
                if (position.y > 0)
                {
                    if (position.x > 0)
                    {
                        cellsToMove.Add(new Vector2Int(position.x - 1, position.y - 1));
                    }

                    if (position.x < _boardSize.x - 1)
                    {
                        cellsToMove.Add(new Vector2Int(position.x + 1, position.y - 1));
                    }

                    cellsToMove.Add(new Vector2Int(position.x, position.y - 1));
                }
            }
            else
            {
                if (position.y < _boardSize.y - 1)
                {
                    if (position.x > 0)
                    {
                        cellsToMove.Add(new Vector2Int(position.x - 1, position.y + 1));
                    }

                    if (position.x < _boardSize.x - 1)
                    {
                        cellsToMove.Add(new Vector2Int(position.x + 1, position.y + 1));
                    }

                    cellsToMove.Add(new Vector2Int(position.x, position.y + 1));
                }
            }

            

            return cellsToMove;
        }

        public List<Vector2Int> GetRookTurns(Vector2Int position)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            for (int x = 0; x < position.x; x++)
            {
                cellsToMove.Add(new Vector2Int(x, position.y));
            }

            for (int x = position.x + 1; x < _boardSize.y; x++)
            {
                cellsToMove.Add(new Vector2Int(x, position.y));
            }

            for (int y = 0; y < position.y; y++)
            {
                cellsToMove.Add(new Vector2Int(position.x, y));
            }

            for (int y = position.y + 1; y < _boardSize.x; y++)
            {
                cellsToMove.Add(new Vector2Int(position.x, y));
            }

            return cellsToMove;
        }

        public List<Vector2Int> GetKnightTurns(Vector2Int position)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            for (int x = -2; x < 3; x += 4)
            {
                for (int y = -1; y < 2; y += 2)
                {
                    cellsToMove.Add(new Vector2Int(position.x + x, position.y + y));
                    cellsToMove.Add(new Vector2Int(position.x + y, position.y + x));
                }
            }

            int i = 0;

            while (i < cellsToMove.Count)
            {
                if (cellsToMove[i].x < 0 || cellsToMove[i].x >= _boardSize.x || cellsToMove[i].y < 0 || cellsToMove[i].y >= _boardSize.y)
                {
                    cellsToMove.Remove(cellsToMove[i]);
                    continue;
                }

                i++;
            }

            return cellsToMove;
        }

        public List<Vector2Int> GetBishopTurns(Vector2Int position)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            int delta = 1;

            while (position.x - delta >= 0 && position.y - delta >= 0)
            {
                cellsToMove.Add(new Vector2Int(position.x - delta, position.y - delta));
                delta++;
            }

            delta = 1;

            while (position.x + delta < _boardSize.x && position.y - delta >= 0)
            {
                cellsToMove.Add(new Vector2Int(position.x + delta, position.y - delta));
                delta++;
            }

            delta = 1;

            while (position.x - delta >= 0 && position.y + delta < _boardSize.y)
            {
                cellsToMove.Add(new Vector2Int(position.x - delta, position.y + delta));
                delta++;
            }

            delta = 1;

            while (position.x + delta < _boardSize.x && position.y + delta < _boardSize.y)
            {
                cellsToMove.Add(new Vector2Int(position.x + delta, position.y + delta));
                delta++;
            }

            return cellsToMove;
        }

        public List<Vector2Int> GetQueenTurns(Vector2Int position)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();
            cellsToMove = GetRookTurns(position);
            cellsToMove.AddRange(GetBishopTurns(position));

            return cellsToMove;
        }

        public List<Vector2Int> GetKingTurns(Vector2Int position)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    cellsToMove.Add(new Vector2Int(position.x + x, position.y + y));
                }
            }

            int i = 0;

            while (i < cellsToMove.Count)
            {
                if (cellsToMove[i].x < 0 || cellsToMove[i].x >= _boardSize.x
                    || cellsToMove[i].y < 0 || cellsToMove[i].y >= _boardSize.y
                    || (cellsToMove[i].x == position.x && cellsToMove[i].y == position.y))
                {
                    cellsToMove.Remove(cellsToMove[i]);
                    continue;
                }

                i++;
            }

            return cellsToMove;
        }

        private Vector2Int GetPosition(int index)
        {
            Vector2Int position = new Vector2Int();

            position.y = index / _boardSize.x;
            position.x = index % _boardSize.x;

            return position;
        }

        private int GetIndex(Vector2Int position)
        {
            int index = position.y * _boardSize.x + position.x;

            return index;
        }
    }
}
