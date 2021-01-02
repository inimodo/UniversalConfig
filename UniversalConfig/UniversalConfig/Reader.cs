using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace UniversalConfig
{
    public partial class UniversalConfigReader : UniversalConfigMeta
    {
        UniversalConfigReader(string i_Path)
        {
            Open(i_Path);
        }
        private string GetRawContent()
        {
            try
            {
                using (StreamReader o_Reader = new StreamReader(this.o_Stream))
                {
                    return o_Reader.ReadToEnd();
                }
            }
            catch (Exception o_Exception)
            {
                this.s_Error = o_Exception.Message;
                return null;

            }
        }
    }
}
