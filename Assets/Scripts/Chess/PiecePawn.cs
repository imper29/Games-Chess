using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PiecePawn : PieceBase
    {
        public PiecePawn(Team team) : base(team)
        {
            moved = false;
        }

        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.pawn : ChessSprites.Instance.Black.pawn;
        private bool moved;

        public override void OnMoved(int fromX, int fromY, int toX, int toY)
        {
            moved = true;
        }

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            int xOff = Mathf.Abs(toX - fromX);
            if (xOff == 0)
            {
                int yOff = Mathf.Abs(toY - fromY);
                PieceBase piece = ChessGame.GetPiece(toX, toY);
                if (piece != null)
                    return false;

                if (moved)
                    return fromY + Forward() == toY;

                int f = yOff * Forward();
                if (f == Forward())
                    return ChessGame.GetPiece(toX, toY) == null;
                else if (f == Forward() * 2)
                    return ChessGame.GetPiece(toX, toY) == null && ChessGame.GetPiece(toX, toY - Forward()) == null;
            }
            else if (xOff == 1 && toY == fromY + Forward())
            {
                return ChessGame.GetPiece(toX, toY) != null;
            }
            return false;
        }

        public int Forward()
        {
            if (team == Team.White)
                return 1;
            return -1;
        }
    }
}

