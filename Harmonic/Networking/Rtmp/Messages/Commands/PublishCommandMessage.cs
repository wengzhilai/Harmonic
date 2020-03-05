using Harmonic.Networking.Rtmp.Serialization;
using Harmonic.Networking.Rtmp.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harmonic.Networking.Rtmp.Messages.Commands
{
    [RtmpCommand(Name = "publish")]
    public class PublishCommandMessage : CommandMessage
    {
        /// <summary>
        /// 推送流名称
        /// </summary>
        [OptionalArgument]
        public string PublishingName { get; set; }

   

        /// <summary>
        /// 推送类型
        /// </summary>
        [OptionalArgument]
        public string PublishingType { get; set; }

        
        /// <summary>
        /// 密码，新添加的，只能放在最后
        /// </summary>
        [OptionalArgument]
        public string pwd { get; set; }

        public PublishCommandMessage(AmfEncodingVersion encoding) : base(encoding)
        {
        }
    }
}
