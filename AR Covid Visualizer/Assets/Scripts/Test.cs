using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I am running!");
        DataGrabber test = new DataGrabber("direct - volt - 292119", "bigquery-public-data", "covid19_open_data", "covid19_open_data");
        test.AddSelect("date");
        test.AddSelect("country_code");
        test.AddSelect("subregion1_name");
        test.AddSelect("location_geometry");
        test.AddSelect("cumulative_confirmed");
        test.AddSelect("cumulative_deceased");
        test.AddSelect("new_confirmed");
        test.AddSelect("new_deceased");
        test.AddWhere("date > \"2020 - 09 - 01\"");
        test.AddWhere("date < \"2020-09-30\"");
        test.AddWhere("subregion1_code IS NULL");
        test.AddWhere("subregion2_code IS NULL");
        test.AddWhere("country_code = \"US\"");
        test.AddWhere("cumulative_deceased IS NOT NULL");
        ArrayList dataTest = new ArrayList();
        dataTest = test.GetData();
        int i = 0;
        while (i < dataTest.Capacity)
        {
            Debug.Log(dataTest[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("???");
    }
}
