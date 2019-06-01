extends Control

const CHAR_HEIGHT = 25;
const CHAR_WIDTH = 24;
const LABLE_WIDTH = 876;
onready var label = $Panel/NinePatchRect/MarginContainer/HBoxContainer/RichTextLabel;
#onready var LABLE_WIDTH = label.get_size().x;
onready var ready = true;

func _ready():
	pass;

func setText(text: String) -> void:
	if(!ready):
		yield(self, "ready");
	label.set_text(text);
	#var height = (label.get_line_count() * CHAR_HEIGHT) - 10;
	var height = 1;
	var remainingWidth = LABLE_WIDTH;
	var wordLength = 0;
	for character in text:
		wordLength += CHAR_WIDTH;
		match character:
			' ', '-':
				if(wordLength > remainingWidth):
					height += 1;
					remainingWidth = LABLE_WIDTH - wordLength;
				else:
					remainingWidth -= wordLength;
				wordLength = 0;
			'\n':
				if((wordLength - CHAR_WIDTH) > remainingWidth):
					height += 1;
				height += 1;
				remainingWidth = LABLE_WIDTH;
				wordLength = 0;
	height *= CHAR_HEIGHT;
	if(height < 144):
		height = 144;
	#label.rect_min_size = Vector2(LABLE_WIDTH, 128);
	self.rect_min_size = Vector2(0, height);
	print(label.get_size().x);