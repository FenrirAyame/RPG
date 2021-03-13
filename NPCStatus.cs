using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class NPCStatus : MonoBehaviour
{
    public GameObject imagePre;
    public GameObject nameTextPre;
    public float imageHeight;
    public float nameHeight;
    public string NPCName;
    public Sprite NPCImage;

    private GameObject image;
    private GameObject nameText;

    private void Update()
    {
        if (image != null && image.activeInHierarchy)
            image.transform.SetPositionAndRotation(transform.position + transform.up * imageHeight, transform.localRotation);
        if (nameText != null && nameText.activeInHierarchy)
            nameText.transform.SetPositionAndRotation(transform.position + transform.up * nameHeight, transform.localRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            image = GameObjectPool.instance.CreateObject("image", imagePre,
            transform.position + transform.up * imageHeight, transform.localRotation);
            image.GetComponent<Image>().sprite = NPCImage;

            nameText = GameObjectPool.instance.CreateObject("nameText", nameTextPre,
            transform.position + transform.up * nameHeight, transform.localRotation);
            nameText.GetComponent<Text>().text = NPCName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObjectPool.instance.CollectObject(image);
            GameObjectPool.instance.CollectObject(nameText);

            image = null;
            nameText = null;
        }
    }
}
