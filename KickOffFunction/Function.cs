using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using LambdaSharp;
using Amazon.S3;
using Amazon.S3.Model;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace My.DockerSwarm.KickOffFunction {

    public class Function : ALambdaFunction<LambdaScheduleEvent, string> {

        //--- Fields ---
        private IAmazonS3 _s3Client;
        private IAmazonSimpleNotificationService _snsClient;
        private string _commandTopic;
        private string _bucketName;
        private Random _random = new Random();

        //--- Methods ---
        public override async Task InitializeAsync(LambdaConfig config) {

            // initialize AWS clients
            _s3Client = new AmazonS3Client();
            _snsClient = new AmazonSimpleNotificationServiceClient();

            // read configuration values
            _commandTopic = config.ReadText("VirtualVacuumRobotTopic");
            _bucketName = config.ReadS3BucketName("Bucket");
        }

        public override async Task<string> ProcessMessageAsync(LambdaScheduleEvent request, ILambdaContext context) {

            // read all keys (bot IDs) from bucket
            var response = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request {
                BucketName = _bucketName,
                MaxKeys = 1000
            });

            // check if any keys exists
            if(response.S3Objects.Any()) {

                // pick a random key
                var random = _random.Next(response.S3Objects.Count);
                var id = response.S3Objects[random].Key;
                await SendCommand(id, "start");
            }
            return "Ok";
        }

        private Task SendCommand(string id, string action) {
            LogInfo($"Sending {action.ToUpperInvariant()} to bot #{id}");
            return _snsClient.PublishAsync(_commandTopic, SerializeJson(new {
                action = action,
                id = id
            }));
        }
    }
}
