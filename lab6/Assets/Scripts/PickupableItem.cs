using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour, IInventoryItem
{
    public Sprite _itemImage;
    public string _itemName;
    public float _fireRate;
    public bool _isWeapon;
    public GameObject _model;
    public GameObject _root;
    public BulletWeapon _weapon;

    public string itemName
    {
        get
        {
            return _itemName;
        }
    }
    public Sprite itemImage
    {
        get
        {
            return _itemImage;
        }
    }

    public float fireRate
    {
        get
        {
            return _fireRate;
        }
    }

    public bool isWeapon
    {
        get
        {
            return _isWeapon;
        }
    }

    public GameObject model
    {
        get
        {
            return _model;
        }

        set
        {
            _model = value;
        }
    }

    public GameObject root
    {
        get
        {
            return _root;
        }

        set
        {
            _root = value;
        }
    }

    public BulletWeapon weapon
    {
        get
        {
            return _weapon;
        }

        set
        {
            _weapon = value;
        }
    }

    public void onPickup()
    {
        gameObject.SetActive(false); // “picking up” merely makes it invisible
        this.root = gameObject;
        this.model = gameObject.transform.Find("Model").gameObject;
        this.weapon = root.GetComponent<BulletWeapon>();
    }
}