using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBuffEffect : BuffEffect
{
    protected override void Update()
    {
        UpdateLifeTime();
        if (_player == null) return;
        transform.position = _player.Position;
    }
}
