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
        public int isEliminated;

        // Empty Constructor
        public resultTracker()
        {
            Wins = 0;
            isEliminated = 0;
        }

        // String Constructor
        public resultTracker(string name) : this()
        {
            ID = name;
        }
    }

    // Fisher-Yates Shuffle
    private static void Shuffle(List<int> list)
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

        return;
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
                entities[i].isEliminated = int.Parse(score[1]);
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
    public static (string, string) Simplify(List<string> names, byte elimNum)
    {
        // Variables
        string runText = "";
        string runCode = null;
        Func<List<string>, string, (string, string)> baseFunc = null;

        // Determine the base function
        switch (elimNum)
        {
            case 1:
                baseFunc = Single;
                break;
            case 2:
                baseFunc = Double;
                break;
        }

        // Run until done
        do
        {
            (runText, runCode) = baseFunc(names, runCode);
        } while (runCode != "Done");

        // Return results
        return (runText, runCode);
    }

    // Run a verbose single elimination
    public static (string, string) Single(List<string> names, string code)
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
            if (validEntries[i].isEliminated == 0)
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
        Shuffle(competitors);

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
                    validEntries[competitors[i * 2 + 1]].isEliminated = 1;
                    break;
                case 1:
                    validEntries[competitors[i * 2 + 1]].Wins++;
                    runText += validEntries[competitors[i * 2 + 1]].ID + " Wins!\n";
                    validEntries[competitors[i * 2]].isEliminated = 1;
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
                case 1:
                    runText += (", Status: Eliminated");
                    break;
                case 0:
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

    // Run a verbose single elimination
    public static (string, string) Double(List<string> names, string code)
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
        List<int> upper = new List<int>();
        List<int> lower = new List<int>();
        List<resultTracker> validEntries = new List<resultTracker>();

        // Store valid entries
        foreach (string ent in names)
            validEntries.Add(new resultTracker(ent));

        // Decodify run code
        decodify(validEntries, code);

        // Record Valid Competitors
        for (int i = 0; i < validEntries.Count; i++)
        {
            switch (validEntries[i].isEliminated)
            {
                case 1:
                    lower.Add(i);
                    break;
                case 0:
                    upper.Add(i);
                    break;
            }
        }

        // Final Runs Check
        if (upper.Count <= 1)
        {
            if (lower.Count == 0 || upper.Count == 0)
            {
                // Sort the Entries
                validEntries = validEntries.OrderByDescending(x => x.Wins).ToList();

                // Return Summary
                runText = "Final Results\n";
                foreach (resultTracker ent in validEntries) {
                    runText += ("ID: " + ent.ID + ", Score: " + ent.Wins + " W");

                    if (ent.isEliminated == 2)
                        runText += (" (Eliminated)\n");
                    else
                        runText += ("\n");
                }

                // Mark Done
                runCode = "Done";

                // Return
                return (runText, runCode);
            } else if (lower.Count == 1) {
                // Determine Current Competitors 
                runText = "Upper-Lower Results\n";
                runText += validEntries[upper[0]].ID + " VS " + validEntries[lower[0]].ID + ": ";

                // Pick Winner
                Random rand = new Random();
                int whoWins = rand.Next(2);

                // Determine Winner
                switch (whoWins)
                {
                    case 0:
                        validEntries[upper[0]].Wins++;
                        runText += validEntries[upper[0]].ID + " Wins!\n";
                        validEntries[lower[0]].isEliminated += 1;
                        break;
                    case 1:
                        validEntries[lower[0]].Wins++;
                        runText += validEntries[lower[0]].ID + " Wins!\n";
                        validEntries[upper[0]].isEliminated += 1;
                        lower.Add(upper[0]);
                        break;
                }

            }
        }

        // Shuffle upper bracket
        Shuffle(upper);
        runText += "Upper Bracket\n";

        // Determine results
        for (int i = 0; i < (upper.Count / 2); i++)
        {
            // Determine Current Competitors 
            runText += validEntries[upper[i * 2]].ID + " VS " + validEntries[upper[i * 2 + 1]].ID + ": ";

            // Pick Winner
            Random rand = new Random();
            int whoWins = rand.Next(2);

            // Determine Winner
            switch (whoWins)
            {
                case 0:
                    validEntries[upper[i * 2]].Wins++;
                    runText += validEntries[upper[i * 2]].ID + " Wins!\n";
                    validEntries[upper[i * 2 + 1]].isEliminated += 1;
                    lower.Add(upper[i * 2 + 1]);
                    break;
                case 1:
                    validEntries[upper[i * 2 + 1]].Wins++;
                    runText += validEntries[upper[i * 2 + 1]].ID + " Wins!\n";
                    validEntries[upper[i * 2]].isEliminated += 1;
                    lower.Add(upper[i * 2]);
                    break;
            }
        }

        // Shuffle lower bracket
        Shuffle(lower);
        runText += "Lower Bracket\n";

        // Determine results
        for (int i = 0; i < (lower.Count / 2); i++)
        {
            // Determine Current Competitors 
            runText += validEntries[lower[i * 2]].ID + " VS " + validEntries[lower[i * 2 + 1]].ID + ": ";

            // Pick Winner
            Random rand = new Random();
            int whoWins = rand.Next(2);

            // Determine Winner
            switch (whoWins)
            {
                case 0:
                    validEntries[lower[i * 2]].Wins++;
                    runText += validEntries[lower[i * 2]].ID + " Wins!\n";
                    validEntries[lower[i * 2 + 1]].isEliminated += 1;
                    break;
                case 1:
                    validEntries[lower[i * 2 + 1]].Wins++;
                    runText += validEntries[lower[i * 2 + 1]].ID + " Wins!\n";
                    validEntries[lower[i * 2]].isEliminated += 1;
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
                case 2:
                    runText += (", Status: Eliminated");
                    break;
                case 1:
                    runText += (", Status: Lower Bracket");
                    break;
                case 0:
                    runText += (", Status: Upper Bracket");
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
