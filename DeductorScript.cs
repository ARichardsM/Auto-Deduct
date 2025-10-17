using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class DeductorScript
{
    // Class to keep track of entity statistics [Basic] 
    private class simpleTracker
    {
        public string ID;
        public int Wins;
        public int Losses;

        // Empty Constructor
        public simpleTracker()
        {
            Wins = 0;
            Losses = 0;
        }

        // String Constructor
        public simpleTracker(string name) : this()
        {
            ID = name;
        }
    }

    // Run a simple basic competition
    public static string Basic(List<string> names)
    {
        // Variables
        string runText = "";
        string runCode = null;

        (runText, runCode) = DeductorScript.Basic(names, runCode);

        while (runCode != "Done")
        {
            (runText, runCode) = DeductorScript.Basic(names, runCode);
        }
        return runText;
    }


    // Run a verbose basic competition
    public static (string, string) Basic(List<string> names, string code)
    {
        // Variables
        string runText = "";
        string runCode = "";
        int currEnt = -1; 
        List <simpleTracker> validEntries = new List<simpleTracker>();

        // Store valid entries
        foreach (string ent in names)
            validEntries.Add(new simpleTracker(ent));

        // Decodify run code
        if (code != null) {
            currEnt = int.Parse(code.Split("\n")[0]);
            string[] entResults = code.Split("\n")[1..^1];

            for (int i = 0; i < names.Count; i++)
            {
                string[] score = entResults[i].Split(",");

                validEntries[i].Wins = int.Parse(score[0]);
                validEntries[i].Losses = int.Parse(score[1]);
            }
        }

        // Last Run Check
        if (++currEnt >= names.Count)
        {
            // Sort the Entries
            validEntries = validEntries.OrderByDescending(x => x.Wins).ToList();

            // Return Summary
            runText = "Final Results\n";
            foreach (simpleTracker ent in validEntries)
                runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W - " + ent.Losses + " L\n");

            // Mark Done
            runCode = "Done";

            return (runText, runCode);
        }

        // Determine results
        for (int i = 0; i < names.Count; i++)
        {
            // Skip Self
            if (i == currEnt)
                continue;

            // Run 
            runText += validEntries[currEnt].ID + " VS " + validEntries[i].ID + ": ";

            // Pick Winner
            Random rand = new Random();
            int whoWins = rand.Next(2);

            // Declare Winner
            string[] whoWon = { validEntries[currEnt].ID, validEntries[i].ID };
            runText += whoWon[whoWins] + " Wins!\n";

            // Adjust Results
            validEntries[i].Wins += whoWins;
            validEntries[currEnt].Losses += whoWins;

            validEntries[i].Losses += (1 - whoWins);
            validEntries[currEnt].Wins += (1 - whoWins);
        }

        // Mark Progress
        runCode += currEnt + "\n";

        // Return Results
        runText += "\nCurrent Results\n";
        foreach (simpleTracker ent in validEntries)
        {
            runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W - " + ent.Losses + " L\n");
            runCode += (ent.Wins + "," + ent.Losses + "\n");
        }

        return (runText, runCode);
    }
}
