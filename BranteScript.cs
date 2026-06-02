using Godot;
using System;
using System.Collections.Generic;

public partial class BranteScript
{
    // Class to track entity statistics
    private class AmongPlayer
    {
        public string name;
        public bool isCrew;
        public bool isActive;

        // Empty Constructor
        public AmongPlayer()
        {
            isCrew = true;
            isActive = true;
        }
    }

    // Run a verbose double round robin competition
    public static (string, string) Double(List<string> names, string code)
    {
        return ("", "");
    }

    // Initialize
    public static string init(List<string> names, string code)
    {
        return "";
    }
}
