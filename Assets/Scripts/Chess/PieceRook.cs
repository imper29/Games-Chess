using System;
using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PieceRook : PieceBase
    {
        public PieceRook(Team team) : base(team)
        {
            moved = false;
        }

        private bool moved;
        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.rook : ChessSprites.Instance.Black.rook;

        public override void OnMoved(int fromX, int fromY, int toX, int toY)
        {
            moved = true;
        }

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            return RookMove(fromX, fromY, toX, toY);
        }

        public static bool RookMove(int fromX, int fromY, int toX, int toY)
        {
            int offX = Mathf.Abs(fromX - toX);
            int offY = Mathf.Abs(fromY - toY);

            PieceBase piece;

            if (offX == 0)
            {
                int delta = fromY < toY ? 1 : -1;
                for (int y = fromY + delta; y != toY; y += delta)
                {
                    piece = ChessGame.GetPiece(fromX, y);
                    if (piece != null)
                        return false;
                }
                return true;
            }
            else if (offY == 0)
            {
                int delta = fromX < toX ? 1 : -1;
                for (int x = fromX + delta; x != toX; x += delta)
                {
                    piece = ChessGame.GetPiece(x, fromY);
                    if (piece != null)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}

