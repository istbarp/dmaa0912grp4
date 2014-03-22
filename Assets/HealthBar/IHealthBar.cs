using UnityEngine;
using System.Collections;

public interface IHealthBar
{
    float BaseHealth { get; set; }
    float CurrentHealth { get; set; }
}