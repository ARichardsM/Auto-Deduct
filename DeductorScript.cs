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

    public static void print()
	{
        GD.Print("1");
    }

    // Run a basic competition
    public static string Basic(List<string> names)
    {
        // Variables
        String runText = "";
        List<simpleTracker> validEntries = new List<simpleTracker>();

        // Store valid entries
        foreach (string ent in names)
        {
            simpleTracker newResult = new simpleTracker();
            newResult.ID = ent;
            newResult.Wins = 0;
            newResult.Losses = 0;
            validEntries.Add(newResult);
        }

        // Determine results
        for (int i = 0; i < names.Count; i++)
        {
            for (int j = 0; j < names.Count; j++)
            {
                // Skip Self
                if (i == j)
                    continue;

                // Pick Winner
                Random rand = new Random();
                int whoWins = rand.Next(2);

                // Adjust Results
                validEntries[i].Wins += whoWins;
                validEntries[j].Losses += whoWins;

                validEntries[i].Losses += (1 - whoWins);
                validEntries[j].Wins += (1 - whoWins);
            }
        }

        // Sort the Entries
        validEntries = validEntries.OrderByDescending(x => x.Wins).ToList();

        // Record Entries
        foreach (simpleTracker ent in validEntries)
            runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W - " + ent.Losses + " L\n");

        return runText;
    }


    // Run a verbose basic competition
    public static (string, string) BasicV(List<string> names)
    {
        // Variables
        String runText = "";
        List<simpleTracker> validEntries = new List<simpleTracker>();

        // Store valid entries
        foreach (string ent in names)
        {
            simpleTracker newResult = new simpleTracker();
            newResult.ID = ent;
            newResult.Wins = 0;
            newResult.Losses = 0;
            validEntries.Add(newResult);
        }

        // Determine results
        for (int i = 0; i < names.Count; i++)
        {
            for (int j = 0; j < names.Count; j++)
            {
                // Skip Self
                if (i == j)
                    continue;

                // Pick Winner
                Random rand = new Random();
                int whoWins = rand.Next(2);

                // Adjust Results
                validEntries[i].Wins += whoWins;
                validEntries[j].Losses += whoWins;

                validEntries[i].Losses += (1 - whoWins);
                validEntries[j].Wins += (1 - whoWins);
            }
        }

        // Sort the Entries
        validEntries = validEntries.OrderByDescending(x => x.Wins).ToList();

        // Record Entries
        foreach (simpleTracker ent in validEntries)
            runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W - " + ent.Losses + " L\n");

        return (runText, "");
    }
}
