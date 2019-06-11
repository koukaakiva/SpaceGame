extends Node;

signal Dialogue_Next(ref, actor, text);
signal Choice_Next(ref, branches);
signal Dialogue_Started;
signal Dialogue_Ended;
export (Resource) var DialogueResource = null;
var in_dialogue = false;
var current_node = {};
var savedKeys = {};

func start_dialogue(start_at = "StartNode"):
	if(DialogueResource == null):
		return;
	emit_signal("Dialogue_Started");
	in_dialogue = true;
	_process_next(DialogueResource.Nodes[find_next_node(start_at)]);

func next_dialogue(branch = 0):
	if(!in_dialogue):
		return;
	var connections = find_connections(current_node["name"]);
	if((connections.size() - 1) < branch):
		print("Error: Not enough branches.");
		end_dialogue();
		return;
	var next_node;
	for connection in connections:
		if(connection["from_port"] == branch):
			next_node = find_next_node(connection["to"]);
	if(next_node == null):
		print("Error: Branch not found.");
		end_dialogue();
		return;
	_process_next(DialogueResource.Nodes[next_node]);

func _process_next(nextDialogue):
	current_node = nextDialogue;
	match current_node["type"]:
		"Dialogue":
			emit_signal("Dialogue_Next", current_node["refName"], current_node["actor"], current_node["dialogue"]);
		"Choice":
			emit_signal("Choice_Next", current_node["refName"], current_node["branches"]);
		"Key Setter":
			setKey(current_node.key, current_node.value);
			next_dialogue();
		"Key Branch":
			if(savedKeys.has(current_node["key"])):
				var value = savedKeys[current_node["key"]];
				for i in range(current_node["branches"].size() - 1):
					if(current_node["branches"][i] == value):
						next_dialogue(i);
						return;
			next_dialogue(current_node["branches"].size() - 1);
		"Random":
			next_dialogue(round(rand_range(0, current_node["branches"] - 1)));
		"Start":
			next_dialogue();
		"End":
			end_dialogue();
		_:
			print("Error: Unknown node type.");
			end_dialogue();

#used in the save data in the dialogue
func setKey(key, value):
	savedKeys[key] = value;

func end_dialogue():
	emit_signal("Dialogue_Ended");
	in_dialogue = false;

func find_connections(name):
	var results = [];
	for connection in DialogueResource.Connections:
		if(connection.from == name):
			results.append(connection);
	return results;

func find_next_node(name):
	for i in range(DialogueResource.Nodes.size()):
		if(DialogueResource.Nodes[i].name == name):
			return i;
	return -1;
