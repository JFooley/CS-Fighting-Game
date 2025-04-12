using SFML.Audio;
using SFML.Graphics;

namespace Data_space {
    public static class DataManagement {
        public static void SaveTextures(string fileName, Dictionary<string, Texture> textures) {
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
        public static Dictionary<string, Texture> LoadTextures(string fileName) {
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

        public static void SaveSounds(string fileName, Dictionary<string, SoundBuffer> sounds) {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(sounds.Count); // Escreve quantos sons serão salvos
                
                foreach (var pair in sounds)
                {
                    // Salva o nome do som
                    writer.Write(pair.Key);
                    
                    // Obtém os dados brutos do SoundBuffer
                    SoundBuffer buffer = pair.Value;
                    short[] samples = buffer.Samples;
                    byte[] sampleBytes = new byte[samples.Length * 2];
                    Buffer.BlockCopy(samples, 0, sampleBytes, 0, sampleBytes.Length);
                    
                    // Escreve os metadados e dados do áudio
                    writer.Write(buffer.SampleRate);
                    writer.Write((byte)buffer.ChannelCount);
                    writer.Write(sampleBytes.Length);
                    writer.Write(sampleBytes);
                }
            }
        }
        public static Dictionary<string, SoundBuffer> LoadSounds(string fileName) {
            Dictionary<string, SoundBuffer> loadedSounds = new Dictionary<string, SoundBuffer>();
            
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int soundCount = reader.ReadInt32();
                
                for (int i = 0; i < soundCount; i++)
                {
                    string soundName = reader.ReadString();
                    uint sampleRate = reader.ReadUInt32();
                    byte channels = reader.ReadByte();
                    int dataLength = reader.ReadInt32();
                    byte[] audioData = reader.ReadBytes(dataLength);
                    
                    // Reconstrói o SoundBuffer
                    short[] samples = new short[dataLength / 2];
                    Buffer.BlockCopy(audioData, 0, samples, 0, dataLength);
                    SoundBuffer buffer = new SoundBuffer(samples, channels, sampleRate);
                    
                    // Cria o Sound e adiciona ao dicionário
                    loadedSounds[soundName] = buffer;
                }
            }
            
            return loadedSounds;
        }
    }
}