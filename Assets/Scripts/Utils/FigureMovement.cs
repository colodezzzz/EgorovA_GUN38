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

        public List<Vector2Int> GetTurns(Team team, Vector2Int figurePosition)
        {
            List<Vector2Int> turns = new List<Vector2Int>();

            return turns;
        }

    }
}