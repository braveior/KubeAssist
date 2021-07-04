using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Braveior.KubeAssist.Helpers
{
    public class ValidatingNodeDeserializer : INodeDeserializer
    {
        private readonly INodeDeserializer _nodeDeserializer;

        public ValidatingNodeDeserializer(INodeDeserializer nodeDeserializer)
        {
            _nodeDeserializer = nodeDeserializer;
        }

        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            if (_nodeDeserializer.Deserialize(parser, expectedType, nestedObjectDeserializer, out value))
            {
                var context = new ValidationContext(value, null, null);
                Validator.ValidateObject(value, context, true);
                return true;
            }
            return false;
        }
    }
}
