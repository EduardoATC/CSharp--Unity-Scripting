using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

	//Inicializador de diálogos
	public void TriggerDialogue () 
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}
}
