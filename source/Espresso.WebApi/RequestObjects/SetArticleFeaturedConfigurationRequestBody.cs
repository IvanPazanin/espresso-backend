namespace Espresso.WebApi.RequestObjects
{
    /// <summary>
    /// 
    /// </summary>
    public record SetArticleFeaturedConfigurationRequestBody
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool? IsFeatured { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int? FeaturedPosition { get; init; }
    }
}