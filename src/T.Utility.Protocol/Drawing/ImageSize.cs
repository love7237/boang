using System.Runtime.Serialization;

namespace T.Utility.Protocol
{
    /// <summary>
    /// 图像的尺寸信息
    /// </summary>
    [DataContract]
    public class ImageSize
    {
        /// <summary>
        /// Gets or sets the horizontal component of this Size structure.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the vertical component of this Size structure.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets a value indicating whether this Size is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSize"/> class.
        /// </summary>
        public ImageSize() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSize"/> struct with the specified parameters.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ImageSize other)
        {
            return other != null && this.Width == other.Width && this.Height == other.Height;
        }
    }
}
