using Godot;
using System;

public partial class plateau : Node2D
{
	
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var dialogueBox = GetNode<PanelContainer>("dialogue");
		dialogueBox.Call("ShowDialogue", "icon", "intro");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
