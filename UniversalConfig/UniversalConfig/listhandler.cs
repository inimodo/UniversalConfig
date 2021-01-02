using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Source
{
    public static class u_Listhandler
    {
        private static class header
        {
            public static readonly string s_header = "[UCFG/L]";
            public static char c_space = ',';
            public static char c_edgeleft = '{';
            public static char c_edgeright = '}';

        }
        private static StreamWriter o_writer;
        private static StreamReader o_reader;

        private static string s_configfile;
        public static void configset(string s_configpath)
        {
            s_configfile = s_configpath;
        }
        public static string read()
        {
            if (!File.Exists(s_configfile))
            {
                return null;
            }
            kill();
            o_reader = new StreamReader(s_configfile);
            if (o_reader != null)
            {
                string s_file = o_reader.ReadToEnd();
                o_reader.Close();

                s_file = s_file.Replace(Environment.NewLine, "");
                s_file = s_file.Replace('\t', ' ');
                s_file = s_file.Replace(" ", "");

                return s_file;
            }
            return null;
        }
        public static string write(string s_value)
        {
            kill();
            try
            {
                o_writer = new StreamWriter(s_configfile);
                o_writer.WriteLine(s_value);
            }
            catch (Exception)
            { }

            kill();
            return null;
        }
        private static void kill()
        {
            if (o_writer != null) o_writer.Close();
            if (o_reader != null) o_reader.Close();
        }

        private static string[] getlist()
        {
            string s_rawformat = read();
            if (s_rawformat == null) {
                return null;
            }
            if (!s_rawformat.Contains(header.s_header)) { return null; }
            if (!s_rawformat.Contains(header.c_edgeleft) || !s_rawformat.Contains(header.c_edgeright)) { return null; }

            s_rawformat = s_rawformat.Replace(header.s_header, "");
            s_rawformat = s_rawformat.Replace(header.c_edgeleft.ToString(), "");
            s_rawformat = s_rawformat.Replace(header.c_edgeright.ToString(), "");

            if (s_rawformat == "")
            {
                return new string[0];
            }
            return s_rawformat.Split(header.c_space);
        }

        private static bool writelist(string[] s_list)
        {
            string s_raw = header.s_header + header.c_edgeleft;
            if (s_list == null) { return false; }
            for (int i_index = 0; i_index < s_list.Length; i_index++)
            {
                s_raw += s_list[i_index];
                if (i_index != s_list.Length-1)
                {
                    s_raw += header.c_space;
                }
            }
            s_raw += header.c_edgeright;
            write(s_raw);
            return true;
        }

        public static bool writeintlist(int[] i_list)
        {
            if (i_list == null) { return false; }
            string[] s_data = new string[i_list.Length];
            for (int i_index = 0; i_index < i_list.Length; i_index++)
            {
                s_data[i_index] = i_list[i_index].ToString();
            }
            return writelist(s_data);           
        }

        public static bool writeintlist(float[] f_list)
        {
            if (f_list == null) { return false; }
            string[] s_data = new string[f_list.Length];
            for (int i_index = 0; i_index < f_list.Length; i_index++)
            {
                s_data[i_index] = f_list[i_index].ToString();
            }
            return writelist(s_data);
        }
        public static bool writestringlist(string[] s_list)
        {
            return writelist(s_list);
        }

        public static int[] readintlist()
        {
            string[] s_list = getlist();
            if(s_list == null) { return null; }

            int[] i_data = new int[s_list.Length];

            for (int i_index = 0; i_index < s_list.Length; i_index++)
            {
                if (!int.TryParse(s_list[i_index], out i_data[i_index])) {
                    return null;
                }
            }
            return i_data;
        }
        public static float[] readfloatlist()
        {
            string[] s_list = getlist();
            if (s_list == null) { return null; }

            float[] f_data = new float[s_list.Length];

            for (int i_index = 0; i_index < s_list.Length; i_index++)
            {
                if (!float.TryParse(s_list[i_index], out f_data[i_index]))
                {
                    return null;
                }
            }
            return f_data;
        }
        public static string[] readstringlist()
        {
             return getlist();
        }
    }
}
