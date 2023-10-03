using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainImageScript : MonoBehaviour
{   
    #region VARIABLES

    [Space()]
    [Header("UserInterfaceGeneral")]
    [SerializeField] private GameObject image_unk; // Imagem abaixo
    [SerializeField] private GameControllerScript gameController; // Controller

    private int _spriteId;

    #endregion

    // Ação do click
    public void OnMouseDown(){
        if(image_unk.activeSelf && gameController.canOpen)
        {
            image_unk.SetActive(false);
            gameController.imageOpened(this);
        }
    }

    public int spriteId{
        get {return _spriteId; }
    }

    // Troca o Sprite
    public void ChangeSprite(int id, Sprite image){
        _spriteId = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    // Altera tamanho da img
    public void ChangeSize(float width, float height)
    {
        Transform transform = gameObject.transform;
        Vector3 scale = new Vector3(width, height, transform.localScale.z);
        transform.localScale = scale;
    }

    // Esconde Image
    public void Close(){
        image_unk.SetActive(true); 
    }
}
