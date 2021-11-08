using iText.Layout.Element;
using iText.IO.Image;
using System.IO;

namespace Tramos.Report.Pdf.Core
{
    public class ImageFactory
    {
        public static Image Create(Stream imageStream)
        {
            var byteAssembly = new byte[imageStream.Length];
            imageStream.Read(byteAssembly, 0, byteAssembly.Length);
            return new Image(ImageDataFactory.Create(byteAssembly));
        }

        public static Image CreateImageScaled(Stream imageStream, float horizontalScaling, float verticalScaling)
        {
            var image = Create(imageStream);
            image.Scale(horizontalScaling, verticalScaling);
            return image;
        }
    }
}
