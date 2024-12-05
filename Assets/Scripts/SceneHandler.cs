using Chess;
using System.IO;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static string[] ListGames()
    {
        DirectoryInfo dir = ChessGame.GetSaveDirectory();
        FileInfo[] files = dir.GetFiles();
        string[] games = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
            games[i] = files[i].Name;
        return games;
    }
    public static void LoadGame(string gameName)
    {
        ChessGame.currentGameName = gameName;
        SceneManager.LoadScene("Play");
    }
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ChessGame.currentGameName = null;
    }
}
