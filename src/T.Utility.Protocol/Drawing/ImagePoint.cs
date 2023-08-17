using System.Runtime.Serialization;

namespace T.Utility.Protocol
{
    /// <summary>
    /// 图像上的点信息
    /// </summary>
    [DataContract]
    public class ImagePoint
    {
        /// <summary>
        /// Gets or sets the x-coordinate of this Point.
        /// </summary>
        [DataMember(Name = "x")]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of this Point.
        /// </summary>
        [DataMember(Name = "y")]
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePoint"/> class.
        /// </summary>
        public ImagePoint() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePoint"/> struct with the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public ImagePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets a value indicating whether this Point is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }

        /// <summary>
        /// Specifies whether this point instance contains the same coordinates as another point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ImagePoint other)
        {
            return other != null && this.X == other.X && this.Y == other.Y;
        }
    }
}
