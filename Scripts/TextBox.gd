extends Control

#TODO: Set these based on the font.
const CHAR_HEIGHT = 25;
const CHAR_WIDTH = 24;
const CONTENT_MIN_HEIGHT = 128;
const BORDER_WIDTH = 8;
onready var label = $Panel/NinePatchRect/RichTextLabel;
#TODO: Divide this by the size of a char and count up by 1 below.
onready var LABLE_WIDTH = label.get_size().x;
onready var ready = true;

func setText(text: String) -> void:
	if(!ready):
		yield(self, "ready");
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
	self.rect_min_size = Vector2(0, height + (BORDER_WIDTH * 2));
	label.rect_min_size = Vector2(LABLE_WIDTH, height);