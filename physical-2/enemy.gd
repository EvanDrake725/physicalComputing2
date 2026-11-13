extends CharacterBody2D

var moveSpeed=200
@onready var navAge:NavigationAgent2D=$NavigationAgent2D
@onready var player:CharacterBody2D=$"../Player"
var inRange=false
var health=2
var fire_rate=0.65
var time_since=0
var bullet=preload("res://Actors/bullet_e.tscn")
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	if health<=0:
		self.queue_free()
	if inRange==true:
		var currentPos:Vector2=self.global_transform.origin
		var nextPos:Vector2=navAge.get_next_path_position()
		var newVel:Vector2=currentPos.direction_to(nextPos)
		navAge.velocity=newVel
		_updateTargetPos(player.global_transform.origin)
		look_at(player.global_position)
		time_since+=delta
		shoot()
	pass

func shoot():
	if time_since >= fire_rate:
		var new_bullet = bullet.instantiate()
		new_bullet.global_transform = self.global_transform
		get_tree().current_scene.add_child(new_bullet)
		time_since = 0


func _updateTargetPos(target_pos:Vector2):
	navAge.target_position=target_pos
	pass

func _on_navigation_agent_2d_velocity_computed(safe_velocity: Vector2) -> void:
	velocity=velocity.move_toward(safe_velocity*moveSpeed, 12)
	move_and_slide()
	pass # Replace with function body.

func Health_Loss():
	health-=1

func _on_area_2d_body_entered(body: Node2D) -> void:
	if body.is_in_group("Player"):
		inRange=true
	pass # Replace with function body.
