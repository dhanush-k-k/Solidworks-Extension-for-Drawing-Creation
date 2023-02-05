using System.Drawing;
using SolidWorks.Interop.swconst;

namespace SwApp
{
    #region helper classes/structs
    public class PictureHelper : System.Windows.Forms.AxHost
    {

        public PictureHelper()
            : base("56174C86-1546-4778-8EE6-B6AC606875E7")
        {

        }

        public static Image Convert(object objIDispImage)
        {
            Image objPicture = GetPictureFromIPicture(objIDispImage);
            return objPicture;
        }

    }
    #region excel help methods
    public static class ExcelHelper
    {
        public static double PixelWidthToExcel(int pixels)
        {
            var tempWidth = pixels * 0.14099;
            var correction = (tempWidth / 100) * -1.30;

            return tempWidth - correction;
        }

        public static double PixelHeightToExcel(int pixels)
        {
            return pixels * 0.75;
        }
    }
    #endregion
    struct TableBoundryCondition
    {
        public swTableHeaderPosition_e HeaderPosition { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public int RowHeaderIndex { get; set; }
    }
    #endregion
}