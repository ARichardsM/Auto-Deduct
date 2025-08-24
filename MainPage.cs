using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainPage : Node
{
	// Variables
	List<LineEdit> entries = new List<LineEdit>();
    private class results
    {
		public results()
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
		List<results> validEntries = new List<results>();

		// Store valid entries
        foreach (LineEdit ent in entries)
			if (ent.Text != "")
			{
				results newResult = new results();
                newResult.ID = ent.Text;
                newResult.Wins = 0;
                newResult.Losses = 0;
                validEntries.Add(newResult);
			}

		// Compete
		for (int i = 0; i < validEntries.Count; i++)
		{
			for (int j = 0; j < validEntries.Count; j++)
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

		// Print Entries
		foreach (results ent in validEntries)
			GD.Print("ID: " + ent.ID + ", Score: " + ent.Wins + " W - " + ent.Losses + " L");
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
		}
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
