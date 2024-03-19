using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenShader : MonoBehaviour
{
    public Shader screenShader = null;
    private Material m_renderMaterial;
    [SerializeField] Texture2D dissolveTexture;
    [SerializeField] float dissolveProgression;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    void Start()
    {
        if (screenShader == null)
        {
            Debug.LogError("no screen shader.");
            m_renderMaterial = null;
            return;
        }
        m_renderMaterial = new Material(screenShader);
        m_renderMaterial.SetTexture("_DissolveTex", dissolveTexture);
    }

    private void Update()
    {
        m_renderMaterial.SetFloat("_DissolveProg", dissolveProgression);
    }

    public void StartDissolve()
    {
        GameObject.FindGameObjectWithTag("MainCanvas").SetActive(false);
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        float maxTime = 5;
        float timer = maxTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            dissolveProgression = 1 - timer / maxTime;
            m_renderMaterial.SetFloat("_DissolveProg", dissolveProgression);
            yield return endOfFrame;
        }
        SceneManager.LoadScene("Credits");
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_renderMaterial);
    }
}
