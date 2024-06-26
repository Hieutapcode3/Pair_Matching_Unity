using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    private Material _firstMateria;
    private Material _secondMateria;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetFirstMaterial(Material mat,string texturePath)
    {
        _firstMateria = mat;
        _firstMateria.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
    }public void SetSecondMaterial(Material mat,string texturePath)
    {
        _secondMateria = mat;
        _secondMateria.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
    }
    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMateria;

    }public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _secondMateria;

    }
}
