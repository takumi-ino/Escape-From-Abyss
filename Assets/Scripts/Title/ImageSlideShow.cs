using UnityEngine;
using UnityEngine.UI;

public class ImageSlideShow : MonoBehaviour
{
    public Image randomImage;
    public Sprite s0, s1, s2, s3;
    Sprite[] sprites;

    private void Start()
    {
        sprites = new Sprite[4];
        sprites[0] = s0;
        sprites[1] = s1;
        sprites[2] = s2;
        sprites[3] = s3;


        InvokeRepeating("ChangeImage", 3, 6);
    }

    void ChangeImage()
    {
        int num = Random.Range(0, sprites.Length);

        randomImage.sprite = sprites[num];
    }
}