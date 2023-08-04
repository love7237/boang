namespace T.Utility.Protocol
{
    /// <summary>
    /// 图像的矩形信息
    /// </summary>
    public class ImageRect
    {
        /// <summary>
        /// Gets the x-coordinate of the left edge of this Rect structure.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets the y-coordinate of the top edge of this Rect structure.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets the x-coordinate that is the sum of Rect.X property of this Rect structure.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Gets the y-coordinate that is the sum of the Rect.Y property of this Rect structure.
        /// </summary>
        public int Bottom { get; set; }

        /// <summary>
        /// Gets or sets the size of this Rect.
        /// </summary>
        public ImageSize Size
        {
            get
            {
                return new ImageSize(Right - Left, Bottom - Top);
            }
        }

        /// <summary>
        /// Tests whether all numeric properties of this Rect have values of zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Left == 0 && Top == 0 && Right == 0 && Bottom == 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRect"/> class.
        /// </summary>
        public ImageRect() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRect"/> struct with the specified coordinates.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public ImageRect(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }
}
