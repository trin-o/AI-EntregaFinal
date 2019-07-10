using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public Texture2D map;
    public Transform parentObj;

    public ColorToPrefab[] colorMappings;

    public void Generator()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                ObjGenerate(x, y);
            }
        }
        RestarPosition();
    }

    public void ObjGenerate(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, y, 0);

                if (colorMapping.back)
                {
                    position = new Vector3(x, y, 5);
                }

                Instantiate(colorMapping.prefab, position, colorMapping.prefab.transform.rotation, parentObj);
            }
        }
    }

    public void RestarPosition()
    {
        float x = 0.0f; //-(map.width - 1) / 2f + 0.5f
        float y = -(map.height - 1) / 2f - 0.5f;
        float z = 0.0f;

        transform.position = new Vector3(x, y, z);
    }

    public void Restar()
    {
        transform.position = Vector3.zero;

        GameObject[] temp = new GameObject[parentObj.childCount];

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = parentObj.GetChild(i).gameObject;
        }

        foreach (GameObject t in temp)
        {
            DestroyImmediate(t);
        }
    }
}
