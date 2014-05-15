﻿namespace Kafka.Tests.Custom.Server
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Kafka.Tests.Utils;

    public class TempKafkaConfig : Dictionary<string, string>, IDisposable
    {
         private TempKafkaConfig(Dictionary<string, string> props) : base(props)
         {
             this.ConfigLocation = TestUtils.TempFile();
             File.WriteAllText(this.ConfigLocation, this.SerializeProperties());
         }

         public string ConfigLocation { get; private set; }

         public string LogDir 
         {
            get
            {
                 return this["log.dir"];
            }
         }

        public int BrokerId
        {
            get
            {
                return int.Parse(this["broker.id"]);
            }
        }

        public int Port
        {
            get
            {
                return int.Parse(this["port"]);
            }
        }

        public string SerializeProperties()
        {
            var sb = new StringBuilder();
            foreach (var kvp in this)
            {
                sb.Append(kvp.Key);
                sb.Append("=");
                sb.Append(kvp.Value);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static TempKafkaConfig Create(Dictionary<string, string> customProps)
        {
            return new TempKafkaConfig(customProps);
        }

        public void Dispose()
        {
            try
            {
                if (this.ConfigLocation != null)
                {
                    File.Delete(this.ConfigLocation);
                }
            }
            catch
            {
            }
            try
            {
                if (this.LogDir != null)
                {
                    Directory.Delete(this.LogDir, true);
                }
            }
            catch
            {
            }
            TestUtils.PortReleased(this.Port);
        }
    }
}