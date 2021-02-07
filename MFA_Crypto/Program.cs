/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/

using System;
using System.IO;
using System.Reflection;
using MFA;

namespace MFA_Crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            if (string.Compare(args[0], "/c") == 0)
            {
                if(!string.IsNullOrEmpty(args[1]))
                {
                    Settings settings = new Settings();
                    string str_xml = settings.ToXml();
                    File.WriteAllText(args[1], str_xml);
                    Console.WriteLine(str_xml);
                }
                else
                {
                    Console.WriteLine("Give a file path to create that file!");
                }
            }
            else
            {
                if (File.Exists(args[1]))
                {
                    Settings settings = new Settings();
                    using (FileStream fs = new FileStream(args[1], FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fs, System.Text.Encoding.UTF8))
                        {
                            try
                            {
                                settings = Settings.FromXml(reader.ReadToEnd());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }
                        fs.Close();
                    }
                    if (string.Compare(args[0], "/d") == 0)
                    {
                        PropertyInfo[] properties = typeof(Settings).GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            Encryptable[] attrib = (Encryptable[])property.GetCustomAttributes(typeof(Encryptable), false);
                            if(attrib.Length != 0)
                            {
                                property.SetValue(settings, Crypto.Unprotect(property.GetValue(settings).ToString()));
                            }
                        }
                    }
                    else if (string.Compare(args[0], "/e") == 0)
                    {
                        PropertyInfo[] properties = typeof(Settings).GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            Encryptable[] attrib = (Encryptable[])property.GetCustomAttributes(typeof(Encryptable), false);
                            if (attrib.Length != 0)
                            {
                                property.SetValue(settings, Crypto.Protect(property.GetValue(settings).ToString()));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown option... Use /d to decrypt, /e to encrypt the file or /c to create a new file");
                    }

                    string str_xml = settings.ToXml();
                    File.WriteAllText(args[1], str_xml);
                    Console.WriteLine(str_xml);
                }
                else
                {
                    Console.WriteLine("File not exists!");
                }
            }
        }
    }
}
