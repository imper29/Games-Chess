using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

namespace Chess
{
    public class ChessGame: MonoBehaviour
    {
        private static ChessGame Instance { get; set; }
        /// <summary>
        /// The name of the current game.
        /// </summary>
        public static string currentGameName;
        /// <summary>
        /// The team of the current player's move.
        /// </summary>
        private static Team currentTeam;
        public static Team CurrentTeam
        {
            get
            {
                return currentTeam;
            }
            set
            {
                currentTeam = value;
                Instance.currentTeamDisplay.text = "<u>Current Move</u>\n" + currentTeam.ToString();
                Instance.winnerTeamDisplay.text = currentTeam.ToString() + " Wins!";
            }
        }
        
        /// <summary>
        /// Called when the board is cleared.
        /// </summary>
        public static event Action EventBoardCleared;
        /// <summary>
        /// Called when the board is loaded.
        /// </summary>
        public static event Action EventBoardLoaded;
        /// <summary>
        /// Called when a piece is placed on a position.
        /// </summary>
        public static event Action<PieceBase, int, int> EventPiecePlaced;
        /// <summary>
        /// Called when a piece is removed from a position.
        /// </summary>
        public static event Action<PieceBase, int, int> EventPieceRemoved;

        /// <summary>
        /// The pieces on the board.
        /// </summary>
        private static PieceBase[,] pieces = new PieceBase[8, 8];


        [SerializeField]
        private GameObject board;
        [SerializeField]
        private GameObject winnerScreen;
        [SerializeField]
        private TextMeshProUGUI currentTeamDisplay, winnerTeamDisplay;



        private void Start()
        {
            Instance = this;
            //Set to white's turn first.
            CurrentTeam = Team.White;
            //The current game needs to be set before this script starts.
            if (currentGameName == null)
                SceneHandler.LoadMainMenu();
            else if (!LoadGame())
                StartNewGame();
        }
        /// <summary>
        /// Clears the board and sets it up with the starting state for chess.
        /// </summary>
        public static void StartNewGame()
        {
            pieces = new PieceBase[8, 8];
            EventBoardCleared?.Invoke();

            //Setup pawns
            for (int i = 0; i < 8; i++)
            {
                PlacePiece(new PiecePawn(Team.White), i, 1);
                PlacePiece(new PiecePawn(Team.Black), i, 6);
            }
            //Setup kings
            PlacePiece(new PieceKing(Team.White), 3, 0);
            PlacePiece(new PieceKing(Team.Black), 3, 7);
            //Setup queens
            PlacePiece(new PieceQueen(Team.White), 4, 0);
            PlacePiece(new PieceQueen(Team.Black), 4, 7);
            //Setup bishops
            PlacePiece(new PieceBishop(Team.White), 2, 0);
            PlacePiece(new PieceBishop(Team.White), 5, 0);
            PlacePiece(new PieceBishop(Team.Black), 2, 7);
            PlacePiece(new PieceBishop(Team.Black), 5, 7);
            //Setup knights
            PlacePiece(new PieceKnight(Team.White), 1, 0);
            PlacePiece(new PieceKnight(Team.White), 6, 0);
            PlacePiece(new PieceKnight(Team.Black), 1, 7);
            PlacePiece(new PieceKnight(Team.Black), 6, 7);
            //Setup rooks
            PlacePiece(new PieceRook(Team.White), 0, 0);
            PlacePiece(new PieceRook(Team.White), 7, 0);
            PlacePiece(new PieceRook(Team.Black), 0, 7);
            PlacePiece(new PieceRook(Team.Black), 7, 7);
        }

        /// <summary>
        /// Places a piece on a position.
        /// </summary>
        /// <param name="piece">The piece to place.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="doEvents">True if the events should be called.</param>
        public static void PlacePiece(PieceBase piece, int x, int y, bool doEvents = true)
        {
            //Destroy the piece at the position if one exists.
            if (doEvents)
                DestroyPiece(x, y);
            else
                RemovePiece(x, y, false);

            //Place the new piece.
            pieces[x, y] = piece;
            if (doEvents)
                EventPiecePlaced?.Invoke(piece, x, y);
        }
        /// <summary>
        /// Removes a piece at a position if one exists at that position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="doEvents">True if the events should be called.</param>
        private static void RemovePiece(int x, int y, bool doEvents = true)
        {
            if (pieces[x, y] != null)
            {
                if (doEvents)
                    EventPieceRemoved?.Invoke(pieces[x, y], x, y);
                pieces[x, y] = null;
            }
        }
        /// <summary>
        /// Destroys a piece at a position if one exists at that position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        private static void DestroyPiece(int x, int y)
        {
            if (pieces[x, y] != null)
            {
                EventPieceRemoved?.Invoke(pieces[x, y], x, y);
                pieces[x, y].OnDestroyed(x, y);
                pieces[x, y] = null;
            }
        }
        /// <summary>
        /// Gets the piece at a position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>The piece at a position. Null if none exists at that position.</returns>
        public static PieceBase GetPiece(int x, int y)
        {
            return pieces[x, y];
        }
        /// <summary>
        /// Tries to move a piece from one position to another position.
        /// </summary>
        /// <param name="fromX">From positoin x.</param>
        /// <param name="fromY">From position y.</param>
        /// <param name="toX">To position x.</param>
        /// <param name="toY">To position y.</param>
        /// <returns>True if the piece was moved.</returns>
        public static bool TryMovePiece(int fromX, int fromY, int toX, int toY)
        {
            //No killing own peaces.
            PieceBase targPiece = GetPiece(toX, toY);
            if (targPiece != null && targPiece.team == currentTeam)
                return false;

            //Don't move if no piece exists or piece is wrong team.
            PieceBase piece = GetPiece(fromX, fromY);
            if (piece != null)
            {
                if (piece.team == CurrentTeam && piece.CanMove(fromX, fromY, toX, toY))
                {
                    //Move the piece and check for self-checking.
                    PlacePiece(piece, toX, toY, false);
                    RemovePiece(fromX, fromY, false);
                    if (GetKing(currentTeam, out int kingX, out int kingY).IsChecked(kingX, kingY))
                    {
                        //Undo the move because the king is in check.
                        PlacePiece(piece, fromX, fromY, false);
                        PlacePiece(targPiece, toX, toY, false);
                        return false;
                    }
                    else
                    {
                        //Undo the move and redo it with events.
                        PlacePiece(piece, fromX, fromY, false);
                        PlacePiece(targPiece, toX, toY, false);

                        //Actually move the piece.
                        piece.OnMoved(fromX, fromY, toX, toY);
                        PlacePiece(piece, toX, toY);
                        RemovePiece(fromX, fromY);

                        //Switch the current team becase the player made a move.
                        CurrentTeam = currentTeam == Team.White ? Team.Black : Team.White;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a team's king.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>The team's king.</returns>
        /// <param name="x">The position of the king.</param>
        /// <param name="y">The position of the king.</param>
        private static PieceKing GetKing(Team team, out int x, out int y)
        {
            for (int xp = 0; xp < 8; xp++)
            {
                for (int yp = 0; yp < 8; yp++)
                {
                    PieceBase piece = GetPiece(xp, yp);
                    if (piece != null)
                        if (piece is PieceKing && piece.team == team)
                        {
                            x = xp;
                            y = yp;
                            return piece as PieceKing;
                        }
                }
            }
            //This should never happen.
            x = -1;
            y = -1;
            return null;
        }



        /// <summary>
        /// Gets the directory that holds all the game saves.
        /// </summary>
        /// <returns>The directory that holds all the game saves.</returns>
        public static DirectoryInfo GetSaveDirectory()
        {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Saves");
            if (!dir.Exists)
                dir.Create();
            return dir;
        }
        /// <summary>
        /// Gets the file info for the save file.
        /// </summary>
        /// <param name="saveName">The name of the save.</param>
        /// <returns>The file info for the save file.</returns>
        public static FileInfo GetSaveFile(string saveName)
        {
            return new FileInfo(string.Format("{0}/{1}.xml", GetSaveDirectory().FullName, saveName));
        }
        /// <summary>
        /// Gets the file info for the save file.
        /// </summary>
        /// <returns>The file info for the save file.</returns>
        private static FileInfo GetCurrentSaveFile()
        {
            return new FileInfo(string.Format("{0}/{1}.xml", GetSaveDirectory().FullName, currentGameName));
        }
        /// <summary>
        /// Saves the game.
        /// </summary>
        public void SaveGame()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = GetCurrentSaveFile().Create();
            formatter.Serialize(stream, new GameSaveData(currentTeam, pieces));
            stream.Close();
        }
        /// <summary>
        /// Loads the game.
        /// </summary>
        /// <returns>True if the game was loaded or it returned to the main menu. False if not.</returns>
        public bool LoadGame()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileInfo saveFile = GetCurrentSaveFile();
            if (saveFile.Exists)
            {
                Stream stream = saveFile.OpenRead();
                try
                {
                    GameSaveData save = (GameSaveData)formatter.Deserialize(stream);
                    CurrentTeam = save.currentTeam;
                    pieces = save.pieces;
                    EventBoardLoaded?.Invoke();
                    stream.Close();
                    return true;
                }
                catch (InvalidCastException exception)
                {
                    //The file is the wrong data type and can't be deserialized. Log error and return to menu.
                    Debug.LogException(exception);
                    SceneHandler.LoadMainMenu();
                    stream.Close();
                    return true;
                }
            }
            //File doesn't exist.
            return false;
        }

        /// <summary>
        /// Saves and exits the game.
        /// </summary>
        /// <param name="deleteSave">True if the save file should be deleted.</param>
        public void LoadMainMenu()
        {
            SceneHandler.LoadMainMenu();
        }
        /// <summary>
        /// Declares the current team as the looser and ends the game.
        /// </summary>
        public void EndGame()
        {
            //Switch to the winner.
            CurrentTeam = CurrentTeam == Team.White ? Team.Black : Team.White;
            //Display winner screen.
            board.SetActive(false);
            winnerScreen.SetActive(true);
            //Destroy save file if exists.
            FileInfo saveFile = GetCurrentSaveFile();
            if (saveFile.Exists)
                saveFile.Delete();
        }

        /// <summary>
        /// A struct to hold a game save.
        /// </summary>
        [Serializable]
        private struct GameSaveData
        {
            public Team currentTeam;
            public PieceBase[,] pieces;

            public GameSaveData(Team currentTeam, PieceBase[,] pieces)
            {
                this.currentTeam = currentTeam;
                this.pieces = pieces;
            }
        }
    }
}
