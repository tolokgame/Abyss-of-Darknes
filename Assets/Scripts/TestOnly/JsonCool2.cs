using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSave : MonoBehaviour
{
    public List<GameObject> objectsToSave;

    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/objects.json";
    }

    public void Save()
    {
        List<ObjectTransform> objects = new List<ObjectTransform>();

        foreach (GameObject obj in objectsToSave)
        {
            ObjectTransform objectTransform = new ObjectTransform(
                obj.transform.position,
                obj.transform.eulerAngles,
                obj.transform.localScale
            );

            objects.Add(objectTransform);
        }

        string json = JsonUtility.ToJson(new ObjectTransformList(objects));
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Ôאיכא חבונוזודÿÿ םולא");
            return;
        }

        string json = File.ReadAllText(savePath);

        ObjectTransformList objectList =
            JsonUtility.FromJson<ObjectTransformList>(json);

        for (int i = 0; i < objectsToSave.Count; i++)
        {
            objectsToSave[i].transform.position = objectList.objects[i].position;
            objectsToSave[i].transform.eulerAngles = objectList.objects[i].rotation;
            objectsToSave[i].transform.localScale = objectList.objects[i].scale;
        }
    }
}

[Serializable]
public class ObjectTransformList
{
    public List<ObjectTransform> objects;

    public ObjectTransformList(List<ObjectTransform> objects)
    {
        this.objects = objects;
    }
}

[Serializable]
public class ObjectTransform
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public ObjectTransform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}