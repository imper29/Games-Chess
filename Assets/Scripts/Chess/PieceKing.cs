using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public class PieceKing : PieceBase
    {
        public PieceKing(Team team) : base(team)
        {
            moved = false;
        }

        public override Sprite Sprite => team == Team.White ? ChessSprites.Instance.White.king : ChessSprites.Instance.Black.king;
        private bool moved;

        public override void OnMoved(int fromX, int fromY, int toX, int toY)
        {
            moved = true;
        }
        public override void OnDestroyed(int x, int y)
        {
            SceneHandler.LoadMainMenu();
        }

        public override bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            int offX = Mathf.Abs(fromX - toX);
            int offY = Mathf.Abs(fromY - toY);
            
            return offX <= 1 && offY <= 1;
        }

        public bool IsChecked(int x, int y)
        {
            //Go over all the pieces.
            for (int xp = 0; xp < 8; xp++)
            {
                for (int yp = 0; yp < 8; yp++)
                {
                    //Check if the piece can move to the king and it is an enemy.
                    PieceBase piece = ChessGame.GetPiece(xp, yp);
                    if (piece != null && piece.team != team && piece.CanMove(xp, yp, x, y))
                        return true;
                }
            }
            return false;
        }
    }
}
