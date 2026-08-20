using System;
using System.IO;
using UnityEngine;

namespace DoofusAdventure
{
    [Serializable]
    public sealed class PlayerData
    {
        public float speed;
    }

    [Serializable]
    public sealed class PulpitData
    {
        public float min_pulpit_destroy_time;
        public float max_pulpit_destroy_time;
        public float pulpit_spawn_time;
    }

    [Serializable]
    public sealed class GameConfig
    {
        public const float PlatformSize = 9f;

        public PlayerData player_data = new PlayerData();
        public PulpitData pulpit_data = new PulpitData();

        public static GameConfig Default => new GameConfig
        {
            player_data = new PlayerData { speed = 3f },
            pulpit_data = new PulpitData
            {
                min_pulpit_destroy_time = 4f,
                max_pulpit_destroy_time = 5f,
                pulpit_spawn_time = 2.5f
            }
        };

        public static GameConfig Load(out string diagnostic)
        {
            var path = Path.Combine(Application.streamingAssetsPath, "doofus_diary.json");

            try
            {
                if (!File.Exists(path))
                {
                    diagnostic = $"Doofus Diary is missing at '{path}'. Using defaults.";
                    return Default;
                }

                return FromJson(File.ReadAllText(path), out diagnostic);
            }
            catch (Exception exception)
            {
                diagnostic = $"Could not read Doofus Diary ({exception.Message}). Using defaults.";
                return Default;
            }
        }

        public static GameConfig FromJson(string json, out string diagnostic)
        {
            try
            {
                var config = JsonUtility.FromJson<GameConfig>(json);
                if (config == null || config.player_data == null || config.pulpit_data == null)
                {
                    diagnostic = "Doofus Diary has a missing section. Using defaults.";
                    return Default;
                }

                if (config.player_data.speed <= 0f ||
                    config.pulpit_data.min_pulpit_destroy_time <= 0f ||
                    config.pulpit_data.max_pulpit_destroy_time <= 0f ||
                    config.pulpit_data.pulpit_spawn_time <= 0f)
                {
                    diagnostic = "Doofus Diary contains non-positive gameplay values. Using defaults.";
                    return Default;
                }

                if (config.pulpit_data.min_pulpit_destroy_time > config.pulpit_data.max_pulpit_destroy_time)
                {
                    var temporary = config.pulpit_data.min_pulpit_destroy_time;
                    config.pulpit_data.min_pulpit_destroy_time = config.pulpit_data.max_pulpit_destroy_time;
                    config.pulpit_data.max_pulpit_destroy_time = temporary;
                    diagnostic = "Doofus Diary lifetime range was reversed and has been corrected.";
                    return config;
                }

                diagnostic = "Loaded Doofus Diary.";
                return config;
            }
            catch (Exception exception)
            {
                diagnostic = $"Doofus Diary is malformed ({exception.Message}). Using defaults.";
                return Default;
            }
        }
    }
}
