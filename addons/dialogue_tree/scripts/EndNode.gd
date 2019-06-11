tool
extends "res://addons/dialogue_tree/scripts/Dialogue.gd";

const type = "End";

func save_data():
	return {
		"type" : type,
		"name" : name,
		"rect_x" : rect_position.x,
		"rect_y" : rect_position.y,
		"rect_size_x" : rect_size.x,
		"rect_size_y" : rect_size.y
	};