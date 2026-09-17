using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechOneCore.Services.Interface
{
    /// <summary>
    /// create an interface for the NumberToWordsConverter service, so that it can be used in other projects like TechOneAPI, and no need to reference the implementation class directly
    /// also, it is a good practice to use interface for dependency injection, and it is easier to mock the interface for unit testing.
    /// </summary>
    public interface INumberToWordsConverter
    {
        string ConvertValueToString(decimal? amount);
    }
}
