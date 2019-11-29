using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

//Controlador del los diálogos



private Queue<string> sentences;
	void Start () {
		sentences = new Queue <string>();
		
	}

	void Update () {



		
	}
	//Inicia el dialogo pasado por parámetro cuando el player interactua con un  NPC
	public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("isOpen", true);// abre dialogo
		nameText.text = dialogue.name;//asigna el nombre del npc
		sentences.Clear();// limpia dialogos anteriores
		//
		foreach (string sentence in dialogue.sentences)
		
		{
			sentences.Enqueue(sentence);
			
		}
		DisplayNextSentence ();

	}

	public void DisplayNextSentence () {


		if (sentences.Count == 0)//Finaliza el diálogo cuando no hay más senteces
		{
			EndDialogue ();
			return;
			
		}

		string sentence = sentences.Dequeue();
		dialogueText.text = sentence;
	}

	public void  EndDialogue () 
	{
		animator.SetBool("isOpen", false);
		}



	[SerializeField]
	private Text nameText;
	[SerializeField]
	private Text dialogueText;
	[SerializeField]
	private Animator animator;
}
 