using SFML.System;
using Character_Space;
using Animation_Space;
using Input_Space;
using Aux_Space;
using System.IO.Compression;

public class Ken : Character {
    private int tatso_speed = 5;

    public Ken(string initialState, int startX, int startY)
        : base("Ken", initialState, startX, startY, "Assets/characters/Ken/sprites", "Assets/characters/Ken/sounds")
    {
        this.LifePoints = new Vector2i(1000, 1000);
        this.StunPoints = new Vector2i(50, 50);

        this.dash_speed = 8;
        this.move_speed = 3;
        this.push_box_width = 25;
    }
    
    public override void Load() {
        // Hurtboxes
        var pushbox = new GenericBox(2, 125 - this.push_box_width, 103, 125 + this.push_box_width, 195);

        // Animations
        var introFrames = new List<FrameData> {
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15568, 0, 0, new List<GenericBox> {}),
            new FrameData(15569, 0, 0, new List<GenericBox> {}),
            new FrameData(15570, 0, 0, new List<GenericBox> {}),
            new FrameData(15571, 0, 0, new List<GenericBox> {}),
            new FrameData(15572, 0, 0, new List<GenericBox> {}),
            new FrameData(15573, 0, 0, new List<GenericBox> {}),
            new FrameData(15574, 0, 0, new List<GenericBox> {}),
            new FrameData(15575, 0, 0, new List<GenericBox> {}),
        };

        var idleFrames = new List<FrameData> {
            new FrameData(14657, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 102, 102, 140, 153), new GenericBox(1, 118, 92, 139, 109), new GenericBox(1, 137, 107, 156, 135), new GenericBox(1, 96, 105, 131, 127), new GenericBox(1, 91, 148, 152, 196) }),
            new FrameData(14658, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 101, 141, 153), new GenericBox(1, 95, 104, 155, 135), new GenericBox(1, 119, 91, 139, 108), new GenericBox(1, 91, 147, 154, 195), pushbox }),
            new FrameData(14659, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 97, 141, 149), new GenericBox(1, 118, 88, 140, 105), new GenericBox(1, 95, 98, 156, 131), new GenericBox(1, 91, 144, 154, 195), pushbox}),
            new FrameData(14660, 0, 0, new List<GenericBox> { new GenericBox(1, 100, 96, 140, 149), new GenericBox(1, 119, 85, 139, 103), new GenericBox(1, 95, 97, 156, 128), new GenericBox(1, 92, 144, 152, 196), pushbox}),
            new FrameData(14661, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 85, 139, 101), new GenericBox(1, 105, 94, 140, 147), new GenericBox(1, 91, 146, 153, 195), new GenericBox(1, 135, 97, 156, 127), new GenericBox(1, 96, 94, 129, 117) }),
            new FrameData(14662, 0, 0, new List<GenericBox> { new GenericBox(1, 118, 84, 138, 100), new GenericBox(1, 102, 94, 140, 146), new GenericBox(1, 96, 95, 157, 127), new GenericBox(1, 90, 142, 152, 195), pushbox }),
            new FrameData(14663, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 84, 138, 101), new GenericBox(1, 103, 94, 140, 148), new GenericBox(1, 96, 96, 157, 128), new GenericBox(1, 91, 133, 153, 195) }),
            new FrameData(14664, 0, 0, new List<GenericBox> { new GenericBox(1, 118, 86, 138, 102), new GenericBox(1, 102, 94, 142, 148), new GenericBox(1, 96, 96, 157, 129), new GenericBox(1, 90, 141, 152, 195), pushbox }),
            new FrameData(14665, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 85, 139, 105), new GenericBox(1, 103, 94, 140, 148), new GenericBox(1, 96, 98, 156, 131), new GenericBox(1, 91, 141, 152, 195), pushbox }),
            new FrameData(14666, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 90, 140, 107), new GenericBox(1, 103, 99, 142, 153), new GenericBox(1, 96, 102, 157, 132), new GenericBox(1, 92, 139, 152, 196), pushbox }),
        };

        var LPFrames = new List<FrameData> { 
            new FrameData(15008, 0, 0, new List<GenericBox> { new GenericBox(1, 105, 97, 144, 154), new GenericBox(1, 136, 103, 162, 129), new GenericBox(1, 91, 96, 125, 121), new GenericBox(1, 120, 87, 141, 105), new GenericBox(1, 87, 137, 162, 194), pushbox}, "golpe_1"),
            new FrameData(15009, 0, 0, new List<GenericBox> { new GenericBox(0, 145, 100, 206, 119), new GenericBox(1, 142, 103, 204, 116), new GenericBox(1, 120, 87, 142, 105), new GenericBox(1, 88, 97, 116, 121), new GenericBox(1, 105, 98, 146, 153), new GenericBox(1, 88, 148, 158, 194), pushbox}),
            new FrameData(15010, 0, 0, new List<GenericBox> { new GenericBox(0, 144, 99, 200, 119), new GenericBox(1, 139, 102, 199, 117), new GenericBox(1, 105, 96, 145, 154), new GenericBox(1, 120, 86, 145, 107), new GenericBox(1, 91, 96, 124, 122), new GenericBox(1, 89, 146, 159, 195), pushbox}),
            new FrameData(15011, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 87, 144, 107), new GenericBox(1, 139, 100, 178, 126), new GenericBox(1, 93, 99, 123, 122), new GenericBox(1, 105, 99, 145, 151), new GenericBox(1, 90, 145, 157, 194), pushbox}),
            new FrameData(15012, 0, 0, new List<GenericBox> { new GenericBox(1, 120, 88, 142, 108), new GenericBox(1, 105, 98, 143, 153), new GenericBox(1, 139, 101, 160, 128), new GenericBox(1, 93, 99, 126, 122), new GenericBox(1, 88, 147, 155, 196), pushbox}),
        };
        
        var MPFrames = new List<FrameData> {
            new FrameData(15013, 0, 0, new List<GenericBox> { new GenericBox(1, 100, 100, 146, 153), new GenericBox(1, 119, 89, 140, 106), new GenericBox(1, 85, 102, 111, 126), new GenericBox(1, 137, 102, 162, 129), new GenericBox(1, 91, 143, 154, 195), pushbox}, "golpe_2"),
            new FrameData(15014, 0, 0, new List<GenericBox> { new GenericBox(1, 121, 90, 142, 108), new GenericBox(1, 105, 98, 147, 153), new GenericBox(1, 143, 116, 164, 134), new GenericBox(1, 84, 100, 110, 115), new GenericBox(1, 92, 139, 157, 195), pushbox}),
            new FrameData(15015, 0, 0, new List<GenericBox> { new GenericBox(1, 125, 92, 148, 108), new GenericBox(1, 108, 108, 148, 157), new GenericBox(1, 146, 118, 161, 132), new GenericBox(1, 90, 102, 123, 119), new GenericBox(1, 93, 144, 164, 196), pushbox}),
            new FrameData(15016, 0, 0, new List<GenericBox> { new GenericBox(1, 111, 104, 154, 157), new GenericBox(1, 127, 94, 154, 113), new GenericBox(1, 94, 143, 162, 195), pushbox, new GenericBox(1, 153, 109, 209, 123), new GenericBox(0, 153, 107, 212, 124) }),
            new FrameData(15017, 0, 0, new List<GenericBox> { new GenericBox(0, 163, 110, 204, 124), new GenericBox(1, 110, 103, 150, 161), new GenericBox(1, 129, 95, 152, 110), new GenericBox(1, 142, 107, 201, 122), new GenericBox(1, 91, 148, 160, 195), pushbox}),
            new FrameData(15018, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 102, 145, 158), new GenericBox(1, 137, 105, 173, 128), new GenericBox(1, 123, 92, 146, 109), new GenericBox(1, 92, 144, 157, 195) }),
            new FrameData(15019, 0, 0, new List<GenericBox> { new GenericBox(1, 106, 99, 142, 156), new GenericBox(1, 96, 102, 156, 130), new GenericBox(1, 123, 90, 146, 106), new GenericBox(1, 89, 144, 160, 195), pushbox}),
        };

        var HPFrames = new List<FrameData> {
            new FrameData(15021, 4, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 97, 146, 158), new GenericBox(1, 88, 150, 164, 195) }, "golpe_grito_2"),
            new FrameData(15022, 0, 0, new List<GenericBox> { new GenericBox(1, 105, 96, 142, 160), new GenericBox(1, 84, 116, 109, 135), new GenericBox(1, 84, 149, 163, 194), pushbox }),
            new FrameData(15023, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 113, 92, 136, 111), new GenericBox(1, 103, 103, 146, 159), new GenericBox(1, 85, 145, 161, 196), new GenericBox(1, 85, 110, 114, 127) }),
            new FrameData(15024, 0, 0, new List<GenericBox> { new GenericBox(1, 106, 95, 148, 156), new GenericBox(1, 94, 117, 111, 135), new GenericBox(1, 144, 101, 182, 113), new GenericBox(0, 144, 96, 191, 119), new GenericBox(0, 171, 93, 199, 121), pushbox, new GenericBox(1, 85, 150, 162, 195) }),
            new FrameData(15025, 0, 0, new List<GenericBox> { new GenericBox(0, 166, 92, 196, 116), pushbox, new GenericBox(1, 95, 96, 153, 143), new GenericBox(1, 143, 99, 189, 112), new GenericBox(1, 82, 142, 161, 193) }),
            new FrameData(15026, 0, 0, new List<GenericBox> { new GenericBox(1, 95, 99, 146, 156), new GenericBox(1, 132, 94, 167, 119), new GenericBox(1, 79, 144, 159, 194), pushbox }),
            new FrameData(15027, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 97, 98, 147, 158), new GenericBox(1, 85, 152, 161, 196) }),
            new FrameData(15028, 0, 0, new List<GenericBox> { new GenericBox(1, 106, 95, 128, 114), new GenericBox(1, 91, 110, 143, 162), new GenericBox(1, 85, 150, 162, 194) }),
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
            new FrameData(15072, 8, 0, new List<GenericBox> { new GenericBox(1, 96, 102, 143, 150), new GenericBox(1, 82, 107, 100, 132), new GenericBox(1, 138, 124, 170, 150), new GenericBox(1, 114, 143, 148, 193), new GenericBox(1, 106, 92, 131, 106), pushbox},  "golpe_3"),
            new FrameData(15073, 2, 0, new List<GenericBox> { new GenericBox(0, 176, 89, 210, 117), new GenericBox(0, 158, 105, 184, 126), new GenericBox(0, 139, 118, 166, 142), new GenericBox(1, 90, 93, 141, 146), new GenericBox(1, 110, 137, 137, 194), new GenericBox(1, 69, 114, 90, 136), pushbox}, "golpe_grito_4"),
            new FrameData(15074, 0, 0, new List<GenericBox> { new GenericBox(0, 186, 95, 219, 120), new GenericBox(0, 162, 106, 189, 129), new GenericBox(0, 145, 117, 171, 140), pushbox , new GenericBox(1, 91, 91, 148, 140), new GenericBox(1, 112, 140, 139, 194), new GenericBox(1, 70, 120, 92, 140) }),
            new FrameData(15075, 0, 0, new List<GenericBox> { new GenericBox(0, 174, 118, 204, 144), new GenericBox(0, 145, 123, 174, 146), pushbox, new GenericBox(1, 93, 94, 148, 139), new GenericBox(1, 112, 137, 144, 193), new GenericBox(1, 79, 128, 101, 148) }),
            new FrameData(15076, 0, 0, new List<GenericBox> { new GenericBox(1, 109, 91, 131, 105), new GenericBox(1, 94, 96, 154, 140), new GenericBox(1, 135, 138, 176, 163), new GenericBox(1, 107, 141, 140, 193), pushbox}),
            new FrameData(15077, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 96, 92, 149, 145), new GenericBox(1, 106, 139, 144, 194) }),
            new FrameData(15078, 0, 0, new List<GenericBox> { new GenericBox(1, 102, 88, 124, 106), new GenericBox(1, 90, 100, 150, 132), new GenericBox(1, 100, 133, 141, 194), pushbox}),
            new FrameData(15079, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 109, 89, 131, 107), new GenericBox(1, 92, 101, 144, 148), new GenericBox(1, 96, 148, 145, 195) }),
            new FrameData(15080, 0, 0, new List<GenericBox> { new GenericBox(1, 117, 88, 137, 105), new GenericBox(1, 108, 100, 142, 151), new GenericBox(1, 95, 100, 153, 130), new GenericBox(1, 98, 143, 156, 195), pushbox}),
        };

        var BackMKFrames = new List<FrameData> {
            new FrameData(15118, 0, 0, new List<GenericBox> { new GenericBox(1, 110, 86, 134, 103), new GenericBox(1, 100, 98, 141, 147), new GenericBox(1, 106, 132, 152, 193), new GenericBox(1, 88, 101, 157, 127), pushbox}, "golpe_3"),
            new FrameData(15119, 3, 0, new List<GenericBox> { pushbox, new GenericBox(1, 76, 88, 154, 128), new GenericBox(1, 106, 116, 159, 194) }, "golpe_grito_5"),
            new FrameData(15120, 5, 0, new List<GenericBox> { new GenericBox(1, 106, 59, 137, 193), new GenericBox(1, 88, 91, 122, 129), new GenericBox(1, 68, 107, 95, 130), pushbox}),
            new FrameData(15121, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 108, 64, 140, 193), new GenericBox(1, 85, 89, 143, 134), new GenericBox(1, 75, 108, 93, 127) }),
            new FrameData(15122, 0, 0, new List<GenericBox> { new GenericBox(1, 108, 67, 144, 193), pushbox, new GenericBox(1, 86, 92, 147, 134) }),
            new FrameData(15123, 0, 0, new List<GenericBox> { pushbox, new GenericBox(0, 140, 68, 170, 97), new GenericBox(1, 94, 90, 151, 140), new GenericBox(1, 119, 140, 146, 193) }),            
            new FrameData(15124, 0, 0, new List<GenericBox> { new GenericBox(0, 164, 101, 211, 129), new GenericBox(0, 144, 113, 171, 138), new GenericBox(1, 93, 92, 148, 145), new GenericBox(1, 81, 111, 105, 147), new GenericBox(1, 116, 137, 151, 193), pushbox}),
            new FrameData(15125, -1, 0, new List<GenericBox> { new GenericBox(0, 176, 132, 222, 160), new GenericBox(0, 152, 126, 183, 150), new GenericBox(1, 103, 95, 154, 149), new GenericBox(1, 122, 141, 162, 193), new GenericBox(1, 66, 111, 102, 148), pushbox}),
            new FrameData(15126, -3, 0, new List<GenericBox> { new GenericBox(0, 165, 149, 191, 190), new GenericBox(1, 109, 95, 163, 152), new GenericBox(1, 79, 112, 109, 150), new GenericBox(1, 131, 150, 163, 193), pushbox}),
            new FrameData(15127, -4, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 98, 148, 148), new GenericBox(1, 122, 91, 148, 109), new GenericBox(1, 140, 101, 164, 126), new GenericBox(1, 86, 102, 115, 142), new GenericBox(1, 111, 136, 160, 194) }),
            new FrameData(15128, -3, 0, new List<GenericBox> { pushbox, new GenericBox(1, 106, 99, 147, 156), new GenericBox(1, 120, 89, 144, 107), new GenericBox(1, 94, 101, 163, 133), new GenericBox(1, 93, 145, 160, 194) }),
        };

        var LKFrames = new List<FrameData> {
            new FrameData(15104, 0, 0, new List<GenericBox> { new GenericBox(1, 124, 91, 145, 109), new GenericBox(1, 104, 96, 141, 155), new GenericBox(1, 139, 114, 156, 131), new GenericBox(1, 93, 144, 155, 195), pushbox}, "golpe_1"),
            new FrameData(15105, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 127, 92, 149, 107), new GenericBox(1, 105, 100, 153, 153), new GenericBox(1, 124, 152, 161, 193), new GenericBox(1, 149, 129, 171, 155) }),
            new FrameData(15106, 0, 0, new List<GenericBox> { new GenericBox(1, 125, 91, 149, 105), new GenericBox(1, 107, 100, 153, 147), new GenericBox(1, 121, 147, 157, 194), new GenericBox(1, 153, 129, 172, 157), pushbox, new GenericBox(0, 186, 154, 207, 181), new GenericBox(0, 171, 139, 187, 169) }),
            new FrameData(15107, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 126, 91, 149, 105), new GenericBox(1, 104, 97, 155, 146), new GenericBox(1, 123, 140, 157, 193), new GenericBox(1, 151, 129, 174, 160) }),
            new FrameData(15108, 0, 0, new List<GenericBox> { new GenericBox(1, 128, 91, 148, 109), new GenericBox(1, 104, 99, 154, 150), new GenericBox(1, 127, 130, 163, 194), pushbox}),
            new FrameData(15109, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 128, 90, 148, 106), new GenericBox(1, 107, 99, 156, 154), new GenericBox(1, 108, 153, 159, 193) }),
            new FrameData(15110, 0, 0, new List<GenericBox> { new GenericBox(1, 122, 90, 141, 107), new GenericBox(1, 100, 97, 139, 153), new GenericBox(1, 137, 105, 156, 132), new GenericBox(1, 97, 143, 155, 195), pushbox}),
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(14671, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 120, 89, 140, 106), new GenericBox(1, 99, 97, 142, 156), new GenericBox(1, 84, 151, 152, 195), new GenericBox(1, 139, 103, 156, 139) }),
            new FrameData(14672, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 119, 88, 140, 108), new GenericBox(1, 98, 99, 148, 151), new GenericBox(1, 143, 106, 157, 138), new GenericBox(1, 74, 151, 149, 195), pushbox}),
            new FrameData(14673, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 119, 89, 141, 108), new GenericBox(1, 98, 101, 156, 138), new GenericBox(1, 72, 137, 145, 195) }),
            new FrameData(14674, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 87, 139, 104), new GenericBox(1, 97, 96, 139, 148), new GenericBox(1, 139, 100, 155, 138), new GenericBox(1, 87, 142, 137, 192), pushbox}),
            new FrameData(14675, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 85, 140, 102), new GenericBox(1, 99, 93, 156, 134), new GenericBox(1, 89, 134, 139, 191) }),
            new FrameData(14676, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 84, 139, 102), new GenericBox(1, 97, 94, 138, 147), new GenericBox(1, 138, 97, 156, 135), new GenericBox(1, 92, 143, 138, 194), pushbox}),
            new FrameData(14677, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 116, 83, 140, 101), new GenericBox(1, 96, 94, 141, 134), new GenericBox(1, 139, 97, 158, 133), new GenericBox(1, 97, 134, 151, 195) }),
            new FrameData(14678, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 105, 94, 139, 148), new GenericBox(1, 118, 83, 138, 102), new GenericBox(1, 97, 96, 129, 117), new GenericBox(1, 138, 96, 157, 133), new GenericBox(1, 97, 141, 155, 195), pushbox}),
            new FrameData(14679, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 117, 83, 140, 101), new GenericBox(1, 97, 94, 141, 150), new GenericBox(1, 138, 98, 158, 133), new GenericBox(1, 93, 149, 169, 195) }),
            new FrameData(14680, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 85, 139, 104), new GenericBox(1, 102, 94, 142, 149), new GenericBox(1, 97, 97, 155, 134), new GenericBox(1, 92, 142, 165, 195), pushbox}),
            new FrameData(14681, this.move_speed, 0, new List<GenericBox> { pushbox, new GenericBox(1, 118, 88, 139, 105), new GenericBox(1, 105, 97, 141, 149), new GenericBox(1, 97, 103, 157, 139), new GenericBox(1, 84, 140, 162, 195) }),
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(14683, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14684, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14685, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14686, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14687, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14688, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14689, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14690, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14691, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14692, -this.move_speed, 0, new List<GenericBox> {}),
            new FrameData(14693, -this.move_speed, 0, new List<GenericBox> {})
        };

        var dashForwardFrames = new List<FrameData> {
            new FrameData(14768, 0, 0, new List<GenericBox> {  }),
            new FrameData(14769, 0, 0, new List<GenericBox> {  }),
            new FrameData(14770, 0, 0, new List<GenericBox> {  }),
            new FrameData(14771, 0, 0, new List<GenericBox> {  }),
            new FrameData(14772, 0, 0, new List<GenericBox> {  }),
            new FrameData(14773, 0, 0, new List<GenericBox> {  })
        };

        var dashBackwardFrames = new List<FrameData> {
            new FrameData(14774, 0, 0, new List<GenericBox> {  }),
            new FrameData(14775, 0, 0, new List<GenericBox> {  }),
            new FrameData(14776, 0, 0, new List<GenericBox> {  }),
            new FrameData(14777, 0, 0, new List<GenericBox> {  }),
            new FrameData(14778, 0, 0, new List<GenericBox> {  }),
            new FrameData(14779, 0, 0, new List<GenericBox> {  })
        };

        var jumpFrames = new List<FrameData> {
            new FrameData(14697, 0, 0, new List<GenericBox> { new GenericBox(1, 124, 106, 144, 125), new GenericBox(1, 99, 113, 149, 150), new GenericBox(1, 87, 150, 160, 195) }, "golpe_2"),
            new FrameData(14720, 0, 0, new List<GenericBox> { new GenericBox(1, 116, 79, 134, 93), new GenericBox(1, 136, 72, 159, 106), new GenericBox(1, 85, 67, 114, 103), new GenericBox(1, 103, 88, 140, 200) }),
            new FrameData(14721, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 87, 141, 202), new GenericBox(1, 115, 78, 135, 94), new GenericBox(1, 86, 67, 115, 105), new GenericBox(1, 133, 68, 156, 108) }),
            new FrameData(14722, 0, 0, new List<GenericBox> { new GenericBox(1, 91, 78, 156, 107), new GenericBox(1, 105, 89, 140, 140), new GenericBox(1, 100, 133, 152, 178) }),
            new FrameData(14723, 0, 0, new List<GenericBox> { new GenericBox(1, 121, 81, 141, 98), new GenericBox(1, 94, 89, 159, 119), new GenericBox(1, 102, 120, 149, 176) }),
            new FrameData(14724, 0, 0, new List<GenericBox> { new GenericBox(1, 122, 84, 145, 104), new GenericBox(1, 91, 93, 145, 127), new GenericBox(1, 104, 126, 157, 168), new GenericBox(1, 143, 108, 161, 126) }),
            new FrameData(14725, 0, 0, new List<GenericBox> { new GenericBox(1, 102, 119, 158, 161), new GenericBox(1, 124, 84, 143, 102), new GenericBox(1, 89, 91, 144, 124) }),
            new FrameData(14726, 0, 0, new List<GenericBox> { new GenericBox(1, 123, 85, 141, 101), new GenericBox(1, 95, 94, 144, 125), new GenericBox(1, 104, 123, 156, 160) }),
            new FrameData(14727, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 82, 141, 99), new GenericBox(1, 94, 92, 144, 127), new GenericBox(1, 107, 122, 153, 174) }),
            new FrameData(14728, 0, 0, new List<GenericBox> { new GenericBox(1, 105, 151, 130, 190), new GenericBox(1, 104, 117, 156, 151), new GenericBox(1, 89, 86, 147, 116), new GenericBox(1, 115, 79, 140, 97) }),
            new FrameData(14729, 0, 0, new List<GenericBox> { new GenericBox(1, 114, 76, 136, 93), new GenericBox(1, 91, 89, 141, 116), new GenericBox(1, 105, 115, 158, 152), new GenericBox(1, 105, 152, 129, 197) }),
            new FrameData(14730, 0, 0, new List<GenericBox> { new GenericBox(1, 114, 75, 134, 95), new GenericBox(1, 89, 90, 143, 117), new GenericBox(1, 106, 116, 159, 158), new GenericBox(1, 106, 158, 129, 196) }),
            new FrameData(14731, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 120, 94, 139, 111), new GenericBox(1, 97, 104, 156, 133), new GenericBox(1, 89, 163, 157, 194), new GenericBox(1, 100, 133, 146, 163) }, "pouso"),
        };

        var jumpForward = new List<FrameData> {
            new FrameData(14732, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 126, 82, 143, 99), new GenericBox(1, 96, 95, 144, 123), new GenericBox(1, 105, 123, 155, 158), new GenericBox(1, 101, 156, 135, 190) }, "golpe_2"),
            new FrameData(14733, 0, 0, new List<GenericBox> { new GenericBox(1, 149, 96, 165, 113), new GenericBox(1, 102, 97, 158, 145), new GenericBox(1, 95, 145, 124, 180) }),
            new FrameData(14734, 0, 0, new List<GenericBox> { new GenericBox(1, 144, 115, 164, 138), new GenericBox(1, 95, 101, 152, 150) }),
            new FrameData(14735, 0, 0, new List<GenericBox> { new GenericBox(1, 93, 104, 155, 147) }),
            new FrameData(14736, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 99, 153, 149) }),
            new FrameData(14737, 0, 0, new List<GenericBox> { new GenericBox(1, 90, 97, 147, 148) }),
            new FrameData(14738, 0, 0, new List<GenericBox> { new GenericBox(1, 88, 96, 149, 145) }),
            new FrameData(14739, 0, 0, new List<GenericBox> { new GenericBox(1, 84, 91, 136, 141), new GenericBox(1, 136, 107, 162, 168) }),
            new FrameData(14740, 0, 0, new List<GenericBox> { new GenericBox(1, 114, 82, 135, 99), new GenericBox(1, 93, 90, 137, 125), new GenericBox(1, 107, 115, 155, 144), new GenericBox(1, 120, 143, 152, 191) }),
            new FrameData(14741, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 80, 138, 98), new GenericBox(1, 93, 94, 140, 124), new GenericBox(1, 107, 123, 149, 185) }),
            new FrameData(14742, 0, 0, new List<GenericBox> { new GenericBox(1, 117, 79, 137, 95), new GenericBox(1, 95, 89, 138, 122), new GenericBox(1, 107, 122, 155, 149), new GenericBox(1, 106, 149, 139, 193) }),
            new FrameData(14743, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 78, 137, 93), new GenericBox(1, 95, 89, 142, 125), new GenericBox(1, 109, 126, 152, 158), new GenericBox(1, 109, 159, 138, 194) }),
            new FrameData(14744, 0, 0, new List<GenericBox> {pushbox, new GenericBox(1, 121, 80, 141, 96), new GenericBox(1, 100, 89, 142, 124), new GenericBox(1, 106, 123, 154, 161), new GenericBox(1, 105, 160, 138, 192) }),
        };

        var JumpBackward = new List<FrameData>(jumpForward);
        JumpBackward.Reverse();

        var crouchingInFrames = new List<FrameData> {
            new FrameData(14696, 0, 0, new List<GenericBox> { new GenericBox(1, 102, 99, 143, 154), new GenericBox(1, 132, 105, 156, 134), new GenericBox(1, 94, 101, 130, 122), new GenericBox(1, 117, 89, 140, 107), new GenericBox(1, 90, 137, 158, 195), pushbox }),
            new FrameData(14697, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 115, 147, 164), new GenericBox(1, 98, 116, 135, 145), new GenericBox(1, 134, 120, 159, 152), new GenericBox(1, 91, 150, 162, 195), pushbox }),
            new FrameData(14698, 0, 0, new List<GenericBox> { new GenericBox(1, 98, 132, 163, 196), pushbox}),
            new FrameData(14699, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 131, 160, 195), pushbox}),
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(14704, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14705, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14706, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 131, 160, 195), pushbox}),
            new FrameData(14707, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 131, 160, 195), pushbox}),
        };

        var crouchingOutFrames = new List<FrameData> {
            new FrameData(14700, 0, 0, new List<GenericBox> { new GenericBox(1, 116, 122, 140, 142), new GenericBox(1, 95, 132, 159, 196), pushbox}),
            new FrameData(14701, 0, 0, new List<GenericBox> { pushbox, new GenericBox(1, 97, 128, 160, 196), new GenericBox(1, 119, 118, 141, 137) }),
            new FrameData(14702, 0, 0, new List<GenericBox> { new GenericBox(1, 97, 98, 157, 135), new GenericBox(1, 117, 86, 139, 102), new GenericBox(1, 107, 98, 139, 152), new GenericBox(1, 90, 142, 154, 194), pushbox }),
        };

        var heavyHadukenFrames = new List<FrameData> {
            new FrameData(15328, 0, 0, new List<GenericBox> {  }, "haduken"),
            new FrameData(15329, 0, 0, new List<GenericBox> {  }),
            new FrameData(15330, 0, 0, new List<GenericBox> {  }),
            new FrameData(15331, 0, 0, new List<GenericBox> {  }),
            new FrameData(15332, 0, 0, new List<GenericBox> {  }),
            new FrameData(15333, 0, 0, new List<GenericBox> {  }),
            new FrameData(15334, 0, 0, new List<GenericBox> {  }),
            new FrameData(15335, 0, 0, new List<GenericBox> {  }),
            new FrameData(15336, 0, 0, new List<GenericBox> {  }),
            new FrameData(15337, 0, 0, new List<GenericBox> {  }),
            new FrameData(15338, 0, 0, new List<GenericBox> {  }),
            new FrameData(15339, 0, 0, new List<GenericBox> {  })
        };

        var lightHadukenFrames = new List<FrameData> {
            new FrameData(15329, 0, 0, new List<GenericBox> {  }, "haduken"),
            new FrameData(15330, 0, 0, new List<GenericBox> {  }),
            new FrameData(15331, 0, 0, new List<GenericBox> {  }),
            new FrameData(15333, 0, 0, new List<GenericBox> {  }),
            new FrameData(15334, 0, 0, new List<GenericBox> {  }),
            new FrameData(15335, 0, 0, new List<GenericBox> {  }),
            new FrameData(15336, 0, 0, new List<GenericBox> {  }),
            new FrameData(15337, 0, 0, new List<GenericBox> {  }),
            new FrameData(15338, 0, 0, new List<GenericBox> {  }),
        };

        var heavyShoryFrames = new List<FrameData> {
            new FrameData(15345, 0, 0, new List<GenericBox> {  }, "shory"),
            new FrameData(15345, 0, 0, new List<GenericBox> {  }),
            new FrameData(15346, 0, 0, new List<GenericBox> {  }),
            new FrameData(15347, 0, 0, new List<GenericBox> {  }),
            new FrameData(15348, 0, 0, new List<GenericBox> {  }),
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15350, 0, 0, new List<GenericBox> {  }),
            new FrameData(15351, 0, 0, new List<GenericBox> {  }),
            new FrameData(15352, 0, 0, new List<GenericBox> {  }),
            new FrameData(15353, 0, 0, new List<GenericBox> {  }),
            new FrameData(15354, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15354, 0, 0, new List<GenericBox> {  }),
            new FrameData(15354, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15355, 0, 0, new List<GenericBox> {  })
        };

        var lightShoryFrames = new List<FrameData> {
            new FrameData(15345, 0, 0, new List<GenericBox> {  }, "shory"),
            new FrameData(15345, 0, 0, new List<GenericBox> {  }),
            new FrameData(15346, 0, 0, new List<GenericBox> {  }),
            new FrameData(15347, 0, 0, new List<GenericBox> {  }),
            new FrameData(15348, 0, 0, new List<GenericBox> {  }),
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15349, 0, 0, new List<GenericBox> {  }),
            // Extend
            new FrameData(15350, 0, 0, new List<GenericBox> {  }),
            new FrameData(15351, 0, 0, new List<GenericBox> {  }),
            new FrameData(15352, 0, 0, new List<GenericBox> {  }),
            new FrameData(15353, 0, 0, new List<GenericBox> {  }),
            new FrameData(15354, 0, 0, new List<GenericBox> {  }),
            //Extend
            new FrameData(15355, 0, 0, new List<GenericBox> {  })
        };

        var heavyTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> {  }, "tatso"),
            new FrameData(15357, 0, 0, new List<GenericBox> {  }),
            new FrameData(15358, 0, 0, new List<GenericBox> {  }),
            new FrameData(15359, 0, 0, new List<GenericBox> {  }),
            new FrameData(15457, 0, 0, new List<GenericBox> {  }),
            new FrameData(15458, 0, 0, new List<GenericBox> {  }),
            new FrameData(15459, 0, 0, new List<GenericBox> {  }),
            new FrameData(15460, 0, 0, new List<GenericBox> {  }),
            new FrameData(15461, 0, 0, new List<GenericBox> {  }),
            new FrameData(15457, 0, 0, new List<GenericBox> {  }),
            new FrameData(15458, 0, 0, new List<GenericBox> {  }),
            new FrameData(15459, 0, 0, new List<GenericBox> {  }),
            new FrameData(15460, 0, 0, new List<GenericBox> {  }),
            new FrameData(15461, 0, 0, new List<GenericBox> {  }),
            new FrameData(15366, 0, 0, new List<GenericBox> {  }),
            new FrameData(15367, 0, 0, new List<GenericBox> {  }),
            new FrameData(15368, 0, 0, new List<GenericBox> {  }),
            new FrameData(15369, 0, 0, new List<GenericBox> {  }),
            new FrameData(15370, 0, 0, new List<GenericBox> {  })
        };

        var lightTatsoFrames = new List<FrameData> {
            new FrameData(15356, 0, 0, new List<GenericBox> {  }, "tatso"),
            new FrameData(15357, 0, 0, new List<GenericBox> {  }),
            new FrameData(15358, 0, 0, new List<GenericBox> {  }),
            new FrameData(15359, 0, 0, new List<GenericBox> {  }),
            new FrameData(15457, 0, 0, new List<GenericBox> {  }),
            new FrameData(15458, 0, 0, new List<GenericBox> {  }),
            new FrameData(15459, 0, 0, new List<GenericBox> {  }),
            new FrameData(15460, 0, 0, new List<GenericBox> {  }),
            new FrameData(15461, 0, 0, new List<GenericBox> {  }),
            new FrameData(15366, 0, 0, new List<GenericBox> {  }),
            new FrameData(15367, 0, 0, new List<GenericBox> {  }),
            new FrameData(15368, 0, 0, new List<GenericBox> {  }),
            new FrameData(15369, 0, 0, new List<GenericBox> {  })
        };
        
        var airbonedFrames = new List<FrameData> {
            new FrameData(14880, 0, 0, new List<GenericBox> { new GenericBox(1, 71, 90, 89, 105), new GenericBox(1, 88, 93, 158, 188) }),
            new FrameData(14881, 0, 0, new List<GenericBox> { new GenericBox(1, 66, 102, 84, 115), new GenericBox(1, 82, 99, 154, 133), new GenericBox(1, 125, 116, 172, 167) }),
            new FrameData(14882, 0, 0, new List<GenericBox> { new GenericBox(1, 67, 105, 145, 131), new GenericBox(1, 138, 112, 177, 165) }),
            new FrameData(14883, 0, 0, new List<GenericBox> { new GenericBox(1, 67, 106, 187, 142) }),
            new FrameData(14884, 0, 0, new List<GenericBox> { new GenericBox(1, 69, 108, 189, 140) }),
            new FrameData(14885, 0, 0, new List<GenericBox> { new GenericBox(1, 69, 109, 192, 141) }),
            new FrameData(14886, 0, 0, new List<GenericBox> { new GenericBox(1, 76, 101, 185, 148) }),
            new FrameData(14887, 0, 0, new List<GenericBox> { new GenericBox(1, 92, 99, 134, 159), new GenericBox(1, 125, 74, 174, 115) }),
            new FrameData(14888, 0, 0, new List<GenericBox> { new GenericBox(1, 79, 146, 129, 196), new GenericBox(1, 78, 102, 117, 147) }),
            new FrameData(14889, 0, 0, new List<GenericBox> { new GenericBox(1, 77, 153, 131, 197), new GenericBox(1, 47, 140, 86, 173) }),
        };

        var fallenFrames = new List<FrameData> {
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

        // States
        var animations = new Dictionary<string, Animation> {
            // Normals
            { "Idle", new Animation(idleFrames, "Idle", 20)},
            { "LPAttack", new Animation(LPFrames, "Idle", 30)},
            { "LKAttack", new Animation(LKFrames, "Idle", 30)},
            { "MPAttack", new Animation(MPFrames, "Idle", 20)},
            { "MKAttack", new Animation(MKFrames, "Idle", 20)},
            { "BackMKAttack", new Animation(BackMKFrames, "Idle", 20)},
            { "BackMPAttack", new Animation(HPFrames, "Idle", 20)},
            { "CloseHPAttack", new Animation(cl_HPFrames, "Idle", 30)},
            // Movment
            { "WalkingForward", new Animation(walkingForwardFrames, "Idle", 20)},
            { "WalkingBackward", new Animation(walkingBackwardFrames, "Idle", 20)},
            { "DashForward", new Animation(dashForwardFrames, "Idle", 20)},
            { "DashBackward", new Animation(dashBackwardFrames, "Idle", 20)},
            { "Jump", new Animation(jumpFrames, "Idle", 20)},
            { "JumpForward", new Animation(jumpForward, "Idle", 20)},
            { "JumpBackward", new Animation(JumpBackward, "Idle", 20)},
            { "CrouchingIn", new Animation(crouchingInFrames, "Crouching", 20)},
            { "Crouching", new Animation(crouchingFrames, "CrouchingOut", 4)},
            { "CrouchingOut", new Animation(crouchingOutFrames, "Idle", 20)},
            // Specials
            { "LightShory", new Animation(lightShoryFrames, "Idle", 20)},
            { "HeavyShory", new Animation(heavyShoryFrames, "Idle", 20)},
            { "LightHaduken", new Animation(lightHadukenFrames, "Idle", 20)},
            { "HeavyHaduken", new Animation(heavyHadukenFrames, "Idle", 20)},
            { "LightTatso", new Animation(lightTatsoFrames, "Idle", 30)},
            { "HeavyTatso", new Animation(heavyTatsoFrames, "Idle", 30)},
            { "AirTatso", new Animation(heavyTatsoFrames, "AirTatso", 60)},
            // Hit and Block
            { "Airboned", new Animation(airbonedFrames, "Fallen", 15)},
            { "Fallen", new Animation(fallenFrames, "Wakeup", 15)},
            { "Wakeup", new Animation(wakeupFrames, "Idle", 15)},
            // Bonus
            { "Intro", new Animation(introFrames, "Idle", 10)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {
        if ((this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward") & !InputManager.Instance.Key_hold("Left") & !InputManager.Instance.Key_hold("Right")) {
            this.ChangeState("Idle");
            physics.reset();
        }

        // Specials
        if (InputManager.Instance.Was_down(new string[] {"Right", "Down", "Right", "C"}, 10) && this.notActing) {
            this.ChangeState("LightShory");
        } else if (InputManager.Instance.Was_down(new string[] {"Right", "Down", "Right", "D"}, 10) && this.notActing) {
            this.ChangeState("HeavyShory");
        }

        if (InputManager.Instance.Was_down(new string[] {"Down", "Right", "C"}, 10) && this.notActing) {
            this.ChangeState("LightHaduken");
        } else if (InputManager.Instance.Was_down(new string[] {"Down", "Right", "D"}, 10) && this.notActing) {
            this.ChangeState("HeavyHaduken");
        }

        if (InputManager.Instance.Was_down(new string[] {"Down", "Left", "A"}, 10) && this.notActing) {
            this.ChangeState("LightTatso");
        } else if (InputManager.Instance.Was_down(new string[] {"Down", "Left", "B"}, 10) && this.notActing) {
            this.ChangeState("HeavyTatso");
        } else if ((InputManager.Instance.Was_down(new string[] {"Down", "Left", "B"}, 10) || InputManager.Instance.Was_down(new string[] {"Down", "Left", "A"}, 10)) && this.notActingAir) {
            this.ChangeState("AirTatso");
        }

        // Cancels
        if (InputManager.Instance.Key_down("B") && this.CurrentState == "LKAttack" && this.CurrentFrameIndex >= 3) {
            this.ChangeState("MKAttack");
        } else if (InputManager.Instance.Key_down("R") && this.CurrentState == "MPAttack" && this.CurrentFrameIndex >= 2) {
            this.ChangeState("CloseHPAttack");
        } 

        // Normals
        if (InputManager.Instance.Key_down("B") && InputManager.Instance.Key_hold("Left") && this.notActing) {
            this.ChangeState("BackMKAttack");
        } else if (InputManager.Instance.Key_down("D") && InputManager.Instance.Key_hold("Left") && this.notActing) {
            this.ChangeState("BackMPAttack");
        }

        if (InputManager.Instance.Key_down("C") && this.notActing) {
            this.ChangeState("LPAttack");
        } else if (InputManager.Instance.Key_down("A") && this.notActing) {
            this.ChangeState("LKAttack");
        } else if (InputManager.Instance.Key_down("D") && this.notActing) {
            this.ChangeState("MPAttack");
        } else if (InputManager.Instance.Key_down("B") && this.notActing ) {
            this.ChangeState("MKAttack");
        } else if (InputManager.Instance.Key_down("R") && this.notActing) {
            this.ChangeState("CloseHPAttack");
        }

        // Crouching
        if (InputManager.Instance.Key_hold("Down") && !InputManager.Instance.Key_hold("Up") && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("CrouchingIn");
        }
        if (this.CurrentState == "Crouching" && !InputManager.Instance.Key_hold("Down")) {
            this.ChangeState("CrouchingOut");
        }
        if (this.CurrentState == "CrouchingOut" && InputManager.Instance.Key_hold("Down") && !InputManager.Instance.Key_hold("Up")) {
            this.ChangeState("Crouching");
        }

        // Dashing
        if (InputManager.Instance.Was_down(new string[] {"Right", "Right"}, 13, flexEntry: false) && this.notActing) {
            this.ChangeState("DashForward");
            this.SetVelocity(
                X: this.dash_speed,
                Y: 0,
                T: 3 * (60 / this.CurrentAnimation.framerate));
        } 
        else if (InputManager.Instance.Was_down(new string[] {"Left", "Left"}, 13, flexEntry: false) && this.notActing) {
            this.ChangeState("DashBackward");
            this.SetVelocity(
                X: - this.dash_speed,
                Y: 0,
                T: 3 * (60 / this.CurrentAnimation.framerate));
        }

        // Walking
        if (InputManager.Instance.Key_hold("Left") && !InputManager.Instance.Key_hold("Right") && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold("Right") && !InputManager.Instance.Key_hold("Left") && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingForward");
        }

        // Jumps
        if (this.notActing && InputManager.Instance.Key_hold("Up") && !InputManager.Instance.Key_hold("Left") && InputManager.Instance.Key_hold("Right")) {
            this.ChangeState("JumpForward");
            this.SetVelocity(
                X: this.move_speed + 1, 
                Y: this.jump_hight, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.notActing && InputManager.Instance.Key_hold("Up") && InputManager.Instance.Key_hold("Left") && !InputManager.Instance.Key_hold("Right")) {
            this.ChangeState("JumpBackward");
            this.SetVelocity(
                X: -(this.move_speed + 1), 
                Y: this.jump_hight, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        }
        else if (this.notActing && InputManager.Instance.Key_hold("Up")) {
            this.ChangeState("Jump");
            this.SetVelocity(
                X: 0, 
                Y: this.jump_hight, 
                T: this.CurrentAnimation.Frames.Count() * (60 / this.CurrentAnimation.framerate));
        } 

        // Air Specials movement
        if (this.CurrentState == "LightShory" && this.CurrentAnimation.currentFrameIndex == 3) {
            this.SetVelocity(
                X: 1, 
                Y: 43, 
                T: (this.CurrentAnimation.Frames.Count() - 3) * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.CurrentState == "HeavyShory" && this.CurrentAnimation.currentFrameIndex == 3) {
            this.SetVelocity(
                X: 2, 
                Y: 73, 
                T: (this.CurrentAnimation.Frames.Count() - 3) * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.CurrentState == "LightTatso" && this.CurrentAnimation.currentFrameIndex == 3) {
            this.SetVelocity(
                X: this.tatso_speed - 1, 
                Y: 10, 
                T: (this.CurrentAnimation.Frames.Count() - 3) * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.CurrentState == "HeavyTatso" && this.CurrentAnimation.currentFrameIndex == 3) {
            this.SetVelocity(
                X: this.tatso_speed, 
                Y: 10, 
                T: (this.CurrentAnimation.Frames.Count() - 3) * (60 / this.CurrentAnimation.framerate));
        } 
        else if (this.CurrentState == "AirTatso" && this.notActing) {
            this.ChangeState("Idle");
        } 
    }

    public override void ImposeBehavior(Character target, bool doHit) {
        switch (this.CurrentState) {
            case "LPAttack":
                if (doHit) {
                    target.ChangeState("Airboned");
                    target.SetVelocity(this.facing * 5, 50, target.CurrentAnimation.realAnimSize);
                } else {
                    target.ChangeState("Idle");
                    this.SetVelocity(-this.facing * 2, 0, 10);
                }
                break;
                
            case "LKAttack":
                if (doHit) {
                    target.ChangeState("Airboned");
                    target.SetVelocity(this.facing * 5, 50, target.CurrentAnimation.realAnimSize);
                } else {
                    target.ChangeState("Idle");
                    this.SetVelocity(-this.facing * 2, 0, 10);
                }
                break;
                
            case "MPAttack":
                if (doHit) {
                    target.ChangeState("Airboned");
                    target.SetVelocity(this.facing * 5, 50, target.CurrentAnimation.realAnimSize);
                } else {
                    target.ChangeState("Idle");
                    this.SetVelocity(-this.facing * 2, 0, 10);
                }
                break;

            case "MKAttack":
                if (doHit) {
                    target.ChangeState("Airboned");
                    target.SetVelocity(this.facing * 5, 50, target.CurrentAnimation.realAnimSize);
                } else {
                    target.ChangeState("Idle");
                    this.SetVelocity(-this.facing * 2, 0, 10);
                }
                break;

            default:
                if (doHit) {
                    target.ChangeState("Idle");
                    target.SetVelocity(this.facing * 2, 0, 10);
                } else {
                    target.ChangeState("Idle");
                    this.SetVelocity(-this.facing * 2, 0, 10);
                }
                break;
        }
    }
}