using UnityEngine;
using UnityEngine.UI;

namespace Chess
{
    public class PawnPromotionHandler : MonoBehaviour
    {
        /// <summary>
        /// When this object is active, you cannot interact with the board.
        /// </summary>
        [SerializeField]
        private GameObject boardRaycastStopper;
        /// <summary>
        /// The menu to display when promoting a pawn.
        /// </summary>
        [SerializeField]
        private GameObject pawnPromotionMenu;
        /// <summary>
        /// The button to save the game. Disabled when showing the pawn promotion menu.
        /// </summary>
        [SerializeField]
        private Button saveGameButton;

        /// <summary>
        /// The position of the pawn to promote.
        /// </summary>
        private int targetPawnX, targetPawnY;

        private void Awake()
        {
            pawnPromotionMenu.SetActive(false);
            boardRaycastStopper.SetActive(false);
            ChessGame.EventPiecePlaced += ChessGame_EventPiecePlaced;
        }
        private void OnDestroy()
        {
            ChessGame.EventPiecePlaced -= ChessGame_EventPiecePlaced;
        }

        private void ChessGame_EventPiecePlaced(PieceBase piece, int x, int y)
        {
            if (piece is PiecePawn)
            {
                PiecePawn pawn = piece as PiecePawn;
                //Get the y index for a pawn of this team at the end.
                int endPoint = Mathf.Clamp(pawn.Forward() * 4 + 4, 0, 7);
                if (y == endPoint)
                {
                    //Disable the save button.
                    saveGameButton.interactable = false;

                    //Enable the pawn promotion menu.
                    pawnPromotionMenu.SetActive(true);
                    boardRaycastStopper.SetActive(true);

                    //Set the target pawn position.
                    targetPawnX = x;
                    targetPawnY = y;

                    //Switch the team back because it gets switched imediately after the piece moves.
                    ChessGame.CurrentTeam = ChessGame.CurrentTeam == Team.White ? Team.Black : Team.White;
                }
            }
        }

        /// <summary>
        /// Promotes the target pawn to the promotion piece.
        /// </summary>
        /// <param name="promotion">The piece to replace the pawn.</param>
        private void PromotePawn(PieceBase promotion)
        {
            //Enable save button.
            saveGameButton.interactable = true;
            //Disable pawn promotion menu.
            pawnPromotionMenu.SetActive(false);
            boardRaycastStopper.SetActive(false);

            //Promote the pawn to the promotion.
            ChessGame.PlacePiece(promotion, targetPawnX, targetPawnY);

            //End the turn.
            ChessGame.CurrentTeam = ChessGame.CurrentTeam == Team.White ? Team.Black : Team.White;
        }
        /// <summary>
        /// Promotes the target pawn to a queen.
        /// </summary>
        public void PromoteToQueen()
        {
            PromotePawn(new PieceQueen(ChessGame.CurrentTeam));
        }
        /// <summary>
        /// Promotes the target pawn to a bishop.
        /// </summary>
        public void PromoteToBishop()
        {
            PromotePawn(new PieceBishop(ChessGame.CurrentTeam));
        }
        /// <summary>
        /// Promotes the target pawn to a knight.
        /// </summary>
        public void PromoteToKnight()
        {
            PromotePawn(new PieceKnight(ChessGame.CurrentTeam));
        }
        /// <summary>
        /// Promotes the target pawn to a rook
        /// </summary>
        public void PromoteToRook()
        {
            PromotePawn(new PieceRook(ChessGame.CurrentTeam));
        }
    }
}
