using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

// Подписка на события управления
public class Map : MonoBehaviour
{
    [Serializable] public struct GameFigure
    {
        public GameObject figure;//Фигура
        [Range(0,1)] public float chance;//Вероятность появления от 0 до 1
    }

    [Header("Настройки карты")]
    [SerializeField] public int Length = 10;
    [SerializeField] public int Width  = 10;
    [SerializeField] protected GameFigure[] figures; //Коллекция фигур участвующих в данном режиме
    [NonSerialized] public UnityEvent EndGameAction = new UnityEvent(); //События проигрыша игрока
    
    public Transform[,] map{get; protected set;}

    protected List<GameObject> figureBlocks = new List<GameObject>();

    protected void Awake()
    {
        map = new Transform[Width, Length];
    }

#region Helper

    /// <summary> Функция создания фигуры </summary>
    public virtual bool CreateFigure()
    {
        try
        {
            GameObject figure = Instantiate(SelectionFigure());
            figure.transform.position = new Vector3(Width / 2, Length - 2, 0);
            figure.SetActive(true);

            if (ValidPos(figure.transform, EnumRound.Round))
            {
                return true;
            }
            else
            {
                Destroy(figure);
                return false;
            }
        }
        catch(Exception ex)
        {
            Debug.Log($"Исключение: {ex.Message}");
            return false;
        }
    }

    /// <summary> Метод проверки линий к удалению </summary>
    public virtual void FindLines()
    {
        for (int i = 0; i < Length;)
        {
            if (CheckLine(i))
            {
                DeleteLine(i);
                BlockDown(i);
            }
            else 
            {
                i++;
            }
        }
    }

    /// <summary> Выбор фигуры из коллекции </summary>
    protected virtual GameObject SelectionFigure()
    {
        try
        {
            float num = UnityEngine.Random.Range(0f,1f);
            foreach(GameFigure figure in figures)
            {
                num -= figure.chance;

                if (num <= 0)
                {
                    return figure.figure;
                }
            }
            return figures[0].figure;
        }
        catch(Exception ex)
        {
            Debug.Log($"Исключение: {ex.Message}");
            return figures[0].figure;;
        }
    }

    /// <summary> Передвижения всех блоков вниз начиная с y </summary>
    protected virtual void BlockDown(int y)
    {
        for(int i = y; i < Length - 1; i++)
        {
            for(int j = 0; j < Width; j++)
            {
                if (map[j, i + 1] != null)
                {
                    map[j, i + 1].position += Vector3.down;
                }                
                map[j, i] = map[j, i + 1];
            }
        }
    }

    /// <summary> Проверка линии на заполненность </summary>
    protected virtual bool CheckLine(int num)
    {
        for (int i = 0; i < Width; i++)
        {
            if (map[i,num] == null) return false;
        }
        return true;
    }

    /// <summary> Удаление линии </summary>
    protected virtual void DeleteLine(int num)
    {
        for (int i = 0; i < Width; i++)
        {
            if (map[i,num] != null)
            {
                Destroy(map[i, num].gameObject,0);
            } 
        }
    }

    /// <summary> Проверка нахождения объекта в пределах игрового поля </summary>
    public virtual bool CheakInMap(Vector2 pos) 
    {
        return ((int)pos.x >= 0 && (int)pos.x < Width && (int)pos.y >= 0);
    }

    /// <summary> Округление позиции обектов </summary>
    public virtual Vector2 RoundVector2(Vector2 v, EnumRound round) 
    {
        switch(round)
        {
            case EnumRound.Round:
                return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));

            case EnumRound.Ceil:
                return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));

            default:
                return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }
    }   

    /// <summary> Проверка возможности находится на этой позиции </summary>
    public virtual bool ValidPos(Transform transform, EnumRound round) 
    {  
        foreach (Transform child in transform) 
        {
            Vector2 v = RoundVector2(child.position, round);

            if (!CheakInMap(v)) 
            {
                return false;
            }

            if (map[(int)v.x, (int)v.y] != null) 
            {
                return false;
            }
        }

        CreateFigureObj(transform, round);
        return true;
    }

    /// <summary> Очистка прошлой фигуры </summary>
    protected virtual void ClearFigureBlock()
    {
        foreach(GameObject gameObject in figureBlocks)
        {
            Destroy(gameObject, 0);
        }
        
        figureBlocks.Clear();
    }

    /// <summary> Создание отображения фигуры на поле </summary>
    protected virtual void CreateFigureObj(Transform transform, EnumRound round)
    {
        ClearFigureBlock();
        foreach (Transform child in transform) 
        {
            GameObject block = Instantiate(child.gameObject);
            Vector2 v = RoundVector2(child.position, round);
            block.transform.position = (Vector3)v;
            block.SetActive(true);
            figureBlocks.Add(block);
        }

        foreach(GameObject child in figureBlocks)
        {
            child.transform.SetParent(gameObject.transform);
        }
    }

    /// <summary> Добавления блоков из которых состои фигура в на поле после падения </summary>
    public virtual void AddBlockInMap(Transform transformFigure, EnumRound round) 
    {
        while (transformFigure.childCount > 0) 
        {
            Transform child = transformFigure.GetChild(0);
            Vector2 v = RoundVector2(child.position, round);
            map[(int)v.x, (int)v.y] = child;
            child.SetParent(transform);
            child.transform.position = (Vector3)v;
            child.gameObject.SetActive(true); 
        }

        ClearFigureBlock();
        NextStage();  

    }

#endregion

#region GameStatus

    protected virtual void NextStage()
    {
        FindLines();

        if (!CreateFigure())
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        CreateFigure();
    }

    public virtual void Restart()
    {
        foreach(Transform block in map)
        {
            if (block != null)
            {
                Destroy(block.gameObject, 0f);
            }
        }

        map = new Transform[Width, Length];
        StartGame();
    }

    public virtual void EndGame()
    {
        EndGameAction.Invoke();
    }

#endregion
}
