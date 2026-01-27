using Godot;
using System;
using System.Collections.Generic;

public partial class MainPage : Node
{
	// Variables
	List<LineEdit> entries = new List<LineEdit>();
    RichTextLabel ReadOutLabel = null;
    string runCode = null;

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
        int compType = GetNode<OptionButton>("InputPanel/Box/ButtonBox/SelectButton").Selected;
        bool isVerbose = (GetNode<OptionButton>("InputPanel/Box/ButtonBox/OutputButton").Selected == 1);

        // Store valid entries
        foreach (LineEdit ent in entries)
			if (ent.Text != "")
			{
                textEntries.Add(ent.Text);
			}

        switch (compType)
        {
            case 0:
                // Run Single Round Robin Competition
                if (isVerbose)
                    (runText, runCode) = RoundRobinScript.Single(textEntries, runCode);
                else
                    (runText, runCode) = RoundRobinScript.Simplify(textEntries, 1);

                break;
            case 1:
                // Run Double Round Robin Competition
                if (isVerbose)
                    (runText, runCode) = RoundRobinScript.Double(textEntries, runCode);
                else
                    (runText, runCode) = RoundRobinScript.Simplify(textEntries, 2);

                break;
            case 2:
                // Run Single Elimination Tournament
                if (isVerbose)
                    (runText, runCode) = TournamentScript.Single(textEntries, runCode);
                else
                    (runText, runCode) = TournamentScript.Simplify(textEntries, 1);

                break;
            case 3:
                // Run Double Elimination Tournament
                if (isVerbose)
                    (runText, runCode) = TournamentScript.Double(textEntries, runCode);
                else
                    (runText, runCode) = TournamentScript.Simplify(textEntries, 2);

                break;
            default:
                GD.Print("New Competition");
                runCode = "Done";
                break;
        }

        // Swap to print-out panel
        SwapVisible(false);

		// Write text to screen
		//Label screenText = GetNode<Label>("Panel2/Box/Label");
        ReadOutLabel.Text = runText;
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
                runCode = null;
                Run();
                break;
            // Continue
            case 3:
                if (runCode == "Done")
                {
                    SwapVisible(true);
                    runCode = null;
                    return;
                }

                Run();
                break;
        }
	}

	// Set visibile panel
	public void SwapVisible(bool isSetup)
	{
		if (isSetup)
		{
            GetNode<Panel>("InputPanel").Visible = true;
            GetNode<Panel>("OutputPanel").Visible = false;
        } else
		{
            GetNode<Panel>("InputPanel").Visible = false;
            GetNode<Panel>("OutputPanel").Visible = true;
        }
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		SwapVisible(true);

        //GetNode<RichTextLabel>("Panel2/Box/RichTextLabel").AppendText("[color=#ffff0f]This text is white[/color]");
        GetNode<LineEdit>("InputPanel/Box/Split7/Control/Box/Text").Text = "WOW";

        // Load the read out panel
        ReadOutLabel = GetNode<RichTextLabel>("OutputPanel/Box/Label");

        // Load each Line node into the array
        for (int i = 1; i < 6; i++)
            for (int j = 1; j < 3; j++)
                entries.Add(GetNode<LineEdit>("InputPanel/Box/Split" + i + "/LineEdit" + j));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
