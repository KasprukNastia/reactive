using Newtonsoft.Json;
using System;

namespace Lesson12.Application.Src.Utils
{
    public class JsonUtils
    {
        public static string WriteAsString(object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
