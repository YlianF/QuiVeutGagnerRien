using Godot;
using System;

public partial class plateau : Node2D
{
	private PanelContainer dialogueBox;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.dialogueBox = GetNode<PanelContainer>("dialogue");
		firstRound();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public void firstRound() {
		this.dialogueBox.Call("ShowDialogue", "icon", "intro");
	}
}
