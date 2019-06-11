extends Control

var dialogueNodes = {
	"Dialogue": load("res://addons/dialogue_tree/scenes/BasicDialogue.tscn"),
	"Choice": load("res://addons/dialogue_tree/scenes/ChoiceDialogue.tscn"),
	"Conditional": load("res://addons/dialogue_tree/scenes/ConditonalDialogue.tscn"),
	"Random": load("res://addons/dialogue_tree/scenes/RandomDialogue.tscn"),
	"Key Setter": load("res://addons/dialogue_tree/scenes/KeySetterDialogue.tscn"),
	"Key Branch": load("res://addons/dialogue_tree/scenes/KeyBranchDialogue.tscn")
};


func _ready():
	print(dialogueNodes["Dialogue"]);
	var plop = dialogueNodes.get("Dialogue").instance();
	print(dialogueNodes.get("Dialogue"));
	print(plop.get_filename() == dialogueNodes.get("Dialogue").get_path());
