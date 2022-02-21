using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    const int STAR_MAX = 3;

    [SerializeField] GameObject[] stars;

    public void TurnOff()
    {
        for (int i = 0; i < STAR_MAX; i++)
        {
            stars[i].SetActive(false);
        }
    }

    public void TurnOn(int starsGot)
    {
        int cnt = 0;
        for (int i = 0; i < STAR_MAX; i++)
        {
            if (cnt < STAR_MAX - starsGot)
            {
                stars[i].SetActive(false);
                cnt++;
                continue;
            }
            stars[i].SetActive(true);
        }
    }
}
