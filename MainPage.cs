using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainPage : Node
{
	// Variables
	List<LineEdit> entries = new List<LineEdit>();
    private class simpleTracker
    {
		public simpleTracker()
		{

		}
		public string ID;
        public int Wins;
        public int Losses;
    }

    // Wipe the entry text
    private void Wipe()
	{
        foreach (LineEdit ent in entries)
            ent.Text = "";
    }

    // Run the sim
    private void Run()
    {
		// Variables
        List<string> textEntries = new List<string>();
		String runText = "";
        int compType;

		// Store valid entries
        foreach (LineEdit ent in entries)
			if (ent.Text != "")
			{
                textEntries.Add(ent.Text);
			}

        // Determine competion type
        compType = GetNode<OptionButton>("Panel/Box/ButtonBox/SelectButton").Selected;

        // Compete
        switch (compType)
        {
            case 0:
                GD.Print("1");
                break;
            case 1:
                GD.Print("2");
                break;
        }
        runText = basicCompete(textEntries);

		// Swap to print-out panel
        SwapVisible(false);

		// Write text to screen
		Label screenText = GetNode<Label>("Panel2/Box/Label");
		screenText.Text = runText;
    }

    // Handle button presses
    public void ButtonPressed(int selected)
	{
		switch (selected)
		{
			// Wipe Line nodes
			case 0:
				Wipe();
				break;
			// Run Simulation
			case 1:
				Run();
				break;
            // Restart
            case 2:
                Run();
                break;
            // Continue
            case 3:
                SwapVisible(true);
                break;
        }
	}

	// Set visibile panel
	public void SwapVisible(bool isSetup)
	{
		if (isSetup)
		{
            GetNode<Panel>("Panel").Visible = true;
            GetNode<Panel>("Panel2").Visible = false;
        } else
		{
            GetNode<Panel>("Panel").Visible = false;
            GetNode<Panel>("Panel2").Visible = true;
        }
	}

    // Run a basic competition
    private string basicCompete(List<string> names)
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		SwapVisible(true);

        // Load each Line node into the array
        for (int i = 1; i < 6; i++)
            for (int j = 1; j < 3; j++)
                entries.Add(GetNode<LineEdit>("Panel/Box/Split" + i + "/LineEdit" + j));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
