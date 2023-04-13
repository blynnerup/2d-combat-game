using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [FormerlySerializedAs("_transparencyAmount")]
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;

    [FormerlySerializedAs("_fadeTime")] [SerializeField] private float fadeTime = 0.4f;

    private SpriteRenderer _spriteRenderer;
    private Tilemap _tilemap;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
            {
                // Fade the tree
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, transparencyAmount));
            } else if (_tilemap)
            {
                StartCoroutine(FadeRoutine(_tilemap, fadeTime, _tilemap.color.a, transparencyAmount));
            }     
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
            {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, 1f));
            } else if (_tilemap)
            {
                StartCoroutine(FadeRoutine(_tilemap, fadeTime, _tilemap.color.a, 1f));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue,
        float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            var color = spriteRenderer.color;
            color = new Color(color.r, color.g, color.b,
                newAlpha);
            spriteRenderer.color = color;
            yield return null;
        }
    }
    
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue,
        float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            var color = tilemap.color;
            color = new Color(color.r, color.g, color.b,
                newAlpha);
            tilemap.color = color;
            yield return null;
        }
    }
}
