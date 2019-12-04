using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class ScenarioController
{

    private static readonly string[] counties = {"hawaii", "maui", "oahu", "kauai"};
    private static readonly int[] slrRange = {0, 11};

    private static int slrFeet = 0;
    private static string county = "oahu";

    public static bool setSLR(int feet) {
        bool valid = false;
        if(feet >= slrRange[0] && feet < slrRange[1]) {
            slrFeet = feet;
            valid = true;
        }
        return valid;
    }


    public static int getSLRFeet() {
        return slrFeet;
    }

    public static float getSLRMeters() {
        return feetToMeters(slrFeet);
    }

    private static float feetToMeters(int feet) {
        return feet * 0.3048f;
    }

    public static SLRTags getSLRTags() {
        SLRTags tags;
        tags.low = slrFeet.ToString() + "ft_low";
        tags.slr = slrFeet.ToString() + "ft_slr";
        return tags;
    }

    public static bool setCounty(string county) {
        bool valid = false;
        if(counties.Contains(county)) {
            ScenarioController.county = county;
            valid = true;
        }
        return valid;
    }

    public static string getCounty() {
        return county;
    }

}

public struct SLRTags {
    public string low;
    public string slr;
}

