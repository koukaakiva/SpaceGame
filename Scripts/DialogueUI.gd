extends Control

onready var scrollContainer = $MarginContainer/ScrollContainer;
onready var contentContainer = $MarginContainer/ScrollContainer/VBoxContainer;
onready var textbox = load("res://Scenes/Textbox.tscn");
onready var button = load("res://Scenes/Button.tscn");
enum textbox_icon_type {NONE, LEFT, RIGHT};
var activeButtons = [];
 #♪

func _ready():
	#var lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ultricies mi quis hendrerit dolor magna eget est lorem ipsum. Massa enim nec dui nunc mattis enim ut. Ut porttitor leo a diam sollicitudin tempor id. Morbi enim nunc faucibus a pellentesque sit. Purus faucibus ornare suspendisse sed nisi lacus. Tellus at urna condimentum mattis pellentesque id. Mattis rhoncus urna neque viverra justo. Sapien pellentesque habitant morbi tristique senectus et netus et. Gravida in fermentum et sollicitudin ac orci. Auctor neque vitae tempus quam pellentesque. Quam pellentesque nec nam aliquam sem et tortor consequat id. Aliquam sem fringilla ut morbi tincidunt. Enim tortor at auctor urna nunc id cursus metus aliquam. Eu lobortis elementum nibh tellus molestie nunc non blandit. Nullam vehicula ipsum a arcu cursus vitae congue.\n\nDui vivamus arcu felis bibendum ut. Eleifend quam adipiscing vitae proin sagittis nisl rhoncus mattis rhoncus. Facilisis magna etiam tempor orci eu lobortis elementum. Id velit ut tortor pretium viverra suspendisse potenti nullam ac. Nunc scelerisque viverra mauris in aliquam sem fringilla ut morbi. Arcu non sodales neque sodales ut etiam sit amet nisl. Ut etiam sit amet nisl. Justo donec enim diam vulputate ut pharetra sit amet aliquam. Elit ut aliquam purus sit amet luctus venenatis. Non diam phasellus vestibulum lorem sed risus ultricies. Justo donec enim diam vulputate ut pharetra sit amet aliquam. Cursus eget nunc scelerisque viverra mauris in aliquam sem fringilla. Posuere urna nec tincidunt praesent semper feugiat nibh. Et malesuada fames ac turpis egestas sed tempus urna et. Tempus urna et pharetra pharetra massa massa ultricies. Varius vel pharetra vel turpis nunc eget lorem dolor. Est ante in nibh mauris cursus mattis molestie a iaculis. Porttitor rhoncus dolor purus non enim praesent. Amet commodo nulla facilisi nullam vehicula. Urna et pharetra pharetra massa massa ultricies.\n\nEt netus et malesuada fames ac turpis egestas integer. Nunc lobortis mattis aliquam faucibus purus. Sed euismod nisi porta lorem mollis aliquam ut porttitor leo. Egestas egestas fringilla phasellus faucibus scelerisque eleifend donec. Ultricies lacus sed turpis tincidunt id aliquet risus feugiat in. Nibh nisl condimentum id venenatis a. Egestas diam in arcu cursus. Commodo nulla facilisi nullam vehicula ipsum. Proin sagittis nisl rhoncus mattis rhoncus urna. Est ante in nibh mauris cursus mattis molestie a. In metus vulputate eu scelerisque. Mi bibendum neque egestas congue quisque egestas diam. Auctor urna nunc id cursus metus aliquam. Nullam vehicula ipsum a arcu cursus vitae congue mauris rhoncus. Vitae congue mauris rhoncus aenean vel elit scelerisque mauris pellentesque. Tempus egestas sed sed risus pretium quam vulputate dignissim suspendisse. Risus nullam eget felis eget nunc lobortis mattis aliquam faucibus. Adipiscing elit ut aliquam purus sit. Suspendisse in est ante in nibh mauris cursus mattis molestie.\n\nIn tellus integer feugiat scelerisque varius morbi enim nunc. Sapien et ligula ullamcorper malesuada. Commodo ullamcorper a lacus vestibulum sed arcu non odio. Cursus euismod quis viverra nibh cras pulvinar mattis. Vitae turpis massa sed elementum tempus egestas sed sed risus. Tincidunt tortor aliquam nulla facilisi cras fermentum odio. Justo eget magna fermentum iaculis eu non diam phasellus vestibulum. Rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt lobortis. Viverra accumsan in nisl nisi scelerisque eu. Pretium nibh ipsum consequat nisl vel pretium. Sed elementum tempus egestas sed sed risus pretium quam. Et leo duis ut diam quam nulla. Nunc mattis enim ut tellus elementum. Morbi tristique senectus et netus. Malesuada fames ac turpis egestas sed tempus. Eget mauris pharetra et ultrices neque.\n\nIn pellentesque massa placerat duis ultricies lacus sed turpis tincidunt. Ornare quam viverra orci sagittis eu volutpat odio facilisis. Enim tortor at auctor urna nunc. Pellentesque eu tincidunt tortor aliquam nulla facilisi cras fermentum odio. Tristique et egestas quis ipsum suspendisse ultrices gravida. Id volutpat lacus laoreet non. Pellentesque elit ullamcorper dignissim cras tincidunt lobortis feugiat vivamus at. Faucibus et molestie ac feugiat sed lectus vestibulum mattis ullamcorper. In nisl nisi scelerisque eu ultrices vitae auctor eu. Pretium nibh ipsum consequat nisl vel pretium lectus. Morbi tincidunt augue interdum velit. Nunc sed id semper risus in hendrerit gravida rutrum quisque. Velit aliquet sagittis id consectetur purus ut faucibus pulvinar elementum. Mollis nunc sed id semper risus in hendrerit gravida rutrum. Mauris ultrices eros in cursus turpis.";
	#addTextBox(textbox_icon_type.NONE, lorem);
	pass;

func addTextBox(var icon_type, var text):
	var instance = textbox.instance();
	instance.initiate(icon_type, text);
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
