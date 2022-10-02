using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button NextBtn;
    public Button PreviousBtn;

    [Header("Configurations")]
    public Image Image;
    public Sprite[] ImageList;
    public int currentImageIndex;

    private void Awake()
    {
        NextBtn.onClick.AddListener(NextImage);
        PreviousBtn.onClick.AddListener(PreviousImage);
        SetCurrentImage();
    }

    private void NextImage()
    {
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        currentImageIndex++;
        SetCurrentImage();
    }

    private void PreviousImage()
    {
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        currentImageIndex--;
        SetCurrentImage();
    }

    private void SetCurrentImage()
    {
        if(currentImageIndex >= ImageList.Length)
        {
            currentImageIndex = 0;
        }
        
        if (currentImageIndex < 0)
        {
            currentImageIndex = ImageList.Length - 1;
        }

        Image.sprite = ImageList[currentImageIndex];
    }
}
