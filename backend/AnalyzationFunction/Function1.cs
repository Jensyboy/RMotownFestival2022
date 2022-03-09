using System;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AnalyzationFunction
{
    public class Function1
    {

        public ComputerVisionClient VisionClient { get; }

        public AnalyzationFunction()
        {

        }

        [FunctionName("Function1")]
        public void Run([BlobTrigger("picsin/{name}", Connection = "BlobStorageConnection")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
