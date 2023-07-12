using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    public Image hitmarkerImage;
    public float showDuration = 0.5f;
    public float fadeDuration = 0.5f;
    public AudioClip hitmarkerAudio;
    private Coroutine hitmarkerCoroutine;
    public AudioSource source;

    void Start()
    {
        GetReferences();
        hitmarkerImage.enabled = false;
    }

    public void ShowHitmarker()
    {
        if (hitmarkerCoroutine != null)
        {
            StopCoroutine(hitmarkerCoroutine);
        }
        hitmarkerCoroutine = StartCoroutine(ShowHitmarkerCoroutine());
    }

    IEnumerator ShowHitmarkerCoroutine()
    {
        hitmarkerImage.enabled = true;

        if (source != null)
        {
            source.PlayOneShot(hitmarkerAudio);
        }

        yield return new WaitForSeconds(showDuration);

        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float alpha = 1 - (Time.time - startTime) / fadeDuration;
            hitmarkerImage.color = new Color(hitmarkerImage.color.r, hitmarkerImage.color.g, hitmarkerImage.color.b, alpha);
            yield return null;
        }
        hitmarkerImage.enabled = false;
    }

    private void GetReferences()
    {
        source = GetComponent<AudioSource>();
    }
}