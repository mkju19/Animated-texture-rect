# Animated texture rect

A simple addon that adds an animated version of the TextureRect node.

## How to use

1. Add the node like you would any other node in Godot.
2. Add an atlas texture that contains your sprite frames.
3. Select the region of the atlas to fit the first frame of your animation.
   - If you have multiple animation that you want to switch between:
   - Add them all to the alternate animations array.
   - The first element in the array will be the default animation.
4. If your frames are seperated by a few pixels you should set the texture seperation property to the number of pixels.
5. The animation can then be started either by toggling auto start, or by calling play() on the AnimatedTextureRect node.

## Available Methods

- play()
  - Starts the current animation.
- stop()
  - Stops the current animation.
  - When the animation is started again it will reset to the first frame.
- pause()
  - Pauses the animation.
  - When the animation is started again it will start where it left off.
- reset()
  - Will put the animation back to the first frame
- change_animation(animation_index : int, texture_separation: int)
  - Changes the animation to the given animation index if multiple animations has been added to the "Alternate Animations" array.
  - texture_separation is optional, but if not supplied it will be assumed to be 0.

## Available Properties

- fps: float
  - Sets the number of frames that should be shown per second.
- animation_looping: bool
  - Sets whether the animation should restart when it has reached the end.
- auto_start: bool
  - Sets whether the animation should start by itself when it is added to the scene tree.
- texture_separation: int
  - Should be set to the number of pixels that separates the frames in the atlas.
- frame: int
  - The current frame that is being shown
  - The active frame can be set by directly changing the property
- alternate_animations: Array(AtlasTexture)
  - Holds all animations if there are multiple
- is_playing: bool
  - Shows whether the animation is playing
