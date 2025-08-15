using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Shop.Application.Common.Errors
{
    public sealed class NotFoundException : Exception
    {
        private string _resource;
        private string _key;

        public NotFoundException(string resource, string key)
            : base($"{resource} with key '{key}' was not found")
        {
            _resource = resource;
            _key = key;
        }

        public string Resource { get { return _resource; } }
        public string Key { get { return _key; } }
    }
}
