using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    [Header("Develop")]
    [SerializeField][Tooltip("시작 턴 모드")] ETurnMode eTurnMode;
    [SerializeField][Tooltip("고속 카드 배분")] bool fastMode;
    [SerializeField][Tooltip("시작 카드 개수")] int startCardCnt;

    [Header("Properties")]
    public bool myTurn;
    public bool isLoading;

    enum ETurnMode
    {
        Random,
        My,
        Enemy
    }

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;

    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Enemy:
                myTurn = false;
                break;

        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;

        for (int i = 0; i < startCardCnt; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(false);
            yield return delay05;
            OnAddCard?.Invoke(true);
        }

        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

        if (myTurn)
            GameManager.Instance.Notification("나의 턴");

        yield return delay07;
        OnAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }
}
