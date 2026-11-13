extends Area2D
var start_position
@export var SPEED=1500
@export var max_dis=800

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	start_position=position
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	global_position+=transform.y*SPEED*delta
	if abs(position.distance_to(start_position))>max_dis:
		self.queue_free()
	pass


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("Enemy"):
		body.Health_Loss()
		self.queue_free()
	pass # Replace with function body.
