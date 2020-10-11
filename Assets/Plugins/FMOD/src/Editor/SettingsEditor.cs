using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

namespace FMODUnity
{
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor
    {
        string[] ToggleParent = new string[] { "Disabled", "Enabled", "Development Build Only",  };

        string[] ToggleEditor = new string[] { "Enabled", "Disabled", };

        string[] FrequencyDisplay = new string[] { "Platform Default", "22050", "24000", "32000", "44100", "48000"};
        int[] FrequencyValues = new int[] { 0, 22050, 24000, 32000, 44100, 48000 };

        string[] SpeakerModeDisplay = new string[] {
            "Stereo",
            "5.1",
            "7.1" };

        FMOD.SPEAKERMODE[] SpeakerModeValues = new FMOD.SPEAKERMODE[] {
            FMOD.SPEAKERMODE.STEREO,
            FMOD.SPEAKERMODE._5POINT1,
            FMOD.SPEAKERMODE._7POINT1};

        bool hasBankSourceChanged = false;
        string targetSubFolder;
        bool focused = false;
        bool bankFoldOutState = true;

        enum SourceType : uint
        {
            Project = 0,
            Single,
            Multi
        }

        void DisplayTriStateBool(string label, Platform platform, Platform.PropertyAccessor<TriStateBool> property)
        {
            TriStateBool current = property.Get(platform);

            if (platform.Parent != null)
            {
                bool overriden = property.HasValue(platform);
                TriStateBool parent = property.Get(platform.Parent);

                string[] toggleChild = new string[ToggleParent.Length + 1];
                Array.Copy(ToggleParent, 0, toggleChild, 1, ToggleParent.Length);
                toggleChild[0] = string.Format("Inherit ({0})", ToggleParent[(int)parent]);

                int next = EditorGUILayout.Popup(label, overriden ? (int)current + 1 : 0, toggleChild);

                if (next == 0)
                {
                    property.Clear(platform);
                }
                else
                {
                    property.Set(platform, (TriStateBool)(next-1));
                }
            }
            else if (platform is PlatformPlayInEditor)
            {
                int next = EditorGUILayout.Popup(label, (current != TriStateBool.Disabled) ? 0 : 1, ToggleEditor);
                property.Set(platform, next == 0 ? TriStateBool.Enabled : TriStateBool.Disabled);
            }
            else
            {
                int next = EditorGUILayout.Popup(label, (int)current, ToggleParent);
                property.Set(platform, (TriStateBool)next);
            }
        }

        void DisplaySampleRate(string label, Platform platform)
        {
            int currentValue = platform.SampleRate;
            int currentIndex = Array.IndexOf(FrequencyValues, currentValue);

            if (platform.Parent != null)
            {
                int parentValue = platform.Parent.SampleRate;
                int parentIndex = Array.IndexOf(FrequencyValues, parentValue);

                string[] valuesChild = new string[FrequencyDisplay.Length + 1];
                Array.Copy(FrequencyDisplay, 0, valuesChild, 1, FrequencyDisplay.Length);
                valuesChild[0] = string.Format("Inherit ({0})", FrequencyDisplay[parentIndex]);

                bool overriden = Platform.PropertyAccessors.SampleRate.HasValue(platform);

                int next = EditorGUILayout.Popup(label, overriden ? currentIndex + 1 : 0, valuesChild);
                if (next == 0)
                {
                    Platform.PropertyAccessors.SampleRate.Clear(platform);
                }
                else
                {
                    Platform.PropertyAccessors.SampleRate.Set(platform, FrequencyValues[next - 1]);
                }
            }
            else
            {
                int next = EditorGUILayout.Popup(label, currentIndex, FrequencyDisplay);
                Platform.PropertyAccessors.SampleRate.Set(platform, FrequencyValues[next]);
            }
        }

        void DisplayBuildDirectory(string label, Platform platform)
        {
            string[] buildDirectories = EditorUtils.GetBankPlatforms();

            string currentValue = platform.BuildDirectory;
            int currentIndex = Math.Max(Array.IndexOf(buildDirectories, currentValue), 0);

            if (platform.Parent != null || platform is PlatformPlayInEditor)
            {
                string[] values = new string[buildDirectories.Length + 1];
                Array.Copy(buildDirectories, 0, values, 1, buildDirectories.Length);

                if (platform is PlatformPlayInEditor)
                {
                    Settings settings = target as Settings;
                    values[0] = string.Format("Current Unity Platform ({0})", settings.CurrentEditorPlatform.BuildDirectory);
                }
                else
                {
                    values[0] = string.Format("Inherit ({0})", platform.Parent.BuildDirectory);
                }

                bool overriden = Platform.PropertyAccessors.BuildDirectory.HasValue(platform);
                int next = EditorGUILayout.Popup(label, overriden ? currentIndex + 1 : 0, values);

                if (next == 0)
                {
                    Platform.PropertyAccessors.BuildDirectory.Clear(platform);
                    Platform.PropertyAccessors.SpeakerMode.Clear(platform);
                }
                else
                {
                    Platform.PropertyAccessors.BuildDirectory.Set(platform, buildDirectories[next - 1]);
                }
            }
            else
            {
                int next = EditorGUILayout.Popup(label, currentIndex, buildDirectories);
                Platform.PropertyAccessors.BuildDirectory.Set(platform, buildDirectories[next]);
            }
        }

        void DisplaySpeakerMode(string label, Platform platform)
        {
            FMOD.SPEAKERMODE currentValue = platform.SpeakerMode;
            int currentIndex = Math.Max(Array.IndexOf(SpeakerModeValues, currentValue), 0);

            if (platform.Parent != null || platform is PlatformPlayInEditor)
            {
                bool overriden = Platform.PropertyAccessors.SpeakerMode.HasValue(platform);

                string[] values = new string[SpeakerModeDisplay.Length + 1];
                Array.Copy(SpeakerModeDisplay, 0, values, 1, SpeakerModeDisplay.Length);

                if (platform is PlatformPlayInEditor)
                {
                    Settings settings = target as Settings;
                    FMOD.SPEAKERMODE currentPlatformValue = settings.CurrentEditorPlatform.SpeakerMode;
                    int index = Array.IndexOf(SpeakerModeValues, currentPlatformValue);
                    values[0] = string.Format("Current Unity Platform ({0})", SpeakerModeDisplay[index]);
                }
                else
                {
                    FMOD.SPEAKERMODE parentValue = platform.Parent.SpeakerMode;
                    int index = Array.IndexOf(SpeakerModeValues, parentValue);
                    values[0] = string.Format("Inherit ({0})", SpeakerModeDisplay[index]);
                }

                bool hasBuildDirectory = Platform.PropertyAccessors.BuildDirectory.HasValue(platform);

                if (!hasBuildDirectory)
                {
                    EditorGUI.BeginDisabledGroup(true);
                }

                int next = EditorGUILayout.Popup(label, overriden ? currentIndex + 1 : 0, values);
                if (next == 0)
                {
                    Platform.PropertyAccessors.SpeakerMode.Clear(platform);
                }
                else
                {
                    Platform.PropertyAccessors.SpeakerMode.Set(platform, SpeakerModeValues[next - 1]);
                }

                if (hasBuildDirectory)
                {
                    EditorGUILayout.HelpBox(string.Format("Match the speaker mode to the setting of the platform <b>{0}</b> inside FMOD Studio", platform.BuildDirectory), MessageType.Info, false);
                }
                else
                {
                    EditorGUI.EndDisabledGroup();
                }
            }
            else
            {
                int next = EditorGUILayout.Popup(label, currentIndex, SpeakerModeDisplay);
                Platform.PropertyAccessors.SpeakerMode.Set(platform, SpeakerModeValues[next]);
                EditorGUILayout.HelpBox(string.Format("Match the speaker mode to the setting of the platform <b>{0}</b> inside FMOD Studio", platform.BuildDirectory), MessageType.Info, false);
            }
        }

        void DisplayInt(string label, Platform platform, Platform.PropertyAccessor<int> property, int min, int max)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);

            int currentValue = property.Get(platform);

            if (platform.Parent != null)
            {
                bool inherit = !property.HasValue(platform);

                inherit = GUILayout.Toggle(inherit, "Inherit");

                EditorGUI.BeginDisabledGroup(inherit);
                int next = EditorGUILayout.IntSlider(currentValue, min, max);
                EditorGUI.EndDisabledGroup();

                if (inherit)
                {
                    property.Clear(platform);
                }
                else
                {
                    property.Set(platform, next);
                }
            }
            else
            {
                int next = EditorGUILayout.IntSlider(currentValue, min, max);
                property.Set(platform, next);
            }

            EditorGUILayout.EndHorizontal();
        }

        private bool DrawLinks()
        {
            string color = EditorGUIUtility.isProSkin ? "#fa4d14" : "#0000FF";
            // Docs link
            UnityEditor.EditorGUILayout.BeginHorizontal();
            {
                var linkStyle = GUI.skin.button;
                linkStyle.richText = true;
                string caption = "Open FMOD Getting Started Guide";
                caption = String.Format("<color={0}>{1}</color>", color, caption);
                bool bClicked = GUILayout.Button(caption, linkStyle, GUILayout.ExpandWidth(false), GUILayout.Height(30), GUILayout.MaxWidth(300));

                var rect = GUILayoutUtility.GetLastRect();
                rect.width = linkStyle.CalcSize(new GUIContent(caption)).x;
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

                if (bClicked)
                {
                    Application.OpenURL("https://fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html");
                }
            }
            GUILayout.FlexibleSpace();
            // Support Link
            {
                var linkStyle = GUI.skin.button;
                linkStyle.richText = true;
                string caption = "Open FMOD Q&A";
                caption = String.Format("<color={0}>{1}</color>", color, caption);
                bool bClicked = GUILayout.Button(caption, linkStyle, GUILayout.ExpandWidth(false), GUILayout.Height(30), GUILayout.MaxWidth(200));

                var rect = GUILayoutUtility.GetLastRect();
                rect.width = linkStyle.CalcSize(new GUIContent(caption)).x;
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

                if (bClicked)
                {
                    Application.OpenURL("https://qa.fmod.com/");
                }
            }
            UnityEditor.EditorGUILayout.EndHorizontal();

            return true;
        }

        Dictionary<string, bool> expandPlatform = new Dictionary<string, bool>();

        private void DisplayPlatform(Platform platform)
        {
            if (!platform.Active)
            {
                return;
            }

            var label = new System.Text.StringBuilder();
            label.AppendFormat("<b>{0}</b>", platform.DisplayName);

            if (!platform.IsIntrinsic && platform.Children.Count > 0)
            {
                IEnumerable<string> children = platform.Children
                    .Where(child => child.Active)
                    .Select(child => child.DisplayName);

                if (children.Any())
                {
                    label.Append(" (");
                    label.Append(string.Join(", ", children.ToArray()));
                    label.Append(")");
                }
            }
            
            EditorGUILayout.BeginHorizontal();

            bool expand = true;

            if (platform.IsIntrinsic)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.richText = true;

                EditorGUILayout.LabelField(label.ToString(), style);
            }
            else
            {
                expand = false;

                if (expandPlatform.ContainsKey(platform.Identifier))
                {
                    expand = expandPlatform[platform.Identifier];
                }

                GUIStyle style = new GUIStyle(GUI.skin.FindStyle("Foldout"));
                style.richText = true;

                expand = EditorGUILayout.Foldout(expand, new GUIContent(label.ToString()), style);

                expandPlatform[platform.Identifier] = expand;

                if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
                {
                    // This avoids modifying the parent platform's children list while we're iterating over it
                    pendingPlatformDelete = platform;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (expand)
            {
                Settings settings = target as Settings;

                EditorGUI.indentLevel++;

                PlatformGroup group = platform as PlatformGroup;

                if (group != null)
                {
                    group.displayName = EditorGUILayout.DelayedTextField("Name", group.displayName);
                }

                DisplayPlatformParent(platform);

                DisplayTriStateBool("Live Update", platform, Platform.PropertyAccessors.LiveUpdate);

                if (platform.IsLiveUpdateEnabled)
                {
                    if (platform.IsIntrinsic)
                    {
                        EditorGUILayout.BeginHorizontal();
                        settings.LiveUpdatePort = ushort.Parse(EditorGUILayout.TextField("Live Update Port", settings.LiveUpdatePort.ToString()));
                        if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
                        {
                            settings.LiveUpdatePort = 9264;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUIStyle style = new GUIStyle(GUI.skin.label);
                        style.richText = true;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(" ");
                        GUILayout.Label(string.Format("Live update will listen on port <b>{0}</b>", settings.LiveUpdatePort), style);
                        EditorGUILayout.EndHorizontal();
                    }
                }

                DisplayTriStateBool("Debug Overlay", platform, Platform.PropertyAccessors.Overlay);
                DisplaySampleRate("Sample Rate", platform);

                if (settings.HasPlatforms)
                {
                    bool prevChanged = GUI.changed;
                    DisplayBuildDirectory("Bank Platform", platform);
                    hasBankSourceChanged |= !prevChanged && GUI.changed;

                    DisplaySpeakerMode("Speaker Mode", platform);
                }

                if (!(platform is PlatformPlayInEditor))
                {
                    DisplayInt("Virtual Channel Count", platform, Platform.PropertyAccessors.VirtualChannelCount, 1, 2048);
                    DisplayInt("Real Channel Count", platform, Platform.PropertyAccessors.RealChannelCount, 1, 256);
                    DisplayPlugins(platform);
                }

                if (!platform.IsIntrinsic)
                {
                    foreach (Platform child in platform.Children)
                    {
                        DisplayPlatform(child);
                    }
                }

                EditorGUI.indentLevel--;
            }
        }

        Dictionary<string, bool> expandPlugins = new Dictionary<string, bool>();

        private void DisplayPlugins(Platform platform)
        {
            List<string> plugins = platform.Plugins;

            bool expand;
            expandPlugins.TryGetValue(platform.Identifier, out expand);

            EditorGUILayout.BeginHorizontal();

            string title = string.Format("Plugins: {0}", plugins.Count);

            expand = EditorGUILayout.Foldout(expand, new GUIContent(title), true);

            var property = Platform.PropertyAccessors.Plugins;

            bool inherit = false;

            if (platform.Parent != null)
            {
                inherit = !property.HasValue(platform);

                EditorGUI.BeginChangeCheck();

                inherit = GUILayout.Toggle(inherit, "Inherit");

                if (EditorGUI.EndChangeCheck())
                {
                    if (inherit)
                    {
                        property.Clear(platform);
                    }
                    else
                    {
                        plugins = new List<string>(property.Get(platform.Parent));
                        property.Set(platform, plugins);

                        if (plugins.Count > 0)
                        {
                            expand = true;
                        }
                    }
                }
            }

            EditorGUI.BeginDisabledGroup(inherit);

            if (GUILayout.Button("Add Plugin", GUILayout.ExpandWidth(false)))
            {
                plugins.Add("");
                expand = true;
            }
            EditorGUILayout.EndHorizontal();

            if (expand)
            {
                EditorGUI.indentLevel++;

                for (int count = 0; count < plugins.Count; count++)
                {
                    EditorGUILayout.BeginHorizontal();
                    plugins[count] = EditorGUILayout.TextField("Plugin " + (count + 1).ToString() + ":", plugins[count]);

                    if (GUILayout.Button("Delete Plugin", GUILayout.ExpandWidth(false)))
                    {
                        plugins.RemoveAt(count);

                        if (plugins.Count == 0)
                        {
                            expand = false;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndDisabledGroup();

            expandPlugins[platform.Identifier] = expand;
        }

        private Platform pendingPlatformDelete;

        public override void OnInspectorGUI()
        {
            Settings settings = target as Settings;

            DrawLinks();

            EditorGUI.BeginChangeCheck();

            hasBankSourceChanged = false;
            bool hasBankTargetChanged = false;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUI.skin.FindStyle("HelpBox").richText = true;

            SourceType sourceType = settings.HasSourceProject ? SourceType.Project : (settings.HasPlatforms ? SourceType.Multi : SourceType.Single);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            sourceType = GUILayout.Toggle(sourceType == SourceType.Project, "Project", "Button") ? 0 : sourceType;
            sourceType = GUILayout.Toggle(sourceType == SourceType.Single, "Single Platform Build", "Button") ? SourceType.Single : sourceType;
            sourceType = GUILayout.Toggle(sourceType == SourceType.Multi, "Multiple Platform Build", "Button") ? SourceType.Multi : sourceType;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.HelpBox(
                "<size=11>Select the way you wish to connect Unity to the FMOD Studio content:\n" +
                "<b>• Project</b>\t\tIf you have the complete FMOD Studio project avaliable\n" +
                "<b>• Single Platform</b>\tIf you have only the contents of the <i>Build</i> folder for a single platform\n" +
                "<b>• Multiple Platforms</b>\tIf you have only the contents of the <i>Build</i> folder for multiple platforms, each platform in its own sub directory\n" + 
                "</size>"
                , MessageType.Info, true);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (sourceType == SourceType.Project)
            {
                EditorGUILayout.BeginHorizontal();
                string oldPath = settings.SourceProjectPath;
                EditorGUILayout.PrefixLabel("Studio Project Path", GUI.skin.textField, style);

                EditorGUI.BeginChangeCheck();
                string newPath = EditorGUILayout.TextField(GUIContent.none, settings.SourceProjectPath);
                if (EditorGUI.EndChangeCheck())
                {
                    if (newPath.EndsWith(".fspro"))
                    {
                        settings.SourceProjectPath = newPath;
                    }
                }

                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    GUI.FocusControl(null);
                    string path = EditorUtility.OpenFilePanel("Locate Studio Project", oldPath, "fspro");
                    if (!string.IsNullOrEmpty(path))
                    {
                        settings.SourceProjectPath = MakePathRelative(path);
                        Repaint();
                    }
                }
                EditorGUILayout.EndHorizontal();

                // Cache in settings for runtime access in play-in-editor mode
                string bankPath = EditorUtils.GetBankDirectory();
                settings.SourceBankPath = bankPath;
                settings.HasPlatforms = true;
                settings.HasSourceProject = true;

                // First time project path is set or changes, copy to streaming assets
                if (settings.SourceProjectPath != oldPath)
                {
                    hasBankSourceChanged = true;
                }
            }
            else if (sourceType == SourceType.Single || sourceType == SourceType.Multi)
            {
                EditorGUILayout.BeginHorizontal();
                string oldPath = settings.SourceBankPath;
                EditorGUILayout.PrefixLabel("Build Path", GUI.skin.textField, style);

                EditorGUI.BeginChangeCheck();
                string tempPath = EditorGUILayout.TextField(GUIContent.none, settings.SourceBankPath);
                if (EditorGUI.EndChangeCheck())
                {
                    settings.SourceBankPath = tempPath;
                }

                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    GUI.FocusControl(null);
                    string newPath = EditorUtility.OpenFolderPanel("Locate Build Folder", oldPath, null);
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        settings.SourceBankPath = MakePathRelative(newPath);
                        Repaint();
                    }
                }
                EditorGUILayout.EndHorizontal();

                settings.HasPlatforms = (sourceType == SourceType.Multi);
                settings.HasSourceProject = false;

                // First time project path is set or changes, copy to streaming assets
                if (settings.SourceBankPath != oldPath)
                {
                    hasBankSourceChanged = true;
                }
            }

            bool validBanks;
            string failReason;
            EditorUtils.ValidateSource(out validBanks, out failReason);
            if (!validBanks)
            {
                failReason += "\n\nFor detailed setup instructions, please see the getting started guide linked above.";
                EditorGUILayout.HelpBox(failReason, MessageType.Error, true);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(settings);
                }
                return;
            }

            ImportType importType = (ImportType)EditorGUILayout.EnumPopup("Import Type", settings.ImportType);
            if (importType != settings.ImportType)
            {
                hasBankTargetChanged = true;
                settings.ImportType = importType;

                bool deleteBanks = EditorUtility.DisplayDialog(
                    "FMOD Bank Import Type Changed", "Do you want to delete the " + settings.ImportType.ToString() + " banks in " + settings.TargetPath,
                    "Yes", "No");
                if (deleteBanks)
                {
                    // Delete the old banks
                    EventManager.removeBanks = true;
                    EventManager.RefreshBanks();
                }
            }

            // ----- Asset Sub Directory -------------
            {
                GUI.SetNextControlName("targetSubFolder");
                targetSubFolder = settings.ImportType == ImportType.AssetBundle
                    ? EditorGUILayout.TextField("FMOD Asset Sub Folder", string.IsNullOrEmpty(targetSubFolder) ? settings.TargetAssetPath : targetSubFolder)
                    : EditorGUILayout.TextField("FMOD Bank Sub Folder", string.IsNullOrEmpty(targetSubFolder) ? settings.TargetSubFolder : targetSubFolder);
                if (GUI.GetNameOfFocusedControl() == "targetSubFolder")
                {
                    focused = true;
                    if (Event.current.isKey)
                    {
                        switch (Event.current.keyCode)
                        {
                            case KeyCode.Return:
                            case KeyCode.KeypadEnter:
                                if (settings.TargetSubFolder != targetSubFolder)
                                {
                                    EventManager.RemoveBanks(settings.TargetPath);
                                    settings.TargetSubFolder = targetSubFolder;
                                    hasBankTargetChanged = true;
                                }
                                targetSubFolder = "";
                                break;
                        }
                    }
                }
                else if (focused)
                {
                    if (settings.TargetPath != targetSubFolder)
                    {
                        EventManager.RemoveBanks(settings.TargetPath);
                        settings.TargetSubFolder = targetSubFolder;
                        hasBankTargetChanged = true;
                    }
                    targetSubFolder = "";
                }
            }

            // ----- Logging -----------------
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<b>Logging</b>", style);
            EditorGUI.indentLevel++;
            settings.LoggingLevel = (FMOD.DEBUG_FLAGS)EditorGUILayout.EnumPopup("Logging Level", settings.LoggingLevel);
            EditorGUI.indentLevel--;

            // ----- Loading -----------------
            EditorGUI.BeginDisabledGroup(settings.ImportType == ImportType.AssetBundle);
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<b>Initialization</b>", style);
            EditorGUI.indentLevel++;

            settings.EnableMemoryTracking = EditorGUILayout.Toggle("Enable Memory Tracking", settings.EnableMemoryTracking);

            settings.BankLoadType = (BankLoadType)EditorGUILayout.EnumPopup("Load Banks", settings.BankLoadType);
            switch (settings.BankLoadType)
            {
                case BankLoadType.All:
                    break;
                case BankLoadType.Specified:
                    settings.AutomaticEventLoading = false;
                    Texture upArrowTexture = EditorGUIUtility.Load("FMOD/ArrowUp.png") as Texture;
                    Texture downArrowTexture = EditorGUIUtility.Load("FMOD/ArrowDown.png") as Texture;
                    bankFoldOutState = EditorGUILayout.Foldout(bankFoldOutState, "Specified Banks", true);
                    if (bankFoldOutState)
                    {
                        for (int i = 0; i < settings.BanksToLoad.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel++;

                            var bankName = settings.BanksToLoad[i];
                            EditorGUILayout.TextField(bankName.Replace(".bank", ""));

                            if (GUILayout.Button(upArrowTexture, GUILayout.ExpandWidth(false)))
                            {
                                if (i > 0)
                                {
                                    var temp = settings.BanksToLoad[i];
                                    settings.BanksToLoad[i] = settings.BanksToLoad[i - 1];
                                    settings.BanksToLoad[i - 1] = temp;
                                }
                                continue;
                            }
                            if (GUILayout.Button(downArrowTexture, GUILayout.ExpandWidth(false)))
                            {
                                if (i < settings.BanksToLoad.Count - 1)
                                {
                                    var temp = settings.BanksToLoad[i];
                                    settings.BanksToLoad[i] = settings.BanksToLoad[i + 1];
                                    settings.BanksToLoad[i + 1] = temp;
                                }
                                continue;
                            }

                            if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                            {
                                GUI.FocusControl(null);
                                string path = EditorUtility.OpenFilePanel("Locate Bank", settings.TargetPath, "bank");
                                if (!string.IsNullOrEmpty(path))
                                {
                                    path = RuntimeUtils.GetCommonPlatformPath(path);
                                    settings.BanksToLoad[i] = path.Replace(settings.TargetPath, "");
                                    Repaint();
                                }
                            }
                            if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                            {
                                Settings.Instance.BanksToLoad.RemoveAt(i);
                                continue;
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel--; 
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        if (GUILayout.Button("Add Bank", GUILayout.ExpandWidth(false)))
                        {
                            settings.BanksToLoad.Add("");
                        }
                        if (GUILayout.Button("Add All Banks", GUILayout.ExpandWidth(false)))
                        {
                            string sourceDir;

                            if (settings.HasSourceProject)
                            {
                                sourceDir = string.Format("{0}/{1}/", settings.SourceBankPath, settings.CurrentEditorPlatform.BuildDirectory);
                            }
                            else
                            {
                                sourceDir = settings.SourceBankPath;
                            }

                            sourceDir = RuntimeUtils.GetCommonPlatformPath(sourceDir);

                            var banksFound = new List<string>(Directory.GetFiles(sourceDir, "*.bank", SearchOption.AllDirectories));
                            for (int i = 0; i < banksFound.Count; i++)
                            {
                                string bankShortName = RuntimeUtils.GetCommonPlatformPath(banksFound[i]).Replace(sourceDir, "");
                                if (!settings.BanksToLoad.Contains(bankShortName))
                                {
                                    settings.BanksToLoad.Add(bankShortName);
                                }
                            }

                            Repaint();
                        }
                        if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
                        {
                            settings.BanksToLoad.Clear();
                        }
                        GUILayout.EndHorizontal();
                    }
                    break;
                case BankLoadType.None:
                    settings.AutomaticEventLoading = false;
                    break;
                default:
                    break;
            }

            EditorGUI.BeginDisabledGroup(settings.BankLoadType == BankLoadType.None);
            settings.AutomaticSampleLoading = EditorGUILayout.Toggle("Load Bank Sample Data", settings.AutomaticSampleLoading);
            EditorGUI.EndDisabledGroup();

            settings.EncryptionKey = EditorGUILayout.TextField("Bank Encryption Key", settings.EncryptionKey);

            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            // ----- PIE ----------------------------------------------
            EditorGUILayout.Separator();
            DisplayPlatform(settings.PlayInEditorPlatform);

            // ----- Default ----------------------------------------------
            EditorGUILayout.Separator();
            DisplayPlatform(settings.DefaultPlatform);

            // Top-level platforms
            EditorGUILayout.Separator();
            DisplayPlatformHeader();

            EditorGUI.indentLevel++;
            foreach (Platform platform in settings.DefaultPlatform.Children)
            {
                DisplayPlatform(platform);
            }
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(settings);
            }

            if (hasBankSourceChanged)
            {
                EventManager.RefreshBanks();
            }
            if (hasBankTargetChanged)
            {
                EventManager.RefreshBanks();
            }
            if (pendingPlatformDelete != null)
            {
                settings.RemovePlatformProperties(pendingPlatformDelete);

                ParentCandidates.Remove(pendingPlatformDelete);

                if (!(pendingPlatformDelete is PlatformGroup))
                {
                    MissingPlatforms.Add(pendingPlatformDelete);
                    MissingPlatforms.Sort(CompareDisplayNames);
                }

                pendingPlatformDelete = null;
            }
        }

        [NonSerialized]
        private Rect AddPlatformButtonRect;

        [NonSerialized]
        private List<Platform> ParentCandidates;

        [NonSerialized]
        private List<Platform> MissingPlatforms;

        private static int CompareDisplayNames(Platform a, Platform b)
        {
            return EditorUtility.NaturalCompare(a.DisplayName, b.DisplayName);
        }

        private void BuildPlatformLists()
        {
            if (MissingPlatforms == null)
            {
                MissingPlatforms = new List<Platform>();
                ParentCandidates = new List<Platform>();

                Settings settings = target as Settings;

                settings.ForEachPlatform(platform =>
                    {
                        if (!platform.Active)
                        {
                            MissingPlatforms.Add(platform);
                        }
                        else if (!platform.IsIntrinsic)
                        {
                            ParentCandidates.Add(platform);
                        }
                    });

                MissingPlatforms.Sort(CompareDisplayNames);
                ParentCandidates.Sort(CompareDisplayNames);
            }
        }

        private void AddPlatformProperties(object data)
        {
            string identifier = data as string;

            Settings settings = target as Settings;
            Platform platform = settings.FindPlatform(identifier);

            settings.AddPlatformProperties(platform);

            MissingPlatforms.Remove(platform);

            ParentCandidates.Add(platform);
            ParentCandidates.Sort(CompareDisplayNames);
        }

        private void DisplayPlatformHeader()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;

            GUIStyle dropdownStyle = new GUIStyle(GUI.skin.FindStyle("dropdownButton"));
            dropdownStyle.fixedHeight = 0;

            BuildPlatformLists();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("<b>Platforms</b>", dropdownStyle, labelStyle);

            EditorGUI.BeginDisabledGroup(MissingPlatforms.Count == 0);

            bool showPlatforms = EditorGUILayout.DropdownButton(new GUIContent("Add Platform"), FocusType.Passive, dropdownStyle);

            EditorGUI.EndDisabledGroup();

            if (Event.current.type == EventType.Repaint)
            {
                AddPlatformButtonRect = GUILayoutUtility.GetLastRect();
            }

            if (GUILayout.Button(new GUIContent("Add Group")))
            {
                Settings settings = target as Settings;
                settings.AddPlatformGroup("Group");
                MissingPlatforms = null;
            }

            EditorGUILayout.EndHorizontal();

            if (showPlatforms)
            {
                GenericMenu menu = new GenericMenu();

                foreach (Platform platform in MissingPlatforms)
                {
                    menu.AddItem(new GUIContent(platform.DisplayName), false, AddPlatformProperties, platform.Identifier);
                }

                menu.DropDown(AddPlatformButtonRect);
            }
        }

        private Dictionary<Platform, Rect> PlatformParentRect = new Dictionary<Platform, Rect>();

        private void DisplayPlatformParent(Platform platform)
        {
            if (!platform.IsIntrinsic)
            {
                BuildPlatformLists();

                Settings settings = target as Settings;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PrefixLabel("Inherit From");
                bool showList = EditorGUILayout.DropdownButton(new GUIContent(platform.Parent.DisplayName), FocusType.Passive);

                if (Event.current.type == EventType.Repaint)
                {
                    PlatformParentRect[platform] = GUILayoutUtility.GetLastRect();
                }

                if (showList)
                {
                    GenericMenu menu = new GenericMenu();
#if UNITY_2018_2_OR_NEWER
                    menu.allowDuplicateNames = true;
#endif

                    GenericMenu.MenuFunction2 setParent = (newParent) =>
                    {
                        platform.Parent = newParent as Platform;
                    };

                    Action<Platform> AddMenuItem = (candidate) =>
                    {
                        bool isCurrent = platform.Parent == candidate;
                        menu.AddItem(new GUIContent(candidate.DisplayName), isCurrent, setParent, candidate);
                    };

                    AddMenuItem(settings.DefaultPlatform);

                    bool separatorAdded = false;

                    foreach (Platform candidate in ParentCandidates)
                    {
                        if (!candidate.InheritsFrom(platform))
                        {
                            if (!separatorAdded)
                            {
                                menu.AddSeparator(string.Empty);
                                separatorAdded = true;
                            }

                            AddMenuItem(candidate);
                        }
                    }

                    menu.DropDown(PlatformParentRect[platform]);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private string MakePathRelative(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            string fullPath = Path.GetFullPath(path);
            string fullProjectPath = Path.GetFullPath(Environment.CurrentDirectory + Path.DirectorySeparatorChar);

            // If the path contains the Unity project path remove it and return the result
            if (fullPath.Contains(fullProjectPath))
            {
                fullPath = fullPath.Replace(fullProjectPath, "");
            }
            // If not, attempt to find a relative path on the same drive
            else if (Path.GetPathRoot(fullPath) == Path.GetPathRoot(fullProjectPath))
            {
                // Remove trailing slash from project path for split count simplicity
                if (fullProjectPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture)) fullProjectPath = fullProjectPath.Substring(0, fullProjectPath.Length - 1);

                string[] fullPathSplit = fullPath.Split(Path.DirectorySeparatorChar);
                string[] projectPathSplit = fullProjectPath.Split(Path.DirectorySeparatorChar);
                int minNumSplits = Mathf.Min(fullPathSplit.Length, projectPathSplit.Length);
                int numCommonElements = 0;
                for (int i = 0; i < minNumSplits; i++)
                {
                    if (fullPathSplit[i] == projectPathSplit[i])
                    {
                        numCommonElements++;
                    }
                    else
                    {
                        break;
                    }
                }
                string result = "";
                int fullPathSplitLength = fullPathSplit.Length;
                for (int i = numCommonElements; i < fullPathSplitLength; i++)
                {
                    result += fullPathSplit[i];
                    if (i < fullPathSplitLength - 1)
                    {
                        result += '/';
                    }
                }

                int numAdditionalElementsInProjectPath = projectPathSplit.Length - numCommonElements;
                for (int i = 0; i < numAdditionalElementsInProjectPath; i++)
                {
                    result = "../" + result;
                }

                fullPath = result;
            }

            return fullPath.Replace(Path.DirectorySeparatorChar, '/');
        }
    }
}
