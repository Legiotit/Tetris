using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary> Скрипт отвечающий за упраление фигурами </summary>
public class InputController : MonoBehaviour
{
    
    [NonSerialized]public static UnityEvent MoveRight = new UnityEvent();
    [NonSerialized]public static UnityEvent MoveLeft = new UnityEvent();
    [NonSerialized]public static UnityEvent MoveDown = new UnityEvent();
    [NonSerialized]public static UnityEvent RotateRight = new UnityEvent();
    [NonSerialized]public static UnityEvent RotateLeft = new UnityEvent();

    [Header("Настройки кнопок управления")]
    [SerializeField] private string keyMoveRight   = "d";
    [SerializeField] private string keyMoveLeft    = "a";
    [SerializeField] private string keyMoveDown    = "s";
    [SerializeField] private string keyRotateRight = "e";
    [SerializeField] private string keyRotateLeft  = "q";

    void Update()
    {
        if (Input.GetKeyDown(keyMoveRight)) 
        {
            MoveRight.Invoke();
        }

        if (Input.GetKeyDown(keyMoveLeft)) 
        {
            MoveLeft.Invoke();
        }

        if (Input.GetKeyDown(keyMoveDown))
        {
            MoveDown.Invoke();
        }
        
        if (Input.GetKeyDown(keyRotateRight))
        {
            RotateRight.Invoke();
        }

        if (Input.GetKeyDown(keyRotateLeft))
        {
            RotateLeft.Invoke();
        }
    }
}
