// Author(s): Ryder Roth, Jack Hael, Shaira Alam
// Date: 10/11/2020
// Description: Holds covid 19 data based, and is 
//              currently setup for the query we are asking,
//              but could be modified to support different 
//              queries in later versions.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData
{
    private ArrayList dates;
    private String country;
    private String state;
    private String coordinates;    //coordinates
    private ArrayList totalConfirmedCases;
    private ArrayList totalDeceased;
    private int currentIndex;

    //constructor
    public StateData()
    {
        dates = new ArrayList();
        country = "";
        state = "";
        coordinates = "";
        totalConfirmedCases = new ArrayList();
        totalDeceased = new ArrayList();
        currentIndex = 0;   //insertion index
    }

    //adds a row of the table to the data for the state that is having data being added to.
    public void AddRow(String country, String state, String coordinates, String date, int confirmedCases, int deceased) //string format from data grabber
    {
        if (this.country.CompareTo("") == 0)
        {
            this.country = country;
        }
        if (this.state.CompareTo("") == 0)
        {
            this.state = state;
        }
        if (this.coordinates.CompareTo("") == 0)
        {
            this.coordinates = coordinates;
        }
        //insert date at index
        dates.Add(date);
        //insert confirmed cases at index
        totalConfirmedCases.Add(confirmedCases);
        //insert deceased at index
        totalDeceased.Add(deceased);
        
        currentIndex++;
    }

    //method to check if the data insertion we are planning on doing is valid for the current statedata
    public bool IsValidInsertion(String state)
    {
        if (state.CompareTo("") == 0 || state.CompareTo(this.state) == 0)
        {
            return true;
        }
        return false;
    }

    public String GetDate(int index)
    {
        return (String)dates[index];
    }

    public String GetCountry()
    {
        return country;
    }

    public String GetState()
    {
        return state;
    }

    public String GetCoordinates()
    {
        return coordinates;
    }

    public int GetConfirmedCases(int index)
    {
        return (int)totalConfirmedCases[index];
    }

    public int GetCaseCount()
    {
        return (int)totalConfirmedCases.Count;
    }

    public int GetDeceased(int index)
    {
        return (int)totalDeceased[index];
    }

    public int GetSize()
    {
        return currentIndex;
    }

    //to string method for debugging purposes and testing if the state data is being maintained correctly.
    public String ToString()
    {
        String stringToReturn = "";
        for(int i = 0; i < currentIndex; i++)
        {
            stringToReturn += $"Date: {GetDate(i)} \t| Country: {country} \t| State: {state} \t| Coordinates: {coordinates} \t| Total Confirmed: {GetConfirmedCases(i)} \t| Total Deaths: {GetDeceased(i)}\n";
        }
        return stringToReturn;
    }

}
