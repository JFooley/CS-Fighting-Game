using SFML.Graphics;

namespace Data_space {
    public static class DataManagement {
        public static void SaveTextures(string fileName, Dictionary<string, Texture> textures) 
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(textures.Count);

                foreach (var kvp in textures)
                {
                    writer.Write(kvp.Key);
                    Image image = kvp.Value.CopyToImage();
                    
                    // Escreve dimensões
                    writer.Write(image.Size.X);
                    writer.Write(image.Size.Y);
                    
                    // Obtém pixels como byte array (sempre RGBA no SFML)
                    byte[] pixels = image.Pixels;
                    writer.Write(pixels.Length);
                    writer.Write(pixels);
                }
            }
        }

        public static Dictionary<string, Texture> LoadTextures(string fileName) 
        {
            var textures = new Dictionary<string, Texture>();

            using (var stream = new FileStream(fileName, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    uint width = reader.ReadUInt32();
                    uint height = reader.ReadUInt32();
                    int pixelDataLength = reader.ReadInt32();
                    byte[] pixelData = reader.ReadBytes(pixelDataLength);

                    // Cria imagem - SFML sempre usa RGBA para Image
                    Image image = new Image(width, height);
                    
                    // Copia os dados pixel a pixel
                    for (uint y = 0; y < height; y++)
                    {
                        for (uint x = 0; x < width; x++)
                        {
                            int index = (int)((y * width + x) * 4);
                            byte r = pixelData[index];
                            byte g = pixelData[index + 1];
                            byte b = pixelData[index + 2];
                            byte a = pixelData[index + 3];
                            image.SetPixel(x, y, new Color(r, g, b, a));
                        }
                    }

                    textures[key] = new Texture(image);
                }
            }

            return textures;
        }
    }
}