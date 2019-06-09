extends Control

#TODO: Set these based on the font.
const CHAR_HEIGHT = 25;
const CHAR_WIDTH = 24;
const CONTENT_MIN_HEIGHT = 128;
const BORDER_WIDTH = 8;
const ICONLESS_LABLE_WIDTH = 968;
const ICONED_LABLE_WIDTH = ICONLESS_LABLE_WIDTH - 132;
onready var contentContainer = $Panel/NinePatchRect/MarginContainer/ContentContainer;
onready var label = $Panel/NinePatchRect/MarginContainer/ContentContainer/Label;
onready var icon = load("res://Scenes/Icon.tscn");
#TODO: Divide this by the size of a char and count up by 1 below.
onready var ready = true;
var LABLE_WIDTH;

func initiate(icon_type, text: String) -> void:
	if(!ready):
		yield(self, "ready");
	if(icon_type == GLOBAL.textbox_icon_type.LEFT):
		contentContainer.add_child(icon.instance());
		contentContainer.move_child(label, 1);
		LABLE_WIDTH = ICONED_LABLE_WIDTH;
	elif(icon_type == GLOBAL.textbox_icon_type.RIGHT):
		contentContainer.add_child(icon.instance());
		LABLE_WIDTH = ICONED_LABLE_WIDTH;
	else:
		LABLE_WIDTH = ICONLESS_LABLE_WIDTH;
	label.set_text(text);
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
	if(wordLength > remainingWidth):
		height += 1;
	if((text.length() != 0) && (text[text.length() - 1] == '\n')):
		height -= 1;
	height *= CHAR_HEIGHT;
	if(height < CONTENT_MIN_HEIGHT):
		height = CONTENT_MIN_HEIGHT;
	height += (BORDER_WIDTH * 2);
	self.rect_min_size = Vector2(0, height);
	if(icon_type == GLOBAL.textbox_icon_type.NONE):
		label.rect_min_size = Vector2(LABLE_WIDTH, height);
	