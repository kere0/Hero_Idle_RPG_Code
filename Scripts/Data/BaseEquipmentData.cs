using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipmentData : InterfaceID
{
    public int Id;
    public int ItemId;
    public EquipmentType EquipmentType;
    public EquipmentRarity EquipmentRarity;
    public int StarGrade;
    public int Value;
    public int ID => Id;
}
