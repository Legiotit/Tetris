using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> Скрипт за фигуры и их передвижение и вращение </summary>
public class Figure : MonoBehaviour
{
    /// <summary> Время через которое фигура передвинется ниже, скорость падения фигуры </summary>
    const float Time = 1;
    protected bool move = true;
    Map map;

    /// <summary> Тип округления для объекта </summary>
    [SerializeField]EnumRound round;

    /// <summary> Цвет в который перекрасится фигура после падения </summary>
    [SerializeField] Color color;

    protected void Start()
    {
        if (GameObject.FindGameObjectWithTag("Map") != null && 
            GameObject.FindGameObjectWithTag("Map").GetComponent<Map>() != null)
        {
            map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
            StartCoroutine(BasicMoveDown());
        }
        else
        {
            gameObject.SetActive(false);
        }

        // Подписка на события управления
        InputController.MoveRight.AddListener(MoveRight);
        InputController.MoveLeft.AddListener(MoveLeft);
        InputController.MoveDown.AddListener(MoveDown);
        InputController.RotateRight.AddListener(RotateRight);
        InputController.RotateLeft.AddListener(RotateLeft);
    }

    protected void OnDestroy()
    {
        InputController.MoveRight.RemoveListener(MoveRight);
        InputController.MoveLeft.RemoveListener(MoveLeft);
        InputController.MoveDown.RemoveListener(MoveDown);
        InputController.RotateRight.RemoveListener(RotateRight);
        InputController.RotateLeft.RemoveListener(RotateLeft);
    }

#region Control
    private void MoveRight()
    {
        Move((Vector3.right));
    }

    private void MoveLeft()
    {
        Move((Vector3.left));
    }

    private void MoveDown()
    {
        Move((Vector3.down));
    }

    private void RotateRight()
    {
        RotateFigure(90);
    }

    private void RotateLeft()
    {
        RotateFigure(-90);
    }
#endregion

    /// <summary> Постепенное передвижение фигуры вниз </summary>
    private IEnumerator BasicMoveDown()
    {
        while(true)
        {
            if (!Move(Vector3.down))
            {
                move = false;
                ChangeColorBlocks();
                map.AddBlockInMap(transform, round);
                Destroy(gameObject,0);
                break;
            }
            yield return new WaitForSeconds(Time);
        }
    }

    /// <summary> Метод смены цвета </summary>
    private void ChangeColorBlocks()
    {
        foreach(Transform transform in transform)
        {
            transform.GetComponent<SpriteRenderer>().color = color;
        } 
    }

    /// <summary> Функция поворота фигуры </summary>
    private void RotateFigure(float angle)
    {
        if (move) 
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle);

            if (!map.ValidPos(transform, round))
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - angle);
            }
        }
    }

    /// <summary> Функция движения фигуры </summary>
    private bool Move(Vector3 vector)
    {
        transform.position += vector;

        if (!map.ValidPos(transform, round) || !move)
        {
            transform.position -= vector;
            return false;
        }
        return true;
    }
}