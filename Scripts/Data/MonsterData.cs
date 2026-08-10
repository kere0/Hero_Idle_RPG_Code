using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : InterfaceID
{
    public int Id;
    public string Name;
    public float Size;
    public MonsterType MonsterType;
    public int ID => Id;
}
