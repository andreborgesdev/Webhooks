using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Primavera.Lithium.Webhooks.Abstractions
{
    /// <summary>
    /// Defines base class for the controller that provides monitoring routes.
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse"/> class.
        /// </summary>
        public BaseResponse()
        {
            this.IsSuccess = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether defines base class for the controller that provides monitoring routes.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets defines base class for the controller that provides monitoring routes.
        /// </summary>
        public string Message { get; set; }
    }
}
