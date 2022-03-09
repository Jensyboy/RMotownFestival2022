using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RMotownFestival.Functions
{
    public class AnalyzationFunction
    {
        public ComputerVisionClient VisionClient { get; }

        private static readonly List<VisualFeatureTypes?> Features = new List<VisualFeatureTypes?>()
        { VisualFeatureTypes.Adult, VisualFeatureTypes.Brands, VisualFeatureTypes.Color, VisualFeatureTypes.Description};

        public AnalyzationFunction(ComputerVisionClient visionClient)
        {
            VisionClient = visionClient;
        }

        [FunctionName("AnalyzationFunction")]
        public async Task Run([BlobTrigger("picsin/{name}", Connection = "BlobStorageConnection")] byte[] myBlob, string name, ILogger log, Binder binder)
        {
            ImageAnalysis analysis = await VisionClient.AnalyzeImageInStreamAsync(new MemoryStream(myBlob), Features);

            Attribute[] attributes;

            if (analysis.Adult.IsAdultContent || analysis.Adult.IsRacyContent || analysis.Adult.IsGoryContent)
            {
                attributes = new Attribute[]
                {
                new BlobAttribute($"picsrejected/{name}", FileAccess.Write),
                new StorageAccountAttribute("BlobStorageConnection")
                };
            }
            else
            {
                attributes = new Attribute[]
                {
                new BlobAttribute($"festivalpics/{name}", FileAccess.Write),
                new StorageAccountAttribute("BlobStorageConnection")
                };
            }

            using Stream fileOutputStream = await binder.BindAsync<Stream>(attributes);

            fileOutputStream.Write(myBlob);


            if (analysis.Brands.Count > 0)
            {
                log.LogInformation($"brand is: " + analysis.Brands[0].Name);

            }

            log.LogInformation($" colour is: " + analysis.Color.DominantColorForeground);
            log.LogInformation($" Description is:  " + analysis.Description.Captions[0]);

            log.LogInformation($"adult content: {analysis.Adult.AdultScore} + {analysis.Adult.RacyScore}  Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}