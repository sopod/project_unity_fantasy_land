using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Values", menuName = "Gyroswing/Level Values")]
public class LevelValues : ScriptableObject
{
    public float MachineSwingSpeed = 10.0f;
    public float MachineSwingAngleMax = 30.0f;
    public float MachineSpinSpeed = 10.0f;

    public EnemyType[] EnemyTypes;
}