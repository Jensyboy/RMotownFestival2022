using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMotownFestival.Api.Common;
using System;
using System.Linq;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        public BlobUtility Blobutility { get; }
        public PicturesController(BlobUtility blobutility)
        {
            Blobutility = blobutility;
        }

        [HttpGet]
        public string[] GetAllPictureUrls()
        {
            var container = Blobutility.GetThumbsContainer();
            return container.GetBlobs()
                .Select(blob => Blobutility.GetSasUri(container, blob.Name))
                .ToArray();
        }

        [HttpPost]
        public void PostPicture(IFormFile file)
        {
            BlobContainerClient container = Blobutility.GetPicturesContainer();
            container.UploadBlobAsync(file.FileName, file.OpenReadStream());
        }
    }
}
