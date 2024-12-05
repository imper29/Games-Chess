using UnityEngine;

namespace Chess
{
    [CreateAssetMenu]
    /// <summary>
    /// The sprites for the game.
    /// </summary>
    public class ChessSprites : ScriptableObject
    {
        private static ChessSprites instance;
        public static ChessSprites Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<ChessSprites>("Sprites");
                return instance;
            }
        }

        [SerializeField]
        private Sprite whiteTile, blackTile;
        public Sprite WhiteTile
        {
            get
            {
                return whiteTile;
            }
        }
        public Sprite BlackTile
        {
            get
            {
                return blackTile;
            }
        }
        [SerializeField]
        private TeamSprites white, black;
        public TeamSprites White
        {
            get
            {
                return white;
            }
        }
        public TeamSprites Black
        {
            get
            {
                return black;
            }
        }

        [System.Serializable]
        public struct TeamSprites
        {
            public Sprite king, queen, bishop, knight, rook, pawn;
        }
    }
}
