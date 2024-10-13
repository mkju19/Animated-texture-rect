using System;
using System.Collections.Generic;
using Godot;

public partial class AnimatedTextureRect
{
    public event Action AnimationFinished;
    public event Action AnimationLooped;
    public event Action FrameChanged;
    public event Action AnimationChanged;

    public int Fps
    {
        get { return (int)Node.Get("fps"); }
        set { Node.Set("fps", value); }
    }
    public bool IsLooping
    {
        get { return (bool)Node.Get("is_looping"); }
        set { Node.Set("is_looping", value); }
    }

    public bool AutoStart
    {
        get { return (bool)Node.Get("auto_start"); }
        set { Node.Set("auto_start", value); }
    }

    public int TextureSeperation
    {
        get { return (int)Node.Get("texture_seperation"); }
        set { Node.Set("texture_seperation", value); }
    }

    public int Frame
    {
        get { return (int)Node.Get("frame"); }
        set { Node.Set("frame", value); }
    }

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

    public bool IsPlaying
    {
        get { return (bool)Node.Get("is_playing"); }
        private set { throw new Exception(); }
    }

    public bool IsPlayingForwards
    {
        get { return (bool)Node.Get("is_playing_forwards"); }
        private set { throw new Exception(); }
    }

    public bool IsPlayingBackwards
    {
        get { return (bool)Node.Get("is_playing_backwards"); }
        private set { throw new Exception(); }
    }

    public int CurrentAnimationIndex
    {
        get { return (int)Node.Get("_current_animation_index"); }
        private set { throw new Exception(); }
    }

    public int NumberOfFrames
    {
        get { return (int)Node.Get("_number_of_frames"); }
        private set { throw new Exception(); }
    }

    public TextureRect Node { get; private set; }

    public void Play()
    {
        Node.Call("play");
    }

    public void PlayBackwards()
    {
        Node.Call("play_backwards");
    }

    public void Stop()
    {
        Node.Call("stop");
    }

    public void Pause()
    {
        Node.Call("pause");
    }

    public void Reset()
    {
        Node.Call("reset");
    }

    public void ChangeAnimation(int animationIndex, int textureSeperation = 0)
    {
        Node.Call("change_animation", animationIndex, textureSeperation);
    }

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
