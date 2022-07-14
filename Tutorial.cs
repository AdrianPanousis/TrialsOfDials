using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //stores the images for the tutorial
    [SerializeField] private List<Sprite> TutorialImages;

    //the actual image displaying on screen
    private Image DisplayImage;

    //the number used for the array to index the image
    private int imageNumber;

    void Start()
    {
        imageNumber = 1;
        DisplayImage = transform.GetChild(0).GetComponent<Image>();
    }

    //proceeds to the next step of the tutorial
    public void NextImage()
    {
        //checks if the tutorial is at the last slide and doesn't change if it is
        if (imageNumber < TutorialImages.Count)
        {
            imageNumber += 1;
            DisplayImage.sprite = TutorialImages[imageNumber - 1];
        }
    }

    //proceeds to the previous step of the tutorial
    public void PreviousImage()
    {
        //checks if the tutorial is at the first slide and doesn't change if it is
        if(imageNumber > 1)
        {
            imageNumber -= 1;
            DisplayImage.sprite = TutorialImages[imageNumber - 1];
        }
        
    }
}
