using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PieceQueen : PieceBase
    {
        public PieceQueen(Team team) : base(team)
        {
        }

        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.queen : ChessSprites.Instance.Black.queen;

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            return PieceBishop.BishopMove(fromX, fromY, toX, toY) || PieceRook.RookMove(fromX, fromY, toX, toY);
        }
    }
}
