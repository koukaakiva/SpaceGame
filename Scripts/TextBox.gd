extends Control

#TODO: Set these based on the font.
const CHAR_HEIGHT = 25;
const CHAR_WIDTH = 24;
const CONTENT_MIN_HEIGHT = 140;
const BORDER_WIDTH = 8;

func initiate(icon_type, text: String):
	var textbox = get_child(icon_type).duplicate();
	var label = textbox.get_node("NinePatchRect/MarginContainer/ContentContainer/Label");
	var LABLE_WIDTH = label.get_size().x;
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
	textbox.rect_min_size.y = height;
	return textbox;
	