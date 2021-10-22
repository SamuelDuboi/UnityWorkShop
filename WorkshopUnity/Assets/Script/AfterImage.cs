using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class AfterImage : MonoBehaviour
{

    MaterialPropertyBlock propertyBlock;
    public SkinnedMeshRenderer MeshRenderer;
    IEnumerator AfterImageCoroutine()
    {
        propertyBlock = new MaterialPropertyBlock();
        MeshRenderer.GetPropertyBlock(propertyBlock);
        for (int i = 0; i < 50f; i++)
        {
            propertyBlock.SetFloat("_Float", i * 2f / 100f);
            MeshRenderer.SetPropertyBlock(propertyBlock);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }

    public void DoAfterImage()
    {
        StartCoroutine(AfterImageCoroutine());
    }
}