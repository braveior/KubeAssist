// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// PersistentVolumeClaimSpec describes the common attributes of storage
    /// devices and allows a Source for provider-specific attributes
    /// </summary>
    public partial class V1PersistentVolumeClaimSpec
    {
        /// <summary>
        /// Initializes a new instance of the V1PersistentVolumeClaimSpec
        /// class.
        /// </summary>
        public V1PersistentVolumeClaimSpec()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1PersistentVolumeClaimSpec
        /// class.
        /// </summary>
        /// <param name="accessModes">AccessModes contains the desired access
        /// modes the volume should have. More info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#access-modes-1</param>
        /// <param name="dataSource">This field can be used to specify either:
        /// * An existing VolumeSnapshot object
        /// (snapshot.storage.k8s.io/VolumeSnapshot) * An existing PVC
        /// (PersistentVolumeClaim) * An existing custom resource that
        /// implements data population (Alpha) In order to use custom resource
        /// types that implement data population, the AnyVolumeDataSource
        /// feature gate must be enabled. If the provisioner or an external
        /// controller can support the specified data source, it will create a
        /// new volume based on the contents of the specified data
        /// source.</param>
        /// <param name="resources">Resources represents the minimum resources
        /// the volume should have. More info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#resources</param>
        /// <param name="selector">A label query over volumes to consider for
        /// binding.</param>
        /// <param name="storageClassName">Name of the StorageClass required by
        /// the claim. More info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#class-1</param>
        /// <param name="volumeMode">volumeMode defines what type of volume is
        /// required by the claim. Value of Filesystem is implied when not
        /// included in claim spec.</param>
        /// <param name="volumeName">VolumeName is the binding reference to the
        /// PersistentVolume backing this claim.</param>
        public V1PersistentVolumeClaimSpec(IList<string> accessModes = default(IList<string>), V1TypedLocalObjectReference dataSource = default(V1TypedLocalObjectReference), V1ResourceRequirements resources = default(V1ResourceRequirements), V1LabelSelector selector = default(V1LabelSelector), string storageClassName = default(string), string volumeMode = default(string), string volumeName = default(string))
        {
            AccessModes = accessModes;
            DataSource = dataSource;
            Resources = resources;
            Selector = selector;
            StorageClassName = storageClassName;
            VolumeMode = volumeMode;
            VolumeName = volumeName;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets accessModes contains the desired access modes the
        /// volume should have. More info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#access-modes-1
        /// </summary>
        [JsonProperty(PropertyName = "accessModes")]
        public IList<string> AccessModes { get; set; }

        /// <summary>
        /// Gets or sets this field can be used to specify either: * An
        /// existing VolumeSnapshot object
        /// (snapshot.storage.k8s.io/VolumeSnapshot) * An existing PVC
        /// (PersistentVolumeClaim) * An existing custom resource that
        /// implements data population (Alpha) In order to use custom resource
        /// types that implement data population, the AnyVolumeDataSource
        /// feature gate must be enabled. If the provisioner or an external
        /// controller can support the specified data source, it will create a
        /// new volume based on the contents of the specified data source.
        /// </summary>
        [JsonProperty(PropertyName = "dataSource")]
        public V1TypedLocalObjectReference DataSource { get; set; }

        /// <summary>
        /// Gets or sets resources represents the minimum resources the volume
        /// should have. More info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#resources
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public V1ResourceRequirements Resources { get; set; }

        /// <summary>
        /// Gets or sets a label query over volumes to consider for binding.
        /// </summary>
        [JsonProperty(PropertyName = "selector")]
        public V1LabelSelector Selector { get; set; }

        /// <summary>
        /// Gets or sets name of the StorageClass required by the claim. More
        /// info:
        /// https://kubernetes.io/docs/concepts/storage/persistent-volumes#class-1
        /// </summary>
        [JsonProperty(PropertyName = "storageClassName")]
        public string StorageClassName { get; set; }

        /// <summary>
        /// Gets or sets volumeMode defines what type of volume is required by
        /// the claim. Value of Filesystem is implied when not included in
        /// claim spec.
        /// </summary>
        [JsonProperty(PropertyName = "volumeMode")]
        public string VolumeMode { get; set; }

        /// <summary>
        /// Gets or sets volumeName is the binding reference to the
        /// PersistentVolume backing this claim.
        /// </summary>
        [JsonProperty(PropertyName = "volumeName")]
        public string VolumeName { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (DataSource != null)
            {
                DataSource.Validate();
            }
        }
    }
}