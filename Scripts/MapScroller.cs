using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroller : MonoBehaviour
{
    private readonly Vector3 _movePosition = new Vector3(64.5f, 0, 0);
    private readonly float _threshold = 30f;
    readonly float _mapWidth = 32.25f;
    private void Update()
    {
        if (GameContainer.Instance.Player.transform.position.x - transform.position.x > _threshold)
        {
            transform.position += _movePosition;
        }
    }
    public void SetPos(int i)
    {
        transform.position = new Vector3(i * _mapWidth -10f, 14.5f, 0f);
    }
}
 