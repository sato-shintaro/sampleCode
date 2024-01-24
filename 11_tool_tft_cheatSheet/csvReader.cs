using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class csvReader : ScriptableObject
{
    public TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();

    /*
    void OnEnable()
    {
        csvFile = Resources.Load("itemMixList") as TextAsset;
        StringReader _reader = new StringReader(csvFile.text);

        while (_reader.Peek() != -1)
        {
            string _line = _reader.ReadLine();
            csvDatas.Add(_line.Split('\t'));
        }

        Debug.Log(csvDatas[0][1]);
    }
    */

    public static List<string[]> csvSplit( TextAsset csv, char splitTarget )
    {
        List<string[]> _result = new List<string[]>();
        StringReader _reader = new StringReader(csv.text);

        while (_reader.Peek() != -1)
        {
            string _line = _reader.ReadLine();
            _result.Add(_line.Split( splitTarget ));
        }

        return _result;
    }
}
