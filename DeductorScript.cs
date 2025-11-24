using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class TournamentScript
{
    // Class to keep track of entity statistics [Tourney] 
    private class resultTracker
    {
        public string ID;
        public int Wins;
        public bool isEliminated;

        // Empty Constructor
        public resultTracker()
        {
            Wins = 0;
            isEliminated = false;
        }

        // String Constructor
        public resultTracker(string name) : this()
        {
            ID = name;
        }
    }

    // Fisher-Yates Shuffle
    private static List<int> Shuffle(List<int> list)
    {
        Random rand = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    // Decodify run code
    private static void decodify(List<resultTracker> entities, string runCode)
    {
        // Check for first run
        if (runCode != null)
        {
            // Split the run code by entities
            string[] entResults = runCode.Split("\n");

            for (int i = 0; i < entities.Count; i++)
            {
                // Split the entity code
                string[] score = entResults[i].Split(",");

                // Assign encoded values
                entities[i].Wins = int.Parse(score[0]);
                entities[i].isEliminated = bool.Parse(score[1]);
            }
        }
        return;
    }

    // Codify run code
    private static string codify(List<resultTracker> entities)
    {
        // Initialize
        string returnStr = "";

        // Encode each entity
        foreach (resultTracker ent in entities)
            returnStr += (ent.Wins + "," + ent.isEliminated + "\n");

        // Return encoded
        return returnStr;
    }

    // Run a simple competition
    public static string Simplify(List<string> names, byte elimNum)
    {
        // Variables
        string runText = "";
        string runCode = null;
        Func<List<string>, string, (string, string)> baseFunc = null;

        // Determine the base function

        /*
        switch (elimNum)
        {
            case 1:
                baseFunc = Single;
                break;
            case 2:
                baseFunc = Double;
                break;
        }
        */

        baseFunc = SingleElim;

        // Run until done
        do
        {
            (runText, runCode) = baseFunc(names, runCode);
        } while (runCode != "Done");

        // Return results
        return runText;
    }

    // Run a verbose single elimination
    public static (string, string) SingleElim(List<string> names, string code)
    {
        // Verify
        List<int> validNum = new List<int> { 2, 4, 8 };

        if (!validNum.Contains(names.Count))
        {
            return ("Invalid Number of Entities", "Done");
        }

        // Variables
        string runText = "";
        string runCode = "";
        List<int> competitors = new List<int>();
        List <resultTracker> validEntries = new List<resultTracker>();

        // Store valid entries
        foreach (string ent in names)
            validEntries.Add(new resultTracker(ent));

        // Decodify run code
        decodify(validEntries, code);

        // Record Valid Competitors
        for (int i = 0; i < validEntries.Count; i++)
        {
            if (validEntries[i].isEliminated == false)
                competitors.Add(i);
        }

        // Last Run Check
        if (competitors.Count <= 1)
        {
            // Sort the Entries
            validEntries = validEntries.OrderByDescending(x => x.Wins).ToList();

            // Return Summary
            runText = "Final Results\n";
            foreach (resultTracker ent in validEntries)
                runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W\n");

            // Mark Done
            runCode = "Done";

            // Return
            return (runText, runCode);
        }

        // Shuffle
        competitors = Shuffle(competitors);

        // Determine results
        for (int i = 0; i < (competitors.Count / 2); i++)
        {
            // Determine Current Competitors 
            runText += validEntries[competitors[i * 2]].ID + " VS " + validEntries[competitors[i * 2 + 1]].ID + ": ";

            // Pick Winner
            Random rand = new Random();
            int whoWins = rand.Next(2);

            // Determine Winner
            switch (whoWins)
            {
                case 0:
                    validEntries[competitors[i * 2]].Wins++;
                    runText += validEntries[competitors[i * 2]].ID + " Wins!\n";
                    validEntries[competitors[i * 2 + 1]].isEliminated = true;
                    break;
                case 1:
                    validEntries[competitors[i * 2 + 1]].Wins++;
                    runText += validEntries[competitors[i * 2 + 1]].ID + " Wins!\n";
                    validEntries[competitors[i * 2]].isEliminated = true;
                    break;
            }
        }

        // Return Results
        runText += "\nCurrent Results\n";
        foreach (resultTracker ent in validEntries)
        {
            runText += ("ID: " + ent.ID);

            switch (ent.isEliminated)
            {
                case true:
                    runText += (", Status: Eliminated");
                    break;
                case false:
                    runText += (", Status: Valid");
                    break;
            }

            runText += (", Score: " + ent.Wins + "\n");
        }

        // Create a run code
        runCode = codify(validEntries);

        // Return
        return (runText, runCode);
    }
}
