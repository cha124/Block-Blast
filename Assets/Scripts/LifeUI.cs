using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public GameObject[] lifeIcons;

    public void UpdateLife(int life)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < lifeIcons.Length - life)
            {
                lifeIcons[i].SetActive(false);
            }
        }
    }
}