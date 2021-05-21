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
    /// Projection that may be projected along with other supported volume
    /// types
    /// </summary>
    public partial class V1VolumeProjection
    {
        /// <summary>
        /// Initializes a new instance of the V1VolumeProjection class.
        /// </summary>
        public V1VolumeProjection()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1VolumeProjection class.
        /// </summary>
        /// <param name="configMap">information about the configMap data to
        /// project</param>
        /// <param name="downwardAPI">information about the downwardAPI data to
        /// project</param>
        /// <param name="secret">information about the secret data to
        /// project</param>
        /// <param name="serviceAccountToken">information about the
        /// serviceAccountToken data to project</param>
        public V1VolumeProjection(V1ConfigMapProjection configMap = default(V1ConfigMapProjection), V1DownwardAPIProjection downwardAPI = default(V1DownwardAPIProjection), V1SecretProjection secret = default(V1SecretProjection), V1ServiceAccountTokenProjection serviceAccountToken = default(V1ServiceAccountTokenProjection))
        {
            ConfigMap = configMap;
            DownwardAPI = downwardAPI;
            Secret = secret;
            ServiceAccountToken = serviceAccountToken;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets information about the configMap data to project
        /// </summary>
        [JsonProperty(PropertyName = "configMap")]
        public V1ConfigMapProjection ConfigMap { get; set; }

        /// <summary>
        /// Gets or sets information about the downwardAPI data to project
        /// </summary>
        [JsonProperty(PropertyName = "downwardAPI")]
        public V1DownwardAPIProjection DownwardAPI { get; set; }

        /// <summary>
        /// Gets or sets information about the secret data to project
        /// </summary>
        [JsonProperty(PropertyName = "secret")]
        public V1SecretProjection Secret { get; set; }

        /// <summary>
        /// Gets or sets information about the serviceAccountToken data to
        /// project
        /// </summary>
        [JsonProperty(PropertyName = "serviceAccountToken")]
        public V1ServiceAccountTokenProjection ServiceAccountToken { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ServiceAccountToken != null)
            {
                ServiceAccountToken.Validate();
            }
        }
    }
}