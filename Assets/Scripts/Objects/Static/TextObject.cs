using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextObject : MonoBehaviour
{
    // - = - for some reason having content fitter enabled on preffered breaks the TypeString() function
    [Header("** use content size fitter to make sure text fits in rect transform **")]
    [Header("** ADD THIS SCRIPT TO TEXT WITH TEXT MESH PRO **")]
    // settings
    public bool invisibleAtStart = true;

    [Tooltip("Set automatically by book manager if exists")]
    public float textSpeed;

    public BookManager bManager;
    [Tooltip("Usually the scrollrect")]
    public GameObject textContainer; 
    public TextMeshProUGUI m_TextMeshPro;
    private int charIndex = 0;

    private IEnumerator coroutine;

    void Start()
    {
        if (bManager == null)
        {
            Debug.LogWarning("Book Manager is null!");
        }

        // force characters to load
        m_TextMeshPro.ForceMeshUpdate();

        if (invisibleAtStart)
        {
            SetTextAlphaZero();
        }

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            if (coroutine != null)
                SkipTypingString();
            else
                CloseBook();
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

            Color32 c = textInfo.characterInfo[i].color;
            c.a = 0;

            SetCharacterColor(textInfo, c, i);
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
                Color32 c = textInfo.characterInfo[charIndex].color;
                c.a = 255;

                SetCharacterColor(textInfo, c, charIndex);

            }

            char currChar = textInfo.characterInfo[charIndex].character;
            if (char.IsPunctuation(currChar) && currChar != '\'' && currChar != '"')
            {
                yield return new WaitForSeconds(2 * textSpeed);
            }
            charIndex++;
            yield return new WaitForSeconds(textSpeed);
        }

        coroutine = null;
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

            for (int i = charIndex; i < textInfo.characterCount; i++)
            {
                // Skip characters that are not visible and thus have no geometry to manipulate.
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                Color32 c = textInfo.characterInfo[i].color;
                c.a = 255;

                SetCharacterColor(textInfo, c, i);
            }

            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    #endregion

    public void OpenBook()
    {
        bManager.OpenBook(this);
    }

    public void StartTyping()
    {
        coroutine = TypeString();
        StartCoroutine(coroutine);
    }

    public void CloseBook()
    {
        bManager.CloseBook();
    }

    public void UpdateTextSpeed(float charDelay)
    {
        textSpeed = charDelay;
    }
}
