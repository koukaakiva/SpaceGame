tool
extends Panel;

#Edit this to add or remove node types.
#Start and End do not apear in the menu so numbers below that access this list are ajusted by 2.
const dialogueNodes = {
	"Start": preload("res://addons/dialogue_tree/scenes/StartNode.tscn"),
	"End": preload("res://addons/dialogue_tree/scenes/EndNode.tscn"),
	"Dialogue": preload("res://addons/dialogue_tree/scenes/BasicDialogue.tscn"),
	"Choice": preload("res://addons/dialogue_tree/scenes/ChoiceDialogue.tscn"),
	"Random": preload("res://addons/dialogue_tree/scenes/RandomDialogue.tscn"),
	"Key Setter": preload("res://addons/dialogue_tree/scenes/KeySetterDialogue.tscn"),
	"Key Branch": preload("res://addons/dialogue_tree/scenes/KeyBranchDialogue.tscn")
};
var current_node = null;

func _ready():
	var menuButton = $TopBar/TopContainer/MenuButton.get_popup();
	for i in range(2, dialogueNodes.size()):
		menuButton.add_item(dialogueNodes.keys()[i]);
	menuButton.connect("id_pressed", self, "_on_menubutton_item_pressed");

func _on_menubutton_item_pressed(id):
	var dialogueNode = dialogueNodes.values()[id + 2].instance();
	dialogueNode.connect("close_request", self, "_on_node_close", [dialogueNode]);
	dialogueNode.connect("resize_request", self, "_on_node_resize", [dialogueNode]);
	print(dialogueNode.type);
	dialogueNode.connect("removed_branch", self, "remove_connection", [dialogueNode]);
	$PrimaryGraphEditor.add_child(dialogueNode, true);

func _on_PrimaryGraphEditor_connection_request(from, from_slot, to, to_slot):
	$PrimaryGraphEditor.connect_node(from, from_slot, to, to_slot);

func _on_PrimaryGraphEditor_disconnection_request(from, from_slot, to, to_slot):
	$PrimaryGraphEditor.disconnect_node(from, from_slot, to, to_slot);

func _on_node_close(ref):
	remove_all_connections(ref);
	ref.queue_free();

func _on_node_resize(newSize, ref):
	ref.rect_size = newSize;

func set_edit_node(ref):
	clear_all_nodes();
	current_node = ref;
	if((current_node == null) or (current_node.DialogueResource == null)):
		return;
	for node in current_node.DialogueResource.Nodes:
		if((node["type"] != "Start") and (node["type"] != "End")):
			var newNode = dialogueNodes[node["type"]].instance();
			newNode.name = node.name;
			newNode.offset = Vector2(node.rect_x, node.rect_y);
			newNode.rect_size = Vector2(node.rect_size_x, node.rect_size_y);
			newNode.connect("close_request", self, "_on_node_close", [newNode]);
			newNode.connect("resize_request", self, "_on_node_resize", [newNode]);
			if(node.has("branches")):
				newNode.connect("removed_branch", self, "remove_connection", [newNode]);
			$PrimaryGraphEditor.add_child(newNode, true);
			newNode.load_data(node);
		else:
			var editNode = $PrimaryGraphEditor.get_node(node["name"]);
			editNode.offset = Vector2(node["rect_x"], node["rect_y"]);
			editNode.rect_size = Vector2(node["rect_size_x"], node["rect_size_y"]);
	for connection in current_node.DialogueResource.Connections:
		$PrimaryGraphEditor.connect_node(connection.from, connection.from_port, connection.to, connection.to_port);

func save_resource():
	if(current_node == null):
		return;
	var resource;
	var resource_path;
	if(current_node.DialogueResource == null):
		resource = load("res://addons/dialogue_tree/resource/DialougeRes.tres").duplicate();
		resource_path = current_node.get_parent().filename;
	else:
		resource = current_node.DialogueResource;
		resource_path = current_node.DialogueResource.resource_path;
	resource.Connections = $PrimaryGraphEditor.get_connection_list();
	resource.Nodes.clear();
	for i in $PrimaryGraphEditor.get_children():
		if i.has_method("save_data"):
			resource.Nodes.append(i.save_data());
	#resource.DialogueTree = make_exported_dialogue();
	ResourceSaver.save(resource_path, resource);
	current_node.DialogueResource = resource;

func clear_all_nodes():
	$PrimaryGraphEditor.clear_connections();
	for child in $PrimaryGraphEditor.get_children():
		if child is GraphNode and child.name != "StartNode" and child.name != "EndNode":
			child.free();

# removes connections going from an id
func remove_connection(id, node):
	for connection in $PrimaryGraphEditor.get_connection_list():
		if connection.from == node.name and connection.from_port == id:
			$PrimaryGraphEditor.disconnect_node(connection.from, connection.from_port, connection.to, connection.to_port);

func remove_all_connections(node):
	for i in $PrimaryGraphEditor.get_connection_list():
		if i.from == node.name or i.to == node.name:
			$PrimaryGraphEditor.disconnect_node(i.from, i.from_port, i.to, i.to_port);
