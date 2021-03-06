tool
extends "res://addons/EventGraph/scripts/Node.gd";

const type = "Dialogue";

func save_data():
	return {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y,
		"actor" : $ActorNameEdit.text,
		"dialogue" : $DialogueEdit.text,
		"refName" : $ReferenceNameEdit.text
	};

func load_data(data):
	$ActorNameEdit.text = data.actor;
	$DialogueEdit.text = data.dialogue;
	$ReferenceNameEdit.text = data.refName;
	$DialogueEdit.rect_min_size.y = data.rect_size_y - 78;


func _on_BasicDialogue_resize_request(new_minsize):
	$DialogueEdit.rect_min_size.y = new_minsize.y - 78;
