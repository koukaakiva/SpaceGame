tool
extends "res://addons/EventGraph/scripts/Node.gd";

const type = "Key Saver";

func save_data():
	return {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y,
		"key" : $KeyEdit.text,
		"value" : $ValueEdit.text,
		"refName" : $ReferenceNameEdit.text
	};

func load_data(data):
	$KeyEdit.text = data.key
	$ValueEdit.text = data.value
	$ReferenceNameEdit.text = data.refName