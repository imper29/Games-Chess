using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chess
{
    [RequireComponent(typeof(Image))]
    public class BoardTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //The currently selected tile.
        private static BoardTile selectedTile;

        //This tile's position.
        private int x, y;
        //The renderer to render the piece on this tile.
        [SerializeField]
        private Image pieceRenderer;
        
        private void Awake()
        {
            //Subscribe to events.
            ChessGame.EventBoardCleared += ChessGame_EventBoardCleared;
            ChessGame.EventBoardLoaded += ChessGame_EventBoardLoaded;
            ChessGame.EventPiecePlaced += ChessGame_EventPiecePlaced;
            ChessGame.EventPieceRemoved += ChessGame_EventPieceRemoved;
            
            //Set the tile position.
            int index = transform.parent.childCount - transform.GetSiblingIndex() - 1;
            x = index % 8;
            y = (index - x) / 8;
            //Set the tile background color.
            GetComponent<Image>().sprite = (x + y) % 2 == 0 ? ChessSprites.Instance.WhiteTile : ChessSprites.Instance.BlackTile;
            pieceRenderer.enabled = false;
        }
        private void OnDestroy()
        {
            //Unsubscribe from events.
            ChessGame.EventBoardCleared -= ChessGame_EventBoardCleared;
            ChessGame.EventBoardLoaded -= ChessGame_EventBoardLoaded;
            ChessGame.EventPiecePlaced -= ChessGame_EventPiecePlaced;
            ChessGame.EventPieceRemoved -= ChessGame_EventPieceRemoved;
        }

        private void ChessGame_EventBoardCleared()
        {
            ChessGame_EventPieceRemoved(null, x, y);
        }
        private void ChessGame_EventBoardLoaded()
        {
            ChessGame_EventPiecePlaced(ChessGame.GetPiece(x, y), x, y);
        }
        private void ChessGame_EventPiecePlaced(PieceBase piece, int x, int y)
        {
            if (x == this.x && y == this.y && piece != null)
            {
                pieceRenderer.enabled = true;
                pieceRenderer.sprite = piece.Sprite;
            }
        }
        private void ChessGame_EventPieceRemoved(PieceBase piece, int x, int y)
        {
            if (x == this.x && y == this.y)
                pieceRenderer.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Is the piece selectable?
            PieceBase piece = ChessGame.GetPiece(x, y);
            if (piece != null)
            {
                if (piece.team == ChessGame.CurrentTeam)
                {
                    //Select this tile.
                    selectedTile = this;
                    //Make the image renderer render after all the other tiles because we are dragging the renderer over those tiles.
                    pieceRenderer.transform.SetParent(transform.parent.parent);
                    pieceRenderer.transform.SetAsLastSibling();
                    pieceRenderer.transform.position = eventData.position;
                }
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (selectedTile == this)
            {
                //Drag the renderer around.
                pieceRenderer.transform.position = eventData.position;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (selectedTile == this)
            {
                Vector3 releasePosition = eventData.pointerPressRaycast.worldPosition;
                GameObject releasedObject = eventData.pointerCurrentRaycast.gameObject;
                if (releasedObject != null)
                {
                    //Release tile is the tile we released the mouse button over.
                    BoardTile releasedTile = releasedObject.GetComponent<BoardTile>();
                    if (releasedTile)
                    {
                        //Try to move the piece from this tile to the released tile.
                        ChessGame.TryMovePiece(x, y, releasedTile.x, releasedTile.y);
                    }
                }
                //Make the renderer render the image over this tile now. Stop dragging the renderer.
                pieceRenderer.transform.SetParent(transform);
                pieceRenderer.transform.localPosition = Vector3.zero;
            }
        }
    }
}
