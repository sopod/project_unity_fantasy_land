using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCarrier : MonoBehaviour
{
    [SerializeField] List<Mover> ms = new List<Mover>();

    Transform t;
    Vector3 _lastPosition;
    Vector3 _lastRotation;
    Vector3 _curPos;
    float _curSpinAngle;

    float spinSpeed = 20.0f;
    bool spinClockWise = false;

    void Start()
    {
        t = transform;
        _lastPosition = t.position;
        _lastRotation = t.eulerAngles;
    }

    void LateUpdate()
    {
        Spin();

        if (ms.Count > 0)
        {
            for (int i = 0; i < ms.Count; i++)
            {
                Mover m = ms[i];
                Vector3 vel = t.position - _lastPosition;
                Vector3 rot = t.eulerAngles - _lastRotation;

                // for spin
                if (m.isOnStage)
                {
                    Quaternion spinQuat = Quaternion.AngleAxis(_curSpinAngle, transform.up);
                    m.rb.rotation *= spinQuat;
                    _curPos = (spinQuat * (m.rb.transform.position - transform.position) + transform.position);
                }
                else
                    _curPos = m.rb.position;

                m.rb.position = vel + _curPos;
                m.rb.angularVelocity += rot;
            }
        }

        _lastPosition = t.position;
        _lastRotation = t.eulerAngles;
    }

    void Spin()
    {
        if (spinClockWise)
            _curSpinAngle = spinSpeed * Time.deltaTime;
        else
            _curSpinAngle = -spinSpeed * Time.deltaTime;

        transform.rotation *= Quaternion.AngleAxis(_curSpinAngle, Vector3.up);
    }

    void OnCollisionEnter(Collision c)
    {
        Mover m = c.collider.GetComponent<Mover>();

        if (m != null)
        {
            AddRigidbody(m);
        }
    }

    void OnCollisionExit(Collision c)
    {
        Mover m = c.collider.GetComponent<Mover>();

        if (m != null)
        {
            RemoveRigidbody(m);
        }
    }

    void AddRigidbody(Mover m)
    {
        if (!ms.Contains(m))
        {
            ms.Add(m);
            m.isOnStage = true;
            m.isJumping = false;
        }
    }

    void RemoveRigidbody(Mover m)
    {
        if (ms.Contains(m))
        {
            ms.Remove(m);
            m.isOnStage = false;
        }
    }



}
