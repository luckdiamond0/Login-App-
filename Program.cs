using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace LoginApp
{
    public class Program
    {
        private static List<User> users;

        static void LoadUsersFromJson()
        {
            string jsonFilePath = "UsersJson/userdata.json"; // Path To Json

            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                users = JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error to find a user Code: {ex.Message}");
                users = new List<User>(); // Inicialize a empity list if find a error
            }
        }
      
        public static void Main(string[] args)
        {
            Renderer renderer = new Renderer();
            renderer.Start().Wait();

            while (true)
            {
                if (renderer.Loading)
                {
                    renderer.loadingProgress += 0.5f; // Charging progress simulation

                    if (renderer.loadingProgress >= 100f)
                    {
                        renderer.Loading = false; // Loading complete 
                        renderer.logintab = true;
                    }

                    Thread.Sleep(35);
                }

                if (renderer.Login) {
                    //System to rember to create account or reset password
                    renderer.CreateAccount += 1;
                    if (renderer.CreateAccount >= 5)
                    {
                        if (renderer.CreateAccount >= 10) //this part is to rest a var and number
                        {
                            renderer.RegisterPageButton = false;
                            renderer.CreateAccount = 0;
                        }

                        renderer.RegisterPageButton = true;
                    }
                    else
                    {
                        renderer.RegisterPageButton = false;
                    }

                    LoadUsersFromJson();

                    string CheckUser = renderer.Username;
                    string CheckPass = renderer.Password;
                    bool CheckKey = renderer.key;

                    User user = users.Find(u => u.Username == CheckUser); //Find a User

                    if (user != null && user.Password == CheckPass)  //Get a user
                    {
                        if (user.Key) //Go to download page
                        {
                            renderer.productarea = true;
                            renderer.logintab = false;
                            renderer.buypage = false;
                        }
                        else if (!user.Key) //if does`t have key redirect for tab
                        {
                            renderer.buypage = true;
                            renderer.logintab = false;
                        }
                        else
                        {
                            renderer.logintab = true;
                            renderer.buypage = false;
                        }         

                    }
                    else // User or Password is incorrect 
                    {
                        renderer.POUI = true;
                        Thread.Sleep(3000);
                        // Reset original pos
                        renderer.yPosition = 100f; // Inicial Y Pos
                        renderer.POUI = false;
                    }

                    renderer.Login = false;
                }
                //System to go a register url
                if (renderer.RegisterPage)
                {
                    string url = "https://github.com/luckdiamond0"; // your url to register

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url register:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.RegisterPage = false;
                }

                //System to go a Reset a password url
                if (renderer.ResetPass)
                {
                    string url = "https://github.com/luckdiamond0"; // your url to Resetpass

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url reset password:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.ResetPass = false;
                }   

                //System to buy
                if (renderer.Buy) 
                {
                    string url = "https://github.com/luckdiamond0"; // your url

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.Buy = false;

                }

                //System to go a Website
                if (renderer.website)
                {
                    string url = "https://github.com/luckdiamond0"; // your url to Website

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.website = false;
                }

                //System to go a Support
                if (renderer.support)
                {
                    string url = "https://github.com/luckdiamond0"; // your url to Support

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.support = false;
                }

                //System to go a Market
                if (renderer.market)
                {
                    string url = "https://github.com/luckdiamond0"; // your url to Market

                    try
                    {
                        // Create a ProcessStartInfo
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        };

                        // Start Process
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro to open url:");
                        Console.WriteLine(ex.Message);
                    }

                    renderer.market = false;
                }
            }
        }
    }
}
