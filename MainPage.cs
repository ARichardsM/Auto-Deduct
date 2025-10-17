using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class MainPage : Node
{
    public DeductorScript z;

	// Variables
	List<LineEdit> entries = new List<LineEdit>();
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
        int compType = GetNode<OptionButton>("Panel/Box/ButtonBox/SelectButton").Selected;
        bool isVerbose = (GetNode<OptionButton>("Panel/Box/ButtonBox/OutputButton").Selected == 1);

        // Store valid entries
        foreach (LineEdit ent in entries)
			if (ent.Text != "")
			{
                textEntries.Add(ent.Text);
			}

        switch (compType)
        {
            case 0:
                // Run Basic Competition
                if (isVerbose)
                {
                    (runText, runCode) = DeductorScript.Basic(textEntries, runCode);
                }
                else
                {
                    runText = DeductorScript.Basic(textEntries);
                    runCode = "Done";
                }
                break;
            case 1:
                GD.Print("Round Robin");
                runCode = "Done";
                break;
            case 2:
                GD.Print("Double Elimination");
                runCode = "Done";
                break;
        }

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
            GetNode<Panel>("Panel").Visible = true;
            GetNode<Panel>("Panel2").Visible = false;
        } else
		{
            GetNode<Panel>("Panel").Visible = false;
            GetNode<Panel>("Panel2").Visible = true;
        }
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
