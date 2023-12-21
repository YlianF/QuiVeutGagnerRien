using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Numerics;

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
				"On prend un échiquier, on y dépose 1 grain de riz sur la première case, puis 2 sur la deuxième, puis 4 sur la troisième etc etc...",
				"Combien y aura t-il de grains de riz sur la dernière case ?",
				"question_1",
			} }
		};
	private HBoxContainer input;
	private HBoxContainer texte;
	
	private string next_dialogue;
	

	[Export] private TextureRect _icon;
	[Export] private Label _label;
	[Export] private TextEdit _input;
	[Export] private Button _next_button;

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
		_next_button.Hide();
		float appearTime = 1.5f; // in seconds
		float appearSpeed = appearTime / (float)(text.Length - 1);

		_label.Text = "";
		foreach (char c in text) {
			_label.Text += c;
			await ToSignal(GetTree().CreateTimer(appearSpeed), Timer.SignalName.Timeout);
		}
		_next_button.Show();
	}
	
	private void _GetNextDialogue()
	{

		string[] sentences = _CONVERSATIONS[_conversationID];
		
		if (_conversationSentenceIdx == sentences.Length) {
			Hide();
			return;
		}
		
		if (sentences[_conversationSentenceIdx++] == "question_1") {
			this.next_dialogue = "question_1";
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
		Type thisType = this.GetType();
		MethodInfo theMethod = thisType.GetMethod(this.next_dialogue);
		theMethod.Invoke(this, null);
	}
	
	public void question_1() {
		Hide();
		double reponse_question = Math.Pow(2, 63);
		// on formate pour afficher le nombre en entier sans décimales
		string reponse_question_display = reponse_question.ToString("n0");
		string reponse_question_trim = reponse_question.ToString("f0");
		var reponse_question_int = BigInteger.Parse(reponse_question_trim);

		// alice répondra toujours la bonne réponse - 1
		var reponse_alice = reponse_question_int - 1;
		string reponse_alice_display = reponse_alice.ToString("n0");
		
		string reponse_joueur = _input.Text.Replace(" ", "");
		GD.Print(reponse_joueur);
		string[] outcome = new string[] {
			"bob répond " + reponse_joueur,
			"tandis qu'Alice à répondu " + reponse_alice_display,
			"et la réponse était...... " + reponse_question_display,
			"placeholder",
			"placeholder"
		};
		
		if (BigInteger.Parse(reponse_joueur) == reponse_question_int) {
			outcome[3] = "bob est le plus proche !";
			outcome[4] = "BOB EST NOTRE GRAND CHAMPION";
		} else {
			outcome[3] = "alice est la plus proche !";
			outcome[4] = "ALICE EST NOTRE GRANDE CHAMPIONE";
		}
		
		_CONVERSATIONS.Add("outcome_question_1", outcome);
		ShowDialogue("icon", "outcome_question_1");
	}
}
