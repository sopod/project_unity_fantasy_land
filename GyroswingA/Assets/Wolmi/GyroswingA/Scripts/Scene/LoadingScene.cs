using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider slider;

    void Start()
    {
        slider.value = 0.0f;
        SceneController.Instance.DoLoadingSceneProcess(slider);
    }

}
