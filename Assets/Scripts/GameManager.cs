using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] NotificationPanel notificationPanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        StartCoroutine(TurnManager.Instance.StartGameCo());
    }

    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            TurnManager.OnAddCard?.Invoke(true);

        if (Input.GetKeyUp(KeyCode.Tab))
            TurnManager.OnAddCard?.Invoke(false);

        if (Input.GetKeyUp(KeyCode.LeftShift))
            TurnManager.Instance.EndTurn();
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }
}
