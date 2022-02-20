using TMPro;
using UnityEngine;


public class Notify : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notify;
    bool isNotifying = false;
    public bool IsNotifying { get => isNotifying; }

    void Start()
    {
        gameObject.SetActive(false);   
    }

    public void On(string text, float time)
    {
        notify.text = text;
        isNotifying = true;
        Invoke("Off", time);
    }

    void Off()
    {
        notify.gameObject.SetActive(false);
        isNotifying = false;
    }
}