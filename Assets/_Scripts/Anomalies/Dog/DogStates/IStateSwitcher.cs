using System;
using UnityEngine;

public interface IStateSwitcher
{
    void SwitchState(IDogState dogState);
}
