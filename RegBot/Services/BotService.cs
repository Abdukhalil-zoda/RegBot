/*using Aspose.PSD;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using Aspose.PSD.ImageOptions;
*/
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.Shapes;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace RegBot.Services
{
    public static class BotService
    {/*
        public static Tuple<byte[], string> GetTuple()
        {
            string exportPath = "cap.psd";
            Random random = new Random();
            int leftPos = (int)random.NextInt64(0, 100);
            int topPos = (int)random.NextInt64(0, 50);
            
            using (var im = (PsdImage)Aspose.PSD.Image.Load(exportPath))
            {
                
                var text = String.Empty;
                string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
                for (int i = 0; i < 5; ++i)
                    text += ALF[random.Next(ALF.Length)];
                im.AddTextLayer("Some text", new Aspose.PSD.Rectangle(50, 50, 100, 100));
                

                TextLayer textLayer = (TextLayer)im.Layers[4];
                *//*

                textLayer.UpdateText(text);
                if (textLayer.Left != leftPos || textLayer.Top != topPos)
                {
                    throw new Exception("Was created incorrect Text Layer");
                }*//*
                PngOptions pngOptions = new PngOptions();
                im.Save("cap.PNG", pngOptions);

                return new Tuple<byte[], string>(File.ReadAllBytes("cap.PNG"), text);
            }
        }
*/
        public static Tuple<byte[], string> GetTuple1()
        {
            Image image = Image.Load("cap.png"); // create any way you like.
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Install("font.ttf");
            Font font = family.CreateFont(24, FontStyle.Italic);
            Random random = new Random();
            int Xpos = random.Next(0, 200 - 60);
            int Ypos = random.Next(15, 50 - 20);
            var text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[random.Next(ALF.Length)];

            image.Mutate(x => x.DrawText(text, font, SixLabors.ImageSharp.Color.Black, new SixLabors.ImageSharp.PointF(Xpos, Ypos)));
           
           
            byte[] bytes;
            using (var outputStream = new MemoryStream())
            {
                image.Save(outputStream, new JpegEncoder());
                bytes = outputStream.ToArray();
            }

            return new Tuple<byte[],string>(bytes, text);
        }
    }
}
