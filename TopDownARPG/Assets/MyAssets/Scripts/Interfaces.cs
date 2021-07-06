using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    void Equip();

    void Unequip();
}

public interface IConsumable
{
    void Consume();
}

public interface IDisplayInfo
{
    void StartHover();

    void EndHover();
}