using Chess;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform buttonList;
    [SerializeField]
    private Button buttonPrefab;

    private void Awake()
    {
        string[] games = SceneHandler.ListGames();
        for (int i = 0; i < games.Length; i++)
        {
            //Need to cache the value because in the delegate I can't use i because i changes.
            string gameName = games[i].Substring(0, games[i].Length - 4);

            Button b = Instantiate(buttonPrefab, buttonList);
            b.gameObject.SetActive(true);
            b.GetComponentInChildren<Text>().text = gameName;
            b.onClick.AddListener(() => SceneHandler.LoadGame(gameName));
        }
    }
}
