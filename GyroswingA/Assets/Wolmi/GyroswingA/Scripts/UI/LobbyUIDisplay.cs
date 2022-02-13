using UnityEngine;


public class LobbyUIDisplay : MonoBehaviour
{
    [SerializeField] GameObject optionScreen;

    public void Start()
    {
        optionScreen.SetActive(false);
    }
}
