using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    // machine
    public bool isMachineSwinging = true;
    public bool isMachineTurning = true;
    public bool isMachineSpining = true;

    //[Range(0, 50)] public
    public float machineSwingSpeed = 10.0f;
    //[Range(0, 90)] public
    public float machineSwingAngleMax = 30.0f;
    //[Range(0, 50)] public
    public float machineSpinSpeed = 10.0f;
    public bool isSpiningCW = true;

    // player
    //[Range(0, 5)] public
    public float playerMoveSpeed = 2.0f;
    //[Range(0, 150)] public
    public float playerRotateSpeed = 40.0f;
    //[Range(0, 5)] public
    public float playerJumpPower = 5.0f;
}
