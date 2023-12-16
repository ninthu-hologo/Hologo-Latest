using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starsUIElement : MonoBehaviour
{

    [SerializeField]
    private GameObject starOne;
    [SerializeField]
    private GameObject starTwo;
    [SerializeField]
    private GameObject starThree;


    public void turnOffStars()
    {
        starOne.SetActive(false);
        starTwo.SetActive(false);
        starThree.SetActive(false);
    }

    public void setStarCount(int count)
    {
        switch (count)
        {
            case 1:
                starOne.SetActive(true);
                starTwo.SetActive(false);
                starThree.SetActive(false);
                break;
            case 2:
                starOne.SetActive(true);
                starTwo.SetActive(true);
                starThree.SetActive(false);
                break;
            case 3:
                starOne.SetActive(true);
                starTwo.SetActive(true);
                starThree.SetActive(true);
                break;
            default:
                break;
        }
    }

}
