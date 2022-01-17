using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    const int starMax = 3;

    [SerializeField] GameObject[] stars;

    public void TurnOff()
    {
        for (int i = 0; i < starMax; i++)
        {
            stars[i].SetActive(false);
        }
    }

    public void TurnOn(int starsGot)
    {
        int cnt = 0;
        for (int i = 0; i < starMax; i++)
        {
            if (cnt < starMax - starsGot)
            {
                stars[i].SetActive(false);
                cnt++;
                continue;
            }
            stars[i].SetActive(true);
        }
    }

}
