using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FigureMovement
    {
        private Board _board;

        public FigureMovement(Board board)
        {
            _board = board;
        }

        public List<Vector2Int> GetTurns(Figure figure, Vector2Int position)
        {
            List<Vector2Int> turns = new List<Vector2Int>();

            switch (figure.Type)
            {
                case FigureType.Checker:
                    return GetCheckerTurns(figure, position);

                case FigureType.King:
                    return GetKingTurns(figure, position);

                case FigureType.None:
                default:
                    Debug.LogError("Unknown or None type of figure!");
                    break;
            }

            return turns;
        }

        private List<Vector2Int> GetCheckerTurns(Figure figure, Vector2Int position)
        {
            List<Vector2Int> turns = new List<Vector2Int>();
            Vector2Int direction;

            if (figure.Team == Team.Black)
            {
                // Äâèæåíèå ÂÍÈÇ-ÂËÅÂÎ
                direction = new Vector2Int(+1, -1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, false));

                // Äâèæåíèå ÂÍÈÇ-ÂÏĞÀÂÎ
                direction = new Vector2Int(+1, +1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, false));

                // Äâèæåíèå ÂÂÅĞÕ-ÂËÅÂÎ
                direction = new Vector2Int(-1, -1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, true));

                // Äâèæåíèå ÂÂÅĞÕ-ÂÏĞÀÂÎ
                direction = new Vector2Int(-1, +1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, true));
            }
            else if (figure.Team == Team.White)
            {
                // Äâèæåíèå ÂÍÈÇ-ÂËÅÂÎ
                direction = new Vector2Int(+1, -1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, true));

                // Äâèæåíèå ÂÍÈÇ-ÂÏĞÀÂÎ
                direction = new Vector2Int(+1, +1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, true));

                // Äâèæåíèå ÂÂÅĞÕ-ÂËÅÂÎ
                direction = new Vector2Int(-1, -1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, false));

                // Äâèæåíèå ÂÂÅĞÕ-ÂÏĞÀÂÎ
                direction = new Vector2Int(-1, +1);
                turns.AddRange(GetCheckerTurnsByDirection(figure, position, direction, false));
            }

            return turns;
        }

        private List<Vector2Int> GetKingTurns(Figure figure, Vector2Int position)
        {
            List<Vector2Int> turns = new List<Vector2Int>();
            Vector2Int direction;

            // Äâèæåíèå ÂÍÈÇ-ÂËÅÂÎ
            direction = new Vector2Int(+1, -1);
            turns.AddRange(GetKingTurnsByDirection(figure, position, direction));

            // Äâèæåíèå ÂÍÈÇ-ÂÏĞÀÂÎ
            direction = new Vector2Int(+1, +1);
            turns.AddRange(GetKingTurnsByDirection(figure, position, direction));

            // Äâèæåíèå ÂÂÅĞÕ-ÂËÅÂÎ
            direction = new Vector2Int(-1, -1);
            turns.AddRange(GetKingTurnsByDirection(figure, position, direction));

            // Äâèæåíèå ÂÂÅĞÕ-ÂÏĞÀÂÎ
            direction = new Vector2Int(-1, +1);
            turns.AddRange(GetKingTurnsByDirection(figure, position, direction));

            return turns;
        }

        private List<Vector2Int> GetCheckerTurnsByDirection(Figure figure, Vector2Int position, Vector2Int direction, bool isOpposite)
        {
            List<Vector2Int> turns = new List<Vector2Int>();
            Vector2Int newPosition = position;

            newPosition += direction;

            if (_board.IsPositionCorrect(newPosition))
            {
                Figure figureOnNewPosition = _board.GetFigureByPosition(newPosition);

                if (isOpposite)
                {
                    if (figureOnNewPosition != null && figureOnNewPosition.Team != figure.Team)
                    {
                        newPosition += direction;

                        if (_board.IsPositionCorrect(newPosition) && _board.GetFigureByPosition(newPosition) == null)
                        {
                            turns.Add(newPosition);
                        }
                    }
                }
                else
                {
                    if (figureOnNewPosition == null)
                    {
                        turns.Add(newPosition);
                    }
                    else if (figureOnNewPosition.Team != figure.Team)
                    {
                        newPosition += direction;

                        if (_board.IsPositionCorrect(newPosition) && _board.GetFigureByPosition(newPosition) == null)
                        {
                            turns.Add(newPosition);
                        }
                    }
                }
            }

            return turns;
        }

        private List<Vector2Int> GetKingTurnsByDirection(Figure figure, Vector2Int position, Vector2Int direction)
        {
            List<Vector2Int> turns = new List<Vector2Int>();
            Vector2Int newPosition = position;
            newPosition += direction;

            while (_board.IsPositionCorrect(newPosition) && _board.GetFigureByPosition(newPosition) == null)
            {
                turns.Add(newPosition);
                newPosition += direction;
            }

            if (_board.IsPositionCorrect(newPosition))
            {
                if (_board.GetFigureByPosition(newPosition).Team != figure.Team)
                {
                    newPosition += direction;

                    if (_board.IsPositionCorrect(newPosition) && _board.GetFigureByPosition(newPosition) == null)
                    {
                        turns.Add(newPosition);
                    }
                }
            }

            return turns;
        }
    }
}