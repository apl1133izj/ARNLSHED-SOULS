using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class typing : MonoBehaviour
{
    public Text typingText;
    public Image typingImage;

    private string typingString;
    public float typingSpeed = 0.1f;
    public float textColorFadeSpeed = 2f;
    public int count;//0인 경우에만 실행

    public bool[] text_0_Image_1_Bool;//fade하고 싶은 대상이 image인지 text인지 확인 0:이 text 1:image
    public bool[] fade_Text_Color; //0:Text 1:Color
    public bool backImageBool;
    public Image backImage;//구역 진입 뒤 이미지
    public bool loopBool;//계속 실행 하고 싶은 경우 true 아닌 경우 flase
    private void Start()
    {
        if (loopBool)
        {
            StartCoroutine(LoopTypingTextCoroutine());
        }
    }
    private void Update()
    {
        if (!loopBool)
        {
            if (text_0_Image_1_Bool[0])
            {
                if (count == 0)
                {
                    typingString = typingText.text;
                    typingText.text = ""; // 시작할 때 텍스트를 지워줍니다.
                    if (fade_Text_Color[0] && fade_Text_Color[1])
                    {
                        if (backImageBool) backImage.enabled = true;

                        StartCoroutine(TypingTextCoroutine());
                        StartCoroutine(FadeInColorText());
                    }
                    else if (fade_Text_Color[0])
                    {
                        StartCoroutine(TypingTextCoroutine());
                    }

                }
            }
            else if (text_0_Image_1_Bool[1])
            {
                if (count == 0)
                {
                    if (backImageBool) backImage.enabled = true;
                    StartCoroutine(FadeInColorImage());
                }
            }
        }
    }
    public IEnumerator TypingTextCoroutine()
    {
        count = 1;
        foreach (char letter in typingString)
        {
            typingText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 한 글자씩 출력하는 시간을 조절할 수 있습니다.
        }  
    }
    public IEnumerator LoopTypingTextCoroutine()
    {
        typingString = typingText.text;
        typingText.text = ""; // 시작할 때 텍스트를 지워줍니다.
        foreach (char letter in typingString)
        {
            typingText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 한 글자씩 출력하는 시간을 조절할 수 있습니다.
        }
        yield return null;
        StartCoroutine(LoopTypingTextCoroutine());
    }
    public IEnumerator FadeInColorText()
    {
        Color color = typingText.color;
        color.a = 0f;
        typingText.color = color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime / textColorFadeSpeed;
            typingText.color = color;
            yield return null;
        }

        color.a = 1f;
        typingText.color = color;

        // 텍스트가 완전히 표시된 후 페이드 아웃
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / textColorFadeSpeed;
            typingText.color = color;
            yield return null;
        }
        if (backImageBool)backImage.enabled = false;
        color.a = 0f;
        typingText.color = color;

    }

    public IEnumerator FadeInColorImage()
    {
        count = 1;
        Color color = typingImage.color;
        color.a = 0f;
        typingImage.color = color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime / textColorFadeSpeed;
            typingImage.color = color;
            yield return null;
        }

        color.a = 1f;
        typingImage.color = color;
        yield return new WaitForSeconds(1);
        // 텍스트가 완전히 표시된 후 페이드 아웃
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / textColorFadeSpeed;
            typingImage.color = color;
            yield return null;
        }
        if (backImageBool)backImage.enabled = false;
        color.a = 0f;
        typingImage.color = color;
    }
}
