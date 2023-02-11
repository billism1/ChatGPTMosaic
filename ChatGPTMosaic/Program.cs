using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PhotoMosaic
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceImageFile = "source.jpg";
            string imageDirectory = "images";
            int tileSize = 25;

            // Load the source image
            Image sourceImage = Image.FromFile(sourceImageFile);

            // Create a target bitmap to hold the mosaic
            Bitmap targetImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            // Loop through each tile in the target image
            for (int y = 0; y < sourceImage.Height; y += tileSize)
            {
                for (int x = 0; x < sourceImage.Width; x += tileSize)
                {
                    // Get the average color of the current tile
                    Color averageColor = GetAverageColor(sourceImage, x, y, tileSize, tileSize);

                    // Find the closest match in the directory of images
                    string closestMatchFile = FindClosestMatch(averageColor, imageDirectory);

                    // Load the closest match image
                    Image closestMatchImage = Image.FromFile(closestMatchFile);

                    // Draw the closest match image onto the target image
                    using (Graphics g = Graphics.FromImage(targetImage))
                    {
                        g.DrawImage(closestMatchImage, new Rectangle(x, y, tileSize, tileSize),
                            new Rectangle(0, 0, closestMatchImage.Width, closestMatchImage.Height),
                            GraphicsUnit.Pixel);
                    }
                }
            }

            // Save the target image
            targetImage.Save("mosaic.jpg", ImageFormat.Jpeg);
        }

        static Color GetAverageColor(Image image, int x, int y, int width, int height)
        {
            // Calculate the average color of the specified region of the image
            int red = 0;
            int green = 0;
            int blue = 0;
            int count = 0;
            for (int i = y; i < y + height; i++)
            {
                for (int j = x; j < x + width; j++)
                {
                    Color pixelColor = ((Bitmap)image).GetPixel(j, i);
                    red += pixelColor.R;
                    green += pixelColor.G;
                    blue += pixelColor.B;
                    count++;
                }
            }
            red /= count;
            green /= count;
            blue /= count;
            return Color.FromArgb(red, green, blue);
        }

        static string FindClosestMatch(Color color, string imageDirectory)
        {
            // Find the file in the directory of images with the closest color match
            int closestDistance = int.MaxValue;
            string closestMatchFile = null;
            foreach (string file in Directory.GetFiles(imageDirectory))
            {
                Image image = Image.FromFile(file);
                Color averageColor = GetAverageColor(image, 0, 0, image.Width, image.Height);
                int distance = GetColorDistance(color, averageColor);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMatchFile = file;
                }
            }
            return closestMatchFile;
        }

        static int GetColorDistance(Color color1, Color color2)
        {
            // Calculate the distance between two colors
            int redDiff = color1.R - color2.R;
            int greenDiff = color1.G - color2.G;
            int blueDiff = color1.B - color2.B;
            return redDiff * redDiff + greenDiff * greenDiff + blueDiff * blueDiff;
        }
    }
}