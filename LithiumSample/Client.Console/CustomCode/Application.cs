using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Primavera.Lithium.Faturacao.Client.Console
{
    /// <inheritdoc />
    internal sealed partial class Application
    {
        /// <inheritdoc />
        protected override void PrintCustomMenuOptions()
        {
        }

        /// <inheritdoc />
        protected override Task<bool> HandleCustomMenuOptionsAsync(ConsoleKeyInfo key)
        {
            return Task.FromResult(true);
        }
    }
}
