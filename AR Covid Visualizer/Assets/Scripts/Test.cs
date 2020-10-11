using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private float currentTime = 0f;
    private float timeBetweenDays = 0.15f;
    private int currentDay = 0;
    private float maxHeight = 1f;
    private bool playback = false;


    private int daysTotal = 265;
    private ArrayList data = new ArrayList();
    private float max = 850000;

    [SerializeField]
    private GameObject stateWrapper;
    private Dictionary<String,int> states = new Dictionary<String,int>();

    [SerializeField]
    private GameObject rectTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject currentState;
        int size = stateWrapper.transform.childCount;
        for(int i = 0; i < size; i++)
        {
            states.Add(stateWrapper.transform.GetChild(i).name, i);
            stateWrapper.transform.GetChild(i).localPosition += new Vector3(0, -stateWrapper.transform.GetChild(i).localPosition.y, 0);
            currentState = Instantiate(rectTransform, stateWrapper.transform.GetChild(i));
            currentState.transform.localPosition = new Vector3(0, 0, 0);
        }
        Debug.Log("I am running!");
        DataGrabber test = new DataGrabber("direct-volt-292119", "bigquery-public-data", "covid19_open_data", "covid19_open_data");
        test.AddSelect("date");
        test.AddSelect("country_code");
        test.AddSelect("subregion1_name");
        test.AddSelect("location_geometry");
        test.AddSelect("cumulative_confirmed");
        test.AddSelect("cumulative_deceased");
        test.AddWhere("date > \"2020-01-20\""); //Originally 2020-09-01
        test.AddWhere("date < \"2020-10-10\""); //Originally 2020-09-30
        test.AddWhere("subregion2_code IS NULL");
        test.AddWhere("subregion1_name IS NOT NULL");
        test.AddWhere("country_code = \"US\"");
        test.AddWhere("cumulative_deceased IS NOT NULL");
        data = test.GetData();
        for(int i = 0; i < test.GetDataSize(); i++)
        {
            StateData currentData = (StateData)data[i];
            Debug.Log(currentData.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playback)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeBetweenDays)
            {
                currentTime = 0;
                RenderRectangles((float)currentDay);
                currentDay++;
            }
        }
        
    }

    
    public void TogglePlayback()
    {
        if (!playback)
        {
            currentDay = 0;
            playback = true;
        }
        else
        {
            playback = false;
        }
    }
    public void RenderRectangles(float daysFromStart)
    {
        daysFromStart = (int)daysFromStart;
        StateData currentData;
        GameObject currentState;
        int size = stateWrapper.transform.childCount;
        for(int i = 0; i < data.Count; i++)
        {
            currentData = (StateData)data[i];

            if (states.ContainsKey(currentData.GetState()))
            {
                Debug.Log(currentData.GetState());
                Debug.Log(states[currentData.GetState()]);
                currentState = stateWrapper.transform.GetChild(states[currentData.GetState()]).gameObject;
                Debug.Log(currentData.GetCaseCount());
                if (currentData.GetCaseCount() > (int)daysFromStart)
                {
                    currentState.transform.GetChild(0).transform.localScale = new Vector3(0.02f, (((float)currentData.GetConfirmedCases((int)daysFromStart)) / max)*maxHeight, 0.02f);
                    currentState.transform.GetChild(0).transform.localPosition = new Vector3(0, currentState.transform.GetChild(0).transform.localScale.y / 2, 0);
                    currentState.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color32((byte)(255*(int)((float)currentData.GetConfirmedCases((int)daysFromStart)) / max), 0, 0,255);
                }
            }
        }
    }
}
