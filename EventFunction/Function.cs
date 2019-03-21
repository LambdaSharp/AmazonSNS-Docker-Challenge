using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using LambdaSharp;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace My.DockerSwarm.EventFunction {

    public class BotNotification {

        //--- Properties ---

        // Sample event: {"id":4270,"message":"78","eventType":"READY","timestamp":"2019-03-21T02:28:31.5225079Z"}
        public string Id { get; set; }
        public string Message { get; set; }
        public string EventType { get; set; }
    }

    public class Function : ALambdaTopicFunction<BotNotification> {

        //--- Fields ---
        private IAmazonS3 _s3Client;
        private IAmazonSimpleNotificationService _snsClient;
        private string _commandTopic;
        private string _notifyTopic;
        private string _bucketName;

        //--- Methods ---
        public override async Task InitializeAsync(LambdaConfig config) {

            // initialize AWS clients
            _s3Client = new AmazonS3Client();
            _snsClient = new AmazonSimpleNotificationServiceClient();

            // read configuration values
            _commandTopic = config.ReadText("VirtualVacuumRobotTopic");
            _notifyTopic = config.ReadText("VirtualVacuumRobotNotifyTopic");
            _bucketName = config.ReadS3BucketName("Bucket");
        }

        public override async Task ProcessMessageAsync(BotNotification message, ILambdaContext context) {
            LogInfo($"Received '{message.EventType}' from '{message.Id}'");
            switch(message.EventType) {
            case "READY":

                // add bot to available pool
                LogInfo($"Bot #{message.Id} added");
                await AddId(message.Id);
                break;
            case "SHUTDOWN":

                // remove bot from available pool
                LogInfo($"Bot #{message.Id} permanently removed");
                await RemoveId(message.Id);
                break;
            case "DUSTBIN_FULL":

                // instruct bot to clean its dustin
                await SendCommand(message.Id, "dustbin");

                // add bot back to available pool
                await AddId(message.Id);
                break;
            case "CLEANING":

                // check for battery level
                if(double.TryParse(message.Message, out var batteryLevel) && (batteryLevel < 20)) {

                    // instruct bot to recharge
                    await SendCommand(message.Id, "charge");
                }
                break;
            case "STUCK":

                // instruct bot the shutdown
                await SendCommand(message.Id, "shutdown");

                // send out notification about bot shutdown
                await _snsClient.PublishAsync(_notifyTopic, $"Bot #{message.Id} had to be shutdown.");
                break;
            case "STARTED":

                // bot is busy, remove it from available pool
                LogInfo($"Bot #{message.Id} is cleaning");
                await RemoveId(message.Id);
                break;
            case "ENDED":

                // bot ran out of battery => no action required
                break;
            case "STARTED_CHARGE":

                // bot is charging => no action required
                break;
            case "SLEEPING":

                // bot has finished, add it back to available pool
                LogInfo($"Bot #{message.Id} is idle");
                await AddId(message.Id);
                break;
            }
        }

        private Task SendCommand(string id, string action) {
            LogInfo($"Sending {action.ToUpperInvariant()} to bot #{id}");
            return _snsClient.PublishAsync(_commandTopic, SerializeJson(new {
                action = action,
                id = id
            }));
        }

        private Task AddId(string id) {

            // add the ID as a key in S3, so we can determine which bots are available
            return _s3Client.PutObjectAsync(new PutObjectRequest {
                BucketName = _bucketName,
                Key = id
            });
        }

        private async Task RemoveId(string id) {

            // remove ID from S3 to mark bot as unavailable
            try {
                await _s3Client.DeleteObjectAsync(new DeleteObjectRequest {
                    BucketName = _bucketName,
                    Key = id
                });
            } catch { }
        }
    }
}
