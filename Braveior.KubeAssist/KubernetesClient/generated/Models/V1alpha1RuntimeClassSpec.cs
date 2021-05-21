// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// RuntimeClassSpec is a specification of a RuntimeClass. It contains
    /// parameters that are required to describe the RuntimeClass to the
    /// Container Runtime Interface (CRI) implementation, as well as any other
    /// components that need to understand how the pod will be run. The
    /// RuntimeClassSpec is immutable.
    /// </summary>
    public partial class V1alpha1RuntimeClassSpec
    {
        /// <summary>
        /// Initializes a new instance of the V1alpha1RuntimeClassSpec class.
        /// </summary>
        public V1alpha1RuntimeClassSpec()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1alpha1RuntimeClassSpec class.
        /// </summary>
        /// <param name="runtimeHandler">RuntimeHandler specifies the
        /// underlying runtime and configuration that the CRI implementation
        /// will use to handle pods of this class. The possible values are
        /// specific to the node &amp; CRI configuration.  It is assumed that
        /// all handlers are available on every node, and handlers of the same
        /// name are equivalent on every node. For example, a handler called
        /// "runc" might specify that the runc OCI runtime (using native Linux
        /// containers) will be used to run the containers in a pod. The
        /// RuntimeHandler must be lowercase, conform to the DNS Label (RFC
        /// 1123) requirements, and is immutable.</param>
        /// <param name="overhead">Overhead represents the resource overhead
        /// associated with running a pod for a given RuntimeClass. For more
        /// details, see
        /// https://git.k8s.io/enhancements/keps/sig-node/20190226-pod-overhead.md
        /// This field is alpha-level as of Kubernetes v1.15, and is only
        /// honored by servers that enable the PodOverhead feature.</param>
        /// <param name="scheduling">Scheduling holds the scheduling
        /// constraints to ensure that pods running with this RuntimeClass are
        /// scheduled to nodes that support it. If scheduling is nil, this
        /// RuntimeClass is assumed to be supported by all nodes.</param>
        public V1alpha1RuntimeClassSpec(string runtimeHandler, V1alpha1Overhead overhead = default(V1alpha1Overhead), V1alpha1Scheduling scheduling = default(V1alpha1Scheduling))
        {
            Overhead = overhead;
            RuntimeHandler = runtimeHandler;
            Scheduling = scheduling;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets overhead represents the resource overhead associated
        /// with running a pod for a given RuntimeClass. For more details, see
        /// https://git.k8s.io/enhancements/keps/sig-node/20190226-pod-overhead.md
        /// This field is alpha-level as of Kubernetes v1.15, and is only
        /// honored by servers that enable the PodOverhead feature.
        /// </summary>
        [JsonProperty(PropertyName = "overhead")]
        public V1alpha1Overhead Overhead { get; set; }

        /// <summary>
        /// Gets or sets runtimeHandler specifies the underlying runtime and
        /// configuration that the CRI implementation will use to handle pods
        /// of this class. The possible values are specific to the node
        /// &amp;amp; CRI configuration.  It is assumed that all handlers are
        /// available on every node, and handlers of the same name are
        /// equivalent on every node. For example, a handler called "runc"
        /// might specify that the runc OCI runtime (using native Linux
        /// containers) will be used to run the containers in a pod. The
        /// RuntimeHandler must be lowercase, conform to the DNS Label (RFC
        /// 1123) requirements, and is immutable.
        /// </summary>
        [JsonProperty(PropertyName = "runtimeHandler")]
        public string RuntimeHandler { get; set; }

        /// <summary>
        /// Gets or sets scheduling holds the scheduling constraints to ensure
        /// that pods running with this RuntimeClass are scheduled to nodes
        /// that support it. If scheduling is nil, this RuntimeClass is assumed
        /// to be supported by all nodes.
        /// </summary>
        [JsonProperty(PropertyName = "scheduling")]
        public V1alpha1Scheduling Scheduling { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (RuntimeHandler == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "RuntimeHandler");
            }
        }
    }
}