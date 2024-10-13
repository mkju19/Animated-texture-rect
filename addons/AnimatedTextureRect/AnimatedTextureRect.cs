using System;
using System.Collections.Generic;
using Godot;

public partial class AnimatedTextureRect
{
    public event Action AnimationFinished;
    public event Action AnimationLooped;
    public event Action FrameChanged;
    public event Action AnimationChanged;

    /// <summary>
    /// Frames per second. Gets and sets the current fps.
    /// </summary>
    public int Fps
    {
        get { return (int)Node.Get("fps"); }
        set { Node.Set("fps", value); }
    }

    /// <summary>
    /// When set to true the animation will loop when it reaches the end
    /// </summary>
    public bool IsLooping
    {
        get { return (bool)Node.Get("is_looping"); }
        set { Node.Set("is_looping", value); }
    }

    /// <summary>
    /// When set to true the animation will start automatically when it enters the scene.
    /// </summary>
    public bool AutoStart
    {
        get { return (bool)Node.Get("auto_start"); }
        set { Node.Set("auto_start", value); }
    }

    /// <summary>
    /// The separation between the frames in the atlas texture
    /// </summary>
    public int TextureSeparation
    {
        get { return (int)Node.Get("texture_separation"); }
        set { Node.Set("texture_seperation", value); }
    }

    /// <summary>
    /// The current frame of animation.
    /// Changing this value will also change the frame that is shown
    /// </summary>
    public int Frame
    {
        get { return (int)Node.Get("frame"); }
        set { Node.Set("frame", value); }
    }

    /// <summary>
    /// The list of Atlas textures that can be used to animate the texture rect.
    /// </summary>
    public List<AtlasTexture> AnimationList
    {
        get
        {
            List<AtlasTexture> animationList = new();

            var arr = (Godot.Collections.Array)Node.Get("animation_list");
            foreach (var node in arr)
            {
                animationList.Add((AtlasTexture)node);
            }
            return animationList;
        }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// Shows whether the current animation is playing
    /// </summary>
    public bool IsPlaying
    {
        get { return (bool)Node.Get("is_playing"); }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// Shows whether the current animation is playing forwards
    /// </summary>
    public bool IsPlayingForwards
    {
        get { return (bool)Node.Get("is_playing_forwards"); }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// Shows whether the current animation is playing backwards
    /// </summary>
    public bool IsPlayingBackwards
    {
        get { return (bool)Node.Get("is_playing_backwards"); }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// Shows the index of the current animation in the AnimationList.
    /// </summary>
    public int CurrentAnimationIndex
    {
        get { return (int)Node.Get("_current_animation_index"); }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// Shows the number of frames that are available in
    /// </summary>
    public int NumberOfFrames
    {
        get { return (int)Node.Get("_number_of_frames"); }
        private set { throw new Exception(); }
    }

    /// <summary>
    /// The base TextureRect node. Can be used to access methods of the regular node.
    /// </summary>
    public TextureRect Node { get; private set; }

    /// <summary>
    /// Starts playing the animation from the current frame.
    /// </summary>
    public void Play()
    {
        Node.Call("play");
    }

    /// <summary>
    /// Starts playing the animation backwards from the current frame.
    /// </summary>
    public void PlayBackwards()
    {
        Node.Call("play_backwards");
    }

    /// <summary>
    /// Stops the current animation animation.
    /// The AnimationFinished event will emit.
    /// When started again the animation will start on frame 0.
    /// </summary>
    public void Stop()
    {
        Node.Call("stop");
    }

    /// <summary>
    /// Pauses the current animation animation.
    /// The AnimationFinished event will NOT emit.
    /// When started again the animation will start at the frame it paused on.
    /// </summary>
    public void Pause()
    {
        Node.Call("pause");
    }

    /// <summary>
    /// Resets the animation to the first frame. The animation will be paused at the first frame
    /// </summary>
    public void Reset()
    {
        Node.Call("reset");
    }

    /// <summary>
    /// Changes the current animation to the given index.
    /// The seperation between frames can be given in number of pixels.
    /// If AutoStart is true the animation will start automatically.
    /// </summary>
    /// <param name="animationIndex"></param>
    /// <param name="textureSeperation"></param>
    public void ChangeAnimation(int animationIndex, int textureSeperation = 0)
    {
        Node.Call("change_animation", animationIndex, textureSeperation);
    }

    /// <summary>
    /// Calls QueueFree on the Node
    /// </summary>
    public void QueueFree()
    {
        Node.QueueFree();
    }

    public AnimatedTextureRect(TextureRect animatedTextureRectNode)
    {
        Node = animatedTextureRectNode;

        Node.Connect(
            "animation_finished",
            Callable.From(() =>
            {
                AnimationFinished?.Invoke();
            })
        );

        Node.Connect(
            "animation_looped",
            Callable.From(() =>
            {
                AnimationLooped?.Invoke();
            })
        );

        Node.Connect(
            "frame_changed",
            Callable.From(() =>
            {
                FrameChanged?.Invoke();
            })
        );

        Node.Connect(
            "animation_changed",
            Callable.From(() =>
            {
                AnimationChanged?.Invoke();
            })
        );
    }

    public static explicit operator AnimatedTextureRect(TextureRect node)
    {
        return new AnimatedTextureRect(node);
    }
}
