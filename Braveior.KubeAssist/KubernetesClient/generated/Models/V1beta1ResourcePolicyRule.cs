// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ResourcePolicyRule is a predicate that matches some resource requests,
    /// testing the request's verb and the target resource. A
    /// ResourcePolicyRule matches a resource request if and only if: (a) at
    /// least one member of verbs matches the request, (b) at least one member
    /// of apiGroups matches the request, (c) at least one member of resources
    /// matches the request, and (d) least one member of namespaces matches the
    /// request.
    /// </summary>
    public partial class V1beta1ResourcePolicyRule
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1ResourcePolicyRule class.
        /// </summary>
        public V1beta1ResourcePolicyRule()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1ResourcePolicyRule class.
        /// </summary>
        /// <param name="apiGroups">`apiGroups` is a list of matching API
        /// groups and may not be empty. "*" matches all API groups and, if
        /// present, must be the only entry. Required.</param>
        /// <param name="resources">`resources` is a list of matching resources
        /// (i.e., lowercase and plural) with, if desired, subresource.  For
        /// example, [ "services", "nodes/status" ].  This list may not be
        /// empty. "*" matches all resources and, if present, must be the only
        /// entry. Required.</param>
        /// <param name="verbs">`verbs` is a list of matching verbs and may not
        /// be empty. "*" matches all verbs and, if present, must be the only
        /// entry. Required.</param>
        /// <param name="clusterScope">`clusterScope` indicates whether to
        /// match requests that do not specify a namespace (which happens
        /// either because the resource is not namespaced or the request
        /// targets all namespaces). If this field is omitted or false then the
        /// `namespaces` field must contain a non-empty list.</param>
        /// <param name="namespaces">`namespaces` is a list of target
        /// namespaces that restricts matches.  A request that specifies a
        /// target namespace matches only if either (a) this list contains that
        /// target namespace or (b) this list contains "*".  Note that "*"
        /// matches any specified namespace but does not match a request that
        /// _does not specify_ a namespace (see the `clusterScope` field for
        /// that). This list may be empty, but only if `clusterScope` is
        /// true.</param>
        public V1beta1ResourcePolicyRule(IList<string> apiGroups, IList<string> resources, IList<string> verbs, bool? clusterScope = default(bool?), IList<string> namespaces = default(IList<string>))
        {
            ApiGroups = apiGroups;
            ClusterScope = clusterScope;
            Namespaces = namespaces;
            Resources = resources;
            Verbs = verbs;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets `apiGroups` is a list of matching API groups and may
        /// not be empty. "*" matches all API groups and, if present, must be
        /// the only entry. Required.
        /// </summary>
        [JsonProperty(PropertyName = "apiGroups")]
        public IList<string> ApiGroups { get; set; }

        /// <summary>
        /// Gets or sets `clusterScope` indicates whether to match requests
        /// that do not specify a namespace (which happens either because the
        /// resource is not namespaced or the request targets all namespaces).
        /// If this field is omitted or false then the `namespaces` field must
        /// contain a non-empty list.
        /// </summary>
        [JsonProperty(PropertyName = "clusterScope")]
        public bool? ClusterScope { get; set; }

        /// <summary>
        /// Gets or sets `namespaces` is a list of target namespaces that
        /// restricts matches.  A request that specifies a target namespace
        /// matches only if either (a) this list contains that target namespace
        /// or (b) this list contains "*".  Note that "*" matches any specified
        /// namespace but does not match a request that _does not specify_ a
        /// namespace (see the `clusterScope` field for that). This list may be
        /// empty, but only if `clusterScope` is true.
        /// </summary>
        [JsonProperty(PropertyName = "namespaces")]
        public IList<string> Namespaces { get; set; }

        /// <summary>
        /// Gets or sets `resources` is a list of matching resources (i.e.,
        /// lowercase and plural) with, if desired, subresource.  For example,
        /// [ "services", "nodes/status" ].  This list may not be empty. "*"
        /// matches all resources and, if present, must be the only entry.
        /// Required.
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public IList<string> Resources { get; set; }

        /// <summary>
        /// Gets or sets `verbs` is a list of matching verbs and may not be
        /// empty. "*" matches all verbs and, if present, must be the only
        /// entry. Required.
        /// </summary>
        [JsonProperty(PropertyName = "verbs")]
        public IList<string> Verbs { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ApiGroups == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ApiGroups");
            }
            if (Resources == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Resources");
            }
            if (Verbs == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Verbs");
            }
        }
    }
}
