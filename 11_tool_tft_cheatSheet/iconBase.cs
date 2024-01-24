using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconBase : MonoBehaviour
{
    protected string mNameJP;
    protected string mNameENG;

    public virtual void SetName(string nameJP = "", string nameENG = "")
    {
        mNameJP = nameJP;
        mNameENG = nameENG;
    }

    public string GetNameJP() { return mNameJP; }
    public string GetNameENG(){ return mNameENG; }


    public virtual void SetTexture(string texPath)
    {
        Image _imageComponent = this.GetComponent<Image>();
        Texture2D _texture = Resources.Load<Texture2D>(texPath);
        //Texture2D _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        //AssetBundle

        _imageComponent.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
        _imageComponent.SetNativeSize();
    }


    public virtual void SetPos(Vector2 pos)
    {
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
