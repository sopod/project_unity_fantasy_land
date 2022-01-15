using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelValues
{
    GameMode _modeCur = GameMode.Easy;
    int _levelCur = 0;

    [SerializeField] float MachineSwingSpeed_Default = 10.0f;
    [SerializeField] float MachineSwingAngleMax_Default = 30.0f;
    [SerializeField] float MachineSpinSpeed_Default = 10.0f;

    public float SwingSpeedCur;
    public float SwingAngleMaxCur;
    public float SpinSpeedCur;

    public EnemyType[] EnemyTypesCur;

    public LevelValues()
    {
        SwingSpeedCur = MachineSpinSpeed_Default;
        SwingAngleMaxCur = MachineSwingAngleMax_Default;
        SpinSpeedCur = MachineSpinSpeed_Default;
        EnemyTypesCur = new EnemyType[] { EnemyType.Bwun, EnemyType.Ssak };
    }

    public void ChangeLevel(GameMode mode, int level)
    {
        _modeCur = mode;
        _levelCur = level;

        if (mode == GameMode.Easy)
        {
            switch (level)
            {
                case 1:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default;
                    SpinSpeedCur = MachineSpinSpeed_Default;
                    EnemyTypesCur = new EnemyType[]{ EnemyType.Bwun, EnemyType.Ssak};
                }
                    break;

                case 2:
                case 3:
                case 4:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.1f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.1f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.1f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Bwun, EnemyType.Ssak };
                }
                    break;

                case 5:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.2f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Bwun, EnemyType.Ssak };
                }
                    break;

                case 6:
                case 7:
                case 8:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.2f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Bwun, EnemyType.Ssak, EnemyType.Ral };
                }
                    break;

                case 9:
                case 10:
                    {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.3f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.3f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.3f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Bwun, EnemyType.Ssak, EnemyType.Ral };
                }
                    break;
            }
        }
        else
        {
            switch (level)
            {
                case 1:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default;
                    SpinSpeedCur = MachineSpinSpeed_Default;
                    EnemyTypesCur = new EnemyType[] {EnemyType.Juck, EnemyType.Swook};
                }
                    break;

                case 2:
                case 3:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.1f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.1f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.1f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Juck, EnemyType.Swook };
                }
                    break;

                case 4:
                case 5:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.2f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.2f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Juck, EnemyType.Swook };
                }
                    break;

                case 6:
                case 7:
                case 8:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.3f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.3f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.3f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Juck, EnemyType.Swook, EnemyType.Gum };
                }
                    break;

                case 9:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.4f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.4f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.4f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Juck, EnemyType.Swook, EnemyType.Gum };
                }
                    break;

                case 10:
                {
                    SwingSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.5f;
                    SwingAngleMaxCur = MachineSwingAngleMax_Default + MachineSwingAngleMax_Default * 0.5f;
                    SpinSpeedCur = MachineSpinSpeed_Default + MachineSpinSpeed_Default * 0.5f;
                    EnemyTypesCur = new EnemyType[] { EnemyType.Juck, EnemyType.Swook, EnemyType.Gum };
                }
                    break;
            }
        }
    }




}