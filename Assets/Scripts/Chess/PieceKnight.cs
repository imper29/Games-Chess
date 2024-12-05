using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PieceKnight : PieceBase
    {
        public PieceKnight(Team team) : base(team)
        {
        }

        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.knight : ChessSprites.Instance.Black.knight;

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            int offX = Mathf.Abs(fromX - toX);
            int offY = Mathf.Abs(fromY - toY);

            return (offX == 2 && offY == 1) || (offX == 1 && offY == 2);
        }
    }
}

