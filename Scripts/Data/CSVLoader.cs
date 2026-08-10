using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class CSVLoader
{
    public Dictionary<int, T> LoadCSV<T>(TextAsset ts) where T : InterfaceID, new()
    {
        if (ts == null)
        {
            Debug.LogError("TextAsset is null");
            return null;
        }

        StringReader reader = new StringReader(ts.text);
        string[] headers = reader.ReadLine().Split(',');

        Dictionary<int, T> dict = new Dictionary<int, T>();

        while (reader.Peek() > 0)
        {
            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            T data = new T();

            for (int i = 0; i < headers.Length; i++)
            {
                string title = headers[i];
                string value = values[i];

                FieldInfo field = typeof(T).GetField(title);
                if (field == null) continue;

                object converted = null;

                try
                {
                    if (field.FieldType.IsEnum)
                    {
                        converted = Enum.Parse(field.FieldType, value);
                    }
                    else
                    {
                        converted = Convert.ChangeType(value, field.FieldType);
                    }

                    field.SetValue(data, converted);
                }
                catch (Exception e)
                {
                    Debug.LogError($"변환 실패: {title} / {value} / {e.Message}");
                }
            }
            Debug.Log(data.ID);
            dict.Add(data.ID, data);
        }

        return dict;
    }
}