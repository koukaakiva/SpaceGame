extends Control

onready var label = $Panel/NinePatchRect/MarginContainer/HBoxContainer/RichTextLabel;
var text = "";

func _ready():
	label.set_text(text);
	#var height = label.get_line_count() * label.get_line_height();
	var height = label.get_line_count() * 24.5;
	if(height < 144):
		height = 144;
	var scale = Vector2(0, height);
	self.rect_min_size = scale;
