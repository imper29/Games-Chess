using Chess;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    [SerializeField]
    private InputField inputSaveName;
    [SerializeField]
    private Button createSaveButton;

    private void Awake()
    {
        createSaveButton.onClick.AddListener(StartGame);
        inputSaveName.onValueChanged.AddListener(OnNameChanged);
        OnNameChanged(inputSaveName.text);
    }

    /// <summary>
    /// Called when the createSaveButton is pressed.
    /// </summary>
    public void StartGame()
    {
        SceneHandler.LoadGame(inputSaveName.text);
    }
    /// <summary>
    /// Called when the name input field text changes.
    /// </summary>
    /// <param name="name">The input field text.</param>
    public void OnNameChanged(string name)
    {
        createSaveButton.interactable = !ChessGame.GetSaveFile(name).Exists && !string.IsNullOrWhiteSpace(name);
    }
}
