using System.Collections.Generic;
using System.Drawing;
using Animation_Space;

public class Character
{
    public string name;

    public int LifePoints = 1000;
    public int StunPoints = 0;
    public int onBlockStun = 0;
    public int onHitStun = 0;

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string CurrentState { get; private set; }
    public string LastState { get; private set; }

    private Dictionary<string, Animation> animations;
    public Animation CurrentAnimation => animations[CurrentState];
    private Dictionary<int, Image> spriteImages;
    private string folderPath;

    public int CurrentSprite => CurrentAnimation.GetCurrentFrame().Sprite_index;
    public List<GenericBox> CurrentBoxes => CurrentAnimation.GetCurrentFrame().Boxes;

    public Character(string name, Dictionary<string, Animation> animations, string initialState, int startX, int startY, string folderPath)
    {   
        this.folderPath = folderPath;
        this.animations = animations;
        this.name = name;
        CurrentState = initialState;
        LastState = initialState;
        PositionX = startX;
        PositionY = startY;
    }

    public void Update()
    {
        // Check if state has changed
        if (CurrentState != LastState)
        {
            animations[CurrentState].Reset();
            LastState = CurrentState;
        }

        FrameData frameData = CurrentAnimation.GetCurrentFrame();

        // Update character position
        PositionX += frameData.DeltaX;
        PositionY += frameData.DeltaY;

        // Advance to the next frame
        CurrentAnimation.AdvanceFrame();
        if (this.CurrentAnimation.onLastFrame) {
            this.CurrentAnimation.Reset();
            this.CurrentState = this.CurrentAnimation.post_state;
        }
    }
    
    public void ChangeState(string newState)
    {
        if (animations.ContainsKey(newState))
        {
            LastState = CurrentState;
            CurrentState = newState;
        }
    }

    public Image GetCurrentSpriteImage()
    {
        int spriteIndex = CurrentSprite;
        if (spriteImages.ContainsKey(spriteIndex))
        {
            return spriteImages[spriteIndex];
        }
        return null; 
    }
    
    public void LoadSpriteImages()
    {
        for (int i = 0; ; i++)
        {
            string filePath = $"{this.folderPath}/{i}.png";
            if (System.IO.File.Exists(filePath))
            {
                spriteImages[i] = Image.FromFile(filePath);
            }
            else
            {
                break;
            }
        }
    }

    public void UnloadSpriteImages()
    {
        foreach (var image in spriteImages.Values)
        {
            image.Dispose(); // Free the memory used by the image
        }
        spriteImages.Clear(); // Clear the dictionary
    }
}
