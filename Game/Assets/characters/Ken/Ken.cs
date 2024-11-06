using SFML.System;
using Character_Space;
using Animation_Space;
using Input_Space;
using Aux_Space;
using System.IO.Compression;
using Stage_Space;

public class Ken : Character {
    private int tatso_speed = 5;

    public Ken(string initialState, int startX, int startY, Stage stage)
        : base("Ken", initialState, startX, startY, "Assets/characters/Ken/sprites", "Assets/characters/Ken/sounds", stage)
    {
        this.LifePoints = new Vector2i(1000, 1000);
        this.DizzyPoints = new Vector2i(1000, 1000);

        this.dash_speed = 8;
        this.move_speed = 3;
        this.push_box_width = 25;
    }
    
    public override void Load() {
        // Hurtboxes
        var pushbox = new GenericBox(2, 125 - this.push_box_width, 145, 125 + this.push_box_width, 195);

        // Animations
        var introFrames = new List<FrameData> {
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15568, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15569, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15570, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15571, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15572, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15573, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15574, 0, 0, new List<GenericBox> { pushbox,}),
            new FrameData(15575, 0, 0, new List<GenericBox> { pushbox,}, "ykesse"),
        };

        var idleFrames = new List<FrameData> {
            new FrameData(14657, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 102, 140, 153), new GenericBox(1, 118, 92, 139, 109), new GenericBox(1, 137, 107, 156, 135), new GenericBox(1, 96, 105, 131, 127), new GenericBox(1, 91, 148, 152, 196) }),
            new FrameData(14658, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 103, 101, 141, 153), new GenericBox(1, 95, 104, 155, 135), new GenericBox(1, 119, 91, 139, 108), new GenericBox(1, 91, 147, 154, 195), pushbox }),
            new FrameData(14659, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 103, 97, 141, 149), new GenericBox(1, 118, 88, 140, 105), new GenericBox(1, 95, 98, 156, 131), new GenericBox(1, 91, 144, 154, 195), pushbox}),
            new FrameData(14660, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 100, 96, 140, 149), new GenericBox(1, 119, 85, 139, 103), new GenericBox(1, 95, 97, 156, 128), new GenericBox(1, 92, 144, 152, 196), pushbox}),
            new FrameData(14661, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 85, 139, 101), new GenericBox(1, 105, 94, 140, 147), new GenericBox(1, 91, 146, 153, 195), new GenericBox(1, 135, 97, 156, 127), new GenericBox(1, 96, 94, 129, 117) }),
            new FrameData(14662, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 84, 138, 100), new GenericBox(1, 102, 94, 140, 146), new GenericBox(1, 96, 95, 157, 127), new GenericBox(1, 90, 142, 152, 195), pushbox }),
            new FrameData(14663, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 84, 138, 101), new GenericBox(1, 103, 94, 140, 148), new GenericBox(1, 96, 96, 157, 128), new GenericBox(1, 91, 133, 153, 195) }),
            new FrameData(14664, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 86, 138, 102), new GenericBox(1, 102, 94, 142, 148), new GenericBox(1, 96, 96, 157, 129), new GenericBox(1, 90, 141, 152, 195), pushbox }),
            new FrameData(14665, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 85, 139, 105), new GenericBox(1, 103, 94, 140, 148), new GenericBox(1, 96, 98, 156, 131), new GenericBox(1, 91, 141, 152, 195), pushbox }),
            new FrameData(14666, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 90, 140, 107), new GenericBox(1, 103, 99, 142, 153), new GenericBox(1, 96, 102, 157, 132), new GenericBox(1, 92, 139, 152, 196), pushbox }),
        };

        var OnBlockFrames = new List<FrameData> {
            new FrameData(14751, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 114, 91, 129, 107), new GenericBox(1, 95, 104, 145, 123), new GenericBox(1, 97, 123, 139, 160), new GenericBox(1, 88, 159, 154, 194) }),
            new FrameData(14752, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 113, 93, 129, 109), new GenericBox(1, 96, 102, 142, 128), new GenericBox(1, 100, 128, 135, 156), new GenericBox(1, 90, 155, 148, 194) }),
            new FrameData(14753, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 107, 91, 125, 106), new GenericBox(1, 90, 102, 136, 128), new GenericBox(1, 95, 128, 134, 153), new GenericBox(1, 87, 154, 149, 194) }),
            new FrameData(14754, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 91, 127, 106), new GenericBox(1, 94, 103, 139, 127), new GenericBox(1, 90, 128, 136, 151), new GenericBox(1, 89, 152, 152, 195) }),
        };

        var OnHit3Frames = new List<FrameData> {
            new FrameData(14807, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 111, 92, 127, 105), new GenericBox(1, 89, 107, 140, 135), new GenericBox(1, 92, 135, 141, 161), new GenericBox(1, 98, 161, 152, 194) }),
            new FrameData(14808, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 95, 93, 112, 107), new GenericBox(1, 85, 105, 139, 134), new GenericBox(1, 93, 134, 149, 162), new GenericBox(1, 93, 163, 158, 196) }),
            new FrameData(14809, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 82, 97, 99, 112), new GenericBox(1, 81, 113, 132, 140), new GenericBox(1, 90, 141, 149, 166), new GenericBox(1, 93, 166, 162, 195) }),
            new FrameData(14810, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 67, 107, 87, 123), new GenericBox(1, 76, 110, 144, 141), new GenericBox(1, 92, 141, 146, 168), new GenericBox(1, 91, 168, 164, 195) }),
            new FrameData(14811, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 86, 108, 148, 151), new GenericBox(1, 91, 151, 176, 194) }),
            new FrameData(14812, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 100, 113, 116, 130), new GenericBox(1, 87, 117, 129, 157), new GenericBox(1, 128, 123, 154, 140), new GenericBox(1, 92, 147, 174, 195) }),
            new FrameData(14813, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 101, 127, 118), new GenericBox(1, 89, 116, 142, 144), new GenericBox(1, 90, 144, 154, 165), new GenericBox(1, 88, 164, 157, 195) }),
            new FrameData(14814, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 115, 96, 135, 113), new GenericBox(1, 96, 107, 147, 139), new GenericBox(1, 95, 140, 146, 159), new GenericBox(1, 90, 158, 154, 194) }),
        };

        // Normals
        var LPFrames = new List<FrameData> { 
            new FrameData(15008, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 97, 144, 154), new GenericBox(1, 136, 103, 162, 129), new GenericBox(1, 91, 96, 125, 121), new GenericBox(1, 120, 87, 141, 105), new GenericBox(1, 87, 137, 162, 194), pushbox}, "golpe_1"),
            new FrameData(15009, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 145, 100, 206, 119), new GenericBox(1, 142, 103, 204, 116), new GenericBox(1, 120, 87, 142, 105), new GenericBox(1, 88, 97, 116, 121), new GenericBox(1, 105, 98, 146, 153), new GenericBox(1, 88, 148, 158, 194), pushbox}),
            new FrameData(15010, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 144, 99, 200, 119), new GenericBox(1, 139, 102, 199, 117), new GenericBox(1, 105, 96, 145, 154), new GenericBox(1, 120, 86, 145, 107), new GenericBox(1, 91, 96, 124, 122), new GenericBox(1, 89, 146, 159, 195), pushbox}),
            new FrameData(15011, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 87, 144, 107), new GenericBox(1, 139, 100, 178, 126), new GenericBox(1, 93, 99, 123, 122), new GenericBox(1, 105, 99, 145, 151), new GenericBox(1, 90, 145, 157, 194), pushbox}),
            new FrameData(15012, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 120, 88, 142, 108), new GenericBox(1, 105, 98, 143, 153), new GenericBox(1, 139, 101, 160, 128), new GenericBox(1, 93, 99, 126, 122), new GenericBox(1, 88, 147, 155, 196), pushbox}),
        };
        
        var MPFrames = new List<FrameData> {
            new FrameData(15013, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 100, 100, 146, 153), new GenericBox(1, 119, 89, 140, 106), new GenericBox(1, 85, 102, 111, 126), new GenericBox(1, 137, 102, 162, 129), new GenericBox(1, 91, 143, 154, 195), pushbox}, "golpe_2"),
            new FrameData(15014, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 90, 142, 108), new GenericBox(1, 105, 98, 147, 153), new GenericBox(1, 143, 116, 164, 134), new GenericBox(1, 84, 100, 110, 115), new GenericBox(1, 92, 139, 157, 195), pushbox}),
            new FrameData(15015, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 125, 92, 148, 108), new GenericBox(1, 108, 108, 148, 157), new GenericBox(1, 146, 118, 161, 132), new GenericBox(1, 90, 102, 123, 119), new GenericBox(1, 93, 144, 164, 196), pushbox}),
            new FrameData(15016, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 111, 104, 154, 157), new GenericBox(1, 127, 94, 154, 113), new GenericBox(1, 94, 143, 162, 195), pushbox, new GenericBox(1, 153, 109, 209, 123), new GenericBox(0, 153, 107, 212, 124) }),
            new FrameData(15017, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 163, 110, 204, 124), new GenericBox(1, 110, 103, 150, 161), new GenericBox(1, 129, 95, 152, 110), new GenericBox(1, 142, 107, 201, 122), new GenericBox(1, 91, 148, 160, 195), pushbox}),
            new FrameData(15018, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 102, 145, 158), new GenericBox(1, 137, 105, 173, 128), new GenericBox(1, 123, 92, 146, 109), new GenericBox(1, 92, 144, 157, 195) }),
            new FrameData(15019, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 99, 142, 156), new GenericBox(1, 96, 102, 156, 130), new GenericBox(1, 123, 90, 146, 106), new GenericBox(1, 89, 144, 160, 195), pushbox}),
        };

        var HPFrames = new List<FrameData> {
            new FrameData(15021, 4, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 97, 146, 158), new GenericBox(1, 88, 150, 164, 195) }, "golpe_grito_2"),
            new FrameData(15022, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 96, 142, 160), new GenericBox(1, 84, 116, 109, 135), new GenericBox(1, 84, 149, 163, 194), pushbox }),
            new FrameData(15023, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 113, 92, 136, 111), new GenericBox(1, 103, 103, 146, 159), new GenericBox(1, 85, 145, 161, 196), new GenericBox(1, 85, 110, 114, 127) }),
            new FrameData(15024, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 95, 148, 156), new GenericBox(1, 94, 117, 111, 135), new GenericBox(1, 144, 101, 182, 113), new GenericBox(0, 144, 96, 191, 119), new GenericBox(0, 171, 93, 199, 121), pushbox, new GenericBox(1, 85, 150, 162, 195) }),
            new FrameData(15025, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 166, 92, 196, 116), pushbox, new GenericBox(1, 95, 96, 153, 143), new GenericBox(1, 143, 99, 189, 112), new GenericBox(1, 82, 142, 161, 193) }),
            new FrameData(15026, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 95, 99, 146, 156), new GenericBox(1, 132, 94, 167, 119), new GenericBox(1, 79, 144, 159, 194), pushbox }),
            new FrameData(15027, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 97, 98, 147, 158), new GenericBox(1, 85, 152, 161, 196) }),
            new FrameData(15028, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 95, 128, 114), new GenericBox(1, 91, 110, 143, 162), new GenericBox(1, 85, 150, 162, 194) }),
            new FrameData(15029, -4, 0, new List<GenericBox> { pushbox, new GenericBox(1, 116, 93, 138, 111), new GenericBox(1, 98, 104, 148, 154), new GenericBox(1, 92, 140, 167, 195) }),
        };

        var cl_HPFrames = new List<FrameData> {
            new FrameData(15053, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 92, 141, 110), new GenericBox(1, 93, 104, 146, 145), new GenericBox(1, 146, 107, 162, 119), new GenericBox(1, 100, 145, 156, 195) }, "golpe_3"),
            new FrameData(15054, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 125, 94, 143, 110), new GenericBox(1, 106, 106, 143, 159), new GenericBox(1, 98, 153, 154, 195), new GenericBox(1, 144, 106, 158, 120), new GenericBox(1, 93, 115, 106, 148) }, "golpe_grito_3"),
            new FrameData(15055, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 130, 92, 145, 109), new GenericBox(1, 109, 103, 145, 155), new GenericBox(1, 100, 151, 153, 195) }),
            new FrameData(15056, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 130, 91, 150, 108), new GenericBox(1, 113, 103, 151, 153), new GenericBox(1, 151, 113, 170, 150), new GenericBox(1, 102, 108, 115, 125), new GenericBox(1, 98, 148, 154, 195) }),
            new FrameData(15057, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 162, 101, 190, 136), new GenericBox(1, 129, 89, 149, 107), new GenericBox(1, 113, 102, 154, 152), new GenericBox(1, 102, 110, 120, 129), new GenericBox(1, 100, 148, 155, 195) }),
            new FrameData(15058, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 144, 76, 166, 116), new GenericBox(1, 125, 90, 144, 107), new GenericBox(1, 115, 105, 150, 156), new GenericBox(1, 97, 148, 154, 195) }),
            new FrameData(15059, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 114, 80, 152, 153), new GenericBox(1, 96, 143, 154, 195), new GenericBox(1, 102, 116, 114, 129) }),
            new FrameData(15060, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 90, 141, 107), new GenericBox(1, 140, 76, 165, 120), new GenericBox(1, 113, 104, 149, 153), new GenericBox(1, 95, 146, 154, 195) }),
            new FrameData(15061, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 142, 73, 164, 115), new GenericBox(1, 120, 88, 140, 104), new GenericBox(1, 109, 104, 146, 156), new GenericBox(1, 95, 150, 154, 195) }),
            new FrameData(15062, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 87, 138, 104), new GenericBox(1, 141, 79, 161, 116), new GenericBox(1, 107, 102, 144, 154), new GenericBox(1, 95, 146, 152, 195) }),
            new FrameData(15063, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 87, 138, 104), new GenericBox(1, 105, 99, 146, 153), new GenericBox(1, 145, 98, 162, 122), new GenericBox(1, 96, 148, 153, 195) }),
            new FrameData(15064, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 89, 138, 103), new GenericBox(1, 104, 101, 143, 153), new GenericBox(1, 143, 110, 158, 130) }),
            new FrameData(15065, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 144, 151, 195), new GenericBox(1, 118, 92, 137, 108), new GenericBox(1, 101, 102, 147, 145) }),
            new FrameData(15066, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 120, 92, 135, 105), new GenericBox(1, 100, 102, 144, 151), new GenericBox(1, 93, 146, 153, 195) }),
        };

        var MKFrames = new List<FrameData> {
            new FrameData(15072, 8, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 102, 143, 150), new GenericBox(1, 82, 107, 100, 132), new GenericBox(1, 138, 124, 170, 150), new GenericBox(1, 114, 143, 148, 193), new GenericBox(1, 106, 92, 131, 106), pushbox},  "golpe_3"),
            new FrameData(15073, 2, 0, new List<GenericBox> { pushbox, new GenericBox(0, 176, 89, 210, 117), new GenericBox(0, 158, 105, 184, 126), new GenericBox(0, 139, 118, 166, 142), new GenericBox(1, 90, 93, 141, 146), new GenericBox(1, 110, 137, 137, 194), new GenericBox(1, 69, 114, 90, 136), pushbox}, "golpe_grito_4"),
            new FrameData(15074, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 186, 95, 219, 120), new GenericBox(0, 162, 106, 189, 129), new GenericBox(0, 145, 117, 171, 140), pushbox , new GenericBox(1, 91, 91, 148, 140), new GenericBox(1, 112, 140, 139, 194), new GenericBox(1, 70, 120, 92, 140) }),
            new FrameData(15075, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 174, 118, 204, 144), new GenericBox(0, 145, 123, 174, 146), pushbox, new GenericBox(1, 93, 94, 148, 139), new GenericBox(1, 112, 137, 144, 193), new GenericBox(1, 79, 128, 101, 148) }),
            new FrameData(15076, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 91, 131, 105), new GenericBox(1, 94, 96, 154, 140), new GenericBox(1, 135, 138, 176, 163), new GenericBox(1, 107, 141, 140, 193), pushbox}),
            new FrameData(15077, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 92, 149, 145), new GenericBox(1, 106, 139, 144, 194) }),
            new FrameData(15078, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 88, 124, 106), new GenericBox(1, 90, 100, 150, 132), new GenericBox(1, 100, 133, 141, 194), pushbox}),
            new FrameData(15079, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 89, 131, 107), new GenericBox(1, 92, 101, 144, 148), new GenericBox(1, 96, 148, 145, 195) }),
            new FrameData(15080, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 88, 137, 105), new GenericBox(1, 108, 100, 142, 151), new GenericBox(1, 95, 100, 153, 130), new GenericBox(1, 98, 143, 156, 195), pushbox}),
        };

        var BackMKFrames = new List<FrameData> {
            new FrameData(15118, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 110, 86, 134, 103), new GenericBox(1, 100, 98, 141, 147), new GenericBox(1, 106, 132, 152, 193), new GenericBox(1, 88, 101, 157, 127), pushbox}, "golpe_3"),
            new FrameData(15119, 3, 0, new List<GenericBox> { pushbox, new GenericBox(1, 76, 88, 154, 128), new GenericBox(1, 106, 116, 159, 194) }, "golpe_grito_5"),
            new FrameData(15120, 5, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 59, 137, 193), new GenericBox(1, 88, 91, 122, 129), new GenericBox(1, 68, 107, 95, 130), pushbox}),
            new FrameData(15121, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 64, 140, 193), new GenericBox(1, 85, 89, 143, 134), new GenericBox(1, 75, 108, 93, 127) }),
            new FrameData(15122, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 67, 144, 193), pushbox, new GenericBox(1, 86, 92, 147, 134) }),
            new FrameData(15123, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 140, 68, 170, 97), new GenericBox(1, 94, 90, 151, 140), new GenericBox(1, 119, 140, 146, 193) }),            
            new FrameData(15124, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 164, 101, 211, 129), new GenericBox(0, 144, 113, 171, 138), new GenericBox(1, 93, 92, 148, 145), new GenericBox(1, 81, 111, 105, 147), new GenericBox(1, 116, 137, 151, 193), pushbox}),
            new FrameData(15125, -1, 0, new List<GenericBox> { pushbox, new GenericBox(0, 176, 132, 222, 160), new GenericBox(0, 152, 126, 183, 150), new GenericBox(1, 103, 95, 154, 149), new GenericBox(1, 122, 141, 162, 193), new GenericBox(1, 66, 111, 102, 148), pushbox}, hasHit: false),
            new FrameData(15126, -3, 0, new List<GenericBox> { pushbox, new GenericBox(0, 165, 149, 191, 190), new GenericBox(1, 109, 95, 163, 152), new GenericBox(1, 79, 112, 109, 150), new GenericBox(1, 131, 150, 163, 193), pushbox}, hasHit: false),
            new FrameData(15127, -4, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 98, 148, 148), new GenericBox(1, 122, 91, 148, 109), new GenericBox(1, 140, 101, 164, 126), new GenericBox(1, 86, 102, 115, 142), new GenericBox(1, 111, 136, 160, 194) }),
            new FrameData(15128, -3, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 99, 147, 156), new GenericBox(1, 120, 89, 144, 107), new GenericBox(1, 94, 101, 163, 133), new GenericBox(1, 93, 145, 160, 194) }),
        };

        var LKFrames = new List<FrameData> {
            new FrameData(15104, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 124, 91, 145, 109), new GenericBox(1, 104, 96, 141, 155), new GenericBox(1, 139, 114, 156, 131), new GenericBox(1, 93, 144, 155, 195), pushbox}, "golpe_1"),
            new FrameData(15105, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 127, 92, 149, 107), new GenericBox(1, 105, 100, 153, 153), new GenericBox(1, 124, 152, 161, 193), new GenericBox(1, 149, 129, 171, 155) }),
            new FrameData(15106, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 125, 91, 149, 105), new GenericBox(1, 107, 100, 153, 147), new GenericBox(1, 121, 147, 157, 194), new GenericBox(1, 153, 129, 172, 157), pushbox, new GenericBox(0, 186, 154, 207, 181), new GenericBox(0, 171, 139, 187, 169) }),
            new FrameData(15107, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 126, 91, 149, 105), new GenericBox(1, 104, 97, 155, 146), new GenericBox(1, 123, 140, 157, 193), new GenericBox(1, 151, 129, 174, 160) }),
            new FrameData(15108, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 91, 148, 109), new GenericBox(1, 104, 99, 154, 150), new GenericBox(1, 127, 130, 163, 194), pushbox}),
            new FrameData(15109, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 90, 148, 106), new GenericBox(1, 107, 99, 156, 154), new GenericBox(1, 108, 153, 159, 193) }),
            new FrameData(15110, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 90, 141, 107), new GenericBox(1, 100, 97, 139, 153), new GenericBox(1, 137, 105, 156, 132), new GenericBox(1, 97, 143, 155, 195), pushbox}),
        };

        // Movement
        var walkingForwardFrames = new List<FrameData> {
            new FrameData(14671, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 120, 89, 140, 106), new GenericBox(1, 99, 97, 142, 156), new GenericBox(1, 84, 151, 152, 195), new GenericBox(1, 139, 103, 156, 139) }),
            new FrameData(14672, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 88, 140, 108), new GenericBox(1, 98, 99, 148, 151), new GenericBox(1, 143, 106, 157, 138), new GenericBox(1, 74, 151, 149, 195), pushbox}),
            new FrameData(14673, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 89, 141, 108), new GenericBox(1, 98, 101, 156, 138), new GenericBox(1, 72, 137, 145, 195) }),
            new FrameData(14674, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 87, 139, 104), new GenericBox(1, 97, 96, 139, 148), new GenericBox(1, 139, 100, 155, 138), new GenericBox(1, 87, 142, 137, 192), pushbox}),
            new FrameData(14675, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 85, 140, 102), new GenericBox(1, 99, 93, 156, 134), new GenericBox(1, 89, 134, 139, 191) }),
            new FrameData(14676, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 84, 139, 102), new GenericBox(1, 97, 94, 138, 147), new GenericBox(1, 138, 97, 156, 135), new GenericBox(1, 92, 143, 138, 194), pushbox}),
            new FrameData(14677, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 116, 83, 140, 101), new GenericBox(1, 96, 94, 141, 134), new GenericBox(1, 139, 97, 158, 133), new GenericBox(1, 97, 134, 151, 195) }),
            new FrameData(14678, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 94, 139, 148), new GenericBox(1, 118, 83, 138, 102), new GenericBox(1, 97, 96, 129, 117), new GenericBox(1, 138, 96, 157, 133), new GenericBox(1, 97, 141, 155, 195), pushbox}),
            new FrameData(14679, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 83, 140, 101), new GenericBox(1, 97, 94, 141, 150), new GenericBox(1, 138, 98, 158, 133), new GenericBox(1, 93, 149, 169, 195) }),
            new FrameData(14680, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 85, 139, 104), new GenericBox(1, 102, 94, 142, 149), new GenericBox(1, 97, 97, 155, 134), new GenericBox(1, 92, 142, 165, 195), pushbox}),
            new FrameData(14681, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 88, 139, 105), new GenericBox(1, 105, 97, 141, 149), new GenericBox(1, 97, 103, 157, 139), new GenericBox(1, 84, 140, 162, 195) }),
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(14683, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 89, 138, 105), new GenericBox(1, 99, 102, 159, 136), new GenericBox(1, 101, 136, 154, 158), new GenericBox(1, 90, 159, 165, 196) }),
            new FrameData(14684, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 88, 139, 106), new GenericBox(1, 99, 102, 156, 136), new GenericBox(1, 103, 136, 156, 164), new GenericBox(1, 93, 165, 169, 195) }),
            new FrameData(14685, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 89, 139, 106), new GenericBox(1, 100, 101, 156, 133), new GenericBox(1, 104, 133, 156, 162), new GenericBox(1, 102, 161, 168, 195) }),
            new FrameData(14686, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 87, 138, 101), new GenericBox(1, 101, 98, 158, 132), new GenericBox(1, 106, 131, 155, 154), new GenericBox(1, 105, 155, 161, 193) }),
            new FrameData(14687, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 86, 140, 101), new GenericBox(1, 102, 96, 155, 136), new GenericBox(1, 107, 136, 153, 159), new GenericBox(1, 107, 159, 156, 193) }),
            new FrameData(14688, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 85, 139, 102), new GenericBox(1, 102, 96, 156, 130), new GenericBox(1, 107, 130, 152, 156), new GenericBox(1, 102, 155, 155, 194) }),
            new FrameData(14689, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 120, 83, 140, 101), new GenericBox(1, 103, 97, 154, 132), new GenericBox(1, 105, 132, 147, 155), new GenericBox(1, 98, 155, 145, 195) }),
            new FrameData(14690, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 84, 139, 100), new GenericBox(1, 101, 95, 151, 132), new GenericBox(1, 104, 132, 149, 161), new GenericBox(1, 96, 161, 152, 195) }),
            new FrameData(14691, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 83, 137, 99), new GenericBox(1, 99, 96, 149, 128), new GenericBox(1, 102, 128, 147, 158), new GenericBox(1, 86, 158, 151, 195) }),
            new FrameData(14692, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 120, 86, 138, 101), new GenericBox(1, 101, 97, 154, 138), new GenericBox(1, 100, 139, 148, 161), new GenericBox(1, 78, 160, 151, 195) }),
            new FrameData(14693, -this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 88, 139, 104), new GenericBox(1, 102, 99, 154, 133), new GenericBox(1, 103, 132, 147, 153), new GenericBox(1, 88, 152, 157, 196) })
        };

        var dashForwardFrames = new List<FrameData> {
            new FrameData(14768, 12, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14769, 3, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14770, 13, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14771, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14772, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14773, 0, 0, new List<GenericBox> { pushbox, }),
        };

        var dashBackwardFrames = new List<FrameData> {
            new FrameData(14774, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14775, -17, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14776, -7, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14777, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14778, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(14779, 0, 0, new List<GenericBox> { pushbox, }),
        };

        var jumpFrames = new List<FrameData> {
            new FrameData(14697, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 124, 106, 144, 125), new GenericBox(1, 99, 113, 149, 150), new GenericBox(1, 87, 150, 160, 195) }, "golpe_2"),
            new FrameData(14720, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 116, 79, 134, 93), new GenericBox(1, 136, 72, 159, 106), new GenericBox(1, 85, 67, 114, 103), new GenericBox(1, 103, 88, 140, 200) }),
            new FrameData(14721, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 103, 87, 141, 202), new GenericBox(1, 115, 78, 135, 94), new GenericBox(1, 86, 67, 115, 105), new GenericBox(1, 133, 68, 156, 108) }),
            new FrameData(14722, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 91, 78, 156, 107), new GenericBox(1, 105, 89, 140, 140), new GenericBox(1, 100, 133, 152, 178) }),
            new FrameData(14723, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 121, 81, 141, 98), new GenericBox(1, 94, 89, 159, 119), new GenericBox(1, 102, 120, 149, 176) }),
            new FrameData(14724, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 84, 145, 104), new GenericBox(1, 91, 93, 145, 127), new GenericBox(1, 104, 126, 157, 168), new GenericBox(1, 143, 108, 161, 126) }),
            new FrameData(14725, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 119, 158, 161), new GenericBox(1, 124, 84, 143, 102), new GenericBox(1, 89, 91, 144, 124) }),
            new FrameData(14726, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 123, 85, 141, 101), new GenericBox(1, 95, 94, 144, 125), new GenericBox(1, 104, 123, 156, 160) }),
            new FrameData(14727, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 82, 141, 99), new GenericBox(1, 94, 92, 144, 127), new GenericBox(1, 107, 122, 153, 174) }),
            new FrameData(14728, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 151, 130, 190), new GenericBox(1, 104, 117, 156, 151), new GenericBox(1, 89, 86, 147, 116), new GenericBox(1, 115, 79, 140, 97) }),
            new FrameData(14729, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 114, 76, 136, 93), new GenericBox(1, 91, 89, 141, 116), new GenericBox(1, 105, 115, 158, 152), new GenericBox(1, 105, 152, 129, 197) }),
            new FrameData(14730, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 114, 75, 134, 95), new GenericBox(1, 89, 90, 143, 117), new GenericBox(1, 106, 116, 159, 158), new GenericBox(1, 106, 158, 129, 196) }),
            new FrameData(14731, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 120, 94, 139, 111), new GenericBox(1, 97, 104, 156, 133), new GenericBox(1, 89, 163, 157, 194), new GenericBox(1, 100, 133, 146, 163) }, "pouso"),
        };

        var jumpForward = new List<FrameData> {
            new FrameData(14732, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 126, 82, 143, 99), new GenericBox(1, 96, 95, 144, 123), new GenericBox(1, 105, 123, 155, 158), new GenericBox(1, 101, 156, 135, 190) }, "golpe_2"),
            new FrameData(14733, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 149, 96, 165, 113), new GenericBox(1, 102, 97, 158, 145), new GenericBox(1, 95, 145, 124, 180) }),
            new FrameData(14734, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 144, 115, 164, 138), new GenericBox(1, 95, 101, 152, 150) }),
            new FrameData(14735, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 93, 104, 155, 147) }),
            new FrameData(14736, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 99, 153, 149) }),
            new FrameData(14737, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 90, 97, 147, 148) }),
            new FrameData(14738, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 88, 96, 149, 145) }),
            new FrameData(14739, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 84, 91, 136, 141), new GenericBox(1, 136, 107, 162, 168) }),
            new FrameData(14740, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 114, 82, 135, 99), new GenericBox(1, 93, 90, 137, 125), new GenericBox(1, 107, 115, 155, 144), new GenericBox(1, 120, 143, 152, 191) }),
            new FrameData(14741, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 80, 138, 98), new GenericBox(1, 93, 94, 140, 124), new GenericBox(1, 107, 123, 149, 185) }),
            new FrameData(14742, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 79, 137, 95), new GenericBox(1, 95, 89, 138, 122), new GenericBox(1, 107, 122, 155, 149), new GenericBox(1, 106, 149, 139, 193) }),
            new FrameData(14743, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 78, 137, 93), new GenericBox(1, 95, 89, 142, 125), new GenericBox(1, 109, 126, 152, 158), new GenericBox(1, 109, 159, 138, 194) }),
            new FrameData(14744, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 121, 80, 141, 96), new GenericBox(1, 100, 89, 142, 124), new GenericBox(1, 106, 123, 154, 161), new GenericBox(1, 105, 160, 138, 192) }),
        };

        var JumpBackward = new List<FrameData>(jumpForward);
        JumpBackward.Reverse();

        var crouchingInFrames = new List<FrameData> {
            new FrameData(14696, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 99, 143, 154), new GenericBox(1, 132, 105, 156, 134), new GenericBox(1, 94, 101, 130, 122), new GenericBox(1, 117, 89, 140, 107), new GenericBox(1, 90, 137, 158, 195), pushbox }),
            new FrameData(14697, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 103, 115, 147, 164), new GenericBox(1, 98, 116, 135, 145), new GenericBox(1, 134, 120, 159, 152), new GenericBox(1, 91, 150, 162, 195), pushbox }),
            new FrameData(14698, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 98, 132, 163, 196), pushbox}),
            new FrameData(14699, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 131, 160, 195), pushbox}),
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(14704, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14705, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14706, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14707, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 131, 160, 195), pushbox}),
        };

        // Specials
        var heavyHadukenFrames = new List<FrameData> {
            new FrameData(15328, 0, 0, new List<GenericBox> { pushbox, }, "haduken"),
            new FrameData(15329, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15330, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15331, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15332, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15333, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15334, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15335, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15336, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15337, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15338, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15339, 0, 0, new List<GenericBox> { pushbox, })
        };

        var lightHadukenFrames = new List<FrameData> {
            new FrameData(15329, 0, 0, new List<GenericBox> { pushbox, }, "haduken"),
            new FrameData(15330, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15331, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15333, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15334, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15335, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15336, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15337, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15338, 0, 0, new List<GenericBox> { pushbox, }),
        };

        var heavyShoryFrames = new List<FrameData> {
            new FrameData(15342, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 136, 103, 153, 118), new GenericBox(1, 110, 107, 149, 145), new GenericBox(1, 104, 145, 161, 175), new GenericBox(1, 94, 175, 154, 196) }),
            new FrameData(15343, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 143, 113, 159, 132), new GenericBox(1, 107, 119, 157, 154), new GenericBox(1, 109, 154, 164, 171), new GenericBox(1, 96, 172, 155, 195) }),
            new FrameData(15344, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 144, 122, 161, 140), new GenericBox(1, 107, 130, 151, 167), new GenericBox(1, 107, 155, 162, 193) }),
            new FrameData(15345, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 139, 116, 160, 136), new GenericBox(1, 107, 120, 155, 164), new GenericBox(1, 98, 164, 155, 195) }, "shory"),
            new FrameData(15345, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 139, 116, 160, 136), new GenericBox(1, 107, 120, 155, 164), new GenericBox(1, 98, 164, 155, 195) }, "shory"),
            
            new FrameData(15346, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 154, 115, 187, 153), new GenericBox(0, 153, 153, 170, 171), new GenericBox(1, 109, 108, 151, 152), new GenericBox(1, 109, 153, 152, 172), new GenericBox(1, 97, 171, 151, 195) }),
            new FrameData(15347, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 129, 30, 155, 88), new GenericBox(0, 146, 102, 169, 139), new GenericBox(1, 110, 74, 127, 89), new GenericBox(1, 98, 89, 142, 120), new GenericBox(1, 111, 119, 145, 149), new GenericBox(1, 112, 150, 135, 196) }),
            new FrameData(15348, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 132, 47, 153, 71), new GenericBox(0, 151, 119, 164, 136), new GenericBox(1, 109, 82, 128, 99), new GenericBox(1, 100, 94, 147, 120), new GenericBox(1, 108, 120, 149, 146), new GenericBox(1, 109, 146, 133, 191) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),

            // Extend
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            // Extend
            new FrameData(15350, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 104, 83, 125, 99), new GenericBox(1, 100, 91, 141, 127), new GenericBox(1, 107, 127, 144, 157), new GenericBox(1, 112, 157, 137, 186) }),
            new FrameData(15351, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 85, 123, 99), new GenericBox(1, 105, 90, 141, 128), new GenericBox(1, 105, 127, 143, 151), new GenericBox(1, 108, 151, 141, 177) }),
            new FrameData(15352, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 83, 128, 98), new GenericBox(1, 103, 93, 142, 135), new GenericBox(1, 87, 134, 136, 157), new GenericBox(1, 104, 157, 137, 175) }),
            new FrameData(15353, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 107, 95, 142, 130), new GenericBox(1, 111, 83, 129, 96), new GenericBox(1, 88, 130, 135, 154), new GenericBox(1, 103, 155, 137, 176) }),
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            // Extend
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            // Extend
            new FrameData(15355, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 89, 133, 106), new GenericBox(1, 103, 101, 142, 129), new GenericBox(1, 89, 128, 140, 153), new GenericBox(1, 90, 153, 148, 195) }),
        };

        var defaultShoryFrames = new List<FrameData> {
            new FrameData(15342, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 136, 103, 153, 118), new GenericBox(1, 110, 107, 149, 145), new GenericBox(1, 104, 145, 161, 175), new GenericBox(1, 94, 175, 154, 196) }),
            new FrameData(15343, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 143, 113, 159, 132), new GenericBox(1, 107, 119, 157, 154), new GenericBox(1, 109, 154, 164, 171), new GenericBox(1, 96, 172, 155, 195) }),
            new FrameData(15344, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 144, 122, 161, 140), new GenericBox(1, 107, 130, 151, 167), new GenericBox(1, 107, 155, 162, 193) }),
            new FrameData(15345, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 139, 116, 160, 136), new GenericBox(1, 107, 120, 155, 164), new GenericBox(1, 98, 164, 155, 195) }, "shory"),
            new FrameData(15346, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 154, 115, 187, 153), new GenericBox(0, 153, 153, 170, 171), new GenericBox(1, 109, 108, 151, 152), new GenericBox(1, 109, 153, 152, 172), new GenericBox(1, 97, 171, 151, 195) }),
            new FrameData(15347, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 129, 30, 155, 88), new GenericBox(0, 146, 102, 169, 139), new GenericBox(1, 110, 74, 127, 89), new GenericBox(1, 98, 89, 142, 120), new GenericBox(1, 111, 119, 145, 149), new GenericBox(1, 112, 150, 135, 196) }),
            new FrameData(15348, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 132, 47, 153, 71), new GenericBox(0, 151, 119, 164, 136), new GenericBox(1, 109, 82, 128, 99), new GenericBox(1, 100, 94, 147, 120), new GenericBox(1, 108, 120, 149, 146), new GenericBox(1, 109, 146, 133, 191) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15350, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 104, 83, 125, 99), new GenericBox(1, 100, 91, 141, 127), new GenericBox(1, 107, 127, 144, 157), new GenericBox(1, 112, 157, 137, 186) }),
            new FrameData(15351, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 85, 123, 99), new GenericBox(1, 105, 90, 141, 128), new GenericBox(1, 105, 127, 143, 151), new GenericBox(1, 108, 151, 141, 177) }),
            new FrameData(15352, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 83, 128, 98), new GenericBox(1, 103, 93, 142, 135), new GenericBox(1, 87, 134, 136, 157), new GenericBox(1, 104, 157, 137, 175) }),
            new FrameData(15353, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 107, 95, 142, 130), new GenericBox(1, 111, 83, 129, 96), new GenericBox(1, 88, 130, 135, 154), new GenericBox(1, 103, 155, 137, 176) }),
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            new FrameData(15355, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 89, 133, 106), new GenericBox(1, 103, 101, 142, 129), new GenericBox(1, 89, 128, 140, 153), new GenericBox(1, 90, 153, 148, 195) }),
        };

        var lightShoryFrames = new List<FrameData> {
            new FrameData(15344, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 144, 122, 161, 140), new GenericBox(1, 107, 130, 151, 167), new GenericBox(1, 107, 155, 162, 193) }),
            new FrameData(15345, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 139, 116, 160, 136), new GenericBox(1, 107, 120, 155, 164), new GenericBox(1, 98, 164, 155, 195) }, "shory"),
            new FrameData(15346, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 154, 115, 187, 153), new GenericBox(0, 153, 153, 170, 171), new GenericBox(1, 109, 108, 151, 152), new GenericBox(1, 109, 153, 152, 172), new GenericBox(1, 97, 171, 151, 195) }),
            new FrameData(15347, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 129, 30, 155, 88), new GenericBox(0, 146, 102, 169, 139), new GenericBox(1, 110, 74, 127, 89), new GenericBox(1, 98, 89, 142, 120), new GenericBox(1, 111, 119, 145, 149), new GenericBox(1, 112, 150, 135, 196) }),
            new FrameData(15348, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 132, 47, 153, 71), new GenericBox(0, 151, 119, 164, 136), new GenericBox(1, 109, 82, 128, 99), new GenericBox(1, 100, 94, 147, 120), new GenericBox(1, 108, 120, 149, 146), new GenericBox(1, 109, 146, 133, 191) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15349, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 83, 127, 97), new GenericBox(1, 101, 97, 145, 130), new GenericBox(1, 111, 124, 151, 152), new GenericBox(1, 110, 152, 133, 187) }),
            new FrameData(15350, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 104, 83, 125, 99), new GenericBox(1, 100, 91, 141, 127), new GenericBox(1, 107, 127, 144, 157), new GenericBox(1, 112, 157, 137, 186) }),
            new FrameData(15351, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 105, 85, 123, 99), new GenericBox(1, 105, 90, 141, 128), new GenericBox(1, 105, 127, 143, 151), new GenericBox(1, 108, 151, 141, 177) }),
            new FrameData(15352, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 83, 128, 98), new GenericBox(1, 103, 93, 142, 135), new GenericBox(1, 87, 134, 136, 157), new GenericBox(1, 104, 157, 137, 175) }),
            new FrameData(15353, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 107, 95, 142, 130), new GenericBox(1, 111, 83, 129, 96), new GenericBox(1, 88, 130, 135, 154), new GenericBox(1, 103, 155, 137, 176) }),
            new FrameData(15354, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 112, 84, 129, 99), new GenericBox(1, 105, 96, 147, 129), new GenericBox(1, 91, 130, 141, 155), new GenericBox(1, 93, 155, 138, 182) }),
            new FrameData(15355, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 89, 133, 106), new GenericBox(1, 103, 101, 142, 129), new GenericBox(1, 89, 128, 140, 153), new GenericBox(1, 90, 153, 148, 195) }),
            new FrameData(15355, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 89, 133, 106), new GenericBox(1, 103, 101, 142, 129), new GenericBox(1, 89, 128, 140, 153), new GenericBox(1, 90, 153, 148, 195) }),
        };

        var heavyTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> { pushbox, }, "tatso", hasHit: false),
            new FrameData(15357, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15358, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15359, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15457, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15458, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15459, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15460, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15461, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15457, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15458, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15459, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15460, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15461, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15366, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15367, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15368, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15369, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15370, 0, 0, new List<GenericBox> { pushbox, })
        };

        var lightTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> { pushbox, }, "tatso"),
            new FrameData(15357, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15358, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15359, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15457, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15458, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15459, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15460, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15461, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15366, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15367, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15368, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15369, 0, 0, new List<GenericBox> { pushbox, })
        };
        
        var AirbonedFrames = new List<FrameData> {
            new FrameData(14880, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 71, 90, 89, 105), new GenericBox(1, 88, 93, 158, 188) }),
            new FrameData(14881, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 66, 102, 84, 115), new GenericBox(1, 82, 99, 154, 133), new GenericBox(1, 125, 116, 172, 167) }),
            new FrameData(14882, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 67, 105, 145, 131), new GenericBox(1, 138, 112, 177, 165) }),
            new FrameData(14883, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 67, 106, 187, 142) }),
            new FrameData(14884, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 69, 108, 189, 140) }),
            new FrameData(14885, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 69, 109, 192, 141) }),
            new FrameData(14886, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 76, 101, 185, 148) }),
            new FrameData(14887, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 92, 99, 134, 159), new GenericBox(1, 125, 74, 174, 115) }),
        };

        var fallingFrames = new List<FrameData> {
            new FrameData(14888, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 79, 146, 129, 196), new GenericBox(1, 78, 102, 117, 147) }),
            new FrameData(14889, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 77, 153, 131, 197), new GenericBox(1, 47, 140, 86, 173) }),
            new FrameData(14890, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14891, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14892, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14893, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14894, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14895, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14896, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14897, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14898, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14899, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14900, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14901, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14902, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14903, 0, 0, new List<GenericBox> { pushbox }),
        };

        var OnGroundFrames = new List<FrameData> {
            new FrameData(14904, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14904, 0, 0, new List<GenericBox> { pushbox }),
        };

        var wakeupFrames = new List<FrameData> {
            new FrameData(14976, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14977, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14978, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14979, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14980, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14981, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14982, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14983, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14984, 0, 0, new List<GenericBox> { pushbox }),
            new FrameData(14985, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 98, 134, 111), new GenericBox(1, 93, 105, 155, 139), new GenericBox(1, 90, 139, 152, 195) }),
        };

        // Super
        var SA1 = new List<FrameData>{
            new FrameData(15072, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 102, 143, 150), new GenericBox(1, 82, 107, 100, 132), new GenericBox(1, 138, 124, 170, 150), new GenericBox(1, 114, 143, 148, 193), new GenericBox(1, 106, 92, 131, 106), pushbox},  "shinpu"),
            new FrameData(15073, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 176, 89, 210, 117), new GenericBox(0, 158, 105, 184, 126), new GenericBox(0, 139, 118, 166, 142), new GenericBox(1, 90, 93, 141, 146), new GenericBox(1, 110, 137, 137, 194), new GenericBox(1, 69, 114, 90, 136), pushbox}),
            new FrameData(15074, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 186, 95, 219, 120), new GenericBox(0, 162, 106, 189, 129), new GenericBox(0, 145, 117, 171, 140), pushbox , new GenericBox(1, 91, 91, 148, 140), new GenericBox(1, 112, 140, 139, 194), new GenericBox(1, 70, 120, 92, 140) }),
            new FrameData(15075, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 174, 118, 204, 144), new GenericBox(0, 145, 123, 174, 146), pushbox, new GenericBox(1, 93, 94, 148, 139), new GenericBox(1, 112, 137, 144, 193), new GenericBox(1, 79, 128, 101, 148) }),
            new FrameData(15076, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 91, 131, 105), new GenericBox(1, 94, 96, 154, 140), new GenericBox(1, 135, 138, 176, 163), new GenericBox(1, 107, 141, 140, 193), pushbox}),
            new FrameData(15077, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 92, 149, 145), new GenericBox(1, 106, 139, 144, 194) }),
            new FrameData(15078, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 88, 124, 106), new GenericBox(1, 90, 100, 150, 132), new GenericBox(1, 100, 133, 141, 194), pushbox}),
            new FrameData(15079, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 89, 131, 107), new GenericBox(1, 92, 101, 144, 148), new GenericBox(1, 96, 148, 145, 195) }),
            new FrameData(15080, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 88, 137, 105), new GenericBox(1, 108, 100, 142, 151), new GenericBox(1, 95, 100, 153, 130), new GenericBox(1, 98, 143, 156, 195), pushbox}),
            new FrameData(15104, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 124, 91, 145, 109), new GenericBox(1, 104, 96, 141, 155), new GenericBox(1, 139, 114, 156, 131), new GenericBox(1, 93, 144, 155, 195), pushbox}, hasHit: false),
            new FrameData(15105, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 127, 92, 149, 107), new GenericBox(1, 105, 100, 153, 153), new GenericBox(1, 124, 152, 161, 193), new GenericBox(1, 149, 129, 171, 155) }),
            new FrameData(15106, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 125, 91, 149, 105), new GenericBox(1, 107, 100, 153, 147), new GenericBox(1, 121, 147, 157, 194), new GenericBox(1, 153, 129, 172, 157), pushbox, new GenericBox(0, 186, 154, 207, 181), new GenericBox(0, 171, 139, 187, 169) }),
            new FrameData(15107, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 126, 91, 149, 105), new GenericBox(1, 104, 97, 155, 146), new GenericBox(1, 123, 140, 157, 193), new GenericBox(1, 151, 129, 174, 160) }),
            new FrameData(15108, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 91, 148, 109), new GenericBox(1, 104, 99, 154, 150), new GenericBox(1, 127, 130, 163, 194), pushbox}),
            new FrameData(15109, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 90, 148, 106), new GenericBox(1, 107, 99, 156, 154), new GenericBox(1, 108, 153, 159, 193) }),
            new FrameData(15110, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 90, 141, 107), new GenericBox(1, 100, 97, 139, 153), new GenericBox(1, 137, 105, 156, 132), new GenericBox(1, 97, 143, 155, 195), pushbox}),
            new FrameData(15072, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 102, 143, 150), new GenericBox(1, 82, 107, 100, 132), new GenericBox(1, 138, 124, 170, 150), new GenericBox(1, 114, 143, 148, 193), new GenericBox(1, 106, 92, 131, 106), pushbox}, hasHit: false),
            new FrameData(15073, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 176, 89, 210, 117), new GenericBox(0, 158, 105, 184, 126), new GenericBox(0, 139, 118, 166, 142), new GenericBox(1, 90, 93, 141, 146), new GenericBox(1, 110, 137, 137, 194), new GenericBox(1, 69, 114, 90, 136), pushbox}),
            new FrameData(15074, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 186, 95, 219, 120), new GenericBox(0, 162, 106, 189, 129), new GenericBox(0, 145, 117, 171, 140), pushbox , new GenericBox(1, 91, 91, 148, 140), new GenericBox(1, 112, 140, 139, 194), new GenericBox(1, 70, 120, 92, 140) }),
            new FrameData(15075, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 174, 118, 204, 144), new GenericBox(0, 145, 123, 174, 146), pushbox, new GenericBox(1, 93, 94, 148, 139), new GenericBox(1, 112, 137, 144, 193), new GenericBox(1, 79, 128, 101, 148) }),
            new FrameData(15076, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 91, 131, 105), new GenericBox(1, 94, 96, 154, 140), new GenericBox(1, 135, 138, 176, 163), new GenericBox(1, 107, 141, 140, 193), pushbox}),
            new FrameData(15077, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 92, 149, 145), new GenericBox(1, 106, 139, 144, 194) }),
            new FrameData(15078, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 88, 124, 106), new GenericBox(1, 90, 100, 150, 132), new GenericBox(1, 100, 133, 141, 194), pushbox}),
            new FrameData(15079, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 89, 131, 107), new GenericBox(1, 92, 101, 144, 148), new GenericBox(1, 96, 148, 145, 195) }),
            new FrameData(15080, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 88, 137, 105), new GenericBox(1, 108, 100, 142, 151), new GenericBox(1, 95, 100, 153, 130), new GenericBox(1, 98, 143, 156, 195), pushbox}),
            new FrameData(15104, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 124, 91, 145, 109), new GenericBox(1, 104, 96, 141, 155), new GenericBox(1, 139, 114, 156, 131), new GenericBox(1, 93, 144, 155, 195), pushbox}, hasHit: false),
            new FrameData(15105, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 127, 92, 149, 107), new GenericBox(1, 105, 100, 153, 153), new GenericBox(1, 124, 152, 161, 193), new GenericBox(1, 149, 129, 171, 155) }),
            new FrameData(15106, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 125, 91, 149, 105), new GenericBox(1, 107, 100, 153, 147), new GenericBox(1, 121, 147, 157, 194), new GenericBox(1, 153, 129, 172, 157), pushbox, new GenericBox(0, 186, 154, 207, 181), new GenericBox(0, 171, 139, 187, 169) }),
            new FrameData(15107, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 126, 91, 149, 105), new GenericBox(1, 104, 97, 155, 146), new GenericBox(1, 123, 140, 157, 193), new GenericBox(1, 151, 129, 174, 160) }),
            new FrameData(15108, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 91, 148, 109), new GenericBox(1, 104, 99, 154, 150), new GenericBox(1, 127, 130, 163, 194), pushbox}),
            new FrameData(15109, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 90, 148, 106), new GenericBox(1, 107, 99, 156, 154), new GenericBox(1, 108, 153, 159, 193) }),
            new FrameData(15110, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 122, 90, 141, 107), new GenericBox(1, 100, 97, 139, 153), new GenericBox(1, 137, 105, 156, 132), new GenericBox(1, 97, 143, 155, 195), pushbox}),
            new FrameData(15072, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 102, 143, 150), new GenericBox(1, 82, 107, 100, 132), new GenericBox(1, 138, 124, 170, 150), new GenericBox(1, 114, 143, 148, 193), new GenericBox(1, 106, 92, 131, 106), pushbox}, hasHit: false),
            new FrameData(15073, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 176, 89, 210, 117), new GenericBox(0, 158, 105, 184, 126), new GenericBox(0, 139, 118, 166, 142), new GenericBox(1, 90, 93, 141, 146), new GenericBox(1, 110, 137, 137, 194), new GenericBox(1, 69, 114, 90, 136), pushbox}),
            new FrameData(15074, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 186, 95, 219, 120), new GenericBox(0, 162, 106, 189, 129), new GenericBox(0, 145, 117, 171, 140), pushbox , new GenericBox(1, 91, 91, 148, 140), new GenericBox(1, 112, 140, 139, 194), new GenericBox(1, 70, 120, 92, 140) }),
            new FrameData(15075, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(0, 174, 118, 204, 144), new GenericBox(0, 145, 123, 174, 146), pushbox, new GenericBox(1, 93, 94, 148, 139), new GenericBox(1, 112, 137, 144, 193), new GenericBox(1, 79, 128, 101, 148) }),
            new FrameData(15076, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 91, 131, 105), new GenericBox(1, 94, 96, 154, 140), new GenericBox(1, 135, 138, 176, 163), new GenericBox(1, 107, 141, 140, 193), pushbox}),
            new FrameData(15077, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 92, 149, 145), new GenericBox(1, 106, 139, 144, 194) }),
            new FrameData(15078, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 88, 124, 106), new GenericBox(1, 90, 100, 150, 132), new GenericBox(1, 100, 133, 141, 194), pushbox}),
            new FrameData(15079, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 89, 131, 107), new GenericBox(1, 92, 101, 144, 148), new GenericBox(1, 96, 148, 145, 195) }),
            new FrameData(15080, 6.25f, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 88, 137, 105), new GenericBox(1, 108, 100, 142, 151), new GenericBox(1, 95, 100, 153, 130), new GenericBox(1, 98, 143, 156, 195), pushbox}),
        };

        var SA1_tail = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> { pushbox, }, "Jinraikyaku", hasHit: false),
            new FrameData(15357, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15358, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15359, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15457, 0, 0, new List<GenericBox> { pushbox, }), //
            new FrameData(15458, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15459, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15460, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15461, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15457, 0, 0, new List<GenericBox> { pushbox, }), //
            new FrameData(15458, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15459, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15460, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15461, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15366, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15367, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15368, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15369, 0, 0, new List<GenericBox> { pushbox, }),
            new FrameData(15370, 0, 0, new List<GenericBox> { pushbox, })
        };

        var Shungoku = new List<FrameData> {
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),  
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),  
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),  
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),
            new FrameData(14768, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 129, 200, 173) }),          
        };

        // States
        var animations = new Dictionary<string, Animation> {
            // Normals
            { "Idle", new Animation(idleFrames, "Idle", 20)},
            { "OnBlock", new Animation(OnBlockFrames, "OnBlock", 20, doChangeState: false)}, 
            { "OnHit", new Animation(OnHit3Frames, "OnHit", 20, doChangeState: false)},
            // Normals
            { "LPAttack", new Animation(LPFrames, "Idle", 30)},
            { "LKAttack", new Animation(LKFrames, "Idle", 30)},
            { "MPAttack", new Animation(MPFrames, "Idle", 20)},
            { "MKAttack", new Animation(MKFrames, "Idle", 20)},
            { "BackMKAttack", new Animation(BackMKFrames, "Idle", 20)},
            { "BackMPAttack", new Animation(HPFrames, "Idle", 20)},
            { "CloseHPAttack", new Animation(cl_HPFrames, "Idle", 30)},
            // Movment
            { "WalkingForward", new Animation(walkingForwardFrames, "WalkingForward", 20)},
            { "WalkingBackward", new Animation(walkingBackwardFrames, "WalkingBackward", 20)},
            { "DashForward", new Animation(dashForwardFrames, "Idle", 20)},
            { "DashBackward", new Animation(dashBackwardFrames, "Idle", 20)},
            { "Jump", new Animation(jumpFrames, "Idle", 20)},
            { "JumpForward", new Animation(jumpForward, "Idle", 20)},
            { "JumpBackward", new Animation(JumpBackward, "Idle", 20)},
            { "CrouchingIn", new Animation(crouchingInFrames, "Crouching", 20)},
            { "Crouching", new Animation(crouchingFrames, "Crouching", 4)},
            // Super
            { "SA1", new Animation(SA1, "SA1_tail", 60)},
            { "SA1_tail", new Animation(SA1_tail, "Idle", 30)},
            { "Shungoku", new Animation(Shungoku, "Idle", 10)},
            { "Shungoku_End", new Animation(idleFrames, "Idle", 10)},
            // Specials
            { "LightShory", new Animation(lightShoryFrames, "Idle", 30)},
            { "HeavyShory", new Animation(heavyShoryFrames, "Idle", 30)},
            { "LightHaduken", new Animation(lightHadukenFrames, "Idle", 20)},
            { "HeavyHaduken", new Animation(heavyHadukenFrames, "Idle", 20)},
            { "LightTatso", new Animation(lightTatsoFrames, "Idle", 30)},
            { "HeavyTatso", new Animation(heavyTatsoFrames, "Idle", 30)},
            { "AirTatso", new Animation(heavyTatsoFrames, "AirTatso", 30)},
            // Hit and Block
            { "Airboned", new Animation(AirbonedFrames, "Falling", 15)},
            { "Falling", new Animation(fallingFrames, "OnGround", 15)},
            { "OnGround", new Animation(OnGroundFrames, "Wakeup", 2)},
            { "Wakeup", new Animation(wakeupFrames, "Idle", 15)},
            // Bonus
            { "Intro", new Animation(introFrames, "Idle", 10)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehave() {
        if (this.behave == false) return;

        if (this.StunFrames > 0) {
            this.StunFrames -= 1;
            if (this.StunFrames == 0) this.ChangeState("Idle");
            return;
        }

        this.DizzyPoints.X = Math.Max(Math.Min(this.DizzyPoints.Y, this.DizzyPoints.X + 1), 0);

        if ((this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward") & !InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) & !InputManager.Instance.Key_hold("Right", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("Idle");
            physics.reset();
        }

        // Super
        if ((InputManager.Instance.Was_down(new string[] {"Down", "Right", "Down", "Right", "A"}, 10, player: this.playerIndex, facing: this.facing) || InputManager.Instance.Was_down(new string[] {"Down", "Down", "RB"}, 10, player: this.playerIndex, facing: this.facing) ) && (this.notActing || (this.CurrentState == "CloseHPAttack" && this.hasHit))) {
            this.ChangeState("SA1");
            this.stage.spawnParticle("SALighting", this.Position.X, this.Position.Y, X_offset: 50, Y_offset: -120, facing: this.facing);
            this.stage.SetHitstop(54);
        } else if (this.CurrentState == "SA1_tail" && this.CurrentFrameIndex == 0) {
            this.SetVelocity(
                X: 1, 
                Y: 60, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        }

        if (InputManager.Instance.Was_down(new string[] {"C", "C", "Right", "A", "D"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing && (this.LifePoints.X / this.LifePoints.Y <= 0.5f)) {
            this.ChangeState("Shungoku");
            this.stage.spawnParticle("SABlink", this.Position.X, this.Position.Y, Y_offset: -140, facing: this.facing);
            this.stage.SetHitstop(68);
        } else if (this.CurrentState == "Shungoku" && this.CurrentAnimation.currentFrameIndex == 0) {
            this.SetVelocity(
                X: 8f, 
                Y: 0, 
                T: (this.CurrentAnimation.Frames.Count() - 2) * (60 / this.CurrentAnimation.framerate));
        }

        // Shorys
        if (InputManager.Instance.Was_down(new string[] {"Right", "Down", "Right", "C"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("LightShory");
        } else if (this.CurrentState == "LightShory" && this.CurrentAnimation.currentFrameIndex == 4) {
            this.SetVelocity(
                X: 1.6f, 
                Y: 43, 
                T: (this.CurrentAnimation.Frames.Count() - 4) * (60 / this.CurrentAnimation.framerate));
        } 
        if (InputManager.Instance.Was_down(new string[] {"Right", "Down", "Right", "D"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("HeavyShory");
        } else if (this.CurrentState == "HeavyShory" && this.CurrentAnimation.currentFrameIndex == 6) {
            this.SetVelocity(
                X: 2.4f, 
                Y: 73, 
                T: (this.CurrentAnimation.Frames.Count() - 6) * (60 / this.CurrentAnimation.framerate));
        } 

        // Haduken
        if (InputManager.Instance.Was_down(new string[] {"Down", "Right", "C"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("LightHaduken");
        } else if (this.CurrentState == "LightHaduken" && this.CurrentFrameIndex == 3 && this.CurrentAnimation.frameCounter == 0) {
            stage.spawnFireball("Ken1", this.Position.X, this.Position.Y - 5, this.facing, this.team, X_offset: 25);
        } 
        if (InputManager.Instance.Was_down(new string[] {"Down", "Right", "D"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("HeavyHaduken");
        } else if (this.CurrentState == "HeavyHaduken" && this.CurrentFrameIndex == 4 && this.CurrentAnimation.frameCounter == 0) {
            stage.spawnFireball("Ken2", this.Position.X, this.Position.Y - 5, this.facing, this.team, X_offset: 25);
        }

        // Tatso
        if (InputManager.Instance.Was_down(new string[] {"Down", "Left", "A"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("LightTatso");
        } else if (this.CurrentState == "LightTatso" && this.CurrentAnimation.currentFrameIndex == 2) {
            this.SetVelocity(
                X: this.tatso_speed - 1, 
                Y: 10, 
                T: (this.CurrentAnimation.Frames.Count() - 2) * (60 / this.CurrentAnimation.framerate));
        } 
        if (InputManager.Instance.Was_down(new string[] {"Down", "Left", "B"}, 10, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("HeavyTatso");
        } else if (this.CurrentState == "HeavyTatso" && this.CurrentAnimation.currentFrameIndex == 2) {
            this.SetVelocity(
                X: this.tatso_speed, 
                Y: 10, 
                T: (this.CurrentAnimation.Frames.Count() - 2) * (60 / this.CurrentAnimation.framerate));
        } 
        if ((InputManager.Instance.Was_down(new string[] {"Down", "Left", "B"}, 10, player: this.playerIndex, facing: this.facing) || InputManager.Instance.Was_down(new string[] {"Down", "Left", "A"}, 10)) && this.notActingAir) {
            this.ChangeState("AirTatso");
        } else if (this.CurrentState == "AirTatso" && this.Velocity.Z == 0) {
            this.ChangeState("Idle");
        } 

        // Cancels
        if (InputManager.Instance.Key_down("B", player: this.playerIndex, facing: this.facing) && this.hasHit && this.CurrentState == "LKAttack") {
            this.ChangeState("MKAttack");
        } else if (InputManager.Instance.Was_down(new string[] {"D"}, Config.hitStopTime, player: this.playerIndex, facing: this.facing) && this.hasHit && this.CurrentState == "LPAttack") {
            this.SetVelocity();
            this.ChangeState("CloseHPAttack");
        } else if (InputManager.Instance.Was_down(new string[] {"Right", "Down", "Right", "C"}, 10, player: this.playerIndex, facing: this.facing) && this.hasHit && this.CurrentState == "CloseHPAttack") {
            this.SetVelocity();
            this.ChangeState("LightShory", index: 1);
        } 

        // Normals
        if (InputManager.Instance.Key_down("B", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("BackMKAttack");
        } else if (InputManager.Instance.Key_down("D", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("BackMPAttack");
        }

        if (InputManager.Instance.Key_down("C", player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("LPAttack");
        } else if (InputManager.Instance.Key_down("A", player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("LKAttack");
        } else if (InputManager.Instance.Key_down("D", player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("MPAttack");
        } else if (InputManager.Instance.Key_down("B", player: this.playerIndex, facing: this.facing) && this.notActing ) {
            this.ChangeState("MKAttack");
        } 
        
        // Crouching
        if (InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Up", player: this.playerIndex, facing: this.facing) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("CrouchingIn");
        }
        if (this.CurrentState == "Crouching" && !InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("Idle");
        }
        if (this.CurrentState == "CrouchingOut" && InputManager.Instance.Key_hold("Down", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Up", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("Crouching");
        }

        // Dashing
        if (InputManager.Instance.Was_down(new string[] {"Right", "Right"}, 13, flexEntry: false, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("DashForward");
        } 
        else if (InputManager.Instance.Was_down(new string[] {"Left", "Left"}, 13, flexEntry: false, player: this.playerIndex, facing: this.facing) && this.notActing) {
            this.ChangeState("DashBackward");
        }

        // Walking
        if (InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Right", player: this.playerIndex, facing: this.facing) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold("Right", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingForward");
        }

        // Jumps
        if (this.notActing && this.CurrentFrameIndex > 0 && InputManager.Instance.Key_hold("Up", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Right", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("JumpForward");
            this.SetVelocity(
                X: this.move_speed + 1, 
                Y: this.jump_hight, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.notActing && this.CurrentFrameIndex > 0 && InputManager.Instance.Key_hold("Up", player: this.playerIndex, facing: this.facing) && InputManager.Instance.Key_hold("Left", player: this.playerIndex, facing: this.facing) && !InputManager.Instance.Key_hold("Right", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("JumpBackward");
            this.SetVelocity(
                X: -(this.move_speed + 1), 
                Y: this.jump_hight, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        }
        else if (this.notActing && this.CurrentFrameIndex > 0 && InputManager.Instance.Key_hold("Up", player: this.playerIndex, facing: this.facing)) {
            this.ChangeState("Jump");
        } else if (this.CurrentState == "Jump" && this.CurrentFrameIndex == 1) {
            this.SetVelocity(
                X: 0, 
                Y: this.jump_hight, 
                T: (this.CurrentAnimation.Frames.Count() - 2)* (60 / this.CurrentAnimation.framerate));
        }

    }
    
    public override int ImposeBehavior(Character target) {
        int hit = -1;
        switch (this.CurrentState) {
            case "LPAttack":
                Character.Pushback(target: target, self: this, "Light");
                if (target.isBlocking()) {
                    hit = 0;
                    target.BlockStun(this, 3);
                } else {
                    hit = 1;
                    Character.Damage(target: target, 13, 50);
                    target.HitStun(this, 3);
                }
                break;
                
            case "LKAttack":
                Character.Pushback(target: target, self: this, "Light");
                if (target.isBlocking()) {
                    hit = 0;
                    target.BlockStun(this, 2);
                } else {
                    hit = 1;
                    Character.Damage(target: target, 44, 50);
                    target.HitStun(this, 2);
                }
                break;
                
            case "MPAttack":
                Character.Pushback(target: target, self: this, "Medium");
                if (target.isBlocking()) {
                    hit = 0;
                    target.BlockStun(this, 3);
                } else {
                    hit = 1;
                    Character.Damage(target: target, 100, 172);
                    target.BlockStun(this, 4);
                }
                break;

            case "MKAttack":
                Character.Pushback(target: target, self: this, "Medium");
                if (target.isBlocking()) {
                    hit = 0;
                    target.BlockStun(this, -6);
                } else {
                    hit = 1;
                    Character.Damage(target: target, 120, 235);
                    target.HitStun(this, -2);
                }
                break;

            case "BackMKAttack":
                Character.Pushback(target: target, self: this, "Heavy");
                if (target.isBlockingHigh()) {
                    hit = 0;
                    target.BlockStun(this, 1);

                } else {
                    hit = 1;
                    Character.Damage(target: target, 56, 94);
                    target.HitStun(this, 2);
                }
                break;

            case "CloseHPAttack":
                if (target.isBlocking()) {
                    hit = 0;
                    Character.Pushback(target: target, self: this, "Light");
                    target.BlockStun(this, -2);

                } else {
                    hit = 1;
                    Character.Pushback(target: target, self: this, "Light");
                    Character.Damage(target: target, 107, 110);
                    target.HitStun(this, 0);
                }
                break;
            
            case "LightShory":
                if (target.isBlocking()) {
                    hit = 0;
                    Character.Pushback(target: target, self: this, "Heavy");
                    target.BlockStun(this, -17);

                } else {
                    hit = 1;
                    Character.Pushback(target: target, self: this, "Heavy");
                    Character.Damage(target: target, 163, 170);
                    target.HitStun(this, 0, airbone: true, airbone_height: 100);
                }
                break;

            case "HeavyShory":
                if (target.isBlocking()) {
                    hit = 0;
                    Character.Pushback(target: target, self: this, "Heavy");
                    target.BlockStun(this, -31);

                } else {
                    hit = 1;
                    Character.Pushback(target: target, self: this, "Heavy");
                    Character.Damage(target: target, 200, 170);
                    target.HitStun(this, 0, airbone: true, airbone_height: 120);
                }
                break;

            case "SA1":
                if (target.isBlocking()) {
                    hit = 0;
                    Character.Damage(target: target, 5, 0);
                    Character.Pushback(target: target, self: this, "Heavy");
                    target.BlockStun(this, 30, force: true);
                } else {
                    hit = 1;
                    Character.Damage(target: target, 45, 35);
                    target.HitStun(this, 1);
                }
                break;

            case "Shungoku":
                this.stage.spawnParticle("Shungoku", target.Position.X, this.Position.Y, Y_offset: -125, facing: this.facing);
                this.stage.spawnParticle("Shungoku_text", Camera.Instance.X, Camera.Instance.Y);
                this.stage.SetHitstop(40 * 4);

                this.ChangeState("Shungoku_End");
                this.SetVelocity();
                this.Position.X = target.Position.X - 1 * this.facing;

                target.SetVelocity();
                target.ChangeState("OnGround");

                Character.Damage(target: target, 400, 0);
                break;

            default:
                break;
        }

        return hit;
    }
}