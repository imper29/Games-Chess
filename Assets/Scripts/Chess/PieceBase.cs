using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public abstract class PieceBase
    {
        public readonly Team team;
        public abstract Sprite Sprite
        {
            get;
        }

        public PieceBase(Team team)
        {
            this.team = team;
        }

        /// <summary>
        /// Called when moved.
        /// </summary>
        /// <param name="fromX">From position x.</param>
        /// <param name="fromY">From position y.</param>
        /// <param name="toX">To position x.</param>
        /// <param name="toY">To position y.</param>
        public virtual void OnMoved(int fromX, int fromY, int toX, int toY)
        {

        }
        /// <summary>
        /// Called when the piece is destroyed.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void OnDestroyed(int x, int y)
        {

        }

        /// <summary>
        /// Determines if the piece can move from one spot to another.
        /// </summary>
        /// <param name="fromX">From position x.</param>
        /// <param name="fromY">From position y.</param>
        /// <param name="toX">To position x.</param>
        /// <param name="toY">To position y.</param>
        /// <returns>True if it can move. False if not.</returns>
        public abstract bool CanMove(int fromX, int fromY, int x, int y);
    }
    [System.Serializable]
    public enum Team
    {
        White,
        Black
    }
}

