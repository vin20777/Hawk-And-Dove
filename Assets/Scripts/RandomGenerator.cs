using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Created by Yu-Ting Tsao. All rights reserved.
 * The random Hawk, Dove, Food generator.
 */
public class RandomGenerator : MonoBehaviour
{
    public GameObject Dove;
    public GameObject Hawk;
    public GameObject Food;
    public InputField HawkInputField;
    public InputField DoveInputField;
    public InputField FoodInputField;

    private Vector3 Min;
    private Vector3 Max;
    private float _xAxis;
    private float _yAxis;
    private float _zAxis;
    private Vector3 _randomPosition;
    public bool _canInstantiate;

    private ArrayList hawks = new ArrayList();
    private ArrayList doves = new ArrayList();
    private ArrayList foods = new ArrayList();

    // Slow down the update rate.
    private IEnumerator coroutine;

    private DD_DataDiagram dataDiagram;
    List<GameObject> lineList = new List<GameObject>();
    private float h = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetRange();
        GameObject dd = GameObject.Find("DataDiagram");
        if (null == dd)
        {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        dataDiagram = dd.GetComponent<DD_DataDiagram>();
        dataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
        AddALine();
        AddALine();
        AddALine();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetRange() 
    {
        Min = new Vector3(1000, 0, 0);
        Max = new Vector3(1200, 50, 0);
    }

    void AddALine()
    {
        if (dataDiagram == null) return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = dataDiagram.AddLine(color.ToString(), color);
        if (null != line) lineList.Add(line);
    }

    public void StartSimulate()
    {
        string hString = HawkInputField.text;
        string dString = DoveInputField.text;
        string fString = FoodInputField.text;
        if (hString == ""
            || dString == ""
            || fString == "")
        {
            Debug.Log("Some inputs are missing. Please check the field.");
            return;
        }
        coroutine = TimeFly(0.5f, int.Parse(hString), int.Parse(dString), int.Parse(fString));
        StartCoroutine(coroutine);
    }

    public void StopSimulate()
    {
        StopCoroutine(coroutine);
    }

    private void GenerateRandom()
    {
        _xAxis = Random.Range(Min.x, Max.x);
        _yAxis = Random.Range(Min.y, Max.y);
        _zAxis = Random.Range(Min.z, Max.z);
        _randomPosition = new Vector3(_xAxis, _yAxis, _zAxis);
    }

    private IEnumerator TimeFly(float waitTime, int hNum, int dNum, int fNum)
    {
        yield return new WaitForSeconds(waitTime);
        GenerateRandom();
        if (hawks.Count < hNum)
        {
            GenerateRandom();
            hawks.Add(Instantiate(Hawk, _randomPosition, Quaternion.identity));
        }
        if (doves.Count < dNum)
        {
            GenerateRandom();
            doves.Add(Instantiate(Dove, _randomPosition, Quaternion.identity));
        }
        if (foods.Count < fNum)
        {
            GenerateRandom();
            foods.Add(Instantiate(Food, _randomPosition, Quaternion.identity));
        }
        Debug.Log("Hawks number: " + hawks.Count
            + ", Doves number: " + doves.Count
            + ", Food number: " + foods.Count);
        coroutine = TimeFly(0.5f, hNum, dNum, fNum);
        StartCoroutine(coroutine);
    }
}
