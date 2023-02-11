using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
            using Image<Rgb24> sourceImage = Image.Load<Rgb24>(sourceImageFile);

            // Create a target image to hold the mosaic
            using Image<Rgb24> targetImage = new Image<Rgb24>(sourceImage.Width, sourceImage.Height);

            // Get a list of images from the directory
            var imageFiles = System.IO.Directory.GetFiles(imageDirectory).Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"));
            var images = imageFiles.Select(f => Image.Load<Rgb24>(f)).ToList();

            // Loop through each tile in the target image
            for (int y = 0; y < sourceImage.Height; y += tileSize)
            {
                for (int x = 0; x < sourceImage.Width; x += tileSize)
                {
                    // Get the average color of the current tile
                    Rgb24 averageColor = GetAverageColor(sourceImage, x, y, tileSize, tileSize);

                    // Find the closest match in the list of images
                    Image<Rgb24> closestMatchImage = FindClosestMatch(averageColor, images);

                    // Draw the closest match image onto the target image
                    targetImage.Mutate(c => c.DrawImage(closestMatchImage, new SixLabors.ImageSharp.Rectangle(x, y, tileSize, tileSize),
                        new SixLabors.ImageSharp.Rectangle(0, 0, closestMatchImage.Width, closestMatchImage.Height),
                        GraphicsOptions.Default));
                }
            }

            // Save the target image
            targetImage.Save("mosaic.jpg");
        }

        static Rgb24 GetAverageColor(Image<Rgb24> image, int x, int y, int width, int height)
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
                    Rgb24 pixelColor = image[j, i];
                    red += pixelColor.R;
                    green += pixelColor.G;
                    blue += pixelColor.B;
                    count++;
                }
            }
            red /= count;
            green /= count;
            blue /= count;
            return new Rgb24((byte)red, (byte)green, (byte)blue);
        }

        static Image<Rgb24> FindClosestMatch(Rgb24 color, List<Image<Rgb24>> images)
        {
            // Find the image with the closest color match
            Image<Rgb24> closestMatchImage = null;
            int closestDistance = int.MaxValue;
            foreach (var image in images)
            {
                Rgb24 averageColor = GetAverageColor(image, 0, 0, image.Width, image.Height);
                int distance = GetColorDistance(color, averageColor);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMatchImage = image;
                }
            }
            return closestMatchImage;
        }

        static int GetColorDistance(Rgb24 color1, Rgb24 color2)
        {
            // Calculate the distance between two colors
            int redDiff = color1.R - color2.R;
            int greenDiff = color1.G - color2.G;
            int blueDiff = color1.B - color2.B;
            return redDiff * redDiff + greenDiff * greenDiff + blueDiff * blueDiff;
        }
    }
}
