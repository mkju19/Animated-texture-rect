@tool
## Texture rect that can use an atlas texture to animate textures
class_name AnimatedTextureRect
extends TextureRect

signal animation_finished
signal animation_looped
signal frame_changed
signal animation_changed

 ## Frames per second. The speed of the animation.
@export_range(0.1,1000000, 0.1)
var fps: float = 10

## Whether the animation should loop when finished
@export
var animation_looping: bool = false

## Whether the animation should play when it enters a scene
@export
var auto_start: bool = false

## The number of pixels separating the frames in the atlas texture
@export_range(0,10000)
var texture_seperation: int = 0

## The current frame of the animation
@export_range(0, 100)
var frame: int = 0: 
	get:
		return frame
	set(value):
		frame = value
		_set_current_frame(value)

@export
var alternate_animations: Array

var is_playing: bool = false

var _animation_stopped: bool = false

var _current_animation_index: int
		
var _number_of_frames

var _texture_width: int

var _atlas_texture: AtlasTexture

var _timer: Timer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if (texture == null || texture.atlas == null):
		texture = AtlasTexture.new()
	elif (Engine.is_editor_hint()):
		return
	else:
		_atlas_texture = texture
		_texture_width = _atlas_texture.get_width()
		
		_number_of_frames = _get_number_of_frames()

		_current_animation_index = 0
		
		_timer = Timer.new()
		_timer.wait_time = 1/(fps as float)
		_timer.one_shot = false
		_timer.autostart = false
		add_child(_timer)
		if (auto_start):
			play()


func _process(delta: float) -> void:
	if (Engine.is_editor_hint()):
		if (len(alternate_animations) > 0):
			for i in range(0,len(alternate_animations)):
				if (alternate_animations[i] == null):
					alternate_animations[i] = AtlasTexture.new()
					
			if (alternate_animations[0].atlas != null):
				texture = alternate_animations[0]	


func play() -> void:
	_timer.timeout.connect(_next_frame)
	_timer.start()
	is_playing = true
	if (_animation_stopped):
		frame = 0
		_animation_stopped= false

func play_backwards() -> void:
	_timer.timeout.connect(_previous_frame)
	_timer.start()
	is_playing = true
	if (_animation_stopped):
		frame = _number_of_frames-1
		_animation_stopped= false

func stop() -> void:
	_disconect_timer_signals()
	is_playing = false
	_timer.stop()
	animation_finished.emit()
	_animation_stopped = true


func reset() -> void:
	frame = 0


func pause() -> void:
	_disconect_timer_signals()
	is_playing = false
	_timer.stop()


func change_animation(animation_index:int, new_texture_seperation: int = 0) -> void:
	if (animation_index < 0):
		push_error("The animation index must not be a negative number")
		return
	if (len(alternate_animations) == 0):
		push_error("Cannot change animation. The alternate_animations array is empty")
		return
	if(animation_index >= len(alternate_animations)):
		push_error("Animation index " + str(animation_index) + " is not in the alternate animations array")
		return
	
	if(animation_index > len(alternate_animations) || animation_index < 0 || len(alternate_animations) == 0):
		return
		
	_current_animation_index = animation_index
	texture_seperation = new_texture_seperation
	
	texture = alternate_animations[animation_index]
	_atlas_texture = texture as AtlasTexture
	_atlas_texture.region.position = Vector2(0,0)
	_texture_width = _atlas_texture.get_width()
	
	_number_of_frames = _get_number_of_frames()
	frame = 0
	
	animation_changed.emit()


func _disconect_timer_signals() -> void:
	for dict in _timer.timeout.get_connections():
		_timer.timeout.disconnect(dict.callable)
	
	
func _set_current_frame(frame_number: int) -> void:
	frame_changed.emit()
	
	_atlas_texture = texture
	_texture_width = _atlas_texture.get_width()
	
	var current_position: Vector2 = _atlas_texture.region.position
	var current_size: Vector2 = _atlas_texture.region.size
	var start_position: Vector2 = Vector2(0, current_position.y)
	
	var new_position = Vector2(start_position.x + (frame_number * (_texture_width + texture_seperation)), current_position.y)
	
	_number_of_frames = _get_number_of_frames()
	if (_is_outside_texture(new_position)):
		push_error("Frame index " + str(frame_number) + " is outside the atlas texture.")
		return
	
	_atlas_texture.region = Rect2(new_position, current_size)


func _previous_frame() -> void:
	if((frame - 1) >= 0):
		frame -= 1
	elif (!animation_looping):
			stop()
			return
	else:
		frame = _number_of_frames-1
		animation_looped.emit()
		
		
func _next_frame() -> void:
	if((frame + 1) < _number_of_frames):
		frame += 1
	elif (!animation_looping):
			stop()
			return
	else:
		frame = 0
		animation_looped.emit()


func _get_number_of_frames() -> int:
	var frames = 0
	var current_position: Vector2 = _atlas_texture.region.position
	var current_size: Vector2 = _atlas_texture.region.size
	var position: Vector2 = Vector2(0, current_position.y)
	
	while (!_is_outside_texture(position)):
		frames += 1
		position.x += _texture_width + texture_seperation
		
	return frames


func _is_outside_texture(new_position : Vector2) -> bool:
	return new_position.x + _texture_width + texture_seperation > _atlas_texture.atlas.get_size().x 
