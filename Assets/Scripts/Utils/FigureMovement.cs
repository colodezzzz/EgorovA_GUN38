using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FigureMovement
    {
        private Vector2Int _boardSize;
        private Board _board;

        public FigureMovement(Board board)
        {
            _board = board;
            _boardSize = _board.Size;
        }

        public FigureMovement(Figure[,] board)
        {
            _board = new Board(board);
            _boardSize = _board.Size;
        }

        public List<Vector2Int> GetTurns(FigureType figureType, Team team, Vector2Int figurePosition)
        {
            List<Vector2Int> turns;

            switch (figureType)
            {
                case FigureType.Pawn:
                    turns = GetPawnTurns(figurePosition, team);
                    break;

                case FigureType.Rook:
                    turns = GetRookTurns(figurePosition, team);
                    break;

                case FigureType.Knight:
                    turns = GetKnightTurns(figurePosition, team);
                    break;

                case FigureType.Bishop:
                    turns = GetBishopTurns(figurePosition, team);
                    break;

                case FigureType.Queen:
                    turns = GetQueenTurns(figurePosition, team);
                    break;

                case FigureType.King:
                    turns = GetKingTurns(figurePosition, team);
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
                if (position.x > 0)
                {
                    Vector2Int pos;

                    if (position.y > 0)
                    {
                        pos = new Vector2Int(position.x - 1, position.y - 1);

                        if (_board.GetFigureByPosition(pos) != null)
                            StopAddingToList(cellsToMove, pos, team);
                    }

                    if (position.y < _boardSize.y - 1)
                    {
                        pos = new Vector2Int(position.x - 1, position.y + 1);

                        if (_board.GetFigureByPosition(pos) != null)
                            StopAddingToList(cellsToMove, pos, team);
                    }


                    pos = new Vector2Int(position.x - 1, position.y);

                    if (_board.GetFigureByPosition(pos) == null)
                        StopAddingToList(cellsToMove, pos, team);
                }
            }
            else
            {
                if (position.x < _boardSize.x - 1)
                {
                    Vector2Int pos;

                    if (position.y > 0)
                    {
                        pos = new Vector2Int(position.x + 1, position.y - 1);

                        if (_board.GetFigureByPosition(pos) != null)
                            StopAddingToList(cellsToMove, pos, team);
                    }

                    if (position.y < _boardSize.x - 1)
                    {
                        pos = new Vector2Int(position.x + 1, position.y + 1);

                        if (_board.GetFigureByPosition(pos) != null)
                            StopAddingToList(cellsToMove, pos, team);
                    }

                    pos = new Vector2Int(position.x + 1, position.y);

                    if (_board.GetFigureByPosition(pos) == null)
                        StopAddingToList(cellsToMove, pos, team);
                }
            }

            return cellsToMove;
        }

        private List<Vector2Int> GetRookTurns(Vector2Int position, Team team)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            for (int x = position.x - 1; x > 0; x--)
            {
                Vector2Int pos = new Vector2Int(x, position.y);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }
            }

            for (int x = position.x + 1; x < _boardSize.x; x++)
            {
                Vector2Int pos = new Vector2Int(x, position.y);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }
            }

            for (int y = position.y - 1; y > 0; y--)
            {
                Vector2Int pos = new Vector2Int(position.x, y);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }
            }

            for (int y = position.y + 1; y < _boardSize.y; y++)
            {
                Vector2Int pos = new Vector2Int(position.x, y);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }
            }

            return cellsToMove;
        }

        private List<Vector2Int> GetKnightTurns(Vector2Int position, Team team)
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
                if (cellsToMove[i].x < 0
                    || cellsToMove[i].x >= _boardSize.x
                    || cellsToMove[i].y < 0
                    || cellsToMove[i].y >= _boardSize.y
                    || (_board.GetFigureByPosition(cellsToMove[i]) != null && _board.GetFigureByPosition(cellsToMove[i]).Team == team))
                {
                    cellsToMove.Remove(cellsToMove[i]);
                    continue;
                }

                i++;
            }

            return cellsToMove;
        }

        private List<Vector2Int> GetBishopTurns(Vector2Int position, Team team)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();

            int delta = 1;

            while (position.x - delta >= 0 && position.y - delta >= 0)
            {
                Vector2Int pos = new Vector2Int(position.x - delta, position.y - delta);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }

                delta++;
            }

            delta = 1;

            while (position.x + delta < _boardSize.x && position.y - delta >= 0)
            {
                Vector2Int pos = new Vector2Int(position.x + delta, position.y - delta);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }

                delta++;
            }

            delta = 1;

            while (position.x - delta >= 0 && position.y + delta < _boardSize.y)
            {
                Vector2Int pos = new Vector2Int(position.x - delta, position.y + delta);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }

                delta++;
            }

            delta = 1;

            while (position.x + delta < _boardSize.x && position.y + delta < _boardSize.y)
            {
                Vector2Int pos = new Vector2Int(position.x + delta, position.y + delta);

                if (StopAddingToList(cellsToMove, pos, team))
                {
                    break;
                }

                delta++;
            }

            return cellsToMove;
        }

        private List<Vector2Int> GetQueenTurns(Vector2Int position, Team team)
        {
            List<Vector2Int> cellsToMove = new List<Vector2Int>();
            cellsToMove = GetRookTurns(position, team);
            cellsToMove.AddRange(GetBishopTurns(position, team));

            return cellsToMove;
        }

        private List<Vector2Int> GetKingTurns(Vector2Int position, Team team)
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
                    || (cellsToMove[i].x == position.x && cellsToMove[i].y == position.y)
                    || (_board.GetFigureByPosition(cellsToMove[i]) != null && _board.GetFigureByPosition(cellsToMove[i]).Team == team))
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

        private bool StopAddingToList(List<Vector2Int> cellsToMove, Vector2Int pos, Team team, bool add = true)
        {
            if (_board.GetFigureByPosition(pos) == null)
            {
                if (add)
                {
                    cellsToMove.Add(pos);
                }

                return false;
            }
            else
            {
                if (_board.GetFigureByPosition(pos).Team != team)
                {
                    if (add)
                    {
                        cellsToMove.Add(pos);
                    }
                }

                return true;
            }
        }
    }
}