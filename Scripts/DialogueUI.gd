extends Control

onready var scrollContainer = $MarginContainer/ScrollContainer;
onready var contentContainer = $MarginContainer/ScrollContainer/VBoxContainer;
onready var textbox = preload("res://Scenes/Textbox.tscn").instance();
onready var button = preload("res://Scenes/Button.tscn");
enum textbox_icon_type {NONE, LEFT, RIGHT};
var activeButtons = [];
 #♪

func _ready():
	pass;

func addTextBox(var icon_type, var text):
	var instance = textbox.initiate(icon_type, text);
	contentContainer.add_child(instance);
	yield(get_tree(), "idle_frame");
	scrollContainer.scroll_vertical += (instance.rect_size.y + 5);

func _on_EventManager_Dialogue_Next(ref, actor, text):
	var iconType;
	if(actor == "You"):
		iconType = textbox_icon_type.LEFT;
	elif(actor == ""):
		iconType = textbox_icon_type.NONE;
	else:
		iconType = textbox_icon_type.RIGHT;
	addTextBox(iconType, text);
	$Timer.start();

func _on_EventManager_Choice_Next(ref, branches):
	for i in range(branches.size()):
		activeButtons.append(button.instance());
		activeButtons[i].text = branches[i];
		activeButtons[i].connect("pressed", self, "_on_choice_pressed", [i]);
		contentContainer.add_child(activeButtons[i]);
		yield(get_tree(), "idle_frame");
		scrollContainer.scroll_vertical += (activeButtons[i].rect_size.y + 5);

func _on_choice_pressed(choice):
	for button in activeButtons:
		button.disabled = true;
		button.disconnect("pressed", self, "_on_choice_pressed");
	activeButtons = [];
	get_node("../EventManager").next(choice);

func _on_Timer_timeout():
	get_node("../EventManager").next();

func _on_EventManager_Event_Ended():
	print("Dialogue ended.");

func _on_EventManager_Object_Granted(ref, object):
	addTextBox(textbox_icon_type.NONE, "Ganined " + object.name + "\n" + object.description);
	$Timer.start();
