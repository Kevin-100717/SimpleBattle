using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data
{
    public class BattleData
    {
        [JsonProperty("days")]
        public List<DaysEntity> Days { get; set; }
        [JsonProperty("datas")]
        public List<List<DatasEntity>> Datas { get; set; }
        [JsonProperty("sp_Route")]
        public List<SpRouteEntity> SpRoute { get; set; }

    }

    public class DaysEntity
    {
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("event")]
        public List<EventEntity> Event { get; set; }

    }

    public class EventEntity
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("dataIndex")]
        public int DataIndex { get; set; }

    }

    public class DatasEntity
    {
        [JsonProperty("enemy_key")]
        public string EnemyKey { get; set; }
        [JsonProperty("route")]
        public int Route { get; set; }
        [JsonProperty("sp_range")]
        public int SpRange { get; set; }
        [JsonProperty("interval")]
        public int Interval { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("start")]
        public int Start { get; set; }
        [JsonProperty("repeat")]
        public int Repeat { get; set; }

    }

    public class SpRouteEntity
    {
        [JsonProperty("start")]
        public StartEntity Start { get; set; }
        [JsonProperty("checkpoints")]
        public List<CheckpointsEntity> Checkpoints { get; set; }

    }

    public class StartEntity
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }

    }

    public class CheckpointsEntity
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("pos")]
        public PosEntity Pos { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }

    }

    public class PosEntity
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }

    }

}
