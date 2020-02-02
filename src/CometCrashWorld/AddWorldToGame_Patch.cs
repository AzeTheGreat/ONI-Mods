using Harmony;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;

namespace CometCrashWorld
{
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    class AddWorldToGame_Patch
    {
        public static LocString NAME = "TEST";
        public static LocString DESCRIPTION = "Beep boop.";

        static void Prefix()
        {
            // Add strings used in Fuleria.yaml
            Strings.Add($"STRINGS.WORLDS.ICECOMETCRASH.NAME", NAME);
            Strings.Add($"STRINGS.WORLDS.ICECOMETCRASH.DESCRIPTION", DESCRIPTION);

            AddWorldYaml(NAME, DESCRIPTION, "Asteroid_Emptera", typeof(AddWorldToGame_Patch));
        }

        // Taken from Pholith, review and clean up
        private static void AddWorldYaml(string NAME, string DESCRIPTION, string iconName, Type className)
        {
            if (!iconName.IsNullOrWhiteSpace())
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fullPath = path + "\\" + iconName + ".dds";

                using (var sr = new StreamReader(fullPath).BaseStream)
                {
                    Sprite sprite = CreateSpriteDXT5(sr, 512, 512);
                    Assets.Sprites.Add(iconName, sprite);
                }
            }
        }

        // Taken from Pholith, review and clean up
        private static Sprite CreateSpriteDXT5(Stream inputStream, int width, int height)
        {
            byte[] array = new byte[inputStream.Length - 128L];
            inputStream.Seek(128L, SeekOrigin.Current);
            inputStream.Read(array, 0, array.Length);
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.DXT5, false);
            texture2D.LoadRawTextureData(array);
            texture2D.Apply(false, false);
            // this isn't an efficient way to flip the loaded texture but it only runs once so the performance impact can't be that terrible
            Texture2D texture2DFlipped = new Texture2D(width, height, TextureFormat.RGBA32, false);
            for (int i = 0; i < texture2D.width; i++)
            {
                for (int j = 0; j < texture2D.height; j++)
                {
                    texture2DFlipped.SetPixel(i, j, texture2D.GetPixel(i, height - j - 1));
                }
            }
            texture2DFlipped.Apply(false, true);
            Sprite sprite = Sprite.Create(texture2DFlipped, new Rect(0f, 0f, (float)width, (float)height), new Vector2((float)(width / 2), (float)(height / 2)));
            return sprite;
        }
    }
}
