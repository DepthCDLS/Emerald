﻿using CmlLib.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Emerald.WinUI.Helpers.Settings.JSON
{
    public class JSON : Models.Model
    {
        public string Serialize()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public class SettingsBackup : JSON
    {
        public string Backup { get; set; }

        public DateTime Time { get; set; }
    }

    public class Backups : JSON
    {
        public SettingsBackup[] AllBackups { get; set; } = Array.Empty<SettingsBackup>();
        public string APIVersion { get; private set; } = "1.0";
    }

    public class Settings : JSON
    {
        public static Settings CreateNew() => new()
        {
            App = new()
            {
                Discord = new(),
                Appearance = new()
                {
                    MicaTintColor = ((int)Enums.MicaTintColor.NoColor),
                    Theme = ((int)ElementTheme.Default)
                }
            },
            Minecraft = new()
            {
                Path = MinecraftPath.GetOSDefaultPath(),
                RAM = DirectResoucres.MaxRAM / 2,
                MCVerionsConfiguration = new(),
                JVM = new(),
                Downloader = new()
                {
                    AssetsCheck = true,
                    HashCheck = true
                }
            }
        };

        public string APIVersion { get; set; } = "1.2";

        public Minecraft Minecraft { get; set; } = new();

        public App App { get; set; } =new();
    }

    public partial class Minecraft : JSON
    {
        public Minecraft()
        {
            JVM.PropertyChanged += (_, _)
                => InvokePropertyChanged();
            this.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName != null)
                    InvokePropertyChanged();
            };
        }

        [JsonIgnore]
        public double RAMinGB => Math.Round(RAM / Math.Pow(1024, 1), 1);


        [ObservableProperty]
        private string _Path;

        [ObservableProperty]
        private int _RAM;

        [ObservableProperty]
        private bool _IsAdmin;

        public Account[] Accounts { get; set; }

        public Downloader Downloader { get; set; } = new();

        public MCVerionsConfiguration MCVerionsConfiguration { get; set; }

        public JVM JVM { get; set; } = new();

        public bool ReadLogs()
            => JVM.GameLogs && !IsAdmin;
    }

    public class Account : JSON
    {
        public string Type { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string UUID { get; set; }
        public bool LastAccessed { get; set; }
    }

    public partial class Downloader : JSON
    {

        [ObservableProperty]
        public bool _HashCheck;

        [ObservableProperty]
        public bool _AssetsCheck;
    }

    public partial class JVM : JSON
    {

        [ObservableProperty]
        public string[] _Arguments;

        [ObservableProperty]
        public int _ScreenWidth;

        [ObservableProperty]
        public int _ScreenHeight;

        [ObservableProperty]
        public bool _FullScreen;

        [ObservableProperty]
        private bool _GameLogs;
    }

    public class App : JSON
    {
        public Appearance Appearance { get; set; }
        public bool AutoLogin { get; set; }
        public Discord Discord { get; set; }
        public NewsFilter NewsFilter { get; set; } = new();
        public bool AutoClose { get; set; }
        public bool HideOnLaunch { get; set; }
        public bool WindowsHello { get; set; }
    }

    public partial class NewsFilter : JSON
    {
        [ObservableProperty]
        private bool _Java = true;

        [ObservableProperty]
        private bool _Bedrock = true;

        [ObservableProperty]
        private bool _Dungeons = true;

        [ObservableProperty]
        private bool _Legends = true;

        [JsonIgnore]
        public bool All
        {
            get => false;
            set
            {
                if (value)
                    Java = Bedrock = Dungeons = Legends = true;
            }
        }
        public string[] GetResult()
        {
            var r = new List<string>();

            if (Java)
                r.Add("Minecraft: Java Edition");

            if (Bedrock)
                r.Add("Minecraft for Windows");

            if (Dungeons)
                r.Add("Minecraft Dungeons");

            if (Legends)
                r.Add("Minecraft Legends");

            return r.ToArray();
        }
    }
    public class Discord : JSON
    {
    }
    public partial class MCVerionsConfiguration : JSON
    {
        [ObservableProperty]
        private bool _Release = true;

        [ObservableProperty]
        private bool _Custom = false;

        [ObservableProperty]
        private bool _OldBeta = false;

        [ObservableProperty]
        private bool _OldAlpha = false;

        [ObservableProperty]
        private bool _Snapshot = false;
    }

    public partial class Appearance : JSON
    {
        [ObservableProperty]
        private int _NavIconType = 1;

        public bool ShowFontIcons => NavIconType == 0;

        [ObservableProperty]
        private int _Theme;

        [ObservableProperty]
        private int _MicaTintColor;

        [ObservableProperty]
        private int _MicaType = 0;

        [ObservableProperty]

        private (int A, int R, int G, int B)? _CustomMicaTintColor;
    }
}
