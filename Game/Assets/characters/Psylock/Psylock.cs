using Character_Space;
using Animation_Space;
using Input_Space;
using Aux_Space;

public class Psylock : Character {
    public Psylock(string initialState, int startX, int startY)
        : base("Psylock", initialState, startX, startY, "Assets/characters/Psylock/sprites", "Assets/characters/Psylock/sounds")
    {
        this.LifePoints = 1000;
        this.StunPoints = 50;

        this.dash_speed = 8;
        this.move_speed = 3;
    }
    
    public override void Load() {
        // Boxes
        var pushbox = new GenericBox(2, 85, 177, 155, 195);

        // Animations
        var introFrames = new List<FrameData> {
            new FrameData(196, 0, 0, new List<GenericBox> {}),
            new FrameData(196, 0, 0, new List<GenericBox> {}),
            new FrameData(196, 0, 0, new List<GenericBox> {}),
            new FrameData(197, 0, 0, new List<GenericBox> {}),
            new FrameData(198, 0, 0, new List<GenericBox> {}),
            new FrameData(199, 0, 0, new List<GenericBox> {}),
            new FrameData(200, 0, 0, new List<GenericBox> {}),
            new FrameData(201, 0, 0, new List<GenericBox> {}),
            new FrameData(202, 0, 0, new List<GenericBox> {}),
            new FrameData(203, 0, 0, new List<GenericBox> {}),
            new FrameData(204, 0, 0, new List<GenericBox> {}),
            new FrameData(205, 0, 0, new List<GenericBox> {}),
            new FrameData(206, 0, 0, new List<GenericBox> {}),
            new FrameData(207, 0, 0, new List<GenericBox> {}),
            new FrameData(208, 0, 0, new List<GenericBox> {}),
            new FrameData(209, 0, 0, new List<GenericBox> {}),
            new FrameData(210, 0, 0, new List<GenericBox> {}),
            new FrameData(211, 0, 0, new List<GenericBox> {}),
            new FrameData(212, 0, 0, new List<GenericBox> {}),
            new FrameData(212, 0, 0, new List<GenericBox> {}),
            new FrameData(212, 0, 0, new List<GenericBox> {}),
        };

        var idleFrames = new List<FrameData> { 
            new FrameData(45, 0, 0, new List<GenericBox> { new GenericBox(1, 100, 97, 123, 115), new GenericBox(1, 94, 114, 124, 149), new GenericBox(1, 124, 101, 147, 129), new GenericBox(1, 85, 143, 155, 194), new GenericBox(2, 78, 176, 163, 195) }),
            new FrameData(46, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 181, 163, 194), new GenericBox(1, 103, 100, 122, 114), new GenericBox(1, 94, 115, 125, 152), new GenericBox(1, 126, 101, 148, 131), new GenericBox(1, 87, 146, 154, 193) }),
            new FrameData(47, 0, 0, new List<GenericBox> { new GenericBox(2, 79, 181, 163, 195), new GenericBox(1, 101, 99, 124, 115), new GenericBox(1, 93, 116, 130, 157), new GenericBox(1, 131, 102, 149, 132), new GenericBox(1, 83, 148, 154, 194) }),
            new FrameData(48, 0, 0, new List<GenericBox> { new GenericBox(1, 102, 99, 125, 114), new GenericBox(1, 94, 115, 127, 153), new GenericBox(1, 128, 102, 150, 133), new GenericBox(1, 84, 140, 153, 194), new GenericBox(2, 77, 177, 162, 194) }),
            new FrameData(49, 0, 0, new List<GenericBox> { new GenericBox(2, 79, 180, 163, 194), new GenericBox(1, 103, 100, 124, 116), new GenericBox(1, 93, 115, 128, 158), new GenericBox(1, 128, 104, 149, 134), new GenericBox(1, 81, 148, 154, 193) }),
            new FrameData(50, 0, 0, new List<GenericBox> { new GenericBox(1, 102, 100, 124, 116), new GenericBox(1, 95, 117, 125, 156), new GenericBox(1, 125, 107, 146, 136), new GenericBox(1, 85, 148, 153, 194), new GenericBox(2, 78, 177, 163, 195) }),
            new FrameData(51, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 177, 162, 195), new GenericBox(1, 103, 101, 124, 117), new GenericBox(1, 95, 118, 127, 157), new GenericBox(1, 127, 111, 148, 135), new GenericBox(1, 83, 153, 153, 194) }),
            new FrameData(52, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 101, 124, 115), new GenericBox(1, 95, 118, 126, 158), new GenericBox(1, 127, 116, 145, 137), new GenericBox(1, 85, 145, 152, 194), new GenericBox(2, 78, 175, 162, 194) }),
            new FrameData(53, 0, 0, new List<GenericBox> { new GenericBox(2, 80, 178, 162, 194), new GenericBox(1, 103, 99, 123, 115), new GenericBox(1, 94, 115, 126, 157), new GenericBox(1, 126, 117, 149, 133), new GenericBox(1, 85, 141, 153, 194) }),
            new FrameData(54, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 183, 162, 195), new GenericBox(1, 103, 99, 122, 114), new GenericBox(1, 94, 114, 124, 154), new GenericBox(1, 125, 114, 150, 130), new GenericBox(1, 83, 143, 152, 194) }),
            new FrameData(55, 0, 0, new List<GenericBox> { new GenericBox(1, 100, 98, 122, 113), new GenericBox(1, 95, 114, 125, 154), new GenericBox(1, 124, 112, 145, 129), new GenericBox(1, 85, 146, 149, 194), new GenericBox(2, 78, 178, 162, 195) }),
            new FrameData(56, 0, 0, new List<GenericBox> { new GenericBox(1, 101, 99, 121, 113), new GenericBox(1, 96, 114, 125, 152), new GenericBox(1, 124, 111, 142, 130), new GenericBox(1, 80, 153, 155, 193), new GenericBox(2, 76, 178, 162, 196) }),
        };

        var AFrames = new List<FrameData> {
            new FrameData(307, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 181, 178, 195), new GenericBox(1, 119, 99, 137, 115), new GenericBox(1, 105, 111, 129, 151), new GenericBox(1, 128, 111, 149, 135), new GenericBox(1, 90, 140, 160, 195)}, "golpe_1" ),
            new FrameData(308, 0, 0, new List<GenericBox> { new GenericBox(1, 128, 144, 178, 192), new GenericBox(1, 87, 151, 127, 191), new GenericBox(2, 81, 179, 170, 194), new GenericBox(1, 140, 105, 157, 120), new GenericBox(1, 127, 120, 161, 144), new GenericBox(1, 160, 109, 189, 121), new GenericBox(0, 187, 94, 208, 114) }),
            new FrameData(309, 0, 0, new List<GenericBox> { new GenericBox(0, 183, 99, 199, 114), new GenericBox(1, 138, 105, 156, 119), new GenericBox(1, 157, 111, 180, 125), new GenericBox(1, 126, 118, 158, 156), new GenericBox(1, 113, 143, 179, 192), new GenericBox(1, 87, 163, 113, 192), new GenericBox(2, 81, 183, 173, 194) }),
            new FrameData(310, 0, 0, new List<GenericBox> { new GenericBox(2, 82, 179, 179, 194), new GenericBox(1, 130, 102, 151, 117), new GenericBox(1, 121, 115, 150, 156), new GenericBox(1, 150, 113, 168, 130), new GenericBox(1, 93, 150, 169, 194) }),
            new FrameData(311, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 101, 136, 116), new GenericBox(1, 105, 111, 133, 150), new GenericBox(1, 129, 115, 148, 137), new GenericBox(1, 98, 143, 153, 170), new GenericBox(1, 85, 167, 163, 194), new GenericBox(2, 79, 176, 178, 195) }),
        };
        
        var CFrames = new List<FrameData> {
            new FrameData(312, 0, 0, new List<GenericBox> { new GenericBox(2, 81, 180, 167, 194), new GenericBox(1, 91, 151, 144, 194), new GenericBox(1, 144, 167, 166, 194), new GenericBox(1, 127, 129, 157, 144), new GenericBox(1, 114, 115, 131, 131), new GenericBox(1, 85, 124, 127, 151) }, "golpe_2"),
            new FrameData(313, 0, 0, new List<GenericBox> { new GenericBox(2, 77, 179, 178, 194), new GenericBox(1, 122, 113, 139, 127), new GenericBox(1, 102, 124, 148, 165), new GenericBox(1, 85, 165, 164, 194) }),
            new FrameData(314, 0, 0, new List<GenericBox> { new GenericBox(1, 157, 111, 174, 126), new GenericBox(1, 138, 124, 181, 165), new GenericBox(1, 180, 124, 213, 133), new GenericBox(0, 207, 118, 231, 137), new GenericBox(0, 191, 121, 208, 134), new GenericBox(1, 112, 157, 183, 190), new GenericBox(1, 82, 172, 113, 190), new GenericBox(2, 71, 176, 180, 195) }),
            new FrameData(315, 0, 0, new List<GenericBox> { new GenericBox(2, 77, 176, 182, 195), new GenericBox(0, 202, 116, 221, 137), new GenericBox(1, 163, 122, 202, 134), new GenericBox(1, 151, 109, 171, 124), new GenericBox(1, 127, 121, 168, 163), new GenericBox(1, 182, 191, 113, 154), new GenericBox(1, 82, 167, 112, 193) }),
            new FrameData(316, 0, 0, new List<GenericBox> { new GenericBox(0, 201, 117, 221, 137), new GenericBox(1, 149, 108, 172, 125), new GenericBox(1, 169, 125, 201, 134), new GenericBox(1, 129, 122, 168, 163), new GenericBox(1, 160, 147, 182, 190), new GenericBox(1, 76, 165, 113, 195), new GenericBox(1, 110, 154, 161, 191), new GenericBox(2, 75, 179, 182, 194) }),
            new FrameData(317, 0, 0, new List<GenericBox> { new GenericBox(2, 81, 177, 180, 195), new GenericBox(1, 131, 101, 149, 117), new GenericBox(1, 118, 111, 156, 157), new GenericBox(1, 153, 114, 173, 130), new GenericBox(1, 89, 156, 170, 195) }),
            new FrameData(318, 0, 0, new List<GenericBox> { new GenericBox(1, 119, 98, 137, 115), new GenericBox(1, 99, 112, 149, 157), new GenericBox(1, 84, 157, 165, 194), new GenericBox(2, 78, 178, 177, 195) }),
        };

        var BackCFrames = new List<FrameData> {
            new FrameData(319, 0, 0, new List<GenericBox> { new GenericBox(2, 76, 180, 140, 195), new GenericBox(1, 94, 88, 114, 104), new GenericBox(1, 77, 97, 114, 154), new GenericBox(1, 115, 131, 135, 163), new GenericBox(1, 132, 158, 145, 174), new GenericBox(1, 89, 154, 110, 193) }, "golpe_3"),
            new FrameData(320, 0, 0, new List<GenericBox> { new GenericBox(1, 121, 98, 139, 111), new GenericBox(1, 112, 108, 153, 138), new GenericBox(1, 99, 138, 153, 192), new GenericBox(1, 153, 159, 169, 192), new GenericBox(2, 92, 177, 172, 194) }, "power_1"),
            new FrameData(321, 0, 0, new List<GenericBox> { new GenericBox(2, 93, 178, 177, 194), new GenericBox(1, 133, 97, 150, 113), new GenericBox(1, 114, 105, 139, 121), new GenericBox(1, 123, 112, 152, 157), new GenericBox(1, 151, 122, 173, 131), new GenericBox(1, 103, 153, 164, 194) }),
            new FrameData(323, 0, 0, new List<GenericBox> { new GenericBox(1, 145, 99, 163, 115), new GenericBox(1, 131, 115, 165, 151), new GenericBox(1, 165, 106, 186, 118), new GenericBox(0, 182, 89, 220, 121), new GenericBox(1, 114, 122, 132, 133), new GenericBox(1, 98, 148, 168, 194), new GenericBox(2, 91, 177, 178, 194) }),
            new FrameData(324, 0, 0, new List<GenericBox> { new GenericBox(0, 187, 89, 224, 120), new GenericBox(1, 163, 104, 187, 118), new GenericBox(1, 143, 99, 163, 114), new GenericBox(1, 132, 114, 166, 159), new GenericBox(1, 115, 124, 133, 136), new GenericBox(1, 97, 155, 171, 195), new GenericBox(2, 93, 178, 177, 195) }),
            new FrameData(325, 0, 0, new List<GenericBox> { new GenericBox(0, 194, 96, 217, 115), new GenericBox(1, 163, 107, 188, 118), new GenericBox(1, 143, 99, 161, 115), new GenericBox(1, 132, 115, 167, 161), new GenericBox(1, 101, 158, 170, 194), new GenericBox(1, 112, 124, 131, 139), new GenericBox(2, 93, 177, 177, 195) }),
            new FrameData(326, 0, 0, new List<GenericBox> { new GenericBox(2, 80, 172, 183, 195), new GenericBox(1, 126, 92, 143, 108), new GenericBox(1, 115, 106, 145, 149), new GenericBox(1, 144, 106, 158, 129), new GenericBox(1, 84, 147, 146, 193), new GenericBox(1, 146, 156, 175, 193) }),
            new FrameData(327, 0, 0, new List<GenericBox> { new GenericBox(1, 110, 91, 128, 107), new GenericBox(1, 98, 107, 129, 150), new GenericBox(1, 89, 109, 148, 127), new GenericBox(1, 84, 146, 146, 194), new GenericBox(2, 79, 179, 157, 194) }),
            new FrameData(328, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 181, 163, 195), new GenericBox(1, 102, 107, 120, 122), new GenericBox(1, 91, 120, 125, 164), new GenericBox(1, 121, 124, 138, 141), new GenericBox(1, 86, 153, 149, 194) }),
            new FrameData(329, 0, 0, new List<GenericBox> { new GenericBox(1, 104, 100, 123, 113), new GenericBox(1, 93, 114, 125, 156), new GenericBox(1, 124, 114, 141, 131), new GenericBox(1, 115, 142, 156, 190), new GenericBox(1, 81, 155, 114, 195), new GenericBox(2, 78, 175, 163, 196) }),
            new FrameData(330, 0, 0, new List<GenericBox> { new GenericBox(1, 103, 100, 121, 115), new GenericBox(1, 93, 114, 125, 152), new GenericBox(1, 124, 113, 143, 131), new GenericBox(1, 92, 142, 144, 169), new GenericBox(1, 80, 167, 160, 194), new GenericBox(2, 76, 179, 163, 195) }),
            new FrameData(331, 0, 0, new List<GenericBox> { new GenericBox(2, 78, 179, 163, 194), new GenericBox(1, 104, 100, 121, 113), new GenericBox(1, 94, 115, 127, 157), new GenericBox(1, 127, 116, 143, 132), new GenericBox(1, 81, 156, 161, 194), new GenericBox(1, 123, 143, 142, 159) }),
        };

        var BFrames = new List<FrameData> {
            new FrameData(332, 0, 0, new List<GenericBox> { new GenericBox(2, 92, 181, 159, 194), new GenericBox(1, 122, 96, 154, 119), new GenericBox(1, 104, 109, 137, 156), new GenericBox(1, 105, 143, 156, 194) }, "golpe_1"),
            new FrameData(333, 0, 0, new List<GenericBox> { new GenericBox(1, 108, 91, 129, 107), new GenericBox(1, 101, 105, 140, 143), new GenericBox(1, 140, 109, 167, 145), new GenericBox(1, 115, 139, 138, 195), new GenericBox(2, 97, 185, 152, 195) }),
            new FrameData(334, 0, 0, new List<GenericBox> { new GenericBox(2, 99, 182, 157, 195), new GenericBox(1, 100, 99, 148, 143), new GenericBox(1, 119, 143, 145, 195), new GenericBox(1, 146, 101, 167, 136), new GenericBox(1, 163, 93, 181, 115), new GenericBox(0, 178, 78, 205, 105) }),
            new FrameData(335, 0, 0, new List<GenericBox> { new GenericBox(0, 175, 86, 197, 106), new GenericBox(1, 96, 97, 148, 142), new GenericBox(1, 148, 101, 173, 125), new GenericBox(1, 117, 140, 145, 193), new GenericBox(2, 95, 179, 160, 194) }),
            new FrameData(333, 0, 0, new List<GenericBox> { new GenericBox(1, 108, 91, 129, 107), new GenericBox(1, 101, 105, 140, 143), new GenericBox(1, 140, 109, 167, 145), new GenericBox(1, 115, 139, 138, 195), new GenericBox(2, 97, 185, 152, 195) }),
            new FrameData(332, 0, 0, new List<GenericBox> { new GenericBox(2, 92, 181, 159, 194), new GenericBox(1, 122, 96, 154, 119), new GenericBox(1, 104, 109, 137, 156), new GenericBox(1, 105, 143, 156, 194) }),
        };

        var DFrames = new List<FrameData> {
            new FrameData(345, 0, 0, new List<GenericBox> { new GenericBox(2, 74, 178, 165, 195), new GenericBox(1, 151, 119, 168, 136), new GenericBox(1, 107, 120, 153, 161), new GenericBox(1, 153, 137, 172, 165), new GenericBox(1, 82, 159, 157, 194) }, "golpe_2"),
            new FrameData(346, 0, 0, new List<GenericBox> { new GenericBox(2, 112, 178, 176, 191), new GenericBox(1, 143, 126, 176, 191), new GenericBox(1, 117, 134, 144, 153), new GenericBox(1, 160, 107, 175, 126) }),
            new FrameData(347, 0, 0, new List<GenericBox> { new GenericBox(1, 147, 133, 192, 172), new GenericBox(1, 177, 113, 189, 133), new GenericBox(1, 151, 172, 182, 190), new GenericBox(0, 123, 112, 166, 134), new GenericBox(2, 131, 178, 193, 192) }),
            new FrameData(348, 0, 0, new List<GenericBox> { new GenericBox(2, 149, 179, 195, 191), new GenericBox(0, 217, 93, 250, 130), new GenericBox(0, 207, 124, 231, 145), new GenericBox(1, 159, 131, 180, 150), new GenericBox(1, 173, 134, 211, 170), new GenericBox(1, 152, 169, 195, 191), new GenericBox(1, 182, 119, 195, 135) }),
            new FrameData(349, 0, 0, new List<GenericBox> { new GenericBox(0, 218, 98, 244, 122), new GenericBox(0, 208, 118, 229, 132), new GenericBox(1, 174, 132, 214, 166), new GenericBox(1, 158, 132, 174, 151), new GenericBox(1, 178, 111, 198, 132), new GenericBox(1, 151, 167, 189, 190), new GenericBox(2, 143, 177, 199, 191) }),
            new FrameData(350, 0, 0, new List<GenericBox> { new GenericBox(2, 131, 177, 187, 190), new GenericBox(1, 136, 122, 163, 144), new GenericBox(1, 143, 131, 186, 190), new GenericBox(1, 112, 154, 143, 172) }),
            new FrameData(351, 0, 0, new List<GenericBox> { new GenericBox(1, 116, 142, 170, 191), new GenericBox(1, 152, 135, 173, 153), new GenericBox(1, 82, 164, 116, 191), new GenericBox(2, 78, 179, 184, 195) }),
            new FrameData(352, 0, 0, new List<GenericBox> { new GenericBox(2, 75, 176, 168, 195), new GenericBox(1, 151, 120, 169, 136), new GenericBox(1, 106, 123, 153, 163), new GenericBox(1, 152, 136, 167, 155), new GenericBox(1, 80, 163, 157, 194) }),
        };

        var FrontDFrames = new List<FrameData> {
            new FrameData(336, 0, 0, new List<GenericBox> { new GenericBox(2, 86, 179, 167, 195), new GenericBox(1, 119, 92, 138, 108), new GenericBox(1, 106, 106, 137, 151), new GenericBox(1, 97, 110, 156, 126), new GenericBox(1, 92, 150, 157, 194) }, "golpe_3"),
            new FrameData(337, 0, 0, new List<GenericBox> { new GenericBox(1, 74, 92, 95, 108), new GenericBox(1, 68, 107, 101, 143), new GenericBox(1, 78, 142, 101, 192), new GenericBox(1, 99, 115, 122, 145), new GenericBox(1, 118, 134, 135, 155), new GenericBox(2, 66, 182, 129, 194) }),
            new FrameData(338, 0, 0, new List<GenericBox> { new GenericBox(1, 62, 93, 79, 110), new GenericBox(1, 60, 104, 106, 144), new GenericBox(1, 79, 135, 104, 193), new GenericBox(1, 100, 132, 115, 150), new GenericBox(2, 64, 179, 121, 194) }),
            new FrameData(339, 0, 0, new List<GenericBox> { new GenericBox(2, 62, 181, 112, 194), new GenericBox(1, 56, 100, 75, 118), new GenericBox(1, 62, 111, 112, 168), new GenericBox(1, 79, 164, 101, 194) }),
            new FrameData(340, 0, 0, new List<GenericBox> { new GenericBox(1, 78, 108, 95, 124), new GenericBox(1, 52, 122, 139, 140), new GenericBox(2, 76, 179, 139, 194), new GenericBox(1, 99, 136, 124, 181), new GenericBox(1, 86, 174, 108, 193), new GenericBox(1, 137, 124, 186, 135), new GenericBox(0, 175, 116, 208, 140), new GenericBox(0, 155, 119, 177, 137) }),
            new FrameData(341, 0, 0, new List<GenericBox> { new GenericBox(2, 64, 179, 121, 194), new GenericBox(0, 169, 116, 197, 141), new GenericBox(0, 147, 122, 169, 134), new GenericBox(1, 73, 103, 96, 120), new GenericBox(1, 53, 120, 146, 140), new GenericBox(1, 98, 139, 125, 168), new GenericBox(1, 83, 168, 112, 194) }),
            new FrameData(342, 0, 0, new List<GenericBox> { new GenericBox(1, 71, 96, 92, 115), new GenericBox(1, 61, 109, 96, 135), new GenericBox(1, 76, 124, 114, 158), new GenericBox(1, 112, 136, 130, 155), new GenericBox(1, 79, 157, 102, 194), new GenericBox(2, 54, 183, 132, 194) }),
            new FrameData(343, 0, 0, new List<GenericBox> { new GenericBox(2, 61, 182, 134, 195), new GenericBox(1, 80, 91, 99, 106), new GenericBox(1, 66, 104, 104, 143), new GenericBox(1, 102, 126, 126, 147), new GenericBox(1, 123, 142, 138, 161), new GenericBox(1, 76, 143, 100, 192) }),
            new FrameData(344, 0, 0, new List<GenericBox> { new GenericBox(2, 73, 176, 161, 194), new GenericBox(1, 94, 90, 114, 106), new GenericBox(1, 86, 106, 124, 148), new GenericBox(1, 80, 149, 150, 192) }),
        };

        var walkingForwardFrames = new List<FrameData> {
            new FrameData(60, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 88, 179, 154, 194), new GenericBox(1, 117, 87, 140, 104), new GenericBox(1, 140, 102, 164, 118), new GenericBox(1, 102, 104, 143, 179) }),
            new FrameData(61, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 102, 103, 141, 194), new GenericBox(1, 117, 88, 141, 104), new GenericBox(1, 138, 103, 163, 117), new GenericBox(2, 95, 180, 151, 195) }),
            new FrameData(62, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 96, 181, 145, 195), new GenericBox(1, 116, 86, 141, 104), new GenericBox(1, 102, 98, 138, 194), new GenericBox(1, 138, 102, 162, 116) }),
            new FrameData(63, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 85, 140, 103), new GenericBox(1, 104, 99, 137, 194), new GenericBox(1, 135, 102, 162, 116), new GenericBox(2, 98, 183, 142, 194) }),
            new FrameData(64, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 96, 180, 145, 194), new GenericBox(1, 118, 87, 140, 104), new GenericBox(1, 105, 101, 139, 193), new GenericBox(1, 137, 104, 164, 118) }),
            new FrameData(65, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 118, 87, 139, 103), new GenericBox(1, 105, 102, 141, 195), new GenericBox(1, 135, 103, 162, 118), new GenericBox(2, 96, 180, 150, 195) }),
            new FrameData(66, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 89, 139, 104), new GenericBox(1, 104, 104, 145, 194), new GenericBox(1, 138, 106, 164, 119), new GenericBox(2, 96, 179, 152, 195) }),
            new FrameData(67, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 94, 180, 154, 195), new GenericBox(1, 118, 89, 138, 105), new GenericBox(1, 95, 106, 112, 119), new GenericBox(1, 106, 103, 138, 145), new GenericBox(1, 98, 146, 147, 195), new GenericBox(1, 138, 106, 163, 119) }),
            new FrameData(68, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 91, 137, 107), new GenericBox(1, 95, 107, 161, 124), new GenericBox(1, 106, 123, 135, 163), new GenericBox(1, 95, 163, 152, 194), new GenericBox(2, 93, 178, 155, 195) }),
            new FrameData(69, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 88, 178, 151, 194), new GenericBox(1, 119, 91, 138, 107), new GenericBox(1, 95, 107, 164, 123), new GenericBox(1, 105, 122, 135, 153), new GenericBox(1, 93, 154, 147, 194) }),
            new FrameData(70, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 119, 93, 138, 109), new GenericBox(1, 95, 108, 163, 123), new GenericBox(1, 106, 123, 134, 163), new GenericBox(1, 88, 163, 144, 194), new GenericBox(2, 86, 176, 146, 194) }),
            new FrameData(71, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 82, 177, 143, 195), new GenericBox(1, 118, 92, 140, 109), new GenericBox(1, 96, 106, 163, 125), new GenericBox(1, 103, 123, 132, 164), new GenericBox(1, 85, 163, 141, 194) }),
            new FrameData(72, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 118, 94, 138, 110), new GenericBox(1, 95, 109, 163, 124), new GenericBox(1, 105, 123, 133, 156), new GenericBox(1, 83, 156, 135, 194), new GenericBox(2, 81, 179, 140, 195) }),
            new FrameData(73, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 79, 177, 144, 195), new GenericBox(1, 117, 94, 138, 109), new GenericBox(1, 95, 108, 162, 123), new GenericBox(1, 104, 123, 133, 157), new GenericBox(1, 81, 157, 135, 194) }),
            new FrameData(74, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 119, 89, 138, 104), new GenericBox(1, 96, 102, 162, 120), new GenericBox(1, 104, 121, 136, 195), new GenericBox(2, 96, 178, 151, 195) }),
            new FrameData(75, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 99, 176, 144, 194), new GenericBox(1, 97, 101, 163, 118), new GenericBox(1, 117, 88, 139, 105), new GenericBox(1, 102, 119, 137, 195) }),
            new FrameData(76, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 87, 139, 102), new GenericBox(1, 102, 100, 138, 195), new GenericBox(1, 136, 101, 160, 115), new GenericBox(2, 93, 179, 147, 194) }),
            new FrameData(77, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 180, 147, 195), new GenericBox(1, 117, 89, 138, 102), new GenericBox(1, 98, 100, 142, 195), new GenericBox(1, 136, 103, 162, 116) }),
            new FrameData(78, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 119, 89, 138, 102), new GenericBox(1, 100, 102, 138, 141), new GenericBox(1, 136, 105, 159, 119), new GenericBox(1, 99, 139, 142, 194), new GenericBox(2, 98, 183, 145, 194) }),
            new FrameData(79, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 180, 149, 194), new GenericBox(1, 116, 88, 139, 103), new GenericBox(1, 101, 101, 139, 142), new GenericBox(1, 138, 105, 161, 120), new GenericBox(1, 100, 139, 145, 194) }),
            new FrameData(80, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 118, 90, 138, 105), new GenericBox(1, 98, 104, 139, 142), new GenericBox(1, 138, 106, 162, 120), new GenericBox(1, 98, 142, 151, 195), new GenericBox(2, 95, 181, 155, 194) }),
            new FrameData(81, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 90, 181, 163, 195), new GenericBox(1, 118, 90, 140, 104), new GenericBox(1, 99, 103, 158, 120), new GenericBox(1, 101, 120, 137, 143), new GenericBox(1, 93, 143, 155, 195) }),
            new FrameData(82, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 116, 92, 139, 105), new GenericBox(1, 107, 108, 138, 149), new GenericBox(1, 97, 110, 158, 125), new GenericBox(1, 91, 144, 158, 194), new GenericBox(2, 88, 174, 166, 194) }),
            new FrameData(83, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 84, 180, 163, 194), new GenericBox(1, 118, 92, 138, 107), new GenericBox(1, 106, 106, 138, 145), new GenericBox(1, 96, 109, 157, 126), new GenericBox(1, 89, 145, 155, 194) }),
            new FrameData(84, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 84, 149, 151, 193), new GenericBox(1, 103, 127, 135, 151), new GenericBox(1, 106, 107, 138, 127), new GenericBox(1, 120, 92, 137, 108), new GenericBox(1, 136, 111, 157, 125), new GenericBox(2, 77, 178, 159, 195) }),
            new FrameData(85, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 118, 93, 138, 107), new GenericBox(1, 107, 107, 139, 148), new GenericBox(1, 96, 109, 157, 127), new GenericBox(1, 82, 146, 151, 194), new GenericBox(2, 74, 182, 155, 195) }),
            new FrameData(86, this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 68, 183, 150, 195), new GenericBox(1, 120, 93, 139, 110), new GenericBox(1, 106, 107, 139, 148), new GenericBox(1, 98, 111, 158, 128), new GenericBox(1, 74, 149, 151, 195) }),
            new FrameData(87, this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 117, 93, 138, 109), new GenericBox(1, 107, 107, 138, 147), new GenericBox(1, 97, 110, 156, 127), new GenericBox(1, 73, 148, 151, 193), new GenericBox(2, 65, 182, 152, 195) }),
        };

        var walkingBackwardFrames = new List<FrameData> {
            new FrameData(88, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 180, 152, 194), new GenericBox(1, 115, 87, 132, 102), new GenericBox(1, 100, 102, 144, 193), new GenericBox(1, 139, 109, 151, 133) }),
            new FrameData(89, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 112, 86, 131, 103), new GenericBox(1, 100, 103, 143, 194), new GenericBox(1, 138, 109, 151, 132), new GenericBox(2, 97, 179, 149, 194) }),
            new FrameData(90, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 98, 179, 143, 194), new GenericBox(1, 113, 86, 133, 102), new GenericBox(1, 102, 101, 139, 195), new GenericBox(1, 137, 105, 153, 132) }),
            new FrameData(91, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 113, 87, 134, 101), new GenericBox(1, 103, 100, 139, 194), new GenericBox(1, 137, 105, 152, 132), new GenericBox(2, 100, 180, 146, 194) }),
            new FrameData(92, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 97, 179, 143, 195), new GenericBox(1, 113, 88, 132, 103), new GenericBox(1, 103, 101, 137, 195), new GenericBox(1, 137, 106, 154, 134) }),
            new FrameData(93, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 113, 87, 133, 102), new GenericBox(1, 105, 101, 138, 194), new GenericBox(1, 138, 108, 154, 133), new GenericBox(2, 100, 181, 145, 194) }),
            new FrameData(94, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 179, 146, 194), new GenericBox(1, 114, 88, 133, 103), new GenericBox(1, 102, 103, 139, 147), new GenericBox(1, 99, 148, 136, 194), new GenericBox(1, 138, 107, 155, 134) }),
            new FrameData(95, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 116, 89, 133, 104), new GenericBox(1, 104, 103, 137, 148), new GenericBox(1, 136, 107, 154, 133), new GenericBox(1, 96, 150, 142, 194), new GenericBox(2, 92, 181, 145, 194) }),
            new FrameData(96, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 89, 180, 149, 197), new GenericBox(1, 115, 92, 134, 108), new GenericBox(1, 105, 107, 138, 151), new GenericBox(1, 134, 112, 156, 140), new GenericBox(1, 93, 152, 145, 194) }),
            new FrameData(97, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 114, 91, 132, 107), new GenericBox(1, 104, 106, 138, 160), new GenericBox(1, 135, 114, 155, 139), new GenericBox(1, 94, 160, 147, 195), new GenericBox(2, 92, 178, 153, 195) }),
            new FrameData(98, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 176, 157, 194), new GenericBox(1, 113, 93, 134, 108), new GenericBox(1, 106, 106, 139, 162), new GenericBox(1, 137, 111, 157, 138), new GenericBox(1, 98, 162, 153, 194) }),
            new FrameData(99, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 113, 91, 133, 109), new GenericBox(1, 104, 108, 139, 156), new GenericBox(1, 138, 114, 156, 138), new GenericBox(1, 99, 155, 155, 194), new GenericBox(2, 99, 180, 161, 195) }),
            new FrameData(100, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 103, 179, 161, 194), new GenericBox(1, 114, 92, 134, 109), new GenericBox(1, 105, 109, 140, 163), new GenericBox(1, 139, 115, 156, 139), new GenericBox(1, 103, 162, 154, 193) }),
            new FrameData(101, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 109, 164, 158, 194), new GenericBox(1, 109, 108, 142, 165), new GenericBox(1, 97, 110, 110, 125), new GenericBox(1, 132, 115, 158, 140), new GenericBox(1, 114, 93, 134, 109), new GenericBox(2, 105, 177, 165, 194) }),
            new FrameData(102, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 100, 177, 148, 195), new GenericBox(1, 107, 101, 141, 195), new GenericBox(1, 96, 105, 155, 132), new GenericBox(1, 114, 87, 133, 102) }),
            new FrameData(103, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 115, 88, 132, 102), new GenericBox(2, 100, 181, 143, 194), new GenericBox(1, 105, 102, 140, 195), new GenericBox(1, 97, 106, 155, 134) }),
            new FrameData(104, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 105, 100, 141, 160), new GenericBox(1, 105, 159, 137, 194), new GenericBox(1, 139, 105, 154, 131), new GenericBox(1, 96, 101, 108, 111), new GenericBox(1, 114, 86, 133, 101) }),
            new FrameData(105, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 115, 87, 133, 101), new GenericBox(1, 106, 100, 137, 140), new GenericBox(1, 95, 101, 152, 131), new GenericBox(1, 104, 131, 141, 194), new GenericBox(2, 98, 181, 149, 195) }),
            new FrameData(106, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 95, 179, 146, 195), new GenericBox(1, 114, 87, 134, 102), new GenericBox(1, 105, 101, 150, 134), new GenericBox(1, 100, 132, 143, 195) }),
            new FrameData(107, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 106, 101, 138, 141), new GenericBox(1, 114, 88, 132, 101), new GenericBox(1, 134, 106, 151, 133), new GenericBox(1, 96, 139, 142, 194), new GenericBox(2, 92, 179, 145, 196) }),
            new FrameData(108, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 88, 177, 147, 195), new GenericBox(1, 90, 147, 145, 194), new GenericBox(1, 105, 102, 138, 147), new GenericBox(1, 135, 109, 151, 134), new GenericBox(1, 113, 88, 135, 104) }),
            new FrameData(109, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 114, 89, 133, 104), new GenericBox(1, 105, 102, 137, 143), new GenericBox(1, 135, 108, 150, 134), new GenericBox(1, 88, 143, 146, 194), new GenericBox(2, 81, 178, 151, 195) }),
            new FrameData(110, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 115, 91, 131, 106), new GenericBox(1, 103, 108, 139, 145), new GenericBox(1, 136, 116, 148, 140), new GenericBox(1, 81, 145, 151, 194), new GenericBox(2, 77, 177, 155, 195) }),
            new FrameData(111, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 81, 176, 160, 194), new GenericBox(1, 114, 91, 133, 106), new GenericBox(1, 101, 106, 147, 147), new GenericBox(1, 85, 145, 156, 194) }),
            new FrameData(112, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 113, 92, 133, 109), new GenericBox(1, 103, 107, 141, 148), new GenericBox(1, 138, 114, 150, 139), new GenericBox(1, 87, 145, 159, 194), new GenericBox(2, 83, 175, 164, 194) }),
            new FrameData(113, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 88, 174, 170, 195), new GenericBox(1, 114, 93, 133, 107), new GenericBox(1, 105, 107, 137, 148), new GenericBox(1, 135, 114, 150, 139), new GenericBox(1, 93, 141, 159, 193) }),            
            new FrameData(114, -this.move_speed, 0, new List<GenericBox> { new GenericBox(2, 93, 175, 175, 195), new GenericBox(1, 114, 94, 131, 109), new GenericBox(1, 103, 110, 138, 149), new GenericBox(1, 136, 116, 150, 141), new GenericBox(1, 92, 149, 162, 194) }),
            new FrameData(115, -this.move_speed, 0, new List<GenericBox> { new GenericBox(1, 112, 93, 131, 109), new GenericBox(1, 105, 108, 138, 149), new GenericBox(1, 136, 115, 150, 142), new GenericBox(1, 94, 144, 164, 193), new GenericBox(2, 98, 176, 177, 195) }),        
        };

        var dashForwardFrames = new List<FrameData> {
            new FrameData(116, 0, 0, new List<GenericBox> { new GenericBox(1, 77, 118, 120, 135), new GenericBox(1, 120, 125, 152, 157), new GenericBox(1, 151, 135, 187, 164), new GenericBox(1, 96, 155, 122, 191) }),
            new FrameData(117, this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 82, 131, 133, 149), new GenericBox(1, 112, 100, 138, 131), new GenericBox(1, 132, 130, 161, 174), new GenericBox(1, 160, 159, 178, 173) }),
            new FrameData(118, this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 132, 99, 161, 163), new GenericBox(1, 106, 154, 145, 178) }),
            new FrameData(119, this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 99, 150, 146, 189), new GenericBox(1, 146, 149, 168, 177), new GenericBox(1, 152, 124, 168, 150) }),
            new FrameData(120, this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 140, 168, 174, 184), new GenericBox(1, 84, 134, 103, 149), new GenericBox(1, 76, 147, 140, 195), new GenericBox(1, 58, 167, 76, 188) }),
            new FrameData(121, 0, 0, new List<GenericBox> { new GenericBox(1, 81, 157, 142, 194), new GenericBox(1, 58, 148, 82, 168), new GenericBox(1, 80, 137, 142, 158), new GenericBox(1, 96, 122, 116, 137) }),        
        };

        var dashBackwardFrames = new List<FrameData> {
            new FrameData(122, 0, 0, new List<GenericBox> { new GenericBox(2, 149, 190, 75, 177), new GenericBox(1, 91, 95, 133, 160), new GenericBox(1, 67, 129, 90, 159), new GenericBox(1, 132, 152, 153, 189) }),
            new FrameData(123, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 124, 126, 174, 144), new GenericBox(1, 87, 112, 126, 158), new GenericBox(1, 74, 90, 109, 122) }),
            new FrameData(124, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 68, 91, 90, 131), new GenericBox(1, 90, 112, 125, 154), new GenericBox(1, 120, 138, 138, 157), new GenericBox(1, 123, 100, 149, 126), new GenericBox(1, 146, 91, 162, 111) }),
            new FrameData(125, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 65, 110, 97, 139), new GenericBox(1, 94, 114, 126, 157), new GenericBox(1, 125, 142, 141, 158), new GenericBox(1, 107, 87, 137, 115) }),
            new FrameData(126, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 89, 124, 132, 154), new GenericBox(1, 126, 140, 144, 157), new GenericBox(1, 88, 81, 114, 129), new GenericBox(1, 66, 139, 89, 157), new GenericBox(1, 101, 153, 116, 172) }),
            new FrameData(127, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 52, 100, 75, 120), new GenericBox(1, 72, 111, 106, 140), new GenericBox(1, 72, 150, 101, 180), new GenericBox(1, 85, 128, 133, 162), new GenericBox(1, 133, 141, 148, 157) }),
            new FrameData(128, -this.dash_speed, 0, new List<GenericBox> { new GenericBox(1, 106, 104, 153, 137), new GenericBox(1, 86, 117, 121, 152), new GenericBox(1, 106, 151, 127, 190), new GenericBox(1, 57, 137, 85, 170) }),
            new FrameData(129, 0, 0, new List<GenericBox> { new GenericBox(1, 120, 95, 138, 111), new GenericBox(1, 104, 110, 147, 127), new GenericBox(1, 94, 124, 127, 148), new GenericBox(1, 79, 148, 153, 194) }),
            new FrameData(130, 0, 0, new List<GenericBox> { new GenericBox(1, 96, 118, 123, 146), new GenericBox(1, 122, 118, 142, 134), new GenericBox(1, 104, 104, 122, 118), new GenericBox(1, 86, 146, 155, 193), new GenericBox(2, 77, 177, 163, 195) }),
            new FrameData(131, 0, 0, new List<GenericBox> { new GenericBox(2, 79, 178, 163, 195), new GenericBox(1, 104, 109, 120, 123), new GenericBox(1, 93, 123, 123, 163), new GenericBox(1, 121, 126, 138, 142), new GenericBox(1, 86, 149, 151, 194) }),
        };

        var crouchingFrames = new List<FrameData> {
            new FrameData(57, 0, 0, new List<GenericBox> { new GenericBox(2, 86, 179, 164, 193), new GenericBox(1, 91, 155, 157, 193), new GenericBox(1, 112, 130, 151, 156) }),
            new FrameData(58, 0, 0, new List<GenericBox> { new GenericBox(1, 113, 128, 150, 156), new GenericBox(1, 89, 156, 156, 193), new GenericBox(2, 88, 179, 164, 195) }),
            new FrameData(59, 0, 0, new List<GenericBox> { new GenericBox(1, 112, 128, 150, 163), new GenericBox(1, 89, 158, 157, 192), new GenericBox(2, 88, 179, 165, 194) }),
        };
        
        // States
        var animations = new Dictionary<string, Animation> {
            // Normals
            { "Idle", new Animation(idleFrames, "Idle", 20)},
            { "AAttack", new Animation(AFrames, "Idle", 30)},
            { "BAttack", new Animation(BFrames, "Idle", 20)},
            { "CAttack", new Animation(CFrames, "Idle", 20)},
            { "DAttack", new Animation(DFrames, "Idle", 20)},
            { "FrontDAttack", new Animation(FrontDFrames, "Idle", 15)},
            { "BackCAttack", new Animation(BackCFrames, "Idle", 20)},
            // Movment
            { "WalkingForward", new Animation(walkingForwardFrames, "Idle", 30)},
            { "WalkingBackward", new Animation(walkingBackwardFrames, "Idle", 20)},
            { "DashForward", new Animation(dashForwardFrames, "Idle", 20)},
            { "DashBackward", new Animation(dashBackwardFrames, "Idle", 20)},
            { "Crouching", new Animation(crouchingFrames, "Idle", 4)},
            // Bonus
            { "Intro", new Animation(introFrames, "Idle", 10)},
        };

        this.animations = animations;
        this.LoadSpriteImages();
        this.LoadSounds();
    }

    public override void DoBehavior() {
        if ((this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward") & !InputManager.Instance.Key_hold(8) & !InputManager.Instance.Key_hold(9)) {
            this.ChangeState("Idle");
        }

        // Specials
        int[] A_dp = {9, 7, 9, 0};
        int[] C_dp = {9, 7, 9, 2};
        int[] A_qcf = {7, 9, 0};
        int[] C_qcf = {7, 9, 2};
        int[] B_qcb = {7, 8, 1};
        int[] D_qcb = {7, 8, 3};


        // Cancels


        // Normals
        if (InputManager.Instance.Key_down(3) && InputManager.Instance.Key_hold(9) && this.canNormalAtack) {
            this.ChangeState("FrontDAttack");
        } else if (InputManager.Instance.Key_down(2) && InputManager.Instance.Key_hold(8) && this.canNormalAtack) {
            this.ChangeState("BackCAttack");
        }

        if (InputManager.Instance.Key_down(0) && this.canNormalAtack) {
            this.ChangeState("AAttack");
        } else if (InputManager.Instance.Key_down(1) && this.canNormalAtack) {
            this.ChangeState("BAttack");
        } else if (InputManager.Instance.Key_down(2) && this.canNormalAtack) {
            this.ChangeState("CAttack");
        } else if (InputManager.Instance.Key_down(3) && this.canNormalAtack ) {
            this.ChangeState("DAttack");
        }

        // Crouching
        if (InputManager.Instance.Key_hold(7) && !InputManager.Instance.Key_hold(6) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("Crouching");
        } else if (!InputManager.Instance.Key_hold(7) && this.CurrentState == "Crouching") {
            this.ChangeState("Idle");
        }

        // Dashing
        int[] dash_forward_string = {9, 9}; 
        int[] dash_backward_string = {8, 8};
        if (InputManager.Instance.Was_down(dash_forward_string, 13) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("DashForward");
        } 
        else if (InputManager.Instance.Was_down(dash_backward_string, 13) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("DashBackward");
        }

        // Walking
        if (InputManager.Instance.Key_hold(8) && !InputManager.Instance.Key_hold(9) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingForward" || this.CurrentState == "WalkingBackward")) {
            this.ChangeState("WalkingBackward");
        } else if (InputManager.Instance.Key_hold(9) && !InputManager.Instance.Key_hold(8) && (this.CurrentState == "Idle" || this.CurrentState == "WalkingBackward" || this.CurrentState == "WalkingForward")) {
            this.ChangeState("WalkingForward");
        }

        // On Air

    }
}