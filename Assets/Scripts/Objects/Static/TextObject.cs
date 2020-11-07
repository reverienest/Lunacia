using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextObject : MonoBehaviour
{
    // - = - for some reason having content fitter enabled on preffered breaks the TypeString() function
    //[Header("** use content size fitter to make sure text fits in rect transform **")]
    [Header("** ADD THIS SCRIPT TO TEXT WITH TEXT MESH PRO **")]
    // settings
    public bool invisibleAtStart = true;
    public bool fadeTextIn = true;
    public int rolloverCharacterSpread = 10;

    [Tooltip("Set automatically by book manager if exists")]
    public float textSpeed;

    private TextMeshProUGUI m_TextMeshPro;
    private int charIndex = 0;
    private int startingCharacterIndex = 0; // for fade in

    private IEnumerator coroutine;
    public bool finishedTyping;

    void Awake()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        if (m_TextMeshPro == null)
            Debug.LogWarning("TextMeshPro script not found!");
        // force characters to load
        m_TextMeshPro.ForceMeshUpdate();
    }

    private void Start()
    {
        if (invisibleAtStart)
        {
            m_TextMeshPro.ForceMeshUpdate();
            SetTextAlphaZero();
        }
        finishedTyping = !invisibleAtStart;
    }

    #region - SETTING TEXT STUFF -

    private void SetTextAlphaZero()
    {
        TMP_TextInfo textInfo = m_TextMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Skip characters that are not visible and thus have no geometry to manipulate.
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            textInfo.characterInfo[i].color.a = 0;

            SetCharacterColor(textInfo, textInfo.characterInfo[i].color, i);
        }
    }

    private IEnumerator TypeString()
    {
        TMP_TextInfo textInfo = m_TextMeshPro.textInfo;

        while (charIndex < m_TextMeshPro.text.Length)
        {

            // If No Characters then just yield and wait for some text to be added
            if (textInfo.characterCount == 0)
            {
                yield return new WaitForSeconds(textSpeed);
                continue;
            }

            // Skip characters that are not visible and thus have no geometry to manipulate.
            if (!textInfo.characterInfo[charIndex].isVisible)
            {
                charIndex++;
                continue;
            }
            else
            {
                textInfo.characterInfo[charIndex].color.a = 255;

                SetCharacterColor(textInfo, textInfo.characterInfo[charIndex].color, charIndex);

            }

            char currChar = textInfo.characterInfo[charIndex].character;
            if (char.IsPunctuation(currChar) && currChar != '\'' && currChar != '"')
            {
                yield return new WaitForSeconds(2 * textSpeed);
            }
            charIndex++;
            startingCharacterIndex++;
            yield return new WaitForSeconds(textSpeed);
        }

        coroutine = null;
        finishedTyping = true;
    }

    private IEnumerator TypeStringFadeIn()
    {
        TMP_TextInfo textInfo = m_TextMeshPro.textInfo;

        startingCharacterIndex= charIndex;
        bool isRangeMax = false;

        byte fadeSteps = (byte)Mathf.Max(1, 255 / rolloverCharacterSpread);

        while (!isRangeMax)
        {
            int characterCount = textInfo.characterCount;

            // If No Characters then just yield and wait for some text to be added
            if (characterCount == 0)
            {
                yield return new WaitForSeconds(textSpeed);
                continue;
            }

            for (int i = startingCharacterIndex; i < charIndex + 1; i++)
            {
                // Skip characters that are not visible and thus have no geometry to manipulate.
                if (!textInfo.characterInfo[i].isVisible)
                {
                    continue;
                }
                else
                {
                    byte alpha = (byte)Mathf.Clamp(textInfo.characterInfo[i].color.a + fadeSteps, 0, 255);
                    textInfo.characterInfo[i].color.a = alpha;
                    SetCharacterColor(textInfo, textInfo.characterInfo[i].color, i);

                    if (alpha == 255)
                    {
                        startingCharacterIndex += 1;

                        if (startingCharacterIndex == characterCount)
                        {
                            // Update mesh vertex data one last time.
                            m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                            isRangeMax = true; 
                        }
                    }
                }
            }

            m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            if (charIndex + 1 < characterCount) charIndex += 1;

            yield return new WaitForSeconds(textSpeed);
        }

        coroutine = null;
        finishedTyping = true;
    }

    private void SetCharacterColor(TMP_TextInfo textInfo, Color32 c, int ind)
    {
        int meshIndex = textInfo.characterInfo[ind].materialReferenceIndex;

        int vertexIndex = textInfo.characterInfo[ind].vertexIndex;

        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;


        vertexColors[vertexIndex + 0] = c;
        vertexColors[vertexIndex + 1] = c;
        vertexColors[vertexIndex + 2] = c;
        vertexColors[vertexIndex + 3] = c;

        //m_TextMeshPro.mesh.colors32 = vertexColors;
        textInfo.meshInfo[meshIndex].mesh.colors32 = vertexColors;

        // update mesh
        m_TextMeshPro.UpdateGeometry(textInfo.meshInfo[meshIndex].mesh, meshIndex);
    }

    public void SkipTypingString()
    {
        if (coroutine != null)
        {
            TMP_TextInfo textInfo = m_TextMeshPro.textInfo;

            for (int i = startingCharacterIndex; i < textInfo.characterCount; i++)
            {
                // Skip characters that are not visible and thus have no geometry to manipulate.
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                textInfo.characterInfo[i].color.a = 255;

                SetCharacterColor(textInfo, textInfo.characterInfo[i].color, i);
            }

            StopCoroutine(coroutine);
            coroutine = null;
        }
        finishedTyping = true;
    }

    #endregion

    public void StartTyping()
    {
        if (finishedTyping)
        {
            Debug.Log("Already finished typing");
            return;
        }
        SetTextAlphaZero();
        if (fadeTextIn)
            coroutine = TypeStringFadeIn();
        else
            coroutine = TypeString();
        StartCoroutine(coroutine);
    }

    public void UpdateTextSpeed(float charDelay)
    {
        textSpeed = charDelay;
    }

    // returns true if text is skipped, false if it isn't
    public bool TrySkipText()
    {
        if (coroutine != null)
        {
            SkipTypingString();
            return true;
        }
        return false;
    }
}
