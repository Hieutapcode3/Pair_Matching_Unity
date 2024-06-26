using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f,3.62f);
    [HideInInspector]
    public List<Picture> PictureList;

    private Vector2 _offset = new Vector2(1.5f,1.52f);

    private List<Material> _materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;
    void Start()
    {
        LoadMaterials();
        SpawnPictureMesh(4,5,StartPosition,_offset,false);
        MovePicture(4,5,StartPosition,_offset);
    }
    private void LoadMaterials()
    {

    }

    void Update()
    {
        
    }
    private void SpawnPictureMesh(int rows,int colums,Vector2 Pos,Vector2 offset,bool scaleDown)
    {
        for(int col =0;col < colums; col++)
        {
            for(int row=0;row < rows;row++)
            {
                var tmpPicture = (Picture)Instantiate(PicturePrefab,PicSpawnPosition.position,PicSpawnPosition.transform.rotation);
                tmpPicture.name = tmpPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tmpPicture);
            }
        }
    }
    private void MovePicture(int rows,int colums,Vector2 pos,Vector2 offset)
    {
        var index = 0;
        for(var col =0;col < colums; col++)
        {
            for(int row=0;row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)) ,(pos.y - (offset.y *col)),0.0f);
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                index++;
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 target,Picture obj)
    {
        var randomDis = 40;
        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position,target,randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}
