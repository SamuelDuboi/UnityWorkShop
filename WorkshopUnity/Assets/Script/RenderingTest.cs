using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingTest : MonoBehaviour
{
    public SkinnedMeshRenderer m_MR;

    MaterialPropertyBlock propBlock;
    public SkinnedMeshRenderer glassesMR;
    // Start is called before the first frame update
    void Start()
    {


        propBlock = new MaterialPropertyBlock();

        m_MR.GetPropertyBlock(propBlock);


        propBlock.SetColor("_Color", Color.green);

        m_MR.SetPropertyBlock(propBlock);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
