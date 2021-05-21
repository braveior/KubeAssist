// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Preconditions must be fulfilled before an operation (update, delete,
    /// etc.) is carried out.
    /// </summary>
    public partial class V1Preconditions
    {
        /// <summary>
        /// Initializes a new instance of the V1Preconditions class.
        /// </summary>
        public V1Preconditions()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1Preconditions class.
        /// </summary>
        /// <param name="resourceVersion">Specifies the target
        /// ResourceVersion</param>
        /// <param name="uid">Specifies the target UID.</param>
        public V1Preconditions(string resourceVersion = default(string), string uid = default(string))
        {
            ResourceVersion = resourceVersion;
            Uid = uid;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets specifies the target ResourceVersion
        /// </summary>
        [JsonProperty(PropertyName = "resourceVersion")]
        public string ResourceVersion { get; set; }

        /// <summary>
        /// Gets or sets specifies the target UID.
        /// </summary>
        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; set; }

    }
}
