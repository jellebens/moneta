using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands
{
    public static class Known
    {
        public static Type[] Types() {
            return typeof(ICommand).Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t)).ToArray();
        }
    }
}
