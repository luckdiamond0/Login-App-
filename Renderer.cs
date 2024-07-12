using ImGuiNET;
using ClickableTransparentOverlay;
using System.Numerics;
using SixLabors.ImageSharp;
using System.Text;
using System;
using System.Runtime.InteropServices;

namespace LoginApp
{

    public class Renderer : Overlay
    {
        //load screen
        public bool Loading = true;
        public float loadingProgress = 0f;
        private float animationTimer = 0.0f;
        private const int circleSegments = 12;  // Number of segments in the loading animation
        private const float radius = 65.0f;     // Radius of the loading animation
        private const float speed = 2.0f;       // Speed of the animation

        //login
        public bool logintab = false;
        public string Username = "";
        public string Password = "";
        public bool key = false;
        public bool Login = false;
        public int CreateAccount = 0;
        public bool RegisterPageButton = false;
        public bool RegisterPage = false;
        public bool ResetPass = false;
        public bool POUI = false; //PassOrUserIncorrect

        //popup
        public float yPosition = 100f;

        //Login success
        public bool productarea = false;

        //your product
        private bool yourproduct = false;
        public bool website = false;
        public bool support = false;
        public bool market = false; 


        //buy
        public bool buypage = false;
        public bool Buy = false;

        //View Credencials
        public bool view = false;
        ImGuiInputTextFlags flags = ImGuiInputTextFlags.CharsNoBlank | ImGuiInputTextFlags.Password;

        //style
        private static Vector4 defaultBlack = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        private static Vector4 defaultPurple = new Vector4(0.19339603f, 0.03854112f, 0.3184713f, 1f);
        private static Vector4 defaultPurpleLight = new Vector4(0.6f, 0.4f, 0.8f, 0.8f);
        private static Vector4 defaultPurpleDark = new Vector4(0.12941177f, 0.02745098f, 0.21176471f, 1);
        static void SetupImGuiStyle()
        {
            ImGuiStylePtr style = ImGui.GetStyle();
            style.FrameRounding = 8.0f; 
            style.WindowRounding = 5.0f;
            style.FramePadding = new Vector2(7, 7);

            // Get a draw list
            var drawList = ImGui.GetWindowDrawList();
            var windowPos = ImGui.GetWindowPos();
            var windowSize = ImGui.GetWindowSize();

            // Create a color 
            var topColor = new Vector4(0.6f, 0.4f, 0.8f, 0.8f); 
            var bottomColor = new Vector4(0.6f, 0.4f, 0.8f, 0.0f); 

            // Draw a degrade 
            drawList.AddRectFilledMultiColor(
                windowPos,
                new Vector2(windowPos.X + windowSize.X, windowPos.Y + windowSize.Y),
                ImGui.ColorConvertFloat4ToU32(topColor),
                ImGui.ColorConvertFloat4ToU32(topColor),
                ImGui.ColorConvertFloat4ToU32(bottomColor),
                ImGui.ColorConvertFloat4ToU32(bottomColor)
            );

            ImGui.PushStyleColor(ImGuiCol.Button, defaultPurpleDark); //button original color
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, defaultPurpleLight); //on button click
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, defaultPurple); //mouse on button
            ImGui.PushStyleColor(ImGuiCol.FrameBg, defaultPurpleDark); //text input
            ImGui.PushStyleColor(ImGuiCol.PlotHistogram, new Vector4(0.6f, 0.4f, 0.8f, 1.0f)); // Progressbarcollor
        }

        //images
        string pathfile = "Icons\\LuckProfileImage.png";
        IntPtr imageHandle;
        uint imageHeight;
        uint imageWidth;

        //Icon
        string eye = "Icons\\Eye.png";
        IntPtr EyeHandle;
        uint EyeHeight;
        uint EyeWidth;

        string Close = "Icons\\Close.png";
        IntPtr CloseHandle;
        uint CloseHeight;
        uint CloseWidth;

        string Yourimage = "Icons\\YourImage.png";
        IntPtr YourimageHandle;
        uint YourimageHeight;
        uint YourimageWidth;

        //fonts
        string fontpath = "Fonts\\OpenSans.ttf";
        string loadingpath = "Fonts\\Loading-Regular.ttf";

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;

        int screenWidth = GetSystemMetrics(SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SM_CYSCREEN);


        protected override void Render()
        {
            //loading screen
            if (Loading) 
            {  
                ImGui.SetNextWindowSize(new Vector2(350, 310));
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(
                    ImGui.GetIO().DisplaySize.X / 2 - 165,
                    ImGui.GetIO().DisplaySize.Y / 2 - 200));
                if (ImGui.Begin("Loading Screen", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration))
                {
                    SetupImGuiStyle();
                    ReplaceFont(fontpath, 20, FontGlyphRangeType.English); //Font

                    //animation loading

                    animationTimer += ImGui.GetIO().DeltaTime * speed;

                    // Get the current window position
                    Vector2 windowPos = ImGui.GetWindowPos();
                    Vector2 windowSize = ImGui.GetWindowSize();
                    Vector2 center = new Vector2(windowPos.X + windowSize.X / 2, windowPos.Y + windowSize.Y / 3);

                    // Loop through each segment to draw the rotating balls
                    for (int i = 0; i < circleSegments; i++)
                    {
                        float angle = (i / (float)circleSegments) * 2.0f * (float)Math.PI + animationTimer;
                        Vector2 pos = new Vector2(
                            center.X + radius * (float)Math.Cos(angle),
                            center.Y + radius * (float)Math.Sin(angle)
                        );

                        // Draw the circle
                        ImGui.GetWindowDrawList().AddCircleFilled(pos, 5.0f, ImGui.GetColorU32(new Vector4(1.0f, 1.0f, 1.0f, 1.0f)));
                    }

                    ImGui.SetCursorPosX((windowSize.X - 200) / 2); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.625f); //pos y
                    ImGui.Text("Loading:");

                    ImGui.SetCursorPosX((windowSize.X - 200) / 2); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.693f); //pos y
                    ImGui.ProgressBar(loadingProgress / 100f, new Vector2(200, 28)); // Tama
                }
            }

            //first ui part
            if (logintab)
            {
                ImGui.SetNextWindowSize(new Vector2(300, 450));
                if (ImGui.Begin("Login", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration))
                {
                    ReplaceFont(fontpath, 20, FontGlyphRangeType.English); //Font

                    SetupImGuiStyle();

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.19f); //centralize image
                    AddOrGetImagePointer(pathfile, true, out imageHandle, out imageWidth, out imageHeight);
                    ImGui.Image(imageHandle, new Vector2(190, 190)); //image size
                    ImGui.SameLine();
                    var originalButtonColor = ImGui.GetStyle().Colors[(int)ImGuiCol.Button]; //get a original color
                    var originalButtonHoveredColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered]; //get a original color
                    var originalButtonActiveColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive]; //get a original color
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.758f); //centralize image
                    AddOrGetImagePointer(Close, true, out CloseHandle, out CloseWidth, out CloseHeight);
                    if (ImGui.ImageButton("Close",CloseHandle, new Vector2(65, 50))) // Exit
                    {
                        Environment.Exit(0);
                    }
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = originalButtonColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = originalButtonHoveredColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = originalButtonActiveColor; //reset colors 

                    if (POUI)
                    {
                       
                        ImGui.SetNextWindowSize(new Vector2(200, 100));
                        Vector2 windowPos = new Vector2(
                             ImGui.GetIO().DisplaySize.X - screenWidth - 0, 
                             ImGui.GetIO().DisplaySize.Y - screenHeight - yPosition
                        );
                        ImGui.SetNextWindowPos(windowPos, ImGuiCond.Always);
                        if (ImGui.Begin("PopUp", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove))
                        {
                            ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.3f); //centralize
                            ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), "Cannot Enter");
                            ImGui.Text("User or Password is incorrect ");
                            ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.3f); //centralize
                            ImGui.Text("Womp Womp");
                        }
                        ImGui.End();
                        if (yPosition > 0)
                        {
                            yPosition -= 2f; // Velocity for animation
                        }
                    }

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.110f); //left
                    ImGui.Text("Username");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.087f); //centralize inputtext
                    ImGui.SetNextItemWidth(250); //widht for input text
                    ImGui.InputText("##Username", ref Username, 30);

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.110f); //left
                    ImGui.Text("Password");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.087f); //centralize inputtext                                           
                    ImGui.SetNextItemWidth(250); //widht for input text
                    if (view)
                    {
                        ImGui.InputText("##Password", ref Password, 30); //Remove # your password
                    }
                    else 
                    {
                        ImGui.InputText("##Password", ref Password, 30, flags); //flags to # your password
                    }  
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.28f); //view a password pos x
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.563f); //view a password pos y
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    AddOrGetImagePointer(eye, true, out EyeHandle, out EyeWidth, out EyeHeight);
                    if (ImGui.ImageButton("Eye", EyeHandle, new Vector2(30, 32)))
                    {
                        view = !view;
                    }
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = originalButtonColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = originalButtonHoveredColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = originalButtonActiveColor; //reset colors

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.260f); //centralize
                    var buttonSize = new Vector2(150, 34);
                    if (ImGui.Button("Login", buttonSize))
                    {
                        Login = true;
                    }

                    if (RegisterPageButton)
                    {
                        ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.05f); //centralize
                        if (ImGui.Button("Create a Account"))
                        {
                            RegisterPage = true;
                        }
                        ImGui.SameLine();
                        ImGui.Text("Or");
                        ImGui.SameLine();
                        if (ImGui.Button("Reset Password"))
                        {
                            ResetPass = true;
                        }
                    }
                }
            }

            //product area
            if (productarea)
            {
                ImGui.SetNextWindowSize(new Vector2(500, 400));
                if (ImGui.Begin("Product Area", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration))
                {
                    var originalButtonColor = ImGui.GetStyle().Colors[(int)ImGuiCol.Button]; //get a original color
                    var originalButtonHoveredColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered]; //get a original color
                    var originalButtonActiveColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive]; //get a original color

                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.058f); //pos y
                    AddOrGetImagePointer(pathfile, true, out imageHandle, out imageWidth, out imageHeight);
                    ImGui.Image(imageHandle, new Vector2(75, 75)); //Your logo
                    ImGui.SameLine();
                    ImGui.SetWindowFontScale(1.4f);
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.32f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.088f); //pos y
                    ImGui.Text("Subriscription");
                    ImGui.SetWindowFontScale(1.0f);
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.92f); //Right
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    if (ImGui.Button("X", new Vector2(30, 30))) 
                    {
                        Environment.Exit(0);
                    }
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = originalButtonColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = originalButtonHoveredColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = originalButtonActiveColor; //reset colors
                    ImGui.SetWindowFontScale(1.2f);
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.32f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.18f); //pos y
                    ImGui.Text("Available subriscriptions");
                    ImGui.SetWindowFontScale(1.0f);
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.32f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.3f); //pos y
                    if (ImGui.Button("", new Vector2(310, 65))) //button for you app
                    {
                        productarea = false;
                        yourproduct = true;
                    }
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.35f); //centralize
                    ImGui.Text("Put Here Your Program/app name");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.35f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.37f); //pos y
                    ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1.0f), "Days Left: Unlimited");
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.3f); //pos y

                    if (ImGui.Button("Website")) 
                    {
                        website = true;
                    }

                    if (ImGui.Button("Support"))
                    {
                        support = true;
                    }

                    if (ImGui.Button("Market"))
                    {
                        market = true;
                    }

                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = originalButtonColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = originalButtonHoveredColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = originalButtonActiveColor; //reset colors

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.114f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.9f); //pos y
                    ImGui.Text(Username); //Username
                }
            }

            //your product
            if (yourproduct)
            {
                ImGui.SetNextWindowSize(new Vector2(500, 400));
                if (ImGui.Begin("YourProductName", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration))
                {
                    var originalButtonColor = ImGui.GetStyle().Colors[(int)ImGuiCol.Button]; //get a original color
                    var originalButtonHoveredColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered]; //get a original color
                    var originalButtonActiveColor = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive]; //get a original color

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.22f); //centralize
                    ImGui.SetWindowFontScale(1.4f);
                    ImGui.Text("Put Here Your Program/app name");
                    ImGui.SetWindowFontScale(1.0f);
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.92f); //Right
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0, 0, 0, 0); //transparency 
                    if (ImGui.Button("X", new Vector2(30, 30)))
                    {
                        Environment.Exit(0);
                    }
                    ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = originalButtonColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = originalButtonHoveredColor; //reset colors
                    ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = originalButtonActiveColor; //reset colors

                    /* enter your product description */
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.177f); //centralize
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.15f); //pos y
                    ImGui.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.177f); //centralize
                    ImGui.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.177f); //centralize
                    ImGui.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.177f); //centralize
                    ImGui.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                    /* enter your product description */

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.2f); //centralize image
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.43f); //pos y
                    AddOrGetImagePointer(Yourimage, true, out YourimageHandle, out YourimageWidth, out YourimageHeight);
                    ImGui.Image(YourimageHandle, new Vector2(320, 160)); //image size

                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.37f); //centralize 
                    ImGui.SetCursorPosY((ImGui.GetWindowSize().Y) * 0.88f); //pos y
                    var buttonSize = new Vector2(150, 34);
                    if (ImGui.Button("Download", buttonSize)) 
                    { }
                }
            }

            //if does`t have a product
            if (buypage)
            {
                ImGui.SetNextWindowSize(new Vector2(300, 300));
                if (ImGui.Begin("Buy", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration))
                {
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.32f); //centralize 
                    ImGui.Text("Buy A Licence key");
                    ImGui.SameLine();
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.03f); //left
                    if (ImGui.Button("Go Back"))
                    {
                        buypage = false; //turn off this tab 
                        logintab = true; //and turn on login tab
                    }
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.085f); //centralize image
                    AddOrGetImagePointer(Yourimage, true, out YourimageHandle, out YourimageWidth, out YourimageHeight);
                    ImGui.Image(YourimageHandle, new Vector2(250,180));
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.44f); //centralize 
                    ImGui.Text("$100");
                    ImGui.SetCursorPosX((ImGui.GetWindowSize().X) * 0.385f); //centralize 
                    if (ImGui.Button("Buy Now"))
                    {
                        Buy = true;
                    }     
                }
            }

        }

    }

}
