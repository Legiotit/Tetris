using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNoWalls : Map
{
    new void Awake()
    {
        base.Awake();
    }

    /// <summary> Переопределения поиска заполненных линий на наличие двух заполненных линий</summary>
    public override void FindLines()
    {
        for (int i = 0; i < Length - 1;)
        {
            if (CheckLine(i) && CheckLine(i + 1))
            {
                DeleteLine(i);
                BlockDown(i);
                DeleteLine(i);
                BlockDown(i);
            }
            else 
            {
                i++;
            }
        }
    }

    /// <summary> Переопределения местоположения при воможности проходить через стены</summary>
    public override Vector2 RoundVector2(Vector2 v, EnumRound round) 
    {
        v.x = v.x < 0? Width + v.x % Width: v.x;
        v.x = v.x > Width - 1? v.x % Width: v.x;
        return base.RoundVector2(v, round);
    }

}
