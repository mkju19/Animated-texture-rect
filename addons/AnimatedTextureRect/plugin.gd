@tool
extends EditorPlugin


func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	add_custom_type("AnimatedTextureRect", "TextureRect", preload("res://addons/AnimatedTextureRect/animated_texture_rect.gd"), preload("res://addons/AnimatedTextureRect/animated_texture_rect.svg"))


func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	remove_custom_type("AnimatedTextureRect")
