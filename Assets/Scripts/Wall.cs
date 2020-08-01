using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Скрипт для рисования границ поля </summary>
[RequireComponent(typeof(LineRenderer))]
public class Wall : MonoBehaviour
{

    [SerializeField] LineRenderer line;

    const int CountPoint = 4;

    void Start()
    {
        Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = CountPoint;
        line.SetPosition(0, new Vector2(-1f, map.Length));
        line.SetPosition(1, new Vector2(-1f, -1));
        line.SetPosition(2, new Vector2(map.Width, -1));
        line.SetPosition(3, new Vector2(map.Width, map.Length));
    }
}
