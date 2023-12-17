using Godot;
using System;
using System.Collections.Generic;

public partial class dialogue : PanelContainer
{
	private static Dictionary<string, string[]> _CONVERSATIONS
		= new Dictionary<string, string[]>()
		{
			{ "intro", new string[] {
				"hello world",
				"WOWOWOW LE DIALOGUE IL CONTINUE",
			} }
		};

	[Export] private TextureRect _icon;
	[Export] private Label _label;

	private string _conversationID;
	private int _conversationSentenceIdx;

	public override void _Ready()
	{
		Hide();
	}

	public void ShowDialogue(string npc, string conversationID)
	{
		Show();
		_icon.Texture = GD.Load<Texture2D>($"res://icons/{npc}.png");
		_ShowDialogueText(_CONVERSATIONS[conversationID][0]);
		
		_conversationID = conversationID;
		_conversationSentenceIdx = 1; // for next time!
	}
	
	private async void _ShowDialogueText(string text)
	{

		float appearTime = 1.5f; // in seconds
		float appearSpeed = appearTime / (float)(text.Length - 1);

		_label.Text = "";
		foreach (char c in text) {
			_label.Text += c;
			await ToSignal(GetTree().CreateTimer(appearSpeed), Timer.SignalName.Timeout);
		}
	}
	
	private void _GetNextDialogue()
	{
		string[] sentences = _CONVERSATIONS[_conversationID];
		if (_conversationSentenceIdx < sentences.Length)
			_ShowDialogueText(sentences[_conversationSentenceIdx++]);
		else
			Hide();
	}
	
	private void _on_button_pressed()
	{
		_GetNextDialogue();
	}
}






