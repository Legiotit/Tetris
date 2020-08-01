using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary> Скрипт отвечающий за отображения и взаимодействие с меню </summary>
public class Menu : MonoBehaviour
{

    [Serializable]public struct GameMode
    {
        /// <summary> Префаб реализующий режим игры </summary>
        public GameObject Game;

        /// <summary>Экран с меню после проигрыша игрока </summary>
        public GameObject RetryMenu;

        /// <summary> Экран с меню во время игры </summary>
        public GameObject GameMenu;
    }

    /// <summary> Экран главного меню с режимами игры </summary>
    [SerializeField]private GameObject mainMenu;
    private GameObject Game;
    [SerializeField]private GameMode[] GameModes;//Режимы игры
    private GameMode mode;

    //Метод запуска игры
    public void StartGame(int numGame)
    {   
        mode = GameModes[numGame];
        Game = Instantiate(mode.Game);
        Game.GetComponent<Map>().StartGame();
        Game.GetComponent<Map>().EndGameAction.AddListener(EndGame);
        if (mode.GameMenu != null)
        {
            mode.GameMenu.SetActive(true);
        }
        mainMenu.SetActive(false);
    }

    //Метод окончания текушей попытки
    public void EndGame()
    {
        if (mode.GameMenu != null)
        {
            mode.GameMenu.SetActive(false);
        }
        if (mode.RetryMenu != null)
        {
            mode.RetryMenu.SetActive(true);
        }      
    }

    //Метод возврата на главный экран
    public void BackToMenu()
    {
        if (mode.RetryMenu != null)
        {
            mode.RetryMenu.SetActive(false);
        }
        mainMenu.SetActive(true);    
        Game.GetComponent<Map>().EndGameAction.RemoveListener(EndGame);     
        Destroy(Game);
    }

    //Метод повтора текушей игры
    public void Retry()
    {
        if (mode.RetryMenu != null)
        {
            mode.RetryMenu.SetActive(false);
        }
        if (mode.GameMenu != null)
        {
            mode.GameMenu.SetActive(true);
        }
        Game.GetComponent<Map>().Restart();        
    }
}
