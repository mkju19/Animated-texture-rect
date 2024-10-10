using System;
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

    public TextureRect Node { get; private set; }

    public void Play()
    {
        Node.Call("play");
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

        void af()
        {
            AnimationFinished();
        }
        Node.Connect("animation_finished", Callable.From(af));

        void al()
        {
            AnimationLooped();
        }
        Node.Connect("animation_looped", Callable.From(al));

        void fc()
        {
            FrameChanged();
        }
        Node.Connect("frame_changed", Callable.From(fc));

        void ac()
        {
            AnimationChanged();
        }
        Node.Connect("animation_changed", Callable.From(ac));
    }

    public static explicit operator AnimatedTextureRect(TextureRect node)
    {
        return new AnimatedTextureRect(node);
    }
}
