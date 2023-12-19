using Godot;
using System;
using System.Collections.Generic;

public partial class dialogue : PanelContainer
{
	private static Dictionary<string, string[]> _CONVERSATIONS
		= new Dictionary<string, string[]>()
		{
			{ "intro", new string[] {
				"BONJOUR BONSOIR ET BIENVENUE",
				"DANS LA FINALE DEEEE",
				"QUI VEUT GAGNER RIEN ??????",
				"Aujourd'hui c'est bob et alice qui s'affrontent pour un total astronomique...",
				"de 0 euros",
				"Alors la question est la suivante :",
				"j'ai la flemme de formuler la question vous la connaissez",
				"show_input",
				"test _input"
			} }
		};
	private HBoxContainer input;
	private HBoxContainer texte;

	[Export] private TextureRect _icon;
	[Export] private Label _label;
	[Export] private TextEdit _input;

	private string _conversationID;
	private int _conversationSentenceIdx;

	public override void _Ready()
	{
		this.input = GetNode<HBoxContainer>("input");
		this.texte = GetNode<HBoxContainer>("texte");
		this.input.Hide();
		Hide();
	}

	public void ShowDialogue(string npc, string conversationID)
	{
		Show();
		_icon.Texture = GD.Load<Texture2D>($"res://icons/{npc}.png");
		_ShowDialogueText(_CONVERSATIONS[conversationID][0]);
		
		_conversationID = conversationID;
		_conversationSentenceIdx = 1;
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
		
		if (_conversationSentenceIdx == sentences.Length)
			Hide();
			return;
			
		if (sentences[_conversationSentenceIdx++] == "show_input") {
			showInput();
			return;
		}
		_conversationSentenceIdx -=1;
		if (_conversationSentenceIdx < sentences.Length)
			_ShowDialogueText(sentences[_conversationSentenceIdx++]);

	}
	
	private void _on_button_pressed()
	{
		_GetNextDialogue();
	}
	
	private void showInput() {
		input.Show();
		texte.Hide();
	}
	
	private void hideInput() {
		input.Hide();
		texte.Show();
	}
	
	private void _on_input_button_pressed()
	{
		hideInput();
		_GetNextDialogue();
	}
}
