using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PieceBishop : PieceBase
    {
        public PieceBishop(Team team) : base(team)
        {
        }

        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.bishop : ChessSprites.Instance.Black.bishop;

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            return BishopMove(fromX, fromY, toX, toY);
        }

        public static bool BishopMove(int fromX, int fromY, int toX, int toY)
        {
            int offX = fromX - toX;
            int offY = fromY - toY;

            if (Mathf.Abs(offX) == Mathf.Abs(offY))
            {
                int max = Mathf.Abs(offX);
                int deltaX = -offX / max;
                int deltaY = -offY / max;

                for (int i = 1; i < max; i++)
                {
                    PieceBase piece = ChessGame.GetPiece(fromX + i * deltaX, fromY + i * deltaY);
                    if (piece != null)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}

